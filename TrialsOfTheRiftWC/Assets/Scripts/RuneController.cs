using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class RuneController : MonoBehaviour {
	private float timer = 4.0f;
	private bool activated = false;
	public GameObject go_explosionPrefab;

	public void Start() {

	}

	public void Update() {

		if (activated) {
			timer -= Time.deltaTime;
		}

		if (timer < 0) {
			InvokeDestroy(); 
		}

	}

    void OnTriggerEnter(Collider collider)
    {
        if(collider.gameObject.tag == "Player"  || collider.gameObject.tag == "Enemy")
        {
            activated = true;
        }
    }

	void InvokeDestroy() {
		Instantiate(go_explosionPrefab, transform.position, Quaternion.identity);
		Destroy(gameObject);
    }
} 
