using System;
using System.Diagnostics;

namespace Match3.Core
{
    public static class Debug
    {
        public static event Action<string> OnLog;
        public static event Action<string> OnWarning;

        public static void Log(string text)
        {
            OnLog?.Invoke(text);
        }
        
        public static void Warning(string text)
        {
            OnWarning?.Invoke(text);
        }
        
        [Conditional("DEBUG")]
        public static void Assert(bool value)
        {
            if (!value)
                throw new Exception();
        }
    }
}