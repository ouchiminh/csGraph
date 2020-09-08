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
        /// <summary>
        /// sから到達可能な全ての頂点への最短経路を返します。
        /// </summary>
        /// <typeparam name="Vertex">頂点の型</typeparam>
        /// <param name="G">グラフ</param>
        /// <param name="s">始点</param>
        /// <param name="cost">辺のコストを返す述語</param>
        /// <returns>
        /// <para>Item1 : 頂点をキーとした頂点の辞書です。sからキーの頂点へ向かう最短経路中でキーの頂点の直前に通る頂点が値です。</para>
        /// <para>Item2 : 頂点をキーとした距離の辞書です。</para>
        /// </returns>
        /// <example>
        /// グラフG上でsからtへの最短経路を求めます。なお、辺のコストは全て1とします。
        /// <code>
        /// var res = Algorithm.GetPath(G, s, e=>1);
        /// var s_tPath = new LinkedList&lt;Vertex&gt;(); // stパス
        /// var prev = t;
        /// s_tPath.AddLast(t);
        /// while(res.Item1.TryGetValue(prev, out prev)) s_tPath.AddFirst(prev);
        /// </code>
        /// </example>
        public static Tuple<Dictionary<Vertex, Vertex>, Dictionary<Vertex, dynamic>> GetPath<Vertex>(
            Graph<Vertex> G,
            Vertex s,
            Func<Graph<Vertex>.Edge, dynamic> cost)
        {
            PriorityQueue<(dynamic d, Vertex v)>
                q = new PriorityQueue<(dynamic d, Vertex v)>((a, b) => a.d < b.d ? -1 : a.d > b.d ? 1 : 0);
            Dictionary<Vertex, dynamic> d = new Dictionary<Vertex, dynamic>(G.vertexes.Count());
            Dictionary<Vertex, Vertex> prev = new Dictionary<Vertex, Vertex>(G.vertexes.Count());

            foreach (var v in G.vertexes)
            {
                var distance = v.Equals(s) ? 0 : double.PositiveInfinity;
                d.Add(v, distance);
            }
            q.Enqueue((0, s));
            while (!q.Empty)
            {
                var u = q.Dequeue();
                foreach (var v in G.GetAdjacentVertexes(u.v))
                {
                    G.Exist(new Graph<Vertex>.Edge(u.v, v), out Graph<Vertex>.Edge e);
                    var alt = d[u.v] + cost(e);
                    if (d[v] > alt)
                    {
                        d[v] = alt;
                        if (!prev.ContainsKey(v)) prev.Add(v, u.v);
                        else prev[v] = u.v;
                        q.Enqueue((alt, v));
                    }
                }
            }

            return new Tuple<Dictionary<Vertex, Vertex>, Dictionary<Vertex, dynamic>>(prev, d);
        }

        public static Tuple<Dictionary<Vertex, Vertex>, Dictionary<Vertex, Cost>> GetPath<Cost, Vertex>(
            Graph<Vertex> G,
            Vertex s,
            string costKey)
        {
            var res = GetPath(G, s, e => e.GetTraits<Cost>(costKey));
            var d = new Dictionary<Vertex, Cost>(res.Item2.Count);
            foreach(var v in res.Item2)
            {
                d.Add(v.Key, (Cost)v.Value);
            }
            return new Tuple<Dictionary<Vertex, Vertex>, Dictionary<Vertex, Cost>>(res.Item1, d);

        }
    }
}
