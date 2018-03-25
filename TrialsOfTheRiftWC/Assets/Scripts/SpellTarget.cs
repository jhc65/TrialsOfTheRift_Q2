/*  Spell Target - Zak Olyarnik
 * 
 *  Desc:   Parent class of Player, Enemy, and Objective interactables - anything that can be hit by a Spell
 * 
 */

using System.Collections;
using UnityEngine;

public abstract class SpellTarget : MonoBehaviour {
#region Variables and Declarations
    [SerializeField] protected Constants.Global.Color e_color;  // identifies owning team
    [SerializeField] protected Constants.Global.Side e_startSide;
    [SerializeField] protected Rigidbody rb;
    [SerializeField] protected Animator anim;
    protected float f_health;
    protected float f_speed;
    protected Coroutine cor_AOECoroutine;
    protected Maestro maestro;                  // reference to audio controller singleton
    protected RiftController riftController;    // reference to Rift singleton

    #region Getters and Setters
    public Constants.Global.Color Color {
        get { return e_color; }
    }

    public Coroutine AOECoroutine {
        set { cor_AOECoroutine = value; }
    }
    #endregion
#endregion

#region SpellTarget Shared Methods
    public abstract void ApplySpellEffect(Constants.SpellStats.SpellType spell, Constants.Global.Color color, float damage, Vector3 direction);

    public virtual void NegateSpellEffect(Constants.SpellStats.SpellType spell) {
        if (spell == Constants.SpellStats.SpellType.ELECTRICITYAOE) {
            StopCoroutine(cor_AOECoroutine);
        }
    }

    public virtual IEnumerator ApplyAOE(Constants.Global.Color color, float damage) {
		if (gameObject && gameObject.activeSelf) {
            ApplySpellEffect(Constants.SpellStats.SpellType.ELECTRICITYAOE, color, damage, Vector3.zero);
            yield return new WaitForSeconds(Constants.SpellStats.C_ElectricAOEDamageRate);
            cor_AOECoroutine = StartCoroutine(ApplyAOE(color, damage));
        }
        else {  // hopefully this is not malicious
            NegateSpellEffect(Constants.SpellStats.SpellType.ELECTRICITYAOE);
        }
	}
#endregion
}
