/*  Hot Potato Controller - Dana Thompson
 * 
 *  Desc:   Facilitates movement and timers of Hot Potato Objective's Hot Potato object
 * 
 */

using UnityEngine;

public class HotPotatoController : SpellTarget {
#region Variables and Declarations
    [SerializeField] private HotPotatoObjective hpo_owner;  // identifies objective potato is a part of
    private Constants.Global.Side e_currentSide;
    private int i_completionTimer;      // tracks time of potato being on opposite side
    private int i_destructionTimer;     // tracks time of potato being on team's side
#endregion

#region HotPotatoController Methods
    override public void ApplySpellEffect(Constants.SpellStats.SpellType spell, Constants.Global.Color color, float damage, Vector3 direction) {
        Debug.Log("here1");
        if (spell == Constants.SpellStats.SpellType.WIND) {
            Debug.Log("here2");
            rb.isKinematic = false;
            rb.AddForce(direction * Constants.SpellStats.C_WindForce * 3.0f);
            Invoke("TurnKinematicOn", 0.05f);
        }
    }

    // Makes the potato kinematic again
    private void TurnKinematicOn() {
        rb.isKinematic = true;
    }

    public void UpdateCompletionTimer(int time) {
        i_completionTimer = time;
        hpo_owner.UpdateCompletionTimer(i_completionTimer);
    }

    public void UpdateDestructionTimer(int time) {
        i_destructionTimer = time;
        hpo_owner.UpdateDestructionTimer(i_destructionTimer);
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

    // Potato spawns enemies at its current position
    public void SelfDestruct() {
        // Spawn enemies at current location
        for (int i = 0; i < Constants.ObjectiveStats.C_EnemySpawnAmount; i++) {
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
#endregion

#region Unity Overrides
    void Start () {
        i_completionTimer = Constants.ObjectiveStats.C_PotatoCompletionTimer;     // cannot read from Constants.cs in initialization at top
        i_destructionTimer = Constants.ObjectiveStats.C_PotatoSelfDestructTimer;
        e_currentSide = e_startSide;
        Invoke("DestructionTimerTick", 1);
        riftController = RiftController.Instance;
    }

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
    }

    // Makes the potato unmovable when Interact button is released
    void OnTriggerExit(Collider other) {
        if (other.CompareTag("InteractCollider")) {
            rb.isKinematic = true;
        }
    }
#endregion
}
