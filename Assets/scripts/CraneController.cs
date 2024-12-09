using UnityEngine;

public class CraneController : MonoBehaviour
{
    public float Speed;
    public float RotationSpeed;

    void Update()
    {
        if(Input.GetKey(KeyCode.UpArrow)) {
            transform.Translate(0, 0, Speed * Time.deltaTime);
        }
        if(Input.GetKey(KeyCode.DownArrow)) {
            transform.Translate(0, 0, -Speed * Time.deltaTime);
        }
        if(Input.GetKey(KeyCode.LeftArrow)) {
            transform.Rotate(0, -RotationSpeed * Time.deltaTime, 0);
        }
        if(Input.GetKey(KeyCode.RightArrow)) {
            transform.Rotate(0, RotationSpeed * Time.deltaTime, 0);
        }
    }
}
