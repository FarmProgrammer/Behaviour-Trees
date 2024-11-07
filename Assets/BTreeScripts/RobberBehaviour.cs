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

    public enum ActionState { IDLE, WORKING };

    ActionState state = ActionState.IDLE;

    private void Start()
    {
        agent = GetComponent<NavMeshAgent>();

        tree = new BehaviourTree();
        Node steal = new Node("Steal Something.");
        Leaf goToDiamond = new Leaf("Go To Diamond.", GoToDiamond);
        Leaf goToVan = new Leaf("Go To Van.", GoToVan);

        steal.AddChild(goToDiamond);
        steal.AddChild(goToVan);
        tree.AddChild(steal);

        tree.PrintTree();

        tree.Process();
    }

    Node.Status GoToLocation(Vector3 destination)
    {
        float distanceToTarget = Vector3.Distance(destination, transform.position);
        if(state == ActionState.IDLE)
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
}
