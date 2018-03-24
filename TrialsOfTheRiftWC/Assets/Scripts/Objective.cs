/*  Objective - Zak Olyarnik
 * 
 *  Desc:   Parent class of all in-game Objectives.
 * 
 */

using UnityEngine;

public abstract class Objective : MonoBehaviour {
#region Variables and Declarations
    [SerializeField] protected Constants.Global.Color e_color;  // identifies owning team
    [SerializeField] protected GameObject[] go_roomPool;        // rooms allowed for this objective to be played in

    protected GameObject go_activeRoom;     // active room specific to this objective instance 
    protected int i_numberInList;           // this is the i'th objective faced by this team (1-based)
    protected bool b_isComplete = false;    // set when criteria to beat the objective is met
    protected int i_score;                  // current progress towards i_maxScore (currently used by CTF and Hockey)

    protected Calligrapher calligrapher;        // reference to UI controller singleton
	protected Maestro maestro;                  // reference to audio controller singleton
    protected RiftController riftController;    // reference to rift singleton

    #region Getters and Setters
    public Constants.Global.Color Color {
        get { return e_color; }
    }
    public int NumberInList {
        get { return i_numberInList; }
    }
    public bool IsComplete {
        get { return b_isComplete; }
    }
    #endregion
#endregion

#region Objective Shared Methods
    protected abstract void SetUI();
    protected abstract void ResetUI();

    // Activates all aspects of this objective
    public Objective Activate(int i) {
        calligrapher = Calligrapher.Instance;       // set singletons
        maestro = Maestro.Instance;
        riftController = RiftController.Instance;
        b_isComplete = false;                       // initialize variables
        i_numberInList = i;
        SetUI();                                    // set UI
        calligrapher.RoomUpdate(e_color, i_numberInList);
        calligrapher.Flash(e_color);
        go_activeRoom = SelectRoom();               // set room
        go_activeRoom.SetActive(true);
        gameObject.SetActive(true);                 // finally, turn on objective
        return this;
    }

    // Destroy this objective once it is complete
    public void Complete() {
        maestro.PlayAnnouncementTrialTransition();
        riftController.IncrementObjectiveCount(e_color);
        riftController.IncreaseVolatility(Constants.RiftStats.C_VolatilityIncrease_RoomAdvance);
        ResetUI();                              // turn off UI
        go_activeRoom.SetActive(false);         // turn off room
        Destroy(gameObject);                    // each objective is only played once, so destroy after use
    }

    // Randomly selects a room from the list of approved rooms for this objective
    private GameObject SelectRoom() {
        int i = Random.Range(0, go_roomPool.Length);
        return go_roomPool[i];
    }
#endregion
}
