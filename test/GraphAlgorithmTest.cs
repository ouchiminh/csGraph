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
    public void TestGetPath()
    {
        Graph<int> G = new Graph<int>();
        G.isDirected = false;
        G.AddEdge(0, 1);
        G.AddEdge(1, 2);
        G.AddEdge(2, 3);

        var prev = Algorithm.GetPath(G, 0, x => 1);

        Assert.AreEqual(prev[3], 2);
        Assert.AreEqual(prev[2], 1);
        Assert.AreEqual(prev[1], 0);

        prev = Algorithm.GetPath(G, 1, x => 1);

        Assert.AreEqual(prev[3], 2);
        Assert.AreEqual(prev[2], 1);
        Assert.AreEqual(prev[0], 1);
    }
}
