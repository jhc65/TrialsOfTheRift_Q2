/*  Rune Explosion Controller - Jeff Brown
 * 
 *  Desc:   Damages Players on contact
 * 
 */

using UnityEngine;

public class RuneExplosionController : MonoBehaviour {
#region Unity Overrides
    void Start() {
		Destroy(gameObject, Constants.EnemyStats.C_RuneExplosionLiveTime);
	}

    void OnTriggerEnter(Collider other) {
        if(other.CompareTag("Player")) {
            other.GetComponent<PlayerController>().TakeDamage(Constants.EnemyStats.C_RuneDamage, Constants.Global.DamageType.RUNE);
        }
    }
#endregion
}