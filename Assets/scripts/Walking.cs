using UnityEngine;
using UnityEngine.AI;


public class Walking : MonoBehaviour
{
  
    Animator anim;
    NavMeshAgent agent;

    float speedNow;
    [SerializeField] float shouldNotIdle = 0.1f;

    void Start()
    {
      anim = GetComponent<Animator>();
      agent = GetComponentInParent<NavMeshAgent>(); // behandlar parent component då det är den som ejentligen rör på sig
    }

    void Update()
    {
        speedNow = agent.velocity.magnitude;
        if(speedNow > shouldNotIdle)
        {
            anim.SetBool("Walking", true);

            Vector3 direktion = agent.velocity.normalized;

            if(direktion.sqrMagnitude > 0.0001f)  //riktnings vektorn kan inte vara till längden noll för så finns det ingen riktning
            {
                transform.forward = direktion; //blake kollar mot den riktning han går
            } 
        }
        else
        {
            anim.SetBool("Walking", false);
        }
    }
}
