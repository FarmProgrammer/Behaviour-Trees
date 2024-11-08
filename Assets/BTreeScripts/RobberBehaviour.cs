using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

public class RobberBehaviour : MonoBehaviour
{
    BehaviourTree tree;
    NavMeshAgent agent;
    public GameObject diamond;
    public GameObject van;
    public GameObject backDoor;
    public GameObject frontDoor;

    Node.Status treeStatus = Node.Status.RUNNING;

    public enum ActionState { IDLE, WORKING };

    ActionState state = ActionState.IDLE;

    [Range(0, 1000)]
    public int money = 800;

    private void Start()
    {
        agent = GetComponent<NavMeshAgent>();

        tree = new BehaviourTree();
        Sequence steal = new Sequence("Steal Something.");
        Leaf hasGotMoney = new Leaf("Has Got Money.", HasMoney);
        Leaf goToDiamond = new Leaf("Go To Diamond.", GoToDiamond);
        Leaf goToBackDoor = new Leaf("Go To Back Door.", GoToBackDoor);
        Leaf goToVan = new Leaf("Go To Van.", GoToVan);
        Leaf goToFrontDoor = new Leaf("Go To Front Door.", GoToFrontDoor);
        Selector openDoor = new Selector("Open Door.");

        openDoor.AddChild(goToBackDoor);
        openDoor.AddChild(goToFrontDoor);

        steal.AddChild(hasGotMoney);
        steal.AddChild(openDoor);
        steal.AddChild(goToDiamond);
        steal.AddChild(goToVan);

        tree.AddChild(steal);

        tree.PrintTree();

    }

    private Node.Status GoToFrontDoor()
    {
        return GoToDoor(frontDoor);
    }

    private void Update()
    {
        if(treeStatus!=Node.Status.SUCCESS) treeStatus = tree.Process();
    }

    Node.Status GoToLocation(Vector3 destination)
    {

        float distanceToTarget = Vector3.Distance(destination, transform.position);

        if (state == ActionState.IDLE)
        {
            agent.SetDestination(destination);
            state = ActionState.WORKING;
        }
        else if(Vector3.Distance(agent.pathEndPosition, destination) >= 2)
        {
            state = ActionState.IDLE;
            return Node.Status.FALURE;
        }
        else if (distanceToTarget < 2)
        {
            state = ActionState.IDLE;
            return Node.Status.SUCCESS;
        }
        return Node.Status.RUNNING;
    }

    public Node.Status GoToDiamond()
    {
        Node.Status s = GoToLocation(diamond.transform.position);

        if (s == Node.Status.SUCCESS)
        {
            diamond.transform.parent = transform;
        }

        return GoToLocation(diamond.transform.position);
    }

    public Node.Status GoToDoor(GameObject door)
    {
        Node.Status s = GoToLocation(diamond.transform.position);

        if(s == Node.Status.SUCCESS)
        {
            if (!door.GetComponent<Lock>().isLocked)
            {
                door.SetActive(false);
                return Node.Status.SUCCESS;
            }
            return Node.Status.FALURE;
        }
        else
        {
            return s;
        }
    }

    public Node.Status GoToVan()
    {
        Node.Status s = GoToLocation(van.transform.position);

        if (s == Node.Status.SUCCESS)
        {
            money += 300;
            diamond.SetActive(false);
        }

        return GoToLocation(diamond.transform.position);
    }

    public Node.Status GoToBackDoor()
    {
        return GoToDoor(backDoor);
    }

    public Node.Status HasMoney()
    {
        if (money >= 500)
            return Node.Status.FALURE;
        return Node.Status.SUCCESS;
    }
}
