using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;
public class AIMovement : MonoBehaviour
{
    public List<Transform> agentPath;    
    public bool randomMovement = false;
    public float waitingTime = 0f;
    Animator animator;
    NavMeshAgent navMeshAgent;
    int actualIndex = 0;
    float dist;    
    bool waiting = false;

    // Start is called before the first frame update
    void Start()
    {
        navMeshAgent = GetComponent<NavMeshAgent>();
        animator = GetComponentInChildren<Animator>();
        NextDestination();        
    }

    // Update is called once per frame
    void Update()
    {
        dist = navMeshAgent.remainingDistance;        
        if (dist != Mathf.Infinity && navMeshAgent.pathStatus == NavMeshPathStatus.PathComplete && navMeshAgent.remainingDistance == 0)
        {          
            if (!waiting)
            {
                waiting = true;                    
                StartCoroutine(WaitCooldown());
            }                                                      
        }
    }

    void NextDestination()
    {
        if (randomMovement)
        {
            int aux = actualIndex;
            while (aux == actualIndex)
            {
                aux = Random.Range(0, agentPath.Count);
            }
            actualIndex = aux;          
        }

        else
        {
            navMeshAgent.destination = agentPath[actualIndex].position;
            actualIndex++;
            if (actualIndex >= agentPath.Count) actualIndex = 0;
        }       
    }

    IEnumerator WaitCooldown()
    {
        //yield return new WaitUntil(()=> currentTime==animator.GetCurrentAnimatorStateInfo(0).length);
        animator.enabled = false;
        yield return new WaitForSeconds(waitingTime);
        waiting = false;
        animator.enabled = true;      
        NextDestination();       
    }
}
