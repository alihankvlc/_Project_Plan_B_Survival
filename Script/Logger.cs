using System;
using UnityEngine;

public static class Logger
{
    public static event Action<string> LogAction;
    public static class Log
    {
        public static void Warning(object sender, string message, Color color, bool showToPlayer = false)
        => LogMessage(sender, message, color, LogType.Log, showToPlayer);
        public static void Error(object sender, string message, Color color, bool showToPlayer = false)
        => LogMessage(sender, message, color, LogType.Log, showToPlayer);
        public static void Info(object sender, string message, Color color, bool showToPlayer = false)
        => LogMessage(sender, message, color, LogType.Log, showToPlayer);

        private static void LogMessage(object sender, string message, Color color, LogType logType, bool showToPlayer = false)
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

            logAction($"<color=yellow>[{sender.GetType().Name}] </color>= {formattedMessage}");
            if (showToPlayer) LogAction?.Invoke($"{formattedMessage}");
        }
    }
}
