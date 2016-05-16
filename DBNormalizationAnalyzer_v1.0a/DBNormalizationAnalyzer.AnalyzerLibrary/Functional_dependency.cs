using System;
using System.Collections;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Runtime.Serialization;

namespace DBNormalizationAnalyzer.AnalyzerLibrary
{
    [Serializable]
    public class FunctionalDependency : ISerializable
    {

        public List<Tuple<BitArray, BitArray> > DependencyList { get; }

        public BitArray CurrentPrimaryKey { get; set; }
        
        public List<int> Left, Middle, Right;

        public List<BitArray> SufficientCandidateKeys;

        public List<int> Keys;

        private bool _prepared;
        private BitArray _prime;

        public void GetObjectData(SerializationInfo info, StreamingContext context)
        {
            info.AddValue("CurKey",CurrentPrimaryKey,typeof(BitArray));
            info.AddValue("cnt",Keys.Count);
            var dependecyStrings = new List<string>();
            foreach (var dependency in DependencyList)
            {
                dependecyStrings.Add(dependency.Item1.ToBitString());
                dependecyStrings.Add(dependency.Item2.ToBitString());
            }
            info.AddValue("deps", dependecyStrings, typeof(List<string>));
        }

        public FunctionalDependency(int keysCount, BitArray primaryKey)
        {
            Keys = Enumerable.Range(0, keysCount).ToList();
            _prepared = false;
            SufficientCandidateKeys = new List<BitArray>();
            Left = new List<int>();
            Middle = new List<int>();
            Right = new List<int>();
            _prime = new BitArray(keysCount);
            CurrentPrimaryKey = primaryKey;
            DependencyList = new List<Tuple<BitArray, BitArray>>();
        }

        public FunctionalDependency(SerializationInfo info, StreamingContext context)
        {
            Keys = Enumerable.Range(0, info.GetInt32("cnt")).ToList();
            _prepared = false;
            SufficientCandidateKeys = new List<BitArray>();
            Left = new List<int>();
            Middle = new List<int>();
            Right = new List<int>();
            _prime = new BitArray(Keys.Count);
            CurrentPrimaryKey = (BitArray)info.GetValue("CurKey",typeof(BitArray));
            DependencyList = new List<Tuple<BitArray, BitArray>>();
            var dependecyStrings = (List<string>) info.GetValue("deps", typeof (List<string>));
            for (var i = 0; i < dependecyStrings.Count; i+=2)
            {
                DependencyList.Add(new Tuple<BitArray, BitArray>(dependecyStrings[i].ToBitArray(),
                    dependecyStrings[i + 1].ToBitArray()));
            }
        }

        public bool IsPrimeKey(int key)
        {
            Prepare();
            return _prime[key];
        }

        public bool IsSuperKey(BitArray keys)
        {
            Prepare();
            return !Reachability(keys).ToBitString().Contains('0');
        }

        public bool IsCandidateKey(BitArray keys)
        {
            if (!IsSuperKey(keys))
                return false;

            var clone = keys;
            if (Left.Any(key => !keys[key]))
                return false;
            if (Keys.Any(key => keys[key] && !IsPrimeKey(key)))
                return false;
            for(var i = 0;i < Keys.Count;i++)
            {
                if (!keys[i])
                    continue;
                if (Left.Contains(i))
                    continue;
                clone[i] = false;
                if (IsSuperKey(clone))
                    return false;
                clone[i] = true;
            }
            return true;
        }

        public void AddDependency(BitArray from, BitArray to)
        {
            if(from.Count != Keys.Count)
                return;
            if (to.Count != Keys.Count)
                return;
            to.And(Utils.Not(from));
            var index =
                DependencyList.FindIndex(tuple => tuple.Item1.EqualsTo(from));
            if (index != -1)
                DependencyList[index].Item2.Or(to);
            else
                DependencyList.Add(new Tuple<BitArray, BitArray>(from, to));
            _prepared = false;
        }

        public void RemoveDependency(BitArray from, BitArray to)
        {
            if (from.Count != Keys.Count)
                return;
            if (to.Count != Keys.Count)
                return;
            to.And(Utils.Not(from));
            var index =
                DependencyList.FindIndex(tuple => tuple.Item1.EqualsTo(from));
            if (index == -1)
                throw new ArgumentOutOfRangeException();
            if (to.EqualsTo(DependencyList[index].Item2))
                DependencyList.RemoveAt(index);
            else
                DependencyList[index].Item2.And(Utils.Not(to));
            _prepared = false;
        }

