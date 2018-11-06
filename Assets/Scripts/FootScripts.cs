using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FootScripts : MonoBehaviour {

public GameObject GameManager;
GameManager manager;

	void Start () {
		manager = GameManager.GetComponent<GameManager>();
	}
	
	private void OnTriggerEnter(Collider collision)
    {
        if(collision.gameObject.name == "Player")
        {
			manager.setTarget(this.transform.parent.gameObject);

        }
    }
}
