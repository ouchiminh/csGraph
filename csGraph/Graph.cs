using System;
using System.Linq;
using System.Collections.Generic;

namespace csGraph
{
    public class Graph<Vertex>
    {
        /// <summary>
        /// グラフの辺を表現します。
        /// </summary>
        public class Edge
        {
            public readonly Vertex s, t;
            /// <summary>
            /// 辺の特性を保持します。
            /// </summary>
            public Dictionary<string, object> traits;

            /// <summary>
            /// 辺を構成する二つの頂点を指定し、インスタンスを作成します。
            /// </summary>
            /// <param name="s">始点</param>
            /// <param name="t">終点</param>
            public Edge(Vertex s, Vertex t)
            {
                this.s = s;
                this.t = t;
                traits = new Dictionary<string, object>();
            }
            public Edge(Vertex s, Vertex t, IDictionary<string, object> other)
            {
                this.s = s;
                this.t = t;
                traits = new Dictionary<string, object>(other);
            }
            /// <summary>
            /// 指定された頂点が、この辺の始点か終点のいずれかであるかを返します。
            /// </summary>
            /// <param name="v">頂点</param>
            /// <returns>vが始点か終点ならばtrue。それ以外ならばfalse</returns>
            public bool IsConnectedWith(Vertex v)
            {
                return v.Equals(s) || v.Equals(t);
            }
            /// <summary>
            /// vがこの辺の終点であるかを返します。
            /// </summary>
            /// <param name="v">頂点</param>
            /// <returns>vがこの辺の終点であればtrue。それ以外ならばfalse</returns>
            public bool IsConnectedTo(Vertex v)
            {
                return v.Equals(t);
            }
            /// <summary>
            /// vがこの辺の始点であるかを返します。
            /// </summary>
            /// <param name="v">頂点</param>
            /// <returns>vがこの辺の始点であればtrue。それ以外ならばfalse</returns>
            public bool IsConnectedFrom(Vertex v)
            {
                return v.Equals(s);
            }
            /// <summary>
            /// 辺に指定した特性を取得します。
            /// </summary>
            /// <typeparam name="T">特性の値の型</typeparam>
            /// <param name="key">特性名</param>
            /// <returns>特性の値</returns>
            /// <exception cref="InvalidCastException">
            /// Tが実際の値の型と異なる場合例外が投げられます。
            /// </exception>
            /// <exception cref="KeyNotFoundException">
            /// 指定したキーが存在しない場合例外が投げられます。
            /// </exception>
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
        /// <summary>
        /// グラフの頂点と辺の数をあらかじめ指定し、メモリを確保しておきインスタンスを構築します。
        /// </summary>
        /// <param name="vertexCapacity">頂点の容量</param>
        /// <param name="edgeCapacity">辺の容量</param>
        public Graph(int vertexCapacity, int edgeCapacity)
        {
            vertexes_ = new HashSet<Vertex>(vertexCapacity);
            edges_ = new HashSet<Edge>(edgeCapacity);
            isDirected = false;
        }
        /// <summary>
        /// グラフの頂点と辺のリストを受け取り、グラフを構築します。
        /// </summary>
        /// <param name="vertexes">頂点のリスト</param>
        /// <param name="edges">辺のリスト</param>
        public Graph(IEnumerable<Vertex> vertexes, IEnumerable<Edge> edges)
            : this(vertexes.Count(), edges.Count())
        {
            AddVertexes(vertexes);
            AddEdges(edges);
        }
        /// <summary>
        /// 頂点と辺の容量を0で初期化します。
        /// </summary>
        public Graph()
            : this(0, 0)
        { }

        public bool isDirected;
        public ICollection<Vertex> vertexes { get { return vertexes_; } }
        public ICollection<Edge> edges { get { return edges_; } }
        /// <summary>
        /// 隣接行列
        /// </summary>
        public IReadOnlyDictionary<Vertex, IReadOnlyDictionary<Vertex, bool>> adjacentMatrix {
            get {
                var mat = new Dictionary<Vertex, IReadOnlyDictionary<Vertex, bool>>(vertexes.Count());
                foreach(var s in vertexes)
                {
                    mat.Add(s, new Dictionary<Vertex, bool>(vertexes.Count()));
                    foreach(var t in vertexes)
                    {
                        (mat[s] as Dictionary<Vertex, bool>).Add(t,
                            isDirected ? Exist(new Edge(s, t)) : Exist(new Edge(s, t)) || Exist(new Edge(t, s)));
                    }
                }
                return mat;
            }
        }

