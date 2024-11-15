﻿using System.Collections;
using UnityEngine;


public class Sequence : Node
{
    public Sequence(string n)
    {
        name = n;
    }
        
    public override Node.Status Process()
    {
        Status childStatus = children[currentChild].Process();

        if (childStatus == Status.RUNNING) return Status.RUNNING;
        if (childStatus == Status.FALURE) return childStatus;

        currentChild++;

        if(currentChild>= children.Count)
        {
            currentChild = 0;
            return Status.SUCCESS;
        }

        return Status.RUNNING;
    }
}
