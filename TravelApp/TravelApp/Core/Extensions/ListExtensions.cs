using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace TravelApp.Core.Extensions
{
    public static class ListExtensions
    {
        public static bool IsNullOrEmpty(this List<string> list)
        {
            bool IsNull = false;
            foreach (var l in list)
            {
                if (l == null)
                {
                    IsNull = true;
                    break;
                }
            }
            return IsNull;
        }

        public static bool IsNull<T>(this List<T> list)
        {
            return list == null;
        }
    }
}
