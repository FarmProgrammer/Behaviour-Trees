using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

public class Node
{
    public enum Status { SUCCESS, RUNNING, FALURE };
    public Status status;
    public List<Node> children = new List<Node>();
    public int currentChild = 0;
    public string name;

    public Node() { }

    public Node(string n)
    {
        name = n;
    }

    public void AddChild(Node n)
    {
        children.Add(n);
    }

    public virtual Status Process()
    {
        return children[currentChild].Process();
    }

    public string PrintChildren()
    {
        string childrenString = "";
        childrenString += name + "{";
        foreach (Node node in children)
        {
            if (node.children.Any())
            {
                childrenString += node.PrintChildren();
            }
            else
            {
                childrenString += node.name;
            }
            childrenString += " ";
        }
        childrenString += "}";
        return childrenString;
    }
}
