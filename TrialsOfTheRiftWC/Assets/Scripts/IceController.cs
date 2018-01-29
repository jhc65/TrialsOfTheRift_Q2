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

    protected override void Start() {
        rb_body = GetComponent<Rigidbody>();
        Invoke("InvokeDestroy", Constants.SpellStats.C_IceLiveTime);
        InvokeRepeating("MakeWall", 0.3f, 0.3f);
    }

    void MakeWall() {
        Instantiate(go_iceWall, go_WallSpawn.transform.position, go_WallSpawn.transform.rotation);
    }

    protected override void ApplyEffect(GameObject go_target, Collision collision) {
        if (go_target.tag == "Player")
        {
            go_target.GetComponent<PlayerController>().Freeze();
            go_target.GetComponent<PlayerController>().TakeDamage(f_iceDamage * Constants.SpellStats.C_IcePlayerDamageMultiplier);
        }
        else if (go_target.tag == "Enemy")
        {
            go_target.GetComponent<EnemyController>().TakeDamage(f_iceDamage);
            go_target.GetComponent<EnemyController>().Freeze(0f);
        }
        else if (go_target.tag == "Crystal")
        {
            Constants.Global.Color crystalColor = go_target.GetComponent<CrystalController>().e_color;
			if (crystalColor != e_color){
				go_target.GetComponent<CrystalController>().ChangeHealth(Constants.SpellStats.C_SpellCrystalDamagePercent);
			}
			else if (crystalColor == e_color) {
				go_target.GetComponent<CrystalController>().ChangeHealth(Constants.SpellStats.C_SpellCrystalHealPercent);
			}
        }
    }

    private void FixedUpdate() {
        float f_inputX = p_player.GetAxis("AimHorizontal");
        float f_inputZ = p_player.GetAxis("AimVertical");
        Vector3 v3_dir = new Vector3(f_inputX, 0, f_inputZ).normalized;
        rb_body.velocity = v3_dir * Constants.SpellStats.C_IceSpeed;

    }

    protected override void BuffSpell() {
        // Increase Volatility by 0.5%
        RiftController.GetInstance().IncreaseVolatility(Constants.RiftStats.C_VolatilityIncrease_SpellCross);
        f_iceDamage = f_iceDamage * Constants.SpellStats.C_IceRiftDamageMultiplier;
        transform.localScale += new Vector3(0.5f, 0.5f, 0.5f);
    }

    public void SetPlayer(PlayerController pc_in) {
        p_player = ReInput.players.GetPlayer(pc_in.i_playerNumber);
    }

    public override void Charge(float f_chargeTime) {
        //This shouldn't be being called.
        Debug.Log("You fucked up somewhere.");
    }
}

