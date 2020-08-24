using System;
using System.Linq;
using System.Collections.Generic;

namespace csGraph
{
    public class Graph<Vertex>
    {
        public class Edge
        {
            public readonly Vertex s, t;
            public Dictionary<string, object> traits;

            public Edge(Vertex s, Vertex t)
            {
                this.s = s;
                this.t = t;
                traits = new Dictionary<string, object>();
            }
            public bool IsConnected(Vertex n)
            {
                return n.Equals(s) || n.Equals(t);
            }
            public T GetTraits<T>(string key)
            {
                return (T)traits[key];
            }
            public override bool Equals(object obj)
            {
                var e = obj as Edge;
                if (e == null) return false;
                return e.s.Equals(s) && e.t.Equals(t);
            }
            public override int GetHashCode()
            {
                var shash = ((uint)s.GetHashCode() << 16) | ((uint)s.GetHashCode() >> (31 - 15));
                return (int)shash ^ t.GetHashCode();
            }
            public override string ToString()
            {
                return '(' + s.ToString() + ',' + t.ToString() + ')';
            }
        }
        public Graph(int vertexCapacity, int edgeCapacity)
        {
            vertexes_ = new HashSet<Vertex>(vertexCapacity);
            edges_ = new HashSet<Edge>(edgeCapacity);
        }
        public Graph(IEnumerable<Vertex> vertexes, IEnumerable<Edge> edges)
            : this(vertexes.Count(), edges.Count())
        {
            AddVertexes(vertexes);
            AddEdges(edges);
        }
        public Graph()
        {
            vertexes_ = new HashSet<Vertex>();
            edges_ = new HashSet<Edge>();
        }

        public bool isDirected { get; set; }
        public IEnumerable<Vertex> vertexes { get { return vertexes_; } }
        public IEnumerable<Edge> edges { get { return edges_; } }
        public IReadOnlyDictionary<Vertex, IReadOnlyDictionary<Vertex, bool>> adjacentMatrix {
            get {
                var mat = new Dictionary<Vertex, IReadOnlyDictionary<Vertex, bool>>(vertexes.Count());
                foreach(var s in vertexes)
                {
                    mat.Add(s, new Dictionary<Vertex, bool>(vertexes.Count()));
                    foreach(var t in vertexes)
                    {
                        (mat[s] as Dictionary<Vertex, bool>).Add(t, Exist(new Edge(s, t)));
                        if(!isDirected && !mat[s].ContainsKey(t))
                            (mat[s] as Dictionary<Vertex, bool>).Add(t, Exist(new Edge(s, t)));
                    }
                }
                return mat;
            }
        }

        public bool AddVertex(Vertex v) {
            if (vertexes_.Contains(v)) return false;
            vertexes_.Add(v);
            return true;
        }
        public int AddVertexes(IEnumerable<Vertex> list) {
            int ret = 0;
            foreach (var n in list) ret +=  vertexes_.Add(n) ? 1 : 0;
            return ret;
        }
        public void DeleteVertexes(Vertex v) {
            vertexes_.Remove(v);
            edges_.RemoveWhere(e => v.Equals(e.s) || e.t.Equals(v));
        }
        public bool AddEdge(Edge edge) {
            if (edge.s.Equals(edge.t) ||
                (isDirected && edges_.Contains(edge)) ||
                (!isDirected && (edges_.Contains(edge) || edges_.Contains(new Edge(edge.t, edge.s))))) return false;
            edges_.Add(edge);
            AddVertex(edge.s);
            AddVertex(edge.t);
            return true;
        }
        public bool AddEdge(Vertex s, Vertex t) {
            return AddEdge(new Edge(s, t));
        }
        public int AddEdges(IEnumerable<Edge> list) {
            int ret = 0;
            foreach (var e in list) ret += AddEdge(e) ? 1 : 0;
            return ret;
        }
        public bool DeleteEdge(Edge edge) {
            return edges_.Remove(edge);
        }
        public bool DeleteEdge(Vertex s, Vertex t) {
            bool f = DeleteEdge(new Edge(s, t));
            if (!f && !isDirected) return DeleteEdge(new Edge(t, s));
            return f;
        }


        public bool Exist(Vertex v) {
            return vertexes_.Contains(v);
        }
        public bool Exist(Edge e) {
            return edges_.Contains(e);
        }


        public IEnumerable<Vertex> GetAdjacentVertexes(Vertex v) {
            return from e in edges
                   where e.IsConnected(v)
                   select e.s.Equals(v) ? e.t : e.s;
        }
        public IEnumerable<Edge> GetConnectedEdges(Vertex v) {
            return from e in edges
                   where e.IsConnected(v)
                   select e;
        }
        public int CountDegree(Vertex v) {
            int count = 0;
            foreach(var e in edges)
            {
                count += e.IsConnected(v) ? 1 : 0;
            }
            return count;
        }
        public int CountOutDegree(Vertex v) {
            int count = 0;
            foreach(var e in edges)
            {
                count += e.s.Equals(v) ? 1 : 0;
            }
            return count;
        }
        public int CountInDegree(Vertex v) {
            int count = 0;
            foreach(var e in edges)
            {
                count += e.t.Equals(v) ? 1 : 0;
            }
            return count;
        }

        HashSet<Vertex> vertexes_;
        HashSet<Edge> edges_;
    }
}

