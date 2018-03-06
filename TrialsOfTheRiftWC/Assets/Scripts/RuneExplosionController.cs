/*  Rune Explosion Controller - Jeff Brown
 * 
 *  Desc:   Harms players that collide with this
 * 
 */

using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class RuneExplosionController : MonoBehaviour {

    void OnTriggerEnter(Collider other)
    {
        if(other.CompareTag("Player"))
        {
            other.gameObject.GetComponent<PlayerController>().TakeDamage(Constants.EnemyStats.C_RuneDamage,Constants.Global.DamageType.RUNE);
        }
    }

	void Start() {
		Destroy(gameObject, 1.5f);
	}
} 