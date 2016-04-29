using System.Collections.Generic;
using System.Linq;

namespace DBNormalizationAnalyzer_AnalyzerLibrary
{
    internal class Graph
    {
        public List<int> Scc;
        private readonly List<List<int>> _adjacencyList;
        private Stack<int> _s, _p;
        private List<int> _preoder;
        private int _d, _current;

        public Graph(int n)
        {
            _adjacencyList = new List<List<int>>(n);
            _adjacencyList.AddRange(Enumerable.Repeat(new List<int>(), n));
        }

        public void AddEdge(int from, int to)
        {
            if(from > _adjacencyList.Count)
                return;
            if (to > _adjacencyList.Count)
                return;
            if (!_adjacencyList[from].Contains(to))
                _adjacencyList[from].Add(to);
        }

        public void GetSccs()
        {
            _s = new Stack<int>();
            _p = new Stack<int>();
            _preoder = new List<int>(_adjacencyList.Count);
            Scc = new List<int>(_adjacencyList.Count);
            _preoder.AddRange(Enumerable.Repeat(0,_adjacencyList.Count));
            Scc.AddRange(Enumerable.Repeat(-1, _adjacencyList.Count));
            _d = 1;
            _current = 0;
            for (var i = 0; i < _adjacencyList.Count; i++)
            {
                if (_preoder[i] == 0)
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
                else if (Scc[u] == -1)
                {
                    while (_p.Count != 0 && _preoder[_p.Peek()] > _preoder[u])
                        _p.Pop();
                }
            }
            if (_p.Peek() != v)
                return;
            while (_s.Peek() != v)
            {
                Scc[_s.Pop()] = _current;
            }
            if (_s.Peek() == v)
                Scc[_s.Pop()] = _current;
            _p.Pop();
            _current++;
        }
    }
}
