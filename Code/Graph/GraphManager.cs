using System.Collections.Generic;
using UnityEngine;

public class GraphManager : MonoBehaviour
{
    public static GraphManager Instance { get; private set; }

    [SerializeField] StoryGraph graph;
    Dictionary<string, List<Edge>> graphDict;   // граф как словарь
    Dictionary<string, float> nodesActivationValue = new Dictionary<string, float>();   // уровни активации узлов
    [HideInInspector] public List<string> nodesToActivate = new List<string>(); // список узлов для активации
    [HideInInspector] public string currentRoom = "RoomBeginning";  // текущая локация, в которой находится игрок
    
    float decayFactor = 0.4f; // фактор затухания
    float threshold = 0.0001f; // пороговое значение

    void Awake()
    {
        if (Instance != null && Instance != this)
            Destroy(this);
        else
        {
            Instance = this;
            DontDestroyOnLoad(gameObject);
        }

        graphDict = graph.ConvertGraphToDict(graph);
        foreach (var node in graphDict)   // устанавливаем начальные значения уровней активации узлов
            nodesActivationValue.Add(node.Key, 0);
    }

    // сброс динамических данных графа
    public void ResetGraph()
    {
        currentRoom = "RoomBeginning";
        foreach (var node in graphDict)
            nodesActivationValue[node.Key] = 0;
        nodesToActivate.Clear();
    }

    // активация узлов из списка узлов на активацию
    public void ActivateNodesList()
    {
        foreach (var node in nodesToActivate)
            ActivateNode(node);
        nodesToActivate.Clear();
    }

    // активация конкретного узла
    void ActivateNode(string activatedNode)
    {
        Queue<string> nodesToVisit = new Queue<string>();
        nodesActivationValue[activatedNode] = 1.0f;
        nodesToVisit.Enqueue(activatedNode);

        while (nodesToVisit.Count != 0)
        {
            string currentNode = nodesToVisit.Dequeue();
            if (nodesActivationValue[currentNode] < threshold)
                continue;
            if (graphDict.TryGetValue(currentNode, out var neighbours))
                foreach (var edge in neighbours)
                {
                    // обновляем уровень активации (передаём активацию от текущей вершины к следующей)
                    // активация следующей вершины += активация текущей вершины * вес ребра к следующей вершине * коэф.затухания
                    nodesActivationValue[edge.endNode] = Mathf.Clamp01(nodesActivationValue[edge.endNode] + nodesActivationValue[currentNode] * edge.edgeWeight * decayFactor);
                    if (!nodesToVisit.Contains(edge.endNode))
                        nodesToVisit.Enqueue(edge.endNode);
                }
        }
    }

    // выбор следующей комнаты из множества
    public void NextRoomSearch()
    {
        var candidates = NextRoomCandidates();
        float maxActivationValue = -1.0f;

        foreach (var candidate in candidates)
            if (nodesActivationValue[candidate] > maxActivationValue)
            {
                currentRoom = candidate;
                maxActivationValue = nodesActivationValue[candidate];
            }
    }

    // поиск потенциальных следующих комнат
    HashSet<string> NextRoomCandidates()
    {
        HashSet<string> candidates = new HashSet<string>();
        Queue<string> nodesToVisit = new Queue<string> ();
        HashSet<string> visited = new HashSet<string>();

        nodesToVisit.Enqueue(currentRoom);

        while(nodesToVisit.Count != 0)
        {
            var node = nodesToVisit.Dequeue();
            visited.Add(node);
            foreach(var edge in graphDict[node])
                if (edge.type == NodeType.Room)
                    candidates.Add(edge.endNode);
                else if (!visited.Contains(edge.endNode))
                    nodesToVisit.Enqueue(edge.endNode);
        }
        return candidates;
    }
}
