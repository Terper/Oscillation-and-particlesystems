using System;
using UnityEngine;

public class Pendel : MonoBehaviour
{
    public float winchSpeed = 1f;
    public float minimumWinchLength = 1f;
    public float maximumWinchLength = 10.0f;
    public float damping = 0.999f;
    float angularVelocity = 0;
    float gravity = -9.82f;
    float radius;
    GameObject Connection;
    LineRenderer Line;

    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        Connection = GameObject.Find("Rope");

        // line setup
        Line = new GameObject("Line").AddComponent<LineRenderer>();
        Line.material.SetColor("_Color", Color.black);
        Line.startWidth = 0.1f;
        Line.endWidth = 0.1f;

        // inital radius
        radius = Vector3.Distance(transform.position, Connection.transform.position);
    }

    // Update is called once per frame
    void Update()
    {
        // winch the pendulum up and down
        if (Input.GetKey(KeyCode.W))
        {
            if (radius < minimumWinchLength)
            {
                radius = minimumWinchLength;
            }
            else
            {
                radius -= winchSpeed * Time.deltaTime;
            }
        }
        if (Input.GetKey(KeyCode.S))
        {
            if (radius > maximumWinchLength)
            {
                radius = maximumWinchLength;
            }
            else
            {
                radius += winchSpeed * Time.deltaTime;
            }
        }

        /*
            /FG = follows guide on itslearning
            /SM = self made
        */

        // draw a line between the pendulum and the connection point
        Line.SetPosition(0, transform.position);
        Line.SetPosition(1, Connection.transform.position);

        // calculate the horizontal distance /FG
        Vector3 distanceVector = transform.position - Connection.transform.position;
        float horizontalDistance = Mathf.Sqrt(
            distanceVector.x * distanceVector.x + distanceVector.z * distanceVector.z
        );

        // calculate the angle of the pendulum /FG
        float angle = Mathf.Asin(Mathf.Clamp(horizontalDistance / radius, -1, 1));

        //  calculate the angular acceleration of the pendulum /FG
        float angularAcceleration = (gravity * Mathf.Sin(angle)) / radius;

        //  add the angular acceleration to the angular velocity /FG
        angularVelocity += angularAcceleration * Time.deltaTime;

        // apply damping to the angular velocity /SM
        angularVelocity *= this.damping;

        //  add the angular velocity to a new angle /FG
        float updatedAngle = angle + angularVelocity * Time.deltaTime;

        // lets the pendulum swing back and forth /SM
        if (updatedAngle < 0)
        {
            angularVelocity = -angularVelocity;
        }

        // calculates the new horizontal distance /FG
        float updatedHorizontalDistance = Mathf.Sin(updatedAngle) * radius;

        // calculate the new position of the pendulum /FG
        float x =
            ((distanceVector.x / horizontalDistance) * updatedHorizontalDistance)
            + Connection.transform.position.x;
        float z =
            ((distanceVector.z / horizontalDistance) * updatedHorizontalDistance)
            + Connection.transform.position.z;
        float y = -(Mathf.Cos(angle) * radius) + Connection.transform.position.y;

        // set the new position of the pendulum /FG
        transform.position = new Vector3(x, y, z);

        transform.LookAt(Connection.transform.position);
    }
}
