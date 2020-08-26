﻿using System;
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

        var prev = Algorithm.GetPath(G, 0, e => 1);

        Assert.AreEqual(prev[3], 2);
        Assert.AreEqual(prev[2], 1);
        Assert.AreEqual(prev[1], 0);

        prev = Algorithm.GetPath(G, 1, e => 1);

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

        var prev = Algorithm.GetPath(G, 0, e => 1);

        Assert.AreEqual(prev[3], 2);
        Assert.AreEqual(prev[2], 1);
        Assert.AreEqual(prev[1], 0);

        prev = Algorithm.GetPath(G, 1, e => 1);

        Assert.AreEqual(prev[3], 2);
        Assert.AreEqual(prev[2], 1);
        Assert.IsFalse(prev.TryGetValue(0, out int v));
    }
}