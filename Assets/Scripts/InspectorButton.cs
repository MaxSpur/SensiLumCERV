using UnityEngine;
#if UNITY_EDITOR
using UnityEditor;
#endif
using System.Reflection;

/// <summary>
/// This attribute can only be applied to fields because its
/// associated PropertyDrawer only operates on fields (either
/// public or tagged with the [SerializeField] attribute) in
/// the target MonoBehaviour.
/// </summary>
[System.AttributeUsage(System.AttributeTargets.Field)]
public class InspectorButtonAttribute : PropertyAttribute
{
	public readonly string MethodName;
	public readonly bool ExecutionOnly;
	
	public InspectorButtonAttribute(string MethodName, bool executionOnly = false)
	{
		this.MethodName = MethodName;
		this.ExecutionOnly = executionOnly;
	}
}

#if UNITY_EDITOR
[CustomPropertyDrawer(typeof(InspectorButtonAttribute))]
public class InspectorButtonPropertyDrawer : PropertyDrawer
{
	private MethodInfo _eventMethodInfo = null;
	
	public override void OnGUI(Rect position, SerializedProperty prop, GUIContent label)
	{
		InspectorButtonAttribute inspectorButtonAttribute = (InspectorButtonAttribute)attribute;
		if(inspectorButtonAttribute.ExecutionOnly && !Application.isPlaying) {
			return;
		}
		Rect buttonRect = new Rect(position.x , position.y, position.width, position.height);
		if (GUI.Button(buttonRect, label.text))
		{
			System.Object target = prop.serializedObject.targetObject;
			System.Type eventOwnerType = target.GetType();
			string propertyPath = prop.propertyPath;
			while(propertyPath.Contains(".")) {
				int index = propertyPath.IndexOf(".");
				string propName = propertyPath.Substring(0,index);
				var field = eventOwnerType.GetField(propName);
				if(field == null)  {
					break; //pb
				}
				target = field.GetValue(target);
				eventOwnerType = field.FieldType;
				propertyPath = propertyPath.Substring(index+1);
				string checkList="Array.data[";
				if(propertyPath.StartsWith(checkList)) {
					propertyPath = propertyPath.Substring(checkList.Length);
					index = propertyPath.IndexOf("].");
					int indexInArray=int.Parse(propertyPath.Substring(0,index));
					target = (target as System.Collections.IList)[indexInArray];
					eventOwnerType = target.GetType();
					propertyPath = propertyPath.Substring(index+2);
				}
			}
			string eventName = inspectorButtonAttribute.MethodName;
			
			if (_eventMethodInfo == null)
				_eventMethodInfo = eventOwnerType.GetMethod(eventName, BindingFlags.Instance | BindingFlags.Static | BindingFlags.Public | BindingFlags.NonPublic);
			
			if (_eventMethodInfo != null)
				_eventMethodInfo.Invoke(target, null);
			else
				Debug.LogWarning(string.Format("InspectorButton: Unable to find method {0} in {1}", eventName, eventOwnerType));
		}
	}
}
#endif