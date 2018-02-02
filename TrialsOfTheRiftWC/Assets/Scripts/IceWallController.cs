using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class IceWallController : MonoBehaviour {

    private float f_iceDamage;

	// Use this for initialization
	void Start () {
		Destroy(gameObject,5f);
	}

}
