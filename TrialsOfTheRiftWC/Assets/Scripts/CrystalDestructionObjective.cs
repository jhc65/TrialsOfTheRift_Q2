using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CrystalDestructionObjective : Objective {

	public GameObject go_redCrystal, go_blueCrystal;	// referenced crystal objects
	private GameObject go_activeCrystal;	// active object specific to this objective instance

	override public void Instantiate() {
		// instantiate prefab based on color
		if (e_color == Constants.Global.Color.RED) {
			go_activeCrystal = Instantiate(go_blueCrystal, Constants.ObjectiveStats.C_BlueCrystalSpawn, new Quaternion(0, 0, 0, 0));
		}
		else{
			go_activeCrystal = Instantiate(go_redCrystal, Constants.ObjectiveStats.C_RedCrystalSpawn, new Quaternion(0, 0, 0, 0));
		}
	}

	override public void Complete() {
		// destroy prefab
		b_complete = true;
		GameController.GetInstance().CrystalHealth(go_activeCrystal.GetComponent<CrystalController>().e_color, Constants.ObjectiveStats.C_CrystalMaxHealth);
		Destroy(go_activeCrystal);
	}

	void Update() {
        if (go_activeCrystal.GetComponent<CrystalController>().i_health <= 0) {
            Complete();
        }
	}

    // [Param Fix] - Used in Parameters Screen. Will be removed in main game (probably)
    public override void ParamReset(float param_in) {
        go_activeCrystal.GetComponent<CrystalController>().i_health = (int)param_in;
        GameController.GetInstance().CrystalHealth(go_activeCrystal.GetComponent<CrystalController>().e_color, (int)param_in);
    }
}
