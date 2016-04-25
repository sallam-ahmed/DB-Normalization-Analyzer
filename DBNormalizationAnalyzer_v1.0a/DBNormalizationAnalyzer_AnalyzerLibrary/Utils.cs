using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace DBNormalizationAnalyzer_AnalyzerLibrary
{
    public static class Utils
    {
        public static string ToBitString(this BitArray array)
        {
            var builder = new StringBuilder();
            foreach (var bit in array.Cast<bool>())
                builder.Append(bit ? "1" : "0");
            return builder.ToString();
        }

        public static bool Equals(this BitArray a, BitArray b)
        {
            if (a.Count != b.Count)
                return false;
            for (var i = 0; i < a.Count; i++)
            {
                if (a.Get(i) != b.Get(i))
                    return false;
            }
            return true;
        }

        public static bool IsSuperSet(this BitArray a, BitArray b)
        {
            if (a.Count != b.Count)
                return false;
            for (var i = 0; i < b.Count; i++)
            {
                if (!a.Get(i) && b.Get(i))
                    return false;
            }
            return true;
        }

        public static void Resize<T>(this List<T> list, int sz, T c = default(T))
        {
            var cur = list.Count;
            if (sz < cur)
                list.RemoveRange(sz, cur - sz);
            else if (sz > cur)
            {
                if (sz > list.Capacity)
                    list.Capacity = sz;
                list.AddRange(Enumerable.Repeat(c, sz - cur));
            }
        }
    }
}
