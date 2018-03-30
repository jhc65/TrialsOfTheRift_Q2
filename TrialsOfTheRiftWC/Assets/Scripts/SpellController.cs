/*  Spell Controller - Zak Olyarnik
 * 
 *  Desc:   Parent class of all in-game spells
 * 
 */

using UnityEngine;

[RequireComponent(typeof(Rigidbody))]
[RequireComponent(typeof(SphereCollider))]
public abstract class SpellController : MonoBehaviour {
#region Variables and Declarations
    [SerializeField] protected Constants.SpellStats.SpellType e_spellType;
    [SerializeField] protected Rigidbody rb;
    [SerializeField] protected GameObject go_onDestroyParticles;
    protected Constants.Global.Color e_color;
	protected float f_damage;
    protected float f_charge = 1;         // charging multiplier
    protected PlayerController pc_owner;      // owner of the spell
    protected RiftController riftController;    // reference to Rift singleton

    #region Getters and Setters
    public Constants.Global.Color Color{
        get { return e_color; }
    }
    #endregion
#endregion

#region SpellController Shared Methods
    protected abstract void Charge(float f_chargeTime);
    protected abstract void BuffSpell();

    public void Init(PlayerController owner, Constants.Global.Color color, float chargeTime) {
        pc_owner = owner;
        e_color = color;
        Charge(chargeTime);
    }

    void InvokeDestroy() {
		Destroy(gameObject);
	}
#endregion

#region Unity Overrides
    protected virtual void Start() {
        riftController = RiftController.Instance;
		Invoke("InvokeDestroy", Constants.SpellStats.C_SpellLiveTime);
	}

	protected virtual void OnCollisionEnter(Collision collision) {
        SpellTarget target;
        if(target = collision.gameObject.GetComponent<SpellTarget>()) {
            target.ApplySpellEffect(e_spellType, e_color, f_damage, transform.forward.normalized);
        }

        if (collision.gameObject.CompareTag("Spell")) {
            Constants.Global.Color spellColor = collision.gameObject.GetComponent<SpellController>().e_color;
            if (spellColor != e_color) {    // opposing spells destroy each other
                Destroy(gameObject);
            }
            else {              // ignore collisions between spells of the same color
                Physics.IgnoreCollision(GetComponent<Collider>(), collision.gameObject.GetComponent<Collider>());
            }
        } 
        else {  // destroy spell on collision with anything else (including specific spell target objects above, once the effect has happened) (Rift and portal interactions are controlled by OnTriggerEnter, below)
            Destroy(gameObject);
        }        
	}

	protected virtual void OnTriggerEnter(Collider other) {
		if (other.CompareTag("Rift")) {	    // Rift reacts to spells by trigger rather than collision
			CancelInvoke();     // cancels and restarts spell's live timer
			BuffSpell();
			Invoke("InvokeDestroy", Constants.SpellStats.C_SpellLiveTime);
        }

        if (other.CompareTag("ParryShield")) {
            CancelInvoke();     // cancels and restarts spell's live timer
            Invoke("InvokeDestroy", Constants.SpellStats.C_SpellLiveTime);

            // deflect spell in player's facing direction
            Vector3 v3_direction = other.gameObject.transform.forward.normalized;
            transform.Rotate(v3_direction);
            rb.velocity = v3_direction * rb.velocity.magnitude;
        }
    }
#endregion
}
