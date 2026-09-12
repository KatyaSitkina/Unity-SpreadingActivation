using System.Collections.Generic;
using UnityEngine;
using XNode;

public enum NodeType { Room, Action }

public class Edge
{
    public string endNode;
    public NodeType type;
    public float edgeWeight;
}

public class StoryNode : XNode.Node
{
    [Input(ShowBackingValue.Never)] public float input;
    public string nodeName;
    public NodeType type;
    [Output(dynamicPortList = true)] public List<float> connectedEdges;

    public override object GetValue(NodePort port) {
		return null;
	}
}