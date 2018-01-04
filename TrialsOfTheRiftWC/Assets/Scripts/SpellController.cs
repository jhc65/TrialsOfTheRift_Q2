using System.Collections;
using System.Collections.Generic;
using UnityEngine;
[RequireComponent(typeof(Rigidbody))]
[RequireComponent(typeof(SphereCollider))]
public abstract class SpellController : MonoBehaviour {

    public Constants.Color e_color;
	public float f_damage;			// currently unused, as each individual spell reads its damage value from Constants.cs in Start()
	public string[] s_spellTargetTags; // these are the tags of the objects spells should do damage/effect against

	protected abstract void BuffSpell();
	protected abstract void ApplyEffect(GameObject go_target);

    private Collision coll;  //used to turn the potato objective kinematic back on

	protected virtual void Start() {
		//Destroy(gameObject, Constants.SpellStats.C_SpellLiveTime);
		Invoke("InvokeDestroy", Constants.SpellStats.C_SpellLiveTime);
	}

	protected virtual void OnCollisionEnter(Collision collision) {
		//Debug.Log("Impact:" + coll.gameObject.tag);
		foreach (string tag in s_spellTargetTags) {
			if (collision.gameObject.tag == tag) {
				ApplyEffect(collision.gameObject);

                //makes the potato stop moving after the spell has applied its affect
                //it moves the spell like this to hide it from view so it doesn't affect anyone on the field
                //I really hate this, but its the only good way for now
                if (collision.gameObject.tag == "Potato")
                {
                    coll = collision;
                    this.transform.localPosition = new Vector3(this.transform.localPosition.x, -1000.0f, this.transform.localPosition.z);
                    Invoke("TurnKinematicOn", 0.05f);
                }
                else
                {
                    Destroy(gameObject);
                }

				return;
			}
		}

        if (collision.gameObject.tag == "Spell") {
            Constants.Color spellColor = collision.gameObject.GetComponent<SpellController>().e_color;
            if (spellColor != e_color)
            {
                Destroy(gameObject);
            }
            else {
                //ignores any collision detection between the two spells
                Physics.IgnoreCollision(GetComponent<Collider>(), collision.gameObject.GetComponent<Collider>());
            }
        }
		else if (collision.gameObject.tag != "Portal") { // If we hit something not a player, rift, or portal (walls), just destroy the shot without an effect.
			Destroy(gameObject);
        }
	}

    //makes the potato objective kinematic again
    private void TurnKinematicOn() {
        coll.gameObject.GetComponent<Rigidbody>().isKinematic = true;
        Destroy(gameObject);
    }

	protected virtual void OnTriggerEnter(Collider other) {	// rift reacts to spells by trigger rather than collision
		if (other.tag == "Rift"){
			CancelInvoke(); //If it's the rift cancel the first invoke
			BuffSpell();
			Invoke("InvokeDestroy", 1.07f); //Call another invoke but with enough time to travel 2/3's of the other side, (1.07 is a derived time)
		}
	}

	void InvokeDestroy() {
		Destroy(gameObject);
	}
}
