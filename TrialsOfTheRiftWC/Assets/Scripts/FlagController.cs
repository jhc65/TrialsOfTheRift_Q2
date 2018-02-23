/*  Flag Controller - Zak Olyarnik
 * 
 *  Desc:   Facilitates picking up and scoring with Capture the Flag Objective's Flag object
 * 
 */

using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FlagController : MonoBehaviour {

    [SerializeField] private CaptureTheFlagObjective ctfo_owner;  // identifies objective flag is a part of
    [SerializeField] private Constants.Global.Color e_color; // identifies owning team - MUST BE SET IN INSPECTOR!

    /*/////////////////////////////////////////////////////////////////////////////////////////////////////////////*/

    public void DropFlag() {
        transform.SetParent(ctfo_owner.gameObject.transform);
        transform.localPosition = new Vector3(transform.localPosition.x, 0.5f, transform.localPosition.z);
    }

    public void ResetFlagPosition() {
        if(e_color == Constants.Global.Color.RED) {
            transform.localPosition = Constants.ObjectiveStats.C_RedFlagSpawn;
        }
        else {
            transform.localPosition = Constants.ObjectiveStats.C_BlueFlagSpawn;
        }
    }

    /*/////////////////////////////////////////////////////////////////////////////////////////////////////////////*/

	void OnTriggerEnter(Collider other) {
        // Player trying to pick up flag (and flag not already picked up)
        if (other.CompareTag("InteractCollider") && other.transform.root.gameObject.CompareTag("Player")) {
			other.GetComponentInParent<PlayerController>().Pickup(gameObject);
			other.gameObject.SetActive(false);
		}
		if (other.CompareTag("Goal")) {   // player scoring with flag
			if (other.GetComponent<GoalController>().Color != e_color) {   // check for correct color of flag/goal
                ctfo_owner.UpdateFlagScore();         // increase score and update UI      
				transform.root.GetComponent<PlayerController>().DropFlag();     // make carrying player drop flag (sets player's flag reference to null and calls FlagController.DropFlag)
                ResetFlagPosition();   // reset flag to original spawn position
            }
		}
	}

    // TODO: Reinvestigate with Assets.
    //if the flag happens to drop in the zone but not capture as part of a bug
    //private void OnTriggerStay(Collider other)
    //{
    //    if (other.tag == "Goal")
    //    {   // player scoring with flag
    //        if (other.GetComponent<GoalController>().Color != e_color)
    //        {
    //            b_scored = true;
    //            transform.root.GetComponent<PlayerController>().Drop();
    //            transform.position = v3_home;
    //        }
    //    }
    //}
}
