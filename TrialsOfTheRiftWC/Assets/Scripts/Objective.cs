using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public abstract class Objective : MonoBehaviour {

	public Constants.Color e_color;		// identifies owning team
	public int i_numberInList;			// this is the i'th objective faced by this team (1-based)
	public bool b_complete;				// set when criteria to beat the objective is met

	public void Set(Constants.Color c, int n) {
		e_color = c;
		i_numberInList = n;
		Instantiate();
	}

	public abstract void Instantiate();
	public abstract void Complete();
    public abstract void ParamReset(float param_in);
}
