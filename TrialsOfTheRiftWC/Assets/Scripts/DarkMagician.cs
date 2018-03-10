/*  Dark Magician - Zak Olyarnik
 * 
 *  Desc:   GameController functionality - Facilitates Objective switching and checking for win condition.
 * 
 */

using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;
using UnityEngine.UI;

public sealed class DarkMagician : MonoBehaviour {

    [SerializeField] private Objective[] objv_redObjectiveList;
    [SerializeField] private Objective[] objv_blueObjectiveList;
    public Objective objv_currentRedObjective, objv_currentBlueObjective;   // goddamn debug parameters TODO: change to private for release

    [SerializeField] private Text txt_winMsg;
    [SerializeField] private Button butt_winSelect;
    [SerializeField] private GameObject go_paraButton;
	private bool b_gameOver;
    private RiftController riftController;     // reference to Rift singleton
	protected Maestro maestro;

    /*/////////////////////////////////////////////////////////////////////////////////////////////////////////////*/

    // Shuffles the order of both red and blue objective lists in parallel
    private void ShuffleObjectives() {
        for (int i = 0; i < objv_redObjectiveList.Length - 1; i++) {
            Objective tmp1 = objv_redObjectiveList[i];
            Objective tmp2 = objv_blueObjectiveList[i];
            int j = Random.Range(i, objv_redObjectiveList.Length - 1);
            objv_redObjectiveList[i] = objv_redObjectiveList[j];
            objv_blueObjectiveList[i] = objv_blueObjectiveList[j];
            objv_redObjectiveList[j] = tmp1;
            objv_blueObjectiveList[j] = tmp2;
        }
    }

    private void GetNextObjective(Constants.Global.Color c, int objectiveNumber) {
        // check for game end
        if (objectiveNumber == objv_redObjectiveList.Length) {
			b_gameOver = true;
			txt_winMsg.text = c + " team won!";
			return;
		}
        objectiveNumber++;

        if (c == Constants.Global.Color.RED) {
            objv_currentRedObjective.Complete();
            objv_currentRedObjective = objv_redObjectiveList[objectiveNumber-1].Activate(objectiveNumber);	// objectiveNumber starts with 1 but array is 0-based
        }
        else if (c == Constants.Global.Color.BLUE) {
            objv_currentBlueObjective.Complete();
            objv_currentBlueObjective = objv_blueObjectiveList[objectiveNumber - 1].Activate(objectiveNumber);  // objectiveNumber starts with 1 but array is 0-based
        }
    }

    /*/////////////////////////////////////////////////////////////////////////////////////////////////////////////*/

    void Awake() {  // parameter screen dictates that we do this before its Start() is called

		txt_winMsg.enabled = false;
		b_gameOver = false;
		ShuffleObjectives();
        //Time.timeScale = 0;

        objv_currentRedObjective = objv_redObjectiveList[0].Activate(1);
        objv_currentBlueObjective = objv_blueObjectiveList[0].Activate(1);
    }

    void Start() {
        riftController = RiftController.Instance;
		maestro = Maestro.Instance;
    }

    void Update() {
        // Dev shortcut TODO: remove in release
        if (Input.GetKeyDown(KeyCode.Escape)) {
            Application.Quit();
        }
        if (Input.GetKeyDown(KeyCode.Space)) {
            GetNextObjective(objv_currentRedObjective.Color, objv_currentRedObjective.NumberInList);
        }

        // check for completion of objectives
        if (b_gameOver) {
			txt_winMsg.enabled = true;
            go_paraButton.SetActive(false);
            butt_winSelect.gameObject.SetActive(true);
            butt_winSelect.Select();
			Time.timeScale = 0;
		}
		else {
			if (objv_currentRedObjective.IsComplete) {
                GetNextObjective(objv_currentRedObjective.Color, objv_currentRedObjective.NumberInList);
			}
			if(objv_currentBlueObjective.IsComplete) {
                GetNextObjective(objv_currentBlueObjective.Color, objv_currentBlueObjective.NumberInList);
            }
		}
	}
}
