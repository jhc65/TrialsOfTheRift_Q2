/*  Rune Controller - Noah Nam and Jeff Brown
 * 
 *  Desc:   Instantiates a delayed explosion prefab after triggered
 * 
 */
 
using UnityEngine;

public class RuneController : MonoBehaviour {
#region Variables and Declarations
    [SerializeField] private GameObject go_explosionPrefab;
    private bool activated = false;
#endregion
#region RuneController Methods
    void Explode() {
        Instantiate(go_explosionPrefab, transform.position, Quaternion.identity);
        activated = false;
        gameObject.SetActive(false);
    }
#endregion
#region Unity Overrides
    void OnTriggerEnter(Collider other) {
        if ((other.CompareTag("Player") || other.CompareTag("Enemy")) && !activated) {
            activated = true;
            Invoke("Explode", Constants.EnemyStats.C_RuneExplosionCountDownTime);
        }
    }
#endregion

	void OnDisable() {
		CancelInvoke();
	}
}