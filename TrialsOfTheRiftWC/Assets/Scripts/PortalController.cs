using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PortalController : MonoBehaviour {
	public float f_portalOffset = 1.5f;
    private float f_timeIn = 0;
	private Constants.Global.Side e_side;
	protected Maestro maestro;

	void Start(){
		maestro = Maestro.Instance;     // reference to Rift singleton
		if (transform.position.x < 0) {
			e_side = Constants.Global.Side.LEFT;
		}
		else { 
			e_side = Constants.Global.Side.RIGHT;
		}
	}

	void OnTriggerEnter(Collider other) {

		if (other.tag == "Player" || other.tag == "Spell" || other.tag == "Potato" || other.tag == "Puck") {
            Debug.Log("ow");
            maestro.PlayPortal();
			other.gameObject.transform.position = new Vector3(-1*other.transform.position.x + (int)e_side * Constants.RiftStats.C_PortalTeleportOffset,
				other.transform.position.y, -1*other.transform.position.z);
		}
	}

    private void OnTriggerStay(Collider other) {
        if (!(other.tag == "Player" || other.tag == "Spell")) {
			f_timeIn += Time.deltaTime;
            if (f_timeIn > 2f) {
                Debug.Log("A thing is in here.");
                other.gameObject.transform.position = new Vector3(transform.position.x + -1 * (int)e_side * Constants.RiftStats.C_PortalTeleportOffset,
                    0.5f,
                    transform.position.z);
                f_timeIn = 0;
            }
		}
        
    }

    private void OnTriggerExit(Collider other)
    {
        f_timeIn = 0;
    }
}
