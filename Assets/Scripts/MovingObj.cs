using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MovingObj : MonoBehaviour {

    float desiredTimeToDestination = 2.5f;
    public float pushForce = 0;
    float acceleration = 0.0f;

    float timeElapsed = 0.0f;
    public bool isRunning = false;
    Rigidbody rb = null;
    public Transform destination;
    public bool hasEnteredDestination = false;
    public bool hasExitedDestination = false;

    float forceApp;

    void Start()
    {
        rb = GetComponent<Rigidbody>();

        checkForceShoudApplied();
    }

    public void checkForceShoudApplied(){

        acceleration = CalculateAcceleration();
        float groundDynamicFriction = GameObject.Find("GroundPlane").GetComponent<Collider>().material.dynamicFriction;
        float groundStaticFriction = GameObject.Find("GroundPlane").GetComponent<Collider>().material.staticFriction;
        float objDynamicFriction = GetComponent<Collider>().material.dynamicFriction;
        float objStaticFriction = GetComponent<Collider>().material.staticFriction;

        // float frictionAvg = (groundStaticFriction + objStaticFriction) / 2.0f; //statice friction
        float dynamicfrictionAvg = (objDynamicFriction + groundDynamicFriction) / 2.0f;
        float frictionForce = dynamicfrictionAvg * rb.mass * Physics.gravity.magnitude;  //Fk = umg
        float normalForce = rb.mass * acceleration;   //F = ma
        forceApp = normalForce + frictionForce;   //Fapp = ma + Fk

        Debug.Log("forceApp: " + forceApp);
    }

    public void setPushForce(float Fapp){
        pushForce = Fapp;
    }

    void Update()
    {
        if (!isRunning)
        {
            rb.velocity = Vector3.zero;
            timeElapsed = 0.0f;
        }
    }

    protected float CalculateAcceleration()
    {
        if (desiredTimeToDestination <= 0.0f)
            return 0.0f;

        float Vi = rb.velocity.z;
        float d = Mathf.Abs(destination.position.z - transform.position.z);
        float a = (d - (Vi * desiredTimeToDestination)) / (0.5f * Mathf.Pow(desiredTimeToDestination, 2.0f));

        return a;
    }
    void FixedUpdate()
    {
        if (isRunning)
        {

            timeElapsed += Time.fixedDeltaTime;
           
            rb.AddForce(transform.forward * pushForce, ForceMode.Force);

            if (timeElapsed > desiredTimeToDestination)
            {
                isRunning = false;
            }

            Debug.DrawRay(transform.position, transform.forward * pushForce, Color.red); 
        }
    }

    private void OnTriggerEnter(Collider collision)
    {
        if(collision.gameObject.tag == "Destination")
        {
            hasEnteredDestination = true;
            Debug.Log("has Entered Destination: " + hasEnteredDestination);
        }

    }

    private void OnTriggerExit(Collider collision)
    {
        if(collision.gameObject.tag == "Destination")
        {
            hasExitedDestination = true;
            Debug.Log("hasExitedDestination: " + hasExitedDestination);
        }
    }

    public void ResetTrigger(){
        hasEnteredDestination = false;
        hasExitedDestination = false;
    }

}