        /// <summary>
        /// 頂点を追加します。
        /// </summary>
        /// <param name="v">新しい頂点</param>
        /// <returns>追加に成功した場合true、それ以外はfalse</returns>
        public bool AddVertex(Vertex v) {
            if (vertexes_.Contains(v)) return false;
            vertexes_.Add(v);
            return true;
        }
        /// <summary>
        /// 頂点リストの全ての頂点を追加します。
        /// </summary>
        /// <param name="list">頂点リスト</param>
        /// <returns>追加に成功した数</returns>
        public int AddVertexes(IEnumerable<Vertex> list) {
            int ret = 0;
            foreach (var n in list) ret +=  vertexes_.Add(n) ? 1 : 0;
            return ret;
        }
        /// <summary>
        /// 頂点を削除します。
        /// </summary>
        /// <param name="v">削除する頂点</param>
        public void DeleteVertexes(Vertex v) {
            vertexes_.Remove(v);
            edges_.RemoveWhere(e => v.Equals(e.s) || e.t.Equals(v));
        }
        /// <summary>
        /// 辺を追加します。
        /// </summary>
        /// <param name="edge">追加する辺</param>
        /// <returns>追加に成功した場合true、それ以外はfalse</returns>
        public bool AddEdge(Edge edge) {
            if (edge.s.Equals(edge.t) ||
                (isDirected && edges_.Contains(edge)) ||
                (!isDirected && (edges_.Contains(edge) || edges_.Contains(new Edge(edge.t, edge.s))))) return false;
            edges_.Add(edge);
            AddVertex(edge.s);
            AddVertex(edge.t);
            return true;
        }
        /// <summary>
        /// 辺を追加します。
        /// </summary>
        /// <param name="s">辺の始点</param>
        /// <param name="t">辺の終点</param>
        /// <returns>追加に成功した場合true、それ以外はfalse</returns>
        public bool AddEdge(Vertex s, Vertex t) {
            return AddEdge(new Edge(s, t));
        }
        /// <summary>
        /// 辺リストの全ての辺を追加します。
        /// </summary>
        /// <param name="list">辺リスト</param>
        /// <returns>追加に成功した数</returns>
        public int AddEdges(IEnumerable<Edge> list) {
            int ret = 0;
            foreach (var e in list) ret += AddEdge(e) ? 1 : 0;
            return ret;
        }
        /// <summary>
        /// 辺を削除します。
        /// </summary>
        /// <param name="edge">削除したい辺</param>
        /// <returns>削除に成功した場合true、それ以外はfalse</returns>
        public bool DeleteEdge(Edge edge) {
            return edges_.Remove(edge);
        }
        /// <summary>
        /// 辺を削除します。
        /// </summary>
        /// <param name="s">辺の始点</param>
        /// <param name="t">辺の終点</param>
        /// <returns>削除に成功した場合true、それ以外はfalse</returns>
        public bool DeleteEdge(Vertex s, Vertex t) {
            bool f = DeleteEdge(new Edge(s, t));
            if (!f && !isDirected) return DeleteEdge(new Edge(t, s));
            return f;
        }


        /// <summary>
        /// 頂点の存在を確認します。
        /// </summary>
        /// <param name="v">頂点</param>
        /// <returns>グラフ中に存在していればtrue、それ以外はfalse</returns>
        public bool Exist(Vertex v) {
            return vertexes_.Contains(v);
        }
        public bool Exist(Vertex v, out Vertex o) {
            return vertexes_.TryGetValue(v, out o);
        }
        /// <summary>
        /// 辺の存在を確認します。
        /// </summary>
        /// <param name="e">辺</param>
        /// <returns>グラフ中に存在していればtrue、それ以外はfalse</returns>
        public bool Exist(Edge e) {
            return edges_.Contains(e);
        }
        public bool Exist(Edge e, out Edge o) {
            return edges_.TryGetValue(e, out o);
        }


        /// <summary>
        /// vに隣接する全ての頂点を列挙します。
        /// </summary>
        /// <param name="v">頂点</param>
        /// <returns>vに隣接する頂点</returns>
        /// <remarks>
        /// 有向グラフの場合、vから出る辺が向かう全ての頂点を列挙します。
        /// 無向グラフの場合、vと辺でつながっているすべての頂点を列挙します。
        /// </remarks>
        public IEnumerable<Vertex> GetAdjacentVertexes(Vertex v) {
            return from e in edges
                   where isDirected ? e.IsConnectedFrom(v) : e.IsConnectedWith(v)
                   select e.s.Equals(v) ? e.t : e.s;
        }
        /// <summary>
        /// vから出る辺を全て列挙します。
        /// </summary>
        /// <param name="v">頂点</param>
        /// <returns>vから出るすべての辺</returns>
        /// <remarks>
        /// <seealso cref="GetAdjacentVertexes(Vertex)"/>とおなじルールで辺を列挙します。
        /// </remarks>
        public IEnumerable<Edge> GetEdgesFrom(Vertex v) {
            return from e in edges
                   where isDirected ? e.IsConnectedFrom(v) : e.IsConnectedWith(v)
                   select e;
        }
        /// <summary>
        /// vの次数
        /// </summary>
        public int CountDegree(Vertex v) {
            int count = 0;
            foreach(var e in edges)
            {
                count += e.IsConnectedWith(v) ? 1 : 0;
            }
            return count;
        }
        /// <summary>
        /// vの出次数
        /// </summary>
        public int CountOutDegree(Vertex v) {
            int count = 0;
            foreach(var e in edges)
            {
                count += e.s.Equals(v) ? 1 : 0;
            }
            return count;
        }
        /// <summary>
        /// vの入次数
        /// </summary>
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

