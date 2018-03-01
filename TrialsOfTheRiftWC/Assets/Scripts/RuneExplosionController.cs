using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class RuneExplosionController : MonoBehaviour {

	public void Update() {
		
	}

    void OnTriggerEnter(Collider other)
    {
        if(other.CompareTag("Player"))
        {
            other.gameObject.GetComponent<PlayerController>().TakeDamage(Constants.EnemyStats.C_RuneDamage);
        }
    }

	public void Start() {
		Destroy(gameObject, 1.5f);
	}
} 