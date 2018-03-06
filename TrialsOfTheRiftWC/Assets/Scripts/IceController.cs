using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Rewired;

public class IceController : SpellController {

	private float f_iceDamage = Constants.SpellStats.C_IceDamage;
    [SerializeField]private GameObject go_iceWall;
    [SerializeField]private GameObject go_WallSpawn;
    private Player p_player;
    private Rigidbody rb_body;

    //TODO: explore this more:
    /*
	// SendMessage() calls all functions named the parameter that exist in MonoBehavior 
    // scripts on the GameObject. This way, we don't have worry about differentiating 
    // between freezing a player or freezing an enemy. It'll just find the function
    // named Freeze() on the GameObject's controller script.
    protected override void ApplyEffect(GameObject go_target){
		go_target.SendMessage("Freeze"); 
	}
	*/

    protected override void OnCollisionEnter(Collision collision) {
		//Debug.Log("Impact:" + coll.gameObject.tag);
		foreach (string tag in s_spellTargetTags) {
			if (collision.gameObject.tag == tag && collision.gameObject != pc_owner.gameObject) {
				ApplyEffect(collision.gameObject, collision);
                
                //makes the potato stop moving after the spell has applied its affect
                //it moves the spell like this to hide it from view so it doesn't affect anyone on the field
                //I really hate this, but its the only good way for now
                if (collision.gameObject.tag == "Potato")
                {
                    coll = collision;
                    this.transform.localPosition = new Vector3(this.transform.localPosition.x, -1000.0f, this.transform.localPosition.z);
                    Invoke("TurnKinematicOn", 0.05f);
                }
                else if (collision.gameObject.tag != "Wall")
                {
                    pc_owner.b_iceboltMode = false;
                    Destroy(gameObject);
                }

				return;
			}
		}

        if (collision.gameObject.tag == "Spell") {
            Constants.Global.Color spellColor = collision.gameObject.GetComponent<SpellController>().e_color;
            if (spellColor != e_color)
            {
                pc_owner.b_iceboltMode = false;
                Destroy(gameObject);
            }
            else {
                //ignores any collision detection between the two spells
                Physics.IgnoreCollision(GetComponent<Collider>(), collision.gameObject.GetComponent<Collider>());
            }
        }
		else if (collision.gameObject.tag != "Portal") { // If we hit something not a player, rift, or portal (walls), just destroy the shot without an effect.
            pc_owner.b_iceboltMode = false;
			Destroy(gameObject);
        }
	}

    protected override void Start() {
        rb_body = GetComponent<Rigidbody>();
        Invoke("InvokeDestroy", Constants.SpellStats.C_IceLiveTime);
        InvokeRepeating("MakeWall", 0.15f, 0.15f);
    }

    void MakeWall() {
        Instantiate(go_iceWall, go_WallSpawn.transform.position, go_WallSpawn.transform.rotation);
    }

    protected override void ApplyEffect(GameObject go_target, Collision collision) {
        if (go_target.tag == "Player")
        {
            go_target.GetComponent<PlayerController>().Freeze();
            go_target.GetComponent<PlayerController>().TakeDamage(f_iceDamage * Constants.SpellStats.C_IcePlayerDamageMultiplier,Constants.Global.DamageType.ICE);
        }
        else if (go_target.tag == "Enemy")
        {
            go_target.GetComponent<EnemyController>().TakeDamage(f_iceDamage);
            go_target.GetComponent<EnemyController>().Freeze(0f);
        }
        else if (go_target.tag == "RiftBoss")
        {
            if (go_target.gameObject.GetComponent<RiftBossController>().Color == e_color) {
                go_target.GetComponent<RiftBossController>().TakeDamage(f_iceDamage * Constants.SpellStats.C_IcePlayerDamageMultiplier);
            }
        }
        else if (go_target.tag == "Crystal")
        {
            Constants.Global.Color crystalColor = go_target.GetComponent<CrystalController>().Color;
			if (crystalColor != e_color){
				go_target.GetComponent<CrystalController>().UpdateCrystalHealth(Constants.SpellStats.C_SpellCrystalDamagePercent);
			}
			else if (crystalColor == e_color) {
				go_target.GetComponent<CrystalController>().UpdateCrystalHealth(Constants.SpellStats.C_SpellCrystalHealPercent);
			}
        }
    }

    private void FixedUpdate() {
        float f_inputX = p_player.GetAxis("AimHorizontal");
        float f_inputZ = p_player.GetAxis("AimVertical");
        Vector3 v3_dir = new Vector3(f_inputX, 0, f_inputZ).normalized;
        if (!(v3_dir.Equals(Vector3.zero))) {
            transform.rotation = Quaternion.LookRotation(v3_dir);
        }
        rb_body.velocity = transform.forward * Constants.SpellStats.C_IceSpeed;
    }

    protected override void BuffSpell() {
        // Increase Volatility by 0.5%
        RiftController.Instance.IncreaseVolatility(Constants.RiftStats.C_VolatilityIncrease_SpellCross);
        f_iceDamage = f_iceDamage * Constants.SpellStats.C_IceRiftDamageMultiplier;
        transform.localScale *= Constants.SpellStats.C_SpellScaleMultiplier;
    }

    public void SetPlayer(PlayerController pc_in) {
        p_player = ReInput.players.GetPlayer(pc_in.i_playerNumber);
    }

    public override void Charge(float f_chargeTime) {
        //This shouldn't be being called.
        Debug.Log("You fucked up somewhere.");
    }
}

