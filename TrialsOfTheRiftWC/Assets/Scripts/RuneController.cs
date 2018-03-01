/*  Rune Controller - Noah Nam and Jeff Brown
 * 
 *  Desc:   Instantiate an explosin prefab on trigger
 * 
 */
 
using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class RuneController : MonoBehaviour {
	private bool activated = false;
	[SerializeField] private GameObject go_explosionPrefab;

	void Update() {

		if (activated) {
			Invoke("Deactivate", Constants.EnemyStats.C_RuneExplosionCountDownTime);
			activated = false;
		}
	}

    void OnTriggerEnter(Collider other)
    {
        if(other.CompareTag("Player") || other.CompareTag("Enemy")) {
            activated = true;
        }
    }

	void Deactivate() {
		Instantiate(go_explosionPrefab, transform.position, Quaternion.identity);
		gameObject.SetActive(false);
    }
}