        public void RemoveKey(int index)
        {
            if(Keys.Count <= index)
                return;
            _prepared = false;
            Keys = Enumerable.Range(0, Keys.Count - 1).ToList();
            foreach (var key in Keys.Where(key => CurrentPrimaryKey[key + (key >= index ? 1 : 0)]))
            {
                CurrentPrimaryKey[key] = true;
            }
            CurrentPrimaryKey.Length = Keys.Count;
            for (var i = 0; i < DependencyList.Count; i++)
            {
                if (DependencyList[i].Item1[index])
                {
                    DependencyList.RemoveAt(i);
                    i--;
                    continue;
                }
                var newDependent = new BitArray(Keys.Count);
                var newIndependent = new BitArray(Keys.Count);
                var foundDependent = false;
                foreach (var key in Keys)
                {
                    if (DependencyList[i].Item1[key + (key >= index ? 1 : 0)])
                        newIndependent[key] = true;
                    if (DependencyList[i].Item2[key + (key >= index ? 1 : 0)])
                        newDependent[key] = foundDependent = true;
                }
                if(!foundDependent)
                {
                    DependencyList.RemoveAt(i);
                    i--;
                    continue;
                }
                DependencyList[i] = new Tuple<BitArray, BitArray>(newIndependent,newDependent);
            }
        }

        public void AddKey()
        {
            _prepared = false;
            Keys.Add(Keys.Count);
            CurrentPrimaryKey.Length = Keys.Count;
            foreach (var t in DependencyList)
            {
                t.Item1.Length = Keys.Count;
                t.Item2.Length = Keys.Count;
            }
        }

        private void Minimalize()
        {
            if (_prepared)
                return;
            while (true)
            {
                var change = false;
                for(var i = 0;i < DependencyList.Count;i++)
                {
                    for (var j = 0; j < DependencyList.Count; j++)
                    {
                        if (i == j)
                            continue;
                        if (!DependencyList[i].Item1.IsSuperSet(DependencyList[j].Item1))
                            continue;
                        foreach (var key in Keys.Where(key => DependencyList[j].Item2[key]))
                        {
                            change |= DependencyList[i].Item2[key] || DependencyList[i].Item1[key];
                            DependencyList[i].Item1.Set(key,false);
                            DependencyList[i].Item2.Set(key, false);
                        }
                    }
                }
                if (!change)
                    break;
                DependencyList.RemoveAll(dependency => !dependency.Item2.ToBitString().Contains('1'));
                for (var i = 0; i < DependencyList.Count; i++)
                {
                    for (var j = i+1; j < DependencyList.Count; j++)
                    {
                        if (!DependencyList[i].Item1.EqualsTo(DependencyList[j].Item1))
                            continue;
                        DependencyList[i].Item2.Or(DependencyList[j].Item2);
                        DependencyList.RemoveAt(j);
                        j--;
                    }
                }
            }
        }

        private void Divide()
        {
            Left = new List<int>();
            Right = new List<int>();
            Middle = new List<int>();
            foreach (var dependency in DependencyList)
            {
                for (var i = 0; i < Keys.Count; i++)
                {
                    if (dependency.Item1[i])
                    {
                        if (Right.Contains(i))
                        {
                            Right.Remove(i);
                            Middle.Add(i);
                        }
                        else if (!Middle.Contains(i))
                            Left.Add(i);
                    }
                    if (dependency.Item2[i])
                    {
                        if(Left.Contains(i))
                        {
                            Left.Remove(i);
                            Middle.Add(i);
                        }
                        else if (!Middle.Contains(i))
                            Right.Add(i);
                    }
                }
            }
            Left.AddRange(
                Enumerable.Range(0, Keys.Count)
                    .Where(key => !(Left.Contains(key) || Right.Contains(key) || Middle.Contains(key))));
        }

        public BitArray Reachability(BitArray keys)
        {
            var res = new BitArray(Keys.Count);
            var change = true;
            while (change)
            {
                change = false;
                foreach (var dependency in DependencyList)
                {
                    if (!keys.IsSuperSet(dependency.Item1))
                        continue;
                    var temp = Utils.Or(keys,dependency.Item2);
                    change |= !temp.EqualsTo(keys);
                    keys = temp;
                }
            }
            return res;
        }

