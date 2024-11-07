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

    Node.Status treeStatus = Node.Status.RUNNING;

    public enum ActionState { IDLE, WORKING };

    ActionState state = ActionState.IDLE;

    private void Start()
    {
        agent = GetComponent<NavMeshAgent>();

        tree = new BehaviourTree();
        Sequence steal = new Sequence("Steal Something.");
        Leaf goToDiamond = new Leaf("Go To Diamond.", GoToDiamond);
        Leaf goToBackDoor = new Leaf("Go To Back Door.", GoToBackDoor);
        Leaf goToVan = new Leaf("Go To Van.", GoToVan);

        steal.AddChild(goToBackDoor);
        steal.AddChild(goToDiamond);
        steal.AddChild(goToBackDoor);
        steal.AddChild(goToVan);
        tree.AddChild(steal);

        tree.PrintTree();

    }

    private void Update()
    {
        if(treeStatus==Node.Status.RUNNING) treeStatus = tree.Process();
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
        return GoToLocation(diamond.transform.position);
    }

    public Node.Status GoToVan()
    {
        return GoToLocation(van.transform.position);
    }

    public Node.Status GoToBackDoor()
    {
        return GoToLocation(backDoor.transform.position);
    }
}
