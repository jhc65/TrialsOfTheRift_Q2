/*  Hot Potato Controller - Dana Thompson
 * 
 *  Desc:   Facilitates movement and timers of Hot Potato Objective's Hot Potato object
 * 
 */

using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class HotPotatoController : MonoBehaviour {

    public HotPotatoObjective hpo_owner;  // identifies objective potato is a part of
    [SerializeField] private Constants.Global.Color e_color;     // identifies owning team - MUST BE SET IN EDITOR!
    [SerializeField] private Constants.Global.Side e_startSide;   // MUST BE SET IN EDITOR!
    private Constants.Global.Side e_currentSide;
    private int i_completionTimer;      // tracks time of potato being on opposite side
    private int i_destructionTimer;     // tracks time of potato being on team's side
    private Rigidbody rb;
    private RiftController riftController;     // reference to Rift singleton

    // Getters
    public Constants.Global.Side Side {
        get { return e_currentSide; }
    }

    /*/////////////////////////////////////////////////////////////////////////////////////////////////////////////*/

    public void UpdateCompletionTimer(int time) {
        i_completionTimer = time;
        hpo_owner.UpdateCompletionTimer(i_completionTimer);
    }

    public void UpdateDestructionTimer(int time) {
        i_destructionTimer = time;
        hpo_owner.UpdateDestructionTimer(i_destructionTimer);
    }

    public void ResetPotatoPosition() {
        if (e_color == Constants.Global.Color.RED) {
            transform.localPosition = Constants.ObjectiveStats.C_RedPotatoSpawn;
        }
        else {
            transform.localPosition = Constants.ObjectiveStats.C_BluePotatoSpawn;
        }
    }

    // Counts down to 0 while the potato is not on its original spawn side
        // Cumulative over the life of the objective
    private void CompletionTimerTick() {
        // account for this timer cycle
        i_completionTimer--;

        // set next invoke based on current side
        if (e_currentSide == e_startSide) {
            Invoke("DestructionTimerTick", 1);
        }
        else {
            Invoke("CompletionTimerTick", 1);
        }

        hpo_owner.UpdateCompletionTimer(i_completionTimer);
    }

    // Counts down to 0 while the potato is on its original spawn side
        // Reset any time the potato crosses to the other side
    private void DestructionTimerTick() {
        // Account for this timer cycle
        i_destructionTimer--;

        // Set next invoke based on current side
        if (e_currentSide == e_startSide) {
            Invoke("DestructionTimerTick", 1);
        }
        else {
            i_destructionTimer = Constants.ObjectiveStats.C_PotatoSelfDestructTimer;
            Invoke("CompletionTimerTick", 1);
        }

        hpo_owner.UpdateDestructionTimer(i_destructionTimer);
    }

    // Potato destroys itself, spawns enemies, and resets to original spawn position
    public void SelfDestruct() {
        // Spawn enemies at current location
        for (int i = 0; i < Constants.ObjectiveStats.C_EnemySpawnAmount; i++) {
            Debug.Log("Help.");
            riftController.CircularEnemySpawn(transform.position, e_startSide);
        }

        i_destructionTimer = Constants.ObjectiveStats.C_PotatoSelfDestructTimer;
    }

    // Changes potato side when crossing the Rift or through a portal
    private void ToggleSide() {
        if(e_currentSide == Constants.Global.Side.LEFT) {
            e_currentSide = Constants.Global.Side.RIGHT;
        }
        else if (e_currentSide == Constants.Global.Side.RIGHT) {
            e_currentSide = Constants.Global.Side.LEFT;
        }
    }

    /*/////////////////////////////////////////////////////////////////////////////////////////////////////////////*/

    void Start () {
        i_completionTimer = Constants.ObjectiveStats.C_PotatoCompletionTimer;     // cannot read from Constants.cs in initialization at top
        i_destructionTimer = Constants.ObjectiveStats.C_PotatoSelfDestructTimer;
        e_currentSide = e_startSide;
        Invoke("DestructionTimerTick", 1);
        rb = GetComponent<Rigidbody>();
        riftController = RiftController.Instance;
    }
	
	//// Check to see if the potato has changed sides
	//void Update () {
 //       if (transform.position.x < 0) {
 //           e_currentSide = Constants.Global.Side.LEFT;
 //       }
 //       else {
 //           e_currentSide = Constants.Global.Side.RIGHT;
 //       }
 //   }

    // Forces the potato over the Rift
    void OnTriggerEnter(Collider other) {
        if (other.CompareTag("Rift")) {
            transform.position = transform.position + (int)e_currentSide * 3f * Constants.RiftStats.C_RiftTeleportOffset;
            ToggleSide();
        }
        else if (other.CompareTag("Portal")) {
            ToggleSide();
        }
    }

    // Allows players to push the potato using Interact button
    void OnTriggerStay(Collider other) {
        if (other.CompareTag("InteractCollider")) {
            rb.isKinematic = false;
        }
        //else if (other.CompareTag("Rift")) {
        //    transform.position = transform.position + (int)e_currentSide * new Vector3(-3, 0, 0);
        //}
    }

    // Makes the potato unmovable when Interact button is released
    void OnTriggerExit(Collider other) {
        if (other.CompareTag("InteractCollider")) {
            rb.isKinematic = true;
        }
    }
}
