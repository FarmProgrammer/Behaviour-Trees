using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Selector : Node
{
    public Selector(string n)
    {
        name = n;
    }

    public override Node.Status Process()
    {
        Status childStatus = children[currentChild].Process();

        if (childStatus == Status.RUNNING) return Status.RUNNING;

        if (childStatus == Status.SUCCESS) 
        {
            currentChild = 0;
            return Status.SUCCESS;
        }

        currentChild++;

        if (currentChild >= children.Count)
        {
            currentChild = 0;
            return Status.FALURE;
        }

        return Status.RUNNING;
    }
}
