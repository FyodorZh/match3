using System;
using System.Diagnostics;

namespace Match3.Core
{
    public static class Debug
    {
        [Conditional("DEBUG")]
        public static void Assert(bool value)
        {
            if (!value)
                throw new Exception();
        }
    }
}