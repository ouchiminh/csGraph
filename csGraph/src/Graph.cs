using System;
using System.Linq;
using System.Collections.Generic;

namespace csGraph
{
    public class Graph<Node>
    {
        public struct Edge
        {

            public readonly Node s, t;

            public Edge(Node s, Node t)
            {
                this.s = s;
                this.t = t;
            }
            public bool isConnected(Node n)
            {
                return n.Equals(s) || n.Equals(t);
            }
        }
        public bool isDirected { get; set; }
        public IEnumerable<Node> nodes { get { return nodes_; } }
        public IEnumerable<Edge> edges { get { return edges_; } }

        public bool AddNode(in Node n) {
            if (nodes_.Contains(n)) return false;
            nodes_.Add(n);
            return true;
        }
        public int AddNodes(IEnumerable<Node> list) {
            int ret = 0;
            foreach (var n in list) ret +=  nodes_.Add(n) ? 1 : 0;
            return ret;
        }
        public void DeleteNode(in Node n) {
            nodes_.Remove(n);

        }
        public bool AddEdge(in Edge edge) {
            if (edge.s.Equals(edge.t) ||
                (isDirected && edges_.Contains(edge)) ||
                (!isDirected && (edges_.Contains(edge) || edges_.Contains(new Edge(edge.t, edge.s))))) return false;
            edges_.Add(edge);
            return true;
        }
        public bool AddEdge(in Node s, in Node t) {
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
        public bool DeleteEdge(Node s, Node t) {
            bool f = DeleteEdge(new Edge(s, t));
            if (!f && !isDirected) return DeleteEdge(new Edge(t, s));
            return f;
        }


        public bool Exist(Node n) {
            return nodes_.Contains(n);
        }
        public bool Exist(Edge e) {
            return edges_.Contains(e);
        }


        public IEnumerable<Node> GetConnectedNodes(Node n) {
            return from e in edges
                   where e.isConnected(n)
                   select e.s.Equals(n) ? e.t : e.s;
        }
        public IEnumerable<Edge> GetConnectedEdges(Node n) {
            return from e in edges
                   where e.isConnected(n)
                   select e;
        }
        public int CountDegree(Node n) {
            int count = 0;
            foreach(var e in edges)
            {
                count += e.isConnected(n) ? 1 : 0;
            }
            return count;
        }
        public int CountOutDegree(Node n) {
            int count = 0;
            foreach(var e in edges)
            {
                count += e.s.Equals(n) ? 1 : 0;
            }
            return count;
        }
        public int CountInDegree(Node n) {
            int count = 0;
            foreach(var e in edges)
            {
                count += e.t.Equals(n) ? 1 : 0;
            }
            return count;
        }

        HashSet<Node> nodes_ = new HashSet<Node>();
        HashSet<Edge> edges_ = new HashSet<Edge>();
    }
}

