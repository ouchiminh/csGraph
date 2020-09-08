using System;
using System.Text;
using System.Collections.Generic;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using csGraph;

/// <summary>
/// UnitTest1 の概要の説明
/// </summary>
[TestClass]
public class GraphAlgorithmTest
{
    [TestMethod]
    public void TestGetPathUndirected()
    {
        Graph<int> G = new Graph<int>();
        G.isDirected = false;
        G.AddEdge(0, 1);
        G.AddEdge(1, 2);
        G.AddEdge(2, 3);

        var prev = Algorithm.GetPath(G, 0, e => 1).Item1;

        Assert.AreEqual(prev[3], 2);
        Assert.AreEqual(prev[2], 1);
        Assert.AreEqual(prev[1], 0);

        prev = Algorithm.GetPath(G, 1, e => 1).Item1;

        Assert.AreEqual(prev[3], 2);
        Assert.AreEqual(prev[2], 1);
        Assert.AreEqual(prev[0], 1);
    }
    [TestMethod]
    public void TestGetPathDirected()
    {
        Graph<int> G = new Graph<int>();
        G.isDirected = true;
        G.AddEdge(0, 1);
        G.AddEdge(1, 2);
        G.AddEdge(2, 3);

        var prev = Algorithm.GetPath(G, 0, e => 1).Item1;

        Assert.AreEqual(prev[3], 2);
        Assert.AreEqual(prev[2], 1);
        Assert.AreEqual(prev[1], 0);

        prev = Algorithm.GetPath(G, 1, e => 1).Item1;

        Assert.AreEqual(prev[3], 2);
        Assert.AreEqual(prev[2], 1);
        Assert.IsFalse(prev.TryGetValue(0, out int v));
    }
    [TestMethod]
    public void TestGetPathCostKey()
    {
        Graph<int> G = new Graph<int>();
        var cost = new Dictionary<string, object>();
        cost.Add("cost", 1);
        G.isDirected = true;

        G.AddEdge(new Graph<int>.Edge(0, 1, cost));
        G.AddEdge(new Graph<int>.Edge(1, 2, cost));
        G.AddEdge(new Graph<int>.Edge(2, 3, cost));


        var prev = Algorithm.GetPath<int, int>(G, 0, "cost").Item1;

        Assert.AreEqual(prev[3], 2);
        Assert.AreEqual(prev[2], 1);
        Assert.AreEqual(prev[1], 0);
    }

    [TestMethod]
    public void TestDocCode()
    {
        var G = new Graph<char>();
        var edgeTraits = new Dictionary<string, object>();
        edgeTraits.Add("cost", 1);
        G.AddEdge(new Graph<char>.Edge('s', 'A', edgeTraits));
        G.AddEdge(new Graph<char>.Edge('B', 'E', edgeTraits));

        edgeTraits["cost"] = 2;
        G.AddEdge(new Graph<char>.Edge('A', 'B', edgeTraits));
        G.AddEdge(new Graph<char>.Edge('s', 'C', edgeTraits));

        edgeTraits["cost"] = 3;
        G.AddEdge(new Graph<char>.Edge('D', 't', edgeTraits));

        edgeTraits["cost"] = 4;
        G.AddEdge(new Graph<char>.Edge('D', 'E', edgeTraits));

        edgeTraits["cost"] = 5;
        G.AddEdge(new Graph<char>.Edge('s', 'B', edgeTraits));

        edgeTraits["cost"] = 6;
        G.AddEdge(new Graph<char>.Edge('E', 't', edgeTraits));

        edgeTraits["cost"] = 7;
        G.AddEdge(new Graph<char>.Edge('E', 'C', edgeTraits));

        edgeTraits["cost"] = 9;
        G.AddEdge(new Graph<char>.Edge('B', 'D', edgeTraits));

        //(var prev, var dist) = Algorithm.GetPath<int, char>(G, 's', "cost");
        (var prev, var dist) = Algorithm.GetPath(G, 's', e => e.GetTraits<int>("cost"));

        var stDist = (int)dist['t'];
        var stPath = new LinkedList<char>();
        var cur = 't';
        stPath.AddFirst(cur);
        while (prev.TryGetValue(cur, out cur)) stPath.AddFirst(cur);

        Assert.AreEqual(stDist, 10);

    }
}
