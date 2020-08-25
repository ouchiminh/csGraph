using System;
using System.Collections.Generic;
using System.Linq;

namespace csGraph
{
    public static class Algorithm
    {
        public static IEnumerable<Vertex> GetPath<Vertex>(
            Graph<Vertex> G,
            Vertex s, Vertex t,
            Func<Graph<Vertex>.Edge, dynamic> cost)
        {
            return null;
        }
    }
}
