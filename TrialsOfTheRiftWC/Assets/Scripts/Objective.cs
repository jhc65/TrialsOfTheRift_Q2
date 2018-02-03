using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public abstract class Objective : MonoBehaviour {

	public Constants.Global.Color e_color;		// identifies owning team
	public int i_numberInList;			// this is the i'th objective faced by this team (1-based)
	public bool b_complete;             // set when criteria to beat the objective is met
    public GameObject go_redRoom, go_blueRoom;	// referenced room objects to play this objective in
    protected GameObject go_activeRoom;         // active room specific to this objective instance

    public void Set(Constants.Global.Color c, int n) {
		e_color = c;
		i_numberInList = n;
        if (e_color == Constants.Global.Color.RED) {
            go_activeRoom = Instantiate(go_redRoom, go_redRoom.transform.position, go_redRoom.transform.rotation);
        }
        else {
            go_activeRoom = Instantiate(go_blueRoom, go_blueRoom.transform.position, go_blueRoom.transform.rotation);
        }
        Instantiate();
	}

	public abstract void Instantiate();
	public abstract void Complete();
    public abstract void ParamReset(float param_in);        //[Param Fix] - Used for Param Screen. Probably should be removed in release.
}
