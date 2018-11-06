using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MovingObjOnRamp : MonoBehaviour {

	public	float		pushPower = 1.0f;
    public  float       sampleTime = 1.0f;
	private float       m_elapsedTime = 0.0f;
    private Rigidbody   m_rb = null;
    public bool         m_isRunning = false;
    private Vector3     m_counterForce;


    // Use this for initialization
    void Start () {
        m_rb = GetComponent<Rigidbody>();
        m_rb.useGravity = m_isRunning;
	}
	
	// Update is called once per frame
	void Update () {
		// if(Input.GetKeyDown(KeyCode.A))
        // {
        //     m_isRunning = !m_isRunning;
        //     m_rb.useGravity = m_isRunning;
        // }
	}

	    public void setPushPower(float power){
        pushPower = power;
    }

    void FixedUpdate()
    {
        if(m_isRunning)
        {
			m_rb.useGravity = m_isRunning;
            m_rb.AddForce(m_counterForce);
			Debug.DrawRay(transform.position, m_counterForce, Color.green); 

            m_elapsedTime += Time.fixedDeltaTime;
            if(m_elapsedTime > sampleTime)
            {
                Debug.Log("Velocity: " + m_rb.velocity);
                Debug.Log("Speed: " + m_rb.velocity.magnitude);
                Debug.Log("Time Elapsed: " + m_elapsedTime);
                m_elapsedTime = 0.0f;
            }
        }
    }

    private void OnCollisionEnter(Collision collision)
    {
        //calculate acceleration
        Collider col = collision.gameObject.GetComponent<Collider>();

        if (col.material.frictionCombine != PhysicMaterialCombine.Maximum)
        {
            //ignore any collisions with bad physics materials
            return;
        }

        float dynamicFrictionCoeff = col.material.dynamicFriction;
        Debug.Log("Dynamic Friction Coeff: " + dynamicFrictionCoeff);
        Debug.Log("Mass: " + m_rb.mass);
        Debug.Log("Speed: " + m_rb.velocity.magnitude);
        Vector3 axis;
        float angle;
        collision.transform.rotation.ToAngleAxis(out angle,out axis);
        angle = 90.0f - angle;
        Debug.Log("Angle: " + angle);

        float normalForce = Physics.gravity.magnitude * m_rb.mass * Mathf.Cos(Mathf.Deg2Rad * angle);
        float dynamicFrictionForce = normalForce * dynamicFrictionCoeff;

        float netGravityForce = Physics.gravity.magnitude * m_rb.mass * Mathf.Sin(Mathf.Deg2Rad * angle);
		// float netForce = netGravityForce - dynamicFrictionForce;	//To pull downward
        float netForce = netGravityForce + dynamicFrictionForce;	//To pull upward
		m_counterForce = netForce * collision.transform.forward * pushPower;
        float acceleration = netForce / m_rb.mass;

        Debug.Log("acceleration: " + acceleration);
		Debug.Log("netForce: " + netForce);
    }
   
}

