using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class ExplosionController : MonoBehaviour {

	public void Start() {
		Invoke("InvokeDestroy", 0.0f); 
	}

	public void Update() {
		
	}

    void OnTriggerEnter(Collider collider)
    {
        if(collider.gameObject.tag == "Player" )
        {
            collider.gameObject.GetComponent<PlayerController>().TakeDamage(Constants.EnemyStats.C_RuneDamage);
        }
		else if (collider.gameObject.tag == "Enemy") {
			collider.gameObject.GetComponent<EnemyController>().TakeDamage(Constants.EnemyStats.C_RuneDamage);
		}
    }

	void InvokeDestroy() {
		Destroy(gameObject);
    }
} 