/*  Flag Controller - Zak Olyarnik
 * 
 *  Desc:   Facilitates picking up and scoring with Capture the Flag Objective's Flag object
 * 
 */

using UnityEngine;

public class FlagController : MonoBehaviour {
#region Variables and Declarations
    [SerializeField] private CaptureTheFlagObjective ctfo_owner;    // identifies Objective flag is a part of
    [SerializeField] private Constants.Global.Color e_color;        // identifies owning team
#endregion

#region FlagController Methods
    public void DropFlag() {
        transform.SetParent(ctfo_owner.gameObject.transform);   // resets flag parent so Objective can be deactivated correctly
        transform.localPosition = new Vector3(transform.localPosition.x, Constants.ObjectiveStats.C_RedFlagSpawn.y, transform.localPosition.z);
    }

    public void ResetFlagPosition() {
        if(e_color == Constants.Global.Color.RED) {
            transform.localPosition = Constants.ObjectiveStats.C_RedFlagSpawn;
        }
        else {
            transform.localPosition = Constants.ObjectiveStats.C_BlueFlagSpawn;
        }
    }
#endregion

#region Unity Overrides
    void OnTriggerEnter(Collider other) {
        // Player trying to pick up flag (and flag not already picked up)
        if (other.CompareTag("InteractCollider") && other.transform.root.gameObject.CompareTag("Player")) {
			other.GetComponentInParent<PlayerController>().Pickup(gameObject);
			other.gameObject.SetActive(false);
		}
        // Player scoring with flag
		if (other.CompareTag("Goal")) {
			if (other.GetComponent<GoalController>().Color != e_color) {        // check for correct color of flag/goal
                ctfo_owner.UpdateFlagScore();                                   // increase score and update UI      
				transform.root.GetComponent<PlayerController>().DropFlag();     // make carrying player drop flag (sets player's flag reference to null and calls FlagController.DropFlag)
                ResetFlagPosition();   // reset flag to original spawn position
            }
		}
	}
#endregion
}
