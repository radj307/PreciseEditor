using System;
using UnityEngine;
using System.Diagnostics;

namespace PreciseEditor
{
	public static class Log
	{
		const string MessagePrefix = "PreciseEditor: ";

        [ConditionalAttribute("DEBUG")]
        public static void Info(string message)
        {
            UnityEngine.Debug.Log(MessagePrefix + message);
        }

        [Conditional("DEBUG")]
		public static void Debug(string message)
		{
			UnityEngine.Debug.Log(MessagePrefix + message);
		}

		public static void Error(string message)
		{
			UnityEngine.Debug.LogError(MessagePrefix + message);
		}

		public static void Warning(string message)
		{
			UnityEngine.Debug.LogWarning(MessagePrefix + message);
		}
	}
}

