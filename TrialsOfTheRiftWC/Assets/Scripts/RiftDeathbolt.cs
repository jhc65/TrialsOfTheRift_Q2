using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class RiftDeathbolt : MonoBehaviour {

	// Use this for initialization
	void Start () {
        Invoke("DestroyDeathbolt", Constants.RiftStats.C_VolatilityDeathboltLiveTimer);
	}
	
	// Update is called once per frame
	void Update () {
		
	}

    private void OnCollisionEnter(Collision collision) {
        // DIE! DIE! DIE!
        collision.gameObject.GetComponent<PlayerController>().TakeDamage(Constants.RiftStats.C_VolatilityDeathboltDamage,Constants.Global.DamageType.DEATHBOLT);
        DestroyDeathbolt();

    }

    private void DestroyDeathbolt() {
        Destroy(gameObject);
    }
}
