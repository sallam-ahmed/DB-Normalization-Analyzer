using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DBNormalizationAnalyzer_AnalyzerLibrary
{
    internal class Graph
    {
        private BitArray _primeScc;
        private List< List<int> >  _adjacencyList;
        private BitArray _visited;
        private List<List<int>> _directedAdjacencyList;
        private List<BitArray> _reachability; 
        private List<int> _scc;
        private Dictionary<List<int>,int> _indexes;

        public Graph(int n)
        {
            _indexes = new Dictionary<List<int>, int>(n);
            _adjacencyList = new List<List<int>>(n);
            _scc = new List<int>();
            _reachability = new List<BitArray>();
            for (var i = 0; i < n; i++)
            {
                _indexes[new List<int> {i}] = i;
                _adjacencyList.Add(new List<int>());
            }
        }

        private bool IsSubset(List<int> a, List<int> b)
        {
            return b.TrueForAll(a.Contains);
        }

        public void AddEdge(List<int> from,List<int> to)
        {
            var indexFrom = GetIndex(from);
            var indexto = GetIndex(to);
            if (indexFrom == -1)
            {
                indexFrom = _indexes.Count;
                _indexes[from] = indexFrom;
                _adjacencyList.Add(new List<int>());
            }
            if (indexto == -1)
            {
                indexto = _indexes.Count;
                _indexes[to] = indexto;
                _adjacencyList.Add(new List<int>());
            }
            if (!_adjacencyList[indexFrom].Contains(indexto))
            {
                _adjacencyList[indexFrom].Add(indexto);
                _scc.Clear();
                _reachability.Clear();
            }
        }

        public int GetIndex(List<int> keys)
        {
            if (!_indexes.ContainsKey(keys))
                return -1;
            return _indexes[keys];
        }

        #region Scc
        public int GetScc(List<int> keys)
        {
            var index = GetIndex(keys);
            if (index == -1) throw new ArgumentException();
            if (_scc.Count == 0)
                GetSccs();
            return _scc[GetIndex(keys)];
        }

        private Stack<int> _s, _p;
        private List<int> _preoder;
        private int _d,_current;

        private void GetSccs()
        {
            _s = new Stack<int>();
            _p = new Stack<int>();
            _preoder = new List<int>(_adjacencyList.Count);
            _scc = new List<int>(_adjacencyList.Count);
            _visited = new BitArray(_directedAdjacencyList.Count);
            for (var i = 0; i < _adjacencyList.Count; i++)
            {
                _preoder.Add(0);
                _scc.Add(-1);
            }
            _d = 1;
            _current = 0;
            for (var i = 0; i < _adjacencyList.Count; i++)
            {
                if(_preoder[i] == 0)
                    Dfs(i);
            }
        }

        private void Dfs(int v)
        {
            _preoder[v] = _d++;
            _s.Push(v);
            _p.Push(v);
            foreach (var u in _adjacencyList[v])
            {
                if (_preoder[u] == 0)
                    Dfs(u);
                else if(_scc[u] == -1)
                {
                    while (_p.Count != 0 && _preoder[_p.Peek()] > _preoder[u])
                        _p.Pop();
                }
            }
            if (_p.Peek() == v)
            {
                while (_s.Peek() != v)
                {
                    _scc[_s.Pop()] = _current;
                }
                if (_s.Peek() == v)
                    _scc[_s.Pop()] = _current;
                _p.Pop();
                _current++;
            }
        }

        #endregion

        #region reachability

        private void Prepare()
        {
            if (_scc.Count == 0)
                GetSccs();
            if (_reachability.Count == 0)
                ConstructReachability();
        }
        public BitArray GetReachability(List<int> keys)
        {
            var index = GetIndex(keys);
            if (index == -1) throw new ArgumentException();
            Prepare();
            var result = new BitArray(_directedAdjacencyList.Count);
            foreach (var set in _indexes.Where(set => IsSubset(keys,set.Key)))
            {
                result.Or(_reachability[GetScc(set.Key)]);
            }
            return result;
        }

        public bool IsPrime(int key)
        {
            Prepare();
            return _primeScc[GetScc(new List<int> {key})];
        }
        private List<int> _topoSort;
        private void ConstructReachability()
        {
            if (_adjacencyList.Count == 0)
                return;
            GetDag();
            _topoSort = new List<int>();
            _visited = new BitArray(_directedAdjacencyList.Count);
            _reachability = new List<BitArray>(_directedAdjacencyList.Count);
            foreach (var t in _directedAdjacencyList)
            {
                _reachability.Add(new BitArray(_directedAdjacencyList.Count));
            }
            for (var i = 0 ;i < _directedAdjacencyList.Count;i++)
                if(!_visited[i])
                    TopologicalSort(i);
            foreach (var u in _topoSort)
            {
                _primeScc[u] = true;
                _reachability[u][u] = true;
                foreach (var v in _directedAdjacencyList[u])
                {
                    _primeScc[v] = false;
                    _reachability[u].Or(_reachability[v]);
                }
            }
        }
        private void GetDag()
        {
            var len = _scc.Max() + 1;
            _directedAdjacencyList = new List<List<int>>(len);
            for (int i = 0; i < len; i++)
            {
                _directedAdjacencyList.Add(new List<int>());
            }
            for(var v = 0;v < _adjacencyList.Count;v++)
            {
                foreach (var u in _adjacencyList[v].Where(u => _scc[u] != _scc[v] && !_directedAdjacencyList[_scc[v]].Contains(_scc[u])))
                {
                    _directedAdjacencyList[_scc[v]].Add(_scc[u]);
                }
            }
        }

        private void TopologicalSort(int v)
        {
            foreach (var u in _directedAdjacencyList[v].Where(u => !_visited[u]))
            {
                TopologicalSort(u);
            }
            _topoSort.Add(v);
        }
        #endregion
    }
}
