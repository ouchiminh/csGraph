using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.CompilerServices;
using System.Security.Cryptography.X509Certificates;
using PriorityQueue;

namespace csGraph
{
    public static class Algorithm
    {
        public static Dictionary<Vertex, Vertex> GetPath<Vertex>(
            Graph<Vertex> G,
            Vertex s,
            Func<Graph<Vertex>.Edge, dynamic> cost)
        {
            PriorityQueue<(dynamic d, Vertex v)>
                q = new PriorityQueue<(dynamic d, Vertex v)>((a,  b)=>a.d < b.d ? -1 : a.d > b.d ? 1 : 0);
            Dictionary<Vertex, dynamic> d = new Dictionary<Vertex, dynamic>(G.vertexes.Count());
            Dictionary<Vertex, Vertex> prev = new Dictionary<Vertex, Vertex>(G.vertexes.Count());

            foreach(var v in G.vertexes)
            {
                var distance = v.Equals(s) ? 0 : double.MaxValue;
                d.Add(v, distance);
                q.Enqueue((distance, v));
            }
            while (!q.Empty)
            {
                var u = q.Dequeue();
                foreach(var v in G.GetAdjacentVertexes(u.v))
                {
                    var alt = d[u.v] + cost(new Graph<Vertex>.Edge(u.v, v));
                    if(d[v] > alt)
                    {
                        d[v] = alt;
                        if (!prev.ContainsKey(v)) prev.Add(v, u.v);
                        else prev[v] = u.v;
                        q.Enqueue((alt, v));
                    }
                }
            }

            return prev;
        }
    }
}