        private void GetPrimes()
        {
            _prime = new BitArray(Keys.Count);
            var leftBits = new BitArray(Keys.Count);
            foreach (var key in Left)
            {
                _prime[key] = leftBits[key] = true;
            }
            foreach (var key in Right)
            {
                _prime[key] = false;
            }
            foreach (var key in Middle)
            {
                _prime[key] = false;
            }
            var currentCover = Reachability(leftBits);
            var candidates = Middle;
            candidates.RemoveAll(key => currentCover[key]);
            if (candidates.Count == 0)
                return;
            var graph = new Graph(Keys.Count);
            foreach (var dependency in DependencyList)
            {
                var str = dependency.Item1.ToBitString();
                if (str.Count(c => c == '1') != 1)
                    continue;
                var index = str.IndexOf('1');
                if (!candidates.Contains(index))
                    continue;
                for (var i = 0; i < Keys.Count; i++)
                {
                    if(dependency.Item2[i] && candidates.Contains(i))
                        graph.AddEdge(index,i);
                }
            }
            graph.GetSccs();
            var sccs = new List<List<int>>();
            for (var i = 0; i < candidates.Count; i++)
            {
                sccs.Add(new List<int>());
                sccs.Last().Add(candidates[i]);
                for (var j = i + 1; j < candidates.Count; j++)
                {
                    if (graph.Scc[candidates[i]] != graph.Scc[candidates[j]])
                        continue;
                    sccs.Last().Add(candidates[j]);
                    candidates.RemoveAt(j);
                    j--;
                }
            }
            for (var i = 0; i < candidates.Count; i++)
            {
                var currentKey = new BitArray(Keys.Count) {[candidates[i]] = true};
                var coveredByKey = Reachability(currentKey);
                if (sccs[i].Any(candidate => DependencyList.Any(dependency => dependency.Item2[candidate] && Keys.Any(key => dependency.Item1[key] && !coveredByKey[key]))))
                    continue;
                foreach (var key in sccs[i])
                {
                    _prime[key] = true;
                    leftBits[key] = true;
                    Left.Add(key);
                    Middle.Remove(key);
                }
                sccs.RemoveAt(i);
                candidates.RemoveAt(i);
                i--;
            }
            currentCover = Reachability(leftBits);
            for (var i = 0; i < candidates.Count; i++)
            {
                if (!currentCover[candidates[i]])
                    continue;
                foreach (var key in sccs[i])
                {
                    _prime[key] = false;
                }
                sccs.RemoveAt(i);
                candidates.RemoveAt(i);
                i--;
            }
            if (candidates.Count == 0)
            {
                SufficientCandidateKeys.Add(leftBits);
                return;
            }
            // I claim by this line, the number of candidates can't be more than 16; given that the number of attributes isn't more than 4069, but who knows!
            if (candidates.Count > 20)
            {
                throw new ConstraintException("Too many attributes!");
            }
            var foundSets = new List<int>();
            for (var i = 1; i < (1 << candidates.Count); i++)
            {
                var currentSet = leftBits;
                var newSet = false;
                for (var j = 0; j < candidates.Count; j++)
                    if ((i & (1 << j)) != 0)
                    {
                        currentSet[candidates[j]] = true;
                        newSet |= !_prime[candidates[j]];
                    }
                if (!newSet)
                    continue;
                if (foundSets.Any(set => (i & set) == set))
                {
                    i += i & -i;
                    i--;
                    continue;
                }
                if(Reachability(currentSet).ToBitString().Contains('0'))
                    continue;
                foundSets.Add(i);
                SufficientCandidateKeys.Add(currentSet);
                for (var j = 0; j < candidates.Count; j++)
                    if ((i & (1 << j)) != 0 && !_prime[candidates[j]])
                    {
                        foreach (var k in sccs[j])
                        {
                            _prime[k] = true;
                        }
                    }
                i += i & -i;
                i--;
            }
        }

        private void Prepare()
        {
            if (_prepared)
                return;
            Minimalize();
            Divide();
            GetPrimes();
            _prepared = true;
        }
    }
}
