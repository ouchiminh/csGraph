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

    }
}
