using System;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

internal struct vec3
{
	public float x, y, z;
	//public vec3()
	//{
	//	this.x = 0;
	//	this.y = 0;
	//	this.z = 0;
	//}
	public vec3(float x, float y, float z)
	{
		this.x = x;
		this.y = y;
		this.z = z;
	}
	public static vec3 operator *(vec3 a, vec3 b)
	{
		return new vec3(a.x * b.x, a.y * b.y, a.z * b.z);
	}
	public static vec3 operator /(vec3 a, vec3 b)
	{
		return new vec3(a.x / b.x, a.y / b.y, a.z / b.z);
	}
	public static vec3 operator +(vec3 a, vec3 b)
	{
		return new vec3(a.x + b.x, a.y+ b.y, a.z+ b.z);
	}
	public static vec3 operator -(vec3 a, vec3 b)
	{
		return new vec3(a.x - b.x, a.y - b.y, a.z - b.z);
	}
	public static vec3 operator *(vec3 a, float b)
	{
		return new vec3(a.x * b, a.y * b, a.z * b);
	}
	public static vec3 operator /(vec3 a, float b)
	{
		return new vec3(a.x / b, a.y / b, a.z / b);
	}
	public static vec3 operator +(vec3 a, float b)
	{
		return new vec3(a.x + b, a.y + b, a.z + b);
	}
	public static vec3 operator -(vec3 a, float b)
	{
		return new vec3(a.x - b, a.y - b, a.z - b);
	}
	public static vec3 operator *(float b, vec3 a)
	{
		return a * b;
	}
	public static vec3 operator /(float b, vec3 a)
	{
		return new vec3(b, b, b) / a;
	}
	public static vec3 operator +(float b, vec3 a)
	{
		return a + b;
	}
	public static vec3 operator -(float b, vec3 a)
	{
		return new vec3(b, b, b) - a;
	}
	public static vec3 operator -(vec3 a)
	{
		return new vec3(-a.x, -a.y, -a.z);
	}
	public Color ToSaturatedColor()
	{
		return new Color((WavelengthColor.saturate(x)), (WavelengthColor.saturate(y)), (WavelengthColor.saturate(z)));
	}
	public Color ToRescaledColor()
	{
		var r = x;
		var g = y;
		var b = z;
		var max = Math.Max(r, g);
		max = Math.Max(max, b);
		if(max > 1)
		{
			r/=max;
			g/=max;
			b/=max;
		}
		return new Color((int)(WavelengthColor.saturate(r)), (int)(WavelengthColor.saturate(g)), (int)(WavelengthColor.saturate(b)));
	}
	public Color ToColor()
	{
		return new Color(x,y,z);
	}
	public static vec3 FromColor(Color c)
	{
		return new vec3(c.r, c.g, c.b);
	}
}
