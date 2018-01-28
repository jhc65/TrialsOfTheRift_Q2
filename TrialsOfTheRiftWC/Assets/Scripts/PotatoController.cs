using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PotatoController : MonoBehaviour {

    public Constants.Global.Color e_color;     // identifies owning team
    public Constants.Global.Side e_currentSide;
    public Constants.Global.Color e_currentColor;  //the side the crystal is currently in
    private int i_completionTimer = Constants.ObjectiveStats.C_PotatoCompletionTimer;      //tracks progress of objective being in enemy territory
    private int i_selfDestructTimer = Constants.ObjectiveStats.C_PotatoSelfDestructTimer;    //threshold for objective to disappear and spawn enemies

    private Rigidbody rb;

    // Use this for initialization
    void Start () {
        Invoke("DestructionTime", 1);
        rb = GetComponent<Rigidbody>();
        rb.isKinematic = true;
        rb.constraints = RigidbodyConstraints.FreezePositionY;
    }
	
	// Update is called once per frame and checks to see if the crystal has changed sides
	void Update () {
        float position = transform.position.x;

        if (position < 0)
        {
            e_currentSide = Constants.Global.Side.LEFT;
            e_currentColor = Constants.Global.Color.RED;
        }
        else
        {
            e_currentSide = Constants.Global.Side.RIGHT;
            e_currentColor = Constants.Global.Color.BLUE;
        }
    }

    public int getCompletionTimer() {
        return i_completionTimer;
    }

    public int getSelfDestructTimer() {
        return i_selfDestructTimer;
    }

    public void setCompletionTimer(int time) {
        i_completionTimer = time;
    }

    //makes the timer count up to the completion timer threshold
    //moves to DestructionTime if in own room
    //resets the value of selfDestructTimer when objective crosses to enemy territory
    private void CompletionTime() {
        if (e_currentColor == e_color)
        {
            CancelInvoke();
            Invoke("DestructionTime", 1);
        }
        else
        {
            i_completionTimer--;
            GameController.GetInstance().CompletionProgress(e_color, i_completionTimer);
            Invoke("CompletionTime", 1);
        }
    }

    //makes the selfDestructTimer count down until
    //this value is reset if it starts to tick back up again
    private void DestructionTime() {
        if (e_currentColor == e_color)
        {
            i_selfDestructTimer--;
            GameController.GetInstance().SelfDestructProgress(e_color, i_selfDestructTimer);
            Invoke("DestructionTime", 1);
        }
        else
        {
            CancelInvoke();
            i_selfDestructTimer = Constants.ObjectiveStats.C_PotatoSelfDestructTimer;
            GameController.GetInstance().SelfDestructProgress(e_color, i_selfDestructTimer);
            Invoke("CompletionTime", 1);
        }
    }

    //if it enters the rift, force it to the opposite side
    private void OnTriggerEnter(Collider other)
    {
        if (other.tag == "Rift")
        {
            transform.position = transform.position + (int)e_currentSide * new Vector3(-3, 0, 0);
        }
    }

    //allows players to move the objective farther
    private void OnTriggerStay(Collider other)
    {
        if (other.tag == "InteractCollider")
        {
            rb.isKinematic = false;
        }

        if (other.tag == "Rift")
        {
            transform.position = transform.position + (int)e_currentSide * new Vector3(-3, 0, 0);
        }
    }

    //makes the object unmoveable again
    private void OnTriggerExit(Collider other)
    {
        if (other.tag == "InteractCollider")
        {
            rb.isKinematic = true;
        }
    }
}
