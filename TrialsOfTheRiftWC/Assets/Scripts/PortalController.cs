using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PortalController : MonoBehaviour {
	public float f_portalOffset = 1.5f;
    private float f_timeIn = 0;
	private Constants.Side e_side;

	void Start(){
		if(transform.position.x < 0){
			e_side = Constants.Side.LEFT;
		}
		else{
			e_side = Constants.Side.RIGHT;
		}
	}

	void OnTriggerEnter(Collider other) {
		if (other.tag == "Player" || other.tag == "Spell" || other.tag == "Potato") {
			other.gameObject.transform.position = new Vector3(-1*other.transform.position.x + (int)e_side * f_portalOffset,
				other.transform.position.y, -1*other.transform.position.z);
		}
	}

    private void OnTriggerStay(Collider other) {
        Debug.Log("A thing is in here.");
        if (!(other.tag == "Player" || other.tag == "Spell")) {
			f_timeIn += Time.deltaTime;
            if (f_timeIn > 2f) {
                other.gameObject.transform.position = new Vector3(transform.position.x + -1 * (int)e_side * f_portalOffset,
                    0.5f,
                    transform.position.z);
            }
		}
        
    }
}
