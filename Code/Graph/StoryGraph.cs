using System.Collections.Generic;
using UnityEngine;
using XNode;

[CreateAssetMenu(menuName = "StoryGraph")]
public class StoryGraph : NodeGraph { 
	public Dictionary<string, List<Edge>> ConvertGraphToDict(StoryGraph graph)
	{
		var graphDict = new Dictionary<string, List<Edge>>();
		foreach(var node in graph.nodes)
		{
			StoryNode newNode = node as StoryNode;	// приводим к типу узла
			var edges = new List<Edge>();
			for(int i = 0; i < newNode.connectedEdges.Count; i++)	// цикл по всем рёбрам текущего узла
			{
				NodePort port = newNode.GetOutputPort("connectedEdges " + i).Connection;
				StoryNode neighbour = port.node as StoryNode;
                var edge = new Edge
                {
                    endNode = neighbour.nodeName,
                    type = neighbour.type,
                    edgeWeight = newNode.connectedEdges[i]
                };
                edges.Add(edge);
            }
			graphDict.Add(newNode.nodeName, edges);
		}
		return graphDict;
	}
}