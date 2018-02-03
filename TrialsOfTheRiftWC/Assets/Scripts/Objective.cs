/*  Objective - Zak Olyarnik
 * 
 *  Desc:   Parent class of all in-game Objectives.
 * 
 */

using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public abstract class Objective : MonoBehaviour {

    public Constants.Global.Color e_color;   // identifies owning team - MUST BE SET IN INSPECTOR!
    public GameObject[] go_roomPool;	     // rooms allowed for this objective to be played in
    protected GameObject go_activeRoom;      // active room specific to this objective instance 
    protected int i_numberInList;            // this is the i'th objective faced by this team (1-based)
    protected bool b_isComplete = false;     // set when criteria to beat the objective is met

    // Getters
    public Constants.Global.Color Color {
        get { return e_color; }
    }
    public int NumberInList {
        get { return i_numberInList; }
    }
    public bool IsComplete {
        get { return b_isComplete; }
    }

    /*/////////////////////////////////////////////////////////////////////////////////////////////////////////////*/

    public abstract void SetUI();
    public abstract void ResetUI();
    public abstract void ParamReset(float param_in);        //[Param Fix] - Used for Param Screen. Probably should be removed in release.

    // Activates all aspects of this objective
    public Objective Activate(int i) {
        b_isComplete = false;           // initialize variables
        i_numberInList = i;
        SetUI();                        // set UI
        go_activeRoom = SelectRoom();   // set room
        go_activeRoom.SetActive(true);
        gameObject.SetActive(true);     // finally, turn on objective
        return this;
    }

    // Destroy this objective once it is complete
    public void Complete() {
        ResetUI();                      // turn off UI
        go_activeRoom.SetActive(false); // turn off room
        Destroy(gameObject);
    }

    // Randomly selects a room from the list of approved rooms for this objective
    public GameObject SelectRoom() {
        int i = Random.Range(0, go_roomPool.Length);
        return go_roomPool[i];
    }
}
