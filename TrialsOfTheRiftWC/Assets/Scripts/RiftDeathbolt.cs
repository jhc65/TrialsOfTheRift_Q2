using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class RiftDeathbolt : MonoBehaviour {

	// Use this for initialization
	void Start () {
        Debug.Log("calling DestroyDeathbolt()");
        Invoke("DestroyDeathbolt", Constants.RiftStats.C_VolatilityDeathboltLiveTimer);
	}
	
	// Update is called once per frame
	void Update () {
		
	}

    private void OnCollisionEnter(Collision collision) {
        Debug.Log("Collision: " + collision.gameObject.name);
        if (collision.gameObject.tag != "Player") {
            //ignores any collision that isn't with a player
            Physics.IgnoreCollision(GetComponent<Collider>(), collision.gameObject.GetComponent<Collider>());
        }
        else {
            // DIE! DIE! DIE!
            collision.gameObject.GetComponent<PlayerController>().TakeDamage(Constants.RiftStats.C_VolatilityDeathboltDamage);
        }
    }

    private void DestroyDeathbolt() {
        Debug.Log("Destroying the deathbolt!");
        Destroy(gameObject);
    }
}
