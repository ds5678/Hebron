using System;

namespace Hebron
{
	public static class Logger
	{
		public static Action<string>? LogFunction = Console.Write;

		public static void Log(string data)
		{
			LogFunction?.Invoke(data);
		}

		public static void LogLine(string data)
		{
			LogFunction?.Invoke(data + Environment.NewLine);
		}

		public static void Warning(string message)
		{
			LogLine("Warning: " + message);
		}

		public static void Info(string message)
		{
			LogLine("Info: " + message);
		}
	}
}
