using System.Collections;
using UnityEngine;
using UnityEngine.AI;

public class move : MonoBehaviour
{

    GameObject targetFood;
    GameObject targetPay;
    GameObject targetOut;

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
        targetFood = GameObject.FindWithTag("mat");
        targetPay = GameObject.FindWithTag("kassa");
        targetOut = GameObject.FindWithTag("dörr");
        //Får automatiskt sin första target
        agent.SetDestination(targetFood.transform.position);
        uppdateTrajektory1 = true;
        
    }
    // update kollar att om någon annan redan är vid den matplatsen som har stälts in
    // ifall upptagen byts målet och ifall det int fins lediga mål så väntas det på ett ledigt mål av korrekt typ
    private void Update() 
    {
        if(targetFood.tag != "mat" && uppdateTrajektory1 || targetFood.tag == "nothing" && uppdateTrajektory1)
        {
            StopMoveing();
            targetFood = GameObject.FindWithTag("mat");
            if(targetFood != null)
            {
                agent.isStopped = false;
                agent.SetDestination(targetFood.transform.position); 
            }
            else
            {
                targetFood = GameObject.FindWithTag("nothing");
            }
        }

        if(targetPay.tag != "kassa" && uppdateTrajektory2 || targetPay.tag == "nothing" && uppdateTrajektory2)
        {
            StopMoveing();
            targetPay = GameObject.FindWithTag("kassa");
            if(targetPay != null)
            {
                agent.isStopped = false;
                agent.SetDestination(targetPay.transform.position); 
            }
            else
            {
                targetPay = GameObject.FindWithTag("nothing");
            }
        }     
    }

    // vid varje kollsion så måste målet bytas till nästa typ av mål där köaren inte ennu har varit, hät kommer också köaren fram till ett mål
    void OnCollisionEnter(Collision other)
    {
        if(other.gameObject.tag == "mat")
        {   
            uppdateTrajektory1 = false;
            changeAllTags(other.gameObject, "full");
            StartCoroutine(atFoodStation(other.gameObject));    
        } 
        
        if(other.gameObject.tag == "kassa")
        {   
            uppdateTrajektory2 = false;
            changeAllTags(other.gameObject, "full");
            StopAllCoroutines();
            StartCoroutine(atPayStation(other.gameObject));   
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
    //Då agenten har kommit fram till mat pungten börjar denhär corutinen
    IEnumerator atFoodStation(GameObject other)
    {
        StopMoveing();
        yield return new WaitForSeconds(0.5f);
        agent.velocity = Vector3.zero;

        yield return new WaitForSeconds(randomNumber());

        agent.isStopped = false;
        agent.SetDestination(targetPay.transform.position);
        uppdateTrajektory2 = true;
        changeAllTags(other, "mat");
    }

    // Då agentten kommer fram till kassan kommer denhär corutinen att börja
    IEnumerator atPayStation(GameObject other)
    {
        StopMoveing();
        yield return new WaitForSeconds(0.5f);
        agent.velocity = Vector3.zero;

        yield return new WaitForSeconds(randomNumber());

        agent.isStopped = false;
        agent.SetDestination(targetOut.transform.position);
        changeAllTags(other, "kassa");
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

    void changeAllTags(GameObject objekt, string newTag)
    {
        objekt.tag = newTag;
    }
}
