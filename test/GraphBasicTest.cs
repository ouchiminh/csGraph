using System;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using csGraph;
using System.Collections.Generic;
using System.Linq;

[TestClass]
public class GraphBasicTest
{
    IEnumerable<int> GetNodes()
    {
        for (int i = 1; i < 10; ++i) yield return i;
        yield break;
    }
    [TestMethod]
    public void TestNode()
    {
        Graph<int> G = new Graph<int>();
        G.AddVertex(0);
        G.AddVertexes(GetNodes());
        Assert.AreEqual(10, G.vertexes.Count());
    }
    [TestMethod]
    public void TestEdgeUndirected()
    {
        Graph<int> G = new Graph<int>();
        G.isDirected = false;
        Assert.IsTrue(G.AddEdge(1, 2));
        Assert.IsFalse(G.AddEdge(0, 0));
        Assert.IsFalse(G.AddEdge(2, 1));
        Assert.AreEqual(1, G.edges.Count());
        Assert.AreEqual(2, G.vertexes.Count());
    }
    [TestMethod]
    public void TestEdgeDirected()
    {
        Graph<int> G = new Graph<int>();
        G.isDirected = true;
        Assert.IsTrue(G.AddEdge(1, 2));
        Assert.IsFalse(G.AddEdge(0, 0));
        Assert.IsTrue(G.AddEdge(2, 1));
        Assert.AreEqual(2, G.edges.Count());
        Assert.AreEqual(2, G.vertexes.Count());
    }
    [TestMethod]
    public void TestDegree()
    {
        Graph<int> G = new Graph<int>();
        G.isDirected = true;
        G.AddEdge(1, 2);
        G.AddEdge(0, 0);
        G.AddEdge(2, 1);

        Assert.AreEqual(2, G.CountDegree(1));
        Assert.AreEqual(1, G.CountInDegree(1));
        Assert.AreEqual(1, G.CountOutDegree(1));
        Assert.AreEqual(2, G.CountDegree(2));
        Assert.AreEqual(1, G.CountInDegree(2));
        Assert.AreEqual(1, G.CountOutDegree(2));
    }
    [TestMethod]
    public void TestNodeDelete()
    {
        Graph<int> G = new Graph<int>();
        G.isDirected = true;
        G.AddEdge(1, 2);
        G.AddEdge(0, 0);
        G.AddEdge(2, 1);

        G.DeleteVertexes(1);

        Assert.AreEqual<int>(1, G.vertexes.Count());
        Assert.AreEqual<int>(0, G.edges.Count());
    }
}
