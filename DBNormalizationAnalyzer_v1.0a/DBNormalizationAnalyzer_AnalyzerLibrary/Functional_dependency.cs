using System;
using System.Collections;
using System.Collections.Generic;
using System.Data;
using System.Diagnostics;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DBNormalizationAnalyzer_AnalyzerLibrary
{
    internal class FunctionalDependency
    {

        public List<Tuple<List<int>, List<int>> > DependencyList { get; private set; }

        public List<int> CurrentPrimaryKey { get; set; }

        public Graph mGraph { get; private set; }

        private bool _minimalized;

        private readonly int _keysCount;

        public FunctionalDependency(int keysCount)
        {
            _keysCount = keysCount;
        }

        public bool IsSubset(List<int> a, List<int> b)
        {
            return b.TrueForAll(a.Contains);
        }
        public bool IsPrimeKey(int key)
        {
            Prepare();
            return mGraph.IsPrime(key);
        }

        public bool IsSuperKey(List<int> keys)
        {
            Prepare();
            return !mGraph.GetReachability(keys).ToString().Contains('0');
        }

        public bool IsCandidateKey(List<int> keys)
        {
            if (!IsSuperKey(keys)) return false;
            var clone = keys;
            foreach (var key in keys)
            {
                if (!mGraph.IsPrime(key))
                    return false;
                clone.Remove(key);
                if (!mGraph.GetReachability(keys).ToString().Contains('0'))
                    return false;
                clone.Add(key);
            }
            return true;
        }

        public void AddDependency(List<int> from, List<int> to)
        {
            to.RemoveAll(from.Contains);
            var toAdd = new Tuple<List<int>, List<int>>(from, to);
            if (!DependencyList.Contains(toAdd))
            {
                DependencyList.Add(toAdd);
                _minimalized = false;
            }
        }

        private void Minimalize()
        {
            if (_minimalized) return;
            var change = true;
            while (change)
            {
                change = false;
                DependencyList.Sort((t1, t2) => (t1.Item1.Count.CompareTo(t2.Item1.Count)));
                for (var i = 0; i < DependencyList.Count; i++)
                {
                    for (var j = i + 1; j < DependencyList.Count && DependencyList[j].Item1 == DependencyList[i].Item1;)
                    {
                        var toAdd = DependencyList[j].Item2.Where(key => !DependencyList[i].Item2.Contains(key)).ToList();
                        change |= toAdd.Count != 0;
                        DependencyList[i].Item2.AddRange(toAdd);
                        DependencyList.RemoveAt(j);
                    }
                }
                var n = DependencyList.Count;
                for (var i = n - 1; DependencyList[i].Item1.Count > DependencyList[0].Item1.Count; i--)
                {
                    for (var j = 0; j < i; j++)
                    {
                        if (!IsSubset(DependencyList[i].Item1, DependencyList[j].Item1)) continue;
                        change |= DependencyList[i].Item2.RemoveAll(key => DependencyList[j].Item2.Contains(key)) 
                                  != 0;
                        change |= DependencyList[i].Item1.RemoveAll(key => DependencyList[j].Item2.Contains(key))
                                  != 0;
                    }
                }
                change |= DependencyList.RemoveAll(dependency => dependency.Item2.Count == 0) != 0;

            }
        }

        private void Prepare()
        {
            if (_minimalized) return;
            Minimalize();
            mGraph = new Graph(_keysCount);
            foreach (var dependency in DependencyList)
            {
                foreach (var key in dependency.Item2)
                {
                    mGraph.AddEdge(dependency.Item1,new List<int> {key});
                }   
            }
            _minimalized = true;
        }
    }
}
