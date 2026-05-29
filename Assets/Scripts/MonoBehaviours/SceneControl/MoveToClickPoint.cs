using UnityEngine;
using UnityEngine.AI;

public class MoveToClickPoint : MonoBehaviour
{
    protected NavMeshAgent agent;
    protected Animator anim;

    private int runningHash = Animator.StringToHash("running");

    protected virtual void Start()
    {
        agent = GetComponent<NavMeshAgent>();
        anim = GetComponentInChildren<Animator>();
    }

    protected virtual void Update()
    {
        if (Input.GetMouseButtonDown(0))
        {
            if (Physics.Raycast(Camera.main.ScreenPointToRay(Input.mousePosition), out var hit, 100))
            {
                agent.destination = hit.point;
            }
        }

        if (agent.velocity.sqrMagnitude > Vector3.zero.sqrMagnitude)
        {
            if(anim != null)
                anim.SetBool(runningHash, true);
        }
        else
        {
            if(anim != null)
                anim.SetBool(runningHash, false);
        }
    }
}

