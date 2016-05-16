using System;
using System.Collections;
using System.Linq;
using System.Text;

namespace DBNormalizationAnalyzer.AnalyzerLibrary
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

        public static BitArray ToBitArray(this string str)
        {
            var res = new BitArray(str.Length);
            for (var i = 0; i < str.Length; i++)
            {
                if(str[i] != '0' && str[i] != '1')
                    throw new ArgumentException();
                res[i] = str[i] == '1';
            }
            return res;
        }

        public static bool EqualsTo(this BitArray a, BitArray b)
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

        public static BitArray Not(BitArray array)
        {
            var res = new BitArray(array.Count);
            for (var i = 0; i < array.Count; i++)
            {
                res[i] = !array[i];
            }
            return res;
        }
        public static BitArray Or(BitArray array1,BitArray array2)
        {
            if(array1.Count != array2.Count)
                throw new ArgumentException();
            var res = new BitArray(array1.Count);
            for (var i = 0; i < array1.Count; i++)
            {
                res[i] = array1[i] || array2[i];
            }
            return res;
        }
        public static BitArray And(BitArray array1, BitArray array2)
        {
            if (array1.Count != array2.Count)
                throw new ArgumentException();
            var res = new BitArray(array1.Count);
            for (var i = 0; i < array1.Count; i++)
            {
                res[i] = array1[i] && array2[i];
            }
            return res;
        }
    }
}
