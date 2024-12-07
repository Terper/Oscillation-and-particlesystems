using System.Collections;
using UnityEngine;
using UnityEngine.AI;

public class move : MonoBehaviour
{
    //[SerializeField] Transform target;
    //[SerializeField] Transform target2;

    GameObject target1;
    GameObject target2;
    GameObject target3;

    bool uppdateTrajektory1 = true;
    bool uppdateTrajektory2 = false;
    float atstartTime; // beräknar tiden
    
    NavMeshAgent agent;

    void Awake() 
    {
        agent = GetComponent<NavMeshAgent>(); 
           
    }

    void Start()
    {
        atstartTime = Time.time;
        target1 = GameObject.FindWithTag("mat");
        target2 = GameObject.FindWithTag("matbord");
        target3 = GameObject.FindWithTag("dörr");
        //Får automatiskt sin första target
        agent.SetDestination(target1.transform.position);
        uppdateTrajektory1 = true;
        
    }
    // update kollar att om någon annan redan är vid den matplatsen som har stälts in
    // ifall upptagen byts målet och ifall det int fins lediga mål så väntas det på ett ledigt mål av korrekt typ
    private void Update() 
    {
        if(target1.tag != "mat" && uppdateTrajektory1 || target1.tag == "nothing" && uppdateTrajektory1)
        {
            StopMoveing();
            target1 = GameObject.FindWithTag("mat");
            if(target1 != null)
            {
                agent.isStopped = false;
                agent.SetDestination(target1.transform.position); 
            }
            else
            {
                target1 = GameObject.FindWithTag("nothing");
            }
        }

        if(target2.tag != "matbord" && uppdateTrajektory2 || target2.tag == "nothing" && uppdateTrajektory2)
        {
            StopMoveing();
            target2 = GameObject.FindWithTag("matbord");
            if(target2 != null)
            {
                agent.isStopped = false;
                agent.SetDestination(target2.transform.position); 
            }
            else
            {
                target2 = GameObject.FindWithTag("nothing");
            }
        }     
    }

    // vid varje kollsion så måste målet bytas till nästa typ av mål där köaren inte ennu har varit, hät kommer också köaren fram till ett mål
    void OnCollisionEnter(Collision other)
    {
        if(other.gameObject.tag == "mat")
        {   
            uppdateTrajektory1 = false;
            changeTag(other.gameObject);
            StartCoroutine(rutin(other.gameObject));    
        } 
        
        if(other.gameObject.tag == "matbord")
        {   
            uppdateTrajektory2 = false;
            changeTag(other.gameObject);
            StopAllCoroutines();
            StartCoroutine(rutin2(other.gameObject));   
        }
        
        if(other.gameObject.tag == "dörr")
        {   
            StopMoveing();
            StopAllCoroutines();
            Debug.Log(gameObject.name + " fick mat på: " + ((atstartTime - Time.time) * -1) + " sekunder");
            Destroy(gameObject);
        }
    }

    // efter att köaren kommit fram till sit mål, väntar den vid målet för en kort stund före den fortsätter rill nästa typs av mål
    IEnumerator rutin(GameObject other)
    {
        StopMoveing();
        yield return new WaitForSeconds(0.5f);
        agent.velocity = Vector3.zero;

        yield return new WaitForSeconds(randomNumber());

        agent.isStopped = false;
        agent.SetDestination(target2.transform.position);
        uppdateTrajektory2 = true;
        changeTagBackMat(other);
    }

    IEnumerator rutin2(GameObject other)
    {
        StopMoveing();
        yield return new WaitForSeconds(0.5f);
        agent.velocity = Vector3.zero;

        yield return new WaitForSeconds(randomNumber());

        agent.isStopped = false;
        agent.SetDestination(target3.transform.position);
        changeTagBackMatbord(other);
    }

    //Funktioner som används i koden ovan

    void StopMoveing()
    {
        agent.isStopped = true;
        agent.velocity = Vector3.zero;
    }

    float randomNumber()
    {
        return Random.Range(2,6);
    }

    void changeTag(GameObject objekt)
    {
        objekt.tag = "full";
    }

    void changeTagBackMat(GameObject objekt)
    {
        objekt.tag = "mat";
    }

    void changeTagBackMatbord(GameObject objekt)
    {
        objekt.tag = "matbord";
    }

    IEnumerator fundera(GameObject other)
    {
        StopMoveing();
        yield return new WaitForSeconds(1f); 
    }
}
