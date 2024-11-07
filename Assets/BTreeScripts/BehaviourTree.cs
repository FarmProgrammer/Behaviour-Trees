using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

public class BehaviourTree : Node
{
    public BehaviourTree()
    {
        name = "Tree";

    }

    public BehaviourTree(string n)
    {
        name = n;
    }

    public void PrintTree()
    {
        Debug.Log(name + "{");
        foreach (Node node in children)
        {
            if (node.children.Any())
            {
                Debug.Log(node.PrintChildren());
            }
            else
            {
                Debug.Log(node.name);
            }
        }
        Debug.Log("}");
    }
}
