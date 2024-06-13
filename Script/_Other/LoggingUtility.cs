using System;
using UnityEngine;

namespace _Other_.Runtime.Code
{
    public static class LoggingUtility
    {
        public static event Action<string> LogAction;

        public static class Log
        {
            public static void Warning(object sender, string message, Color color, bool debugging = true,
                bool showToPlayer = false)
                => Message(sender, message, color, LogType.Log, debugging, showToPlayer);

            public static void Error(object sender, string message, Color color, bool debugging = true,
                bool showToPlayer = false)
                => Message(sender, message, color, LogType.Log, debugging, showToPlayer);

            public static void Message(object sender, string message, Color color, bool debugging = true,
                bool showToPlayer = false)
                => Message(sender, message, color, LogType.Log, debugging, showToPlayer);

            private static void Message(object sender, string message, Color color, LogType logType,
                bool debugging = true, bool showToPlayer = false)
            {
                string hexColor = ColorUtility.ToHtmlStringRGB(color);
                string formattedMessage = $"<color=#{hexColor}>{message}</color>";

                Action<string> logAction = logType switch
                {
                    LogType.Log => Debug.Log,
                    LogType.Warning => Debug.LogWarning,
                    LogType.Error => Debug.LogError,
                    _ => throw new ArgumentOutOfRangeException(nameof(logType), logType, null)
                };

                if (debugging) logAction($"<color=yellow>[{sender.GetType().Name}] </color>= {formattedMessage}");
                if (showToPlayer) LogAction?.Invoke($"{formattedMessage}");
            }
        }
    }
}