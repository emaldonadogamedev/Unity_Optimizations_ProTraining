using UnityEngine;
using UnityEngine.AI;

public class MoveToClickPoint : MonoBehaviour
{
    protected NavMeshAgent agent;
    protected Animator anim;

    protected virtual void Start()
    {
        agent = GetComponent<NavMeshAgent>();
        anim = GetComponentInChildren<Animator>();
    }

    protected virtual void Update()
    {
        if (Input.GetMouseButtonDown(0))
        {
            RaycastHit hit;

            if (Physics.Raycast(Camera.main.ScreenPointToRay(Input.mousePosition), out hit, 100))
            {
                agent.destination = hit.point;
            }
        }

        if (agent.velocity.sqrMagnitude > Vector3.zero.sqrMagnitude)
        {
            if(anim != null)
                anim.SetBool("running", true);
        } else
        {
            if(anim != null)
                anim.SetBool("running", false);
        }
    }
}

