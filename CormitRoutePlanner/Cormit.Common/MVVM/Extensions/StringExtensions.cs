#region

using System;

#endregion

namespace Imarda.Lib.MVVM.Extensions
{
    public static class StringExtensions
    {
        public static bool EqualsIgnoreCase(this string me, string otherString)
        {
            return me.Equals(otherString, StringComparison.OrdinalIgnoreCase);
        }

        public static string[] Split(this string me, params string[] separators)
        {
            return me.Split(separators, StringSplitOptions.RemoveEmptyEntries);
        }

        public static string ToLowerFirstAlfa(this string me)
        {
            if (me == null)
            {
                return null;
            }

            if (me.Length < 1)
            {
                return me;
            }

            return string.Concat(me.Substring(0, 1).ToLower(), me.Substring(1, me.Length - 1));
        }
    }
}