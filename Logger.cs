/*
 * Created by SharpDevelop.
 * User: NCDyson
 * Date: 7/21/2017
 * Time: 11:22 PM
 * 
 * To change this template use Tools | Options | Coding | Edit Standard Headers.
 */
using System;
using System.Collections.Generic;
using System.Runtime.CompilerServices;
using System.Drawing;
using System.Windows.Forms;

namespace StudioCCS
{
	/// <summary>
	/// Description of Logger.
	/// </summary>
	public static class Logger
	{
		private static Dictionary<int, bool> FiredWarnings = new Dictionary<int, bool>();
		private static Dictionary<int, bool> FiredShots = new Dictionary<int, bool>();
		public enum LogType {LogAll, LogOnceCode, LogOnceValue}
		public static RichTextBox LogControl = null;
		
		
		public static void SetLogControl(RichTextBox r)
		{
			LogControl = r;
		}
		
		public static void LogError(string errorText, LogType logAs = LogType.LogAll, [CallerMemberName] string callingMethod = "", [CallerLineNumber] int callingLine = 0)
		{
			LogGeneric(errorText, Color.DarkRed, logAs, callingMethod, callingLine);
		}
		
		public static void LogWarning(string warningText, LogType logAs = LogType.LogAll, [CallerMemberName] string callingMethod = "", [CallerLineNumber] int callingLine = 0)
		{
			LogGeneric(warningText, Color.Orange, logAs, callingMethod, callingLine);
		}
		
		public static void LogInfo(string infoText, LogType logAs = LogType.LogAll, [CallerMemberName] string callingMethod = "", [CallerLineNumber] int callingLine = 0)
		{
			LogGeneric(infoText, Color.White, logAs, callingMethod, callingLine);
		}
		
		private static void LogGeneric(string outputText, Color textColor, LogType logAs = LogType.LogAll, [CallerMemberName] string callingMethod = "", [CallerLineNumber] int callingLine = 0)
		{
			if(logAs == LogType.LogOnceCode)
			{
				int logDictKey = string.Format("{0}:{1}", callingMethod, callingLine).GetHashCode();
				if(FiredWarnings.ContainsKey(logDictKey)) return;
				FiredWarnings[logDictKey] = true;
			}
			else if(logAs == LogType.LogOnceValue)
			{
				int logTextKey = outputText.GetHashCode();
				if(FiredShots.ContainsKey(logTextKey)) return;
				FiredShots[logTextKey] = true;
			}
			
			if(LogControl == null) return;
			Color oldColor = LogControl.SelectionColor;
			LogControl.SelectionColor = textColor;
			LogControl.AppendText(outputText);
			LogControl.SelectionColor = oldColor;
		}
	}
}