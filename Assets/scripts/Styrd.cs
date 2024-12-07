using UnityEngine;

public class Styrd : MonoBehaviour
{
    public float speed = 10f;
    public float rotationspeed = 150f;

    Animator anim;

    void Start()
    {
        anim = GetComponent<Animator>();    
    }

    // Update is called once per frame
    void Update()
    {
        if(Input.GetKey(KeyCode.W))
        {
            transform.Translate(0,0,speed * Time.deltaTime);
            anim.SetBool("Walking", true);
        }
            
        else
        {
            anim.SetBool("Walking", false);
        }

        if(Input.GetKey(KeyCode.D))
            transform.Rotate(0,rotationspeed * Time.deltaTime,0);

        if(Input.GetKey(KeyCode.A))
            transform.Rotate(0,-rotationspeed * Time.deltaTime,0);
    }
}
