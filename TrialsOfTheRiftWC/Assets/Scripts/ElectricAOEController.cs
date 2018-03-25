/*  Electric Spell Controller - Dana Thompson
 * 
 *  Desc:   Facilitates Electric Spell interactions (up to AOE spawn)
 * 
 */
 
using UnityEngine;

public class ElectricAOEController : MonoBehaviour {
#region Variables and Declarations
    public Constants.Global.Color e_color;
	public float f_damage;
#endregion

#region Electric AOE Controller Methods
    public void Init(Constants.Global.Color color, float damage) {
        e_color = color;
        f_damage = damage;
    }

    private void InvokeDestroy() {
        transform.position = new Vector3(transform.position.x, -500f, transform.position.z);
        Destroy(gameObject, 0.1f);
    }
#endregion
    
#region Unity Overrides
    void Start () {
		Invoke("InvokeDestroy", Constants.SpellStats.C_ElectricAOELiveTime);
	}
	
	void OnTriggerEnter(Collider other) {
        SpellTarget target;
        if (target = other.GetComponent<SpellTarget>()) {
            target.AOECoroutine = StartCoroutine(target.ApplyAOE(e_color, f_damage));
        }
    }

	void OnTriggerExit(Collider other) {
        SpellTarget target;
        if (target = other.GetComponent<SpellTarget>()) {
            target.NegateSpellEffect(Constants.SpellStats.SpellType.ELECTRICITYAOE);
        }
	}
#endregion
}
