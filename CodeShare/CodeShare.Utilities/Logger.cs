using System;
using System.Collections.Generic;
using System.IO;
using System.Reflection;
using System.Runtime.CompilerServices;
using System.Text;

namespace CodeShare.Utilities
{
    public static class Logger
    {
        private static readonly string _filePath;

        static Logger()
        {
            _filePath = Path.Combine(Path.GetTempPath(), $"log_{CurrentDateTimeString}.txt");
        }

        private static string CurrentDateTimeString
        {
            get
            {
                var time = DateTime.Now;

                return $"{time.Year}{time.Month}{time.Day}{time.Hour}_{time.Minute}{time.Second}{time.Millisecond}";
            }
        }

        [MethodImpl(MethodImplOptions.NoInlining)]
        public static void WriteLine(string message, [CallerMemberName] string callerName = "NO_NAME", [CallerFilePath] string callerFilePath = "NO_PATH", [CallerLineNumber] int callerLineNumberAttribute = 0)
        {
            var callerAssembly = Assembly.GetCallingAssembly()?.GetName()?.Name;
            var callerFile = Path.GetFileNameWithoutExtension(callerFilePath);
            var text = CreateCallerString(message, callerAssembly, callerName, callerFile, callerLineNumberAttribute);

            AppendTextToFile(text);
            System.Diagnostics.Debug.WriteLine(text);
        }

        private static string CreateCallerString(string message, string callerAssembly, string callerName, string callerFile, int callerLineNumber)
        {
            var text = new StringBuilder();
            var hasCallerInfo = false;

            if (!string.IsNullOrEmpty(callerAssembly))
            {
                text.Append($"{callerAssembly}:");
                hasCallerInfo = true;
            }

            if (!string.IsNullOrEmpty(callerFile))
            {
                text.Append($"{callerFile}.");
                hasCallerInfo = true;
            }

            if (!string.IsNullOrEmpty(callerName))
            {
                text.Append($"{callerName}");
                if (callerLineNumber > 0)
                {
                    text.Append($"({callerLineNumber})");
                }
                hasCallerInfo = true;
            }

            if (hasCallerInfo)
            {
                text.Append($" --> ");
            }

            if (!string.IsNullOrEmpty(message))
            {
                text.Append($"{message}");
            }

            return text.ToString();
        }

        private static void AppendTextToFile(string text)
        {
            if (!File.Exists(_filePath))
            {
                text = $"{CurrentDateTimeString} {text}";

                try
                {
                    using (var streamWriter = File.CreateText(_filePath))
                    {
                        streamWriter.WriteLine(text);
                    }
                }
                catch(Exception)
                {
                    throw;
                }
            }
            else
            {
                text = $"\n{CurrentDateTimeString} {text}";

                try
                {
                    File.AppendAllText(_filePath, text);
                }
                catch (Exception)
                {
                    throw;
                }
            }
        }
    }
}
