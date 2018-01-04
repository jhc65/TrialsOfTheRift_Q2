using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FlagController : MonoBehaviour {

	public Constants.Color e_color; // identifies owning team
	public bool b_scored = false;	// identifies when the flag has been used to score
	private Vector3 v3_home;        // location of flag in players' base

	void Start() {
		v3_home = transform.position;
	}

	void OnTriggerEnter(Collider other) {
		if (other.tag == "InteractCollider" && transform.parent == null) {   // player trying to pick up flag
			other.GetComponentInParent<PlayerController>().Pickup(gameObject);
			other.gameObject.SetActive(false);
		}
		if (other.tag == "Goal") {   // player scoring with flag
			if (other.GetComponent<GoalController>().GetColor() != e_color) {
				b_scored = true;
				transform.root.GetComponent<PlayerController>().Drop();
				transform.position = v3_home;
			}
		}
	}

    public void ResetHome() {
        transform.position = v3_home;
    }
}
