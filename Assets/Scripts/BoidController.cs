using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BoidController : MonoBehaviour
{
    // Guided by http://www.kfish.org/boids/pseudocode.html

    public GameObject[] boids;
    public GameObject playerTargetObject;
    public float minBoidDistance = 100;
    public Vector3 playerTarget = new Vector3(0, 0, 0);
    public Vector3 constantForwardMovement = new Vector3(0, 0.005f, 0);
    public float maxPlayerSpeed = 0.015f;
    public float playerSpeedIncrement = 0.003f;
    public float cameraOffset = -4.0f;
    public float boidSpeedLimit = 2.0f;

    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        PlayerControls();
        MoveAllBoids();

        playerTargetObject.transform.position = playerTargetObject.transform.position + playerTarget;
    }

    void PlayerControls()
    {
        if (Input.GetKey(KeyCode.A))
        {
            if (playerTarget.x > -maxPlayerSpeed)
            {
                playerTarget.x -= playerSpeedIncrement;
            }
        }
        if (Input.GetKey(KeyCode.D))
        {
            if (playerTarget.x < maxPlayerSpeed)
            {
                playerTarget.x += playerSpeedIncrement;
            }
        }

        if(Input.GetMouseButtonDown(0))
        {
            playerTargetObject.transform.position = new Vector3(Camera.main.ScreenToWorldPoint(Input.mousePosition).x, Camera.main.ScreenToWorldPoint(Input.mousePosition).y, 0);
        }
    }

    void MoveAllBoids()
    {
        Vector3 v1, v2, v3, v4;

        foreach (GameObject boid in boids)
        {
            v1 = CenterMassCalculations(boid);
            v2 = DontTouchOtherBoids(boid);
            v3 = MatchVelocity(boid);
            v4 = TargetPosition(boid);

            boid.GetComponent<Rigidbody>().velocity = boid.GetComponent<Rigidbody>().velocity + v1 + v2 + v3 + v4 + constantForwardMovement;

            //BoidSpeedLimit(boid);
        }
    }

    Vector3 CenterMassCalculations(GameObject thisBoid)
    {
        Vector3 perceivedCenter = new Vector3(0,0,0);

        foreach(GameObject otherBoid in boids)
        {
            if (otherBoid != thisBoid)
            {
                perceivedCenter = perceivedCenter + otherBoid.transform.position;
            }
        }
        perceivedCenter = perceivedCenter / (boids.Length - 1);

        Camera.main.transform.position = new Vector3(Camera.main.transform.position.x, perceivedCenter.y - cameraOffset, Camera.main.transform.position.z);

        return (perceivedCenter - thisBoid.transform.position) / 100;
    }

    Vector3 DontTouchOtherBoids(GameObject thisBoid)
    {
        Vector3 c = new Vector3(0, 0, 0);

        foreach(GameObject otherBoid in boids)
        {
            if (otherBoid != thisBoid)
            {
                if((otherBoid.transform.position - thisBoid.transform.position).sqrMagnitude < minBoidDistance)
                {
                    c = c - (otherBoid.transform.position - thisBoid.transform.position);
                }
            }
        }
        return c;
    }

    Vector3 MatchVelocity(GameObject thisBoid)
    {
        Vector3 perceivedVelocity = new Vector3(0, 0, 0);

        foreach(GameObject otherBoid in boids)
        {
            if(otherBoid != thisBoid)
            {
                perceivedVelocity = perceivedVelocity + thisBoid.GetComponent<Rigidbody>().velocity;
            }
        }
        perceivedVelocity = perceivedVelocity / (boids.Length - 1);

        return (perceivedVelocity - thisBoid.GetComponent<Rigidbody>().velocity) / 8;
    }

    Vector3 TargetPosition(GameObject thisBoid)
    {
        return (playerTargetObject.transform.position - thisBoid.transform.position) / 100;
    }

    void BoidSpeedLimit(GameObject thisBoid)
    {
        if(thisBoid.GetComponent<Rigidbody>().velocity.sqrMagnitude > boidSpeedLimit)
        {
            thisBoid.GetComponent<Rigidbody>().velocity = ((thisBoid.GetComponent<Rigidbody>().velocity / thisBoid.GetComponent<Rigidbody>().velocity.sqrMagnitude) * boidSpeedLimit) + (playerTargetObject.transform.position - thisBoid.transform.position) / 100 + constantForwardMovement;
        }
    }
}
