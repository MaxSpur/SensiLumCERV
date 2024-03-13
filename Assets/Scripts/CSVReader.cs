
using System;
using System.IO;
using System.Collections.Generic;
using UnityEngine;
using System.Linq;
public class CSVReader
{
	public const char DEFAULT_CSV_SEPARATOR = ',';
	private const string DEFAULT_NEW_LINE = "\r\n";
	public static string[] ReadCSVLine(TextReader sr, char csvSeparator = DEFAULT_CSV_SEPARATOR)
	{
		int nbLine;
		return ReadCSVLine(sr, out nbLine, csvSeparator);
	}
	public static string[] ReadCSVLine (TextReader sr, out int nbLine, char csvSeparator = DEFAULT_CSV_SEPARATOR)
	{
		nbLine=0;
		List<string> fields = new List<string> ();
		string line = sr.ReadLine ();
		if (line == null) {
			return null;
		}
		nbLine=1;
		string currentField = "";
		bool inParenthesis = false;
		while (true) {
			if (line.Length == 0) {
				if (inParenthesis) {
					Debug.LogError ("Pb reading file ((\") parenthesis not closed)");
				} else if(fields.Count > 0){
					Debug.LogWarning ("Empty Line?? " + fields.Count);
				}
				break;
			}
			char currentChar = line [0];
			line = line.Substring (1);
			if (currentChar == '"') {
				if (inParenthesis) {
					if (line.Length > 0 && line [0] == '\"') {
						currentField += '"';
						line = line.Substring (1);
					} else {
						inParenthesis = false;
					}
				} else {
					inParenthesis = true;
				}
			} else if (currentChar == csvSeparator && !inParenthesis) {
				fields.Add (currentField);
				currentField = "";
			} else {
				currentField += currentChar;
			}
			if (line.Length == 0) {
				if (inParenthesis) {
					while (line.Length == 0) {
						var cline = sr.ReadLine();
						if (cline == null)
							break;
						line += cline;
						currentField += DEFAULT_NEW_LINE;
						nbLine++;
					}
				} else {
					fields.Add (currentField);
					currentField = "";
					break;
				}
			}
		}
		return fields.ToArray ();
	}

	public static string ConvertCSVLine(string[] fields, char csvSeparator = DEFAULT_CSV_SEPARATOR)
	{
		for (int i = 0; i < fields.Length; i++){
			if (fields[i].Contains("\"") || fields[i].Contains("\n") || fields[i].Contains(csvSeparator))
			{
				fields[i] = "\"" + fields[i].Replace("\"", "\"\"") + "\"";
			}
		}
		return string.Join(csvSeparator.ToString(), fields);
	}
}


