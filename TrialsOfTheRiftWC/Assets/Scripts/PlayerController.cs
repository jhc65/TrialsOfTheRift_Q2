using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Rewired;

public class PlayerController : MonoBehaviour{

    public GameObject go_playerCapsule;      // player main body
	public GameObject go_playerWisp;         // player wisp body
	public int i_playerNumber;				// designates player's number for controller mappings
	public Constants.Global.Color e_Color;			// identifies player's team
	public Constants.Global.Side e_Side;			// identifies which side of the rift player is on
	public Transform t_flagPos;				// location on character model of flag
	public GameObject go_flagObj;			// flag game object; if not null, player is carrying flag
	public GameObject go_interactCollider;  // activated with button-press to pickup flag
    public GameObject go_parryShield;       // activated with right stick button press
	public Transform t_spellSpawn;			// location spells are instantiated
	public float f_canMove;					// identifies if the player is frozen
	public GameObject go_magicMissileShot;  // wind spell object
	public GameObject go_windShot;			// wind spell object
	public GameObject go_iceShot;           // ice spell object
	public GameObject go_electricShot;      // ice spell object
    [SerializeField]private PlayerHUDController phc_hud;  //HUD object  

	public bool isWisp = false;

    private Player p_player;                // rewired player for input control
    private float f_nextWind;				// time next wind spell can be cast
	private float f_nextIce;                // time next ice spell can be cast
	private float f_nextElectric;           // time next ice spell can be cast
	private float f_nextMagicMissile;       // time next basic attack can be cast
    private float f_nextCast;               // time next spell in general can be cast. (not including MagicMissile)
	private float f_playerHealth;           // player's current health value
    public float f_projectileSize;          // size of player projectiles.
    private Color col_originalColor;        // Color of capsule.


	private void Move() {
        float f_inputX = p_player.GetAxis("MoveHorizontal");
        float f_inputZ = p_player.GetAxis("MoveVertical");
        float f_aimInputX = p_player.GetAxis("AimHorizontal");
        float f_aimInputZ = p_player.GetAxis("AimVertical");

        Vector3 v3_moveDir = new Vector3(f_inputX, 0, f_inputZ).normalized;
		Vector3 v3_aimDir = new Vector3(f_aimInputX, 0, f_aimInputZ).normalized;

		if (v3_aimDir.magnitude > 0) {
			transform.rotation = Quaternion.LookRotation(v3_aimDir);
		}

        if (isWisp) {
			GetComponent<Rigidbody>().velocity = (v3_moveDir * Constants.PlayerStats.C_WispMovementSpeed) * f_canMove;
		}
		else {
			GetComponent<Rigidbody>().velocity = (v3_moveDir * Constants.PlayerStats.C_MovementSpeed) * f_canMove;
		}
	}

	public void Freeze() {
		f_canMove = 0;
		Drop();
		Invoke("Unfreeze", Constants.SpellStats.C_IceFreezeTime);
	}

	private void Unfreeze() {
		f_canMove = 1;
    }

	public void Pickup(GameObject flag) {
		flag.transform.SetParent(t_flagPos);
		flag.transform.localPosition = new Vector3(0, 0, 0);
		go_flagObj = flag;
	}

	public void Drop() {
		if(go_flagObj) {
            //this value right here is where the flag is being dropped from the bug
            //tried to change it to the transform of the player, didn't really work, maybe
            //try getting player transform, but setting y to 0
			//go_flagObj.transform.localPosition = new Vector3(0.0f, -1.5f, 0.0f);	// this is relative to t_flagPos
			go_flagObj.transform.SetParent(null);
			go_flagObj.transform.localPosition = new Vector3(go_flagObj.transform.localPosition.x, 0.5f, go_flagObj.transform.localPosition.z);
			go_flagObj = null;
		}
	}

	public Constants.Global.Color GetColor() {
		return e_Color;
	}

    private void PlayerDeath()
    {
        Drop();
        TurnOff();
        isWisp = true;
        Debug.Log("Increase Volatility by 2.5%");
        RiftController.GetInstance().IncreaseVolatility(Constants.RiftStats.C_VolatilityIncrease_PlayerDeath);
        go_playerCapsule.SetActive(false);
		go_playerWisp.SetActive(true);
		f_nextWind = Time.time + (Constants.PlayerStats.C_RespawnTimer + 3.0f);
        f_nextIce = Time.time + (Constants.PlayerStats.C_RespawnTimer + 3.0f);
        go_playerCapsule.GetComponent<MeshRenderer>().material.color = col_originalColor;
        Invoke("PlayerRespawn", Constants.PlayerStats.C_RespawnTimer);
    }

    private void PlayerRespawn() {
		isWisp = false;
        go_playerCapsule.SetActive(true);
        go_playerWisp.SetActive(false);
        f_playerHealth = Constants.PlayerStats.C_MaxHealth;
        f_nextWind = Time.time;
        f_nextIce = Time.time;
    }

	public void TakeDamage(float damage) {
		if (!isWisp) {
			f_playerHealth -= damage;
            //Damage flicker goes here.
            DamageVisualOn();
            phc_hud.ShakeUI();
			if (f_playerHealth <= 0.0f) {
                PlayerDeath();
			}
		}
	}

    public void DamageVisualOn() {
        go_playerCapsule.GetComponent<MeshRenderer>().material.color = Color.yellow;
        //Call screenshake here.
        Invoke("DamageVisualOff", 0.1666f * 2);
    }

    public void DamageVisualOff() {
        go_playerCapsule.GetComponent<MeshRenderer>().material.color = col_originalColor;
    }

    public void HealVisualOn() {
        go_playerCapsule.GetComponent<MeshRenderer>().material.color = Color.green;
        //Call screenshake here.
        Invoke("HealVisualOff", 0.1666f * 2);
    }

    public void HealVisualOff() {
        go_playerCapsule.GetComponent<MeshRenderer>().material.color = col_originalColor;
    }

    public void Heal(float heal) {
        if (!isWisp) {
            print("heal" + heal);
            int tempHp = (int)(f_playerHealth + heal);
            if (tempHp >= Constants.PlayerStats.C_MaxHealth) {
                f_playerHealth = Constants.PlayerStats.C_MaxHealth;
            } else {
                f_playerHealth = tempHp;
            }
            HealVisualOn();
        }
    }

	// used by UI
    public float GetNextWind() {
        return f_nextWind;
    }

    public float GetNextIce() {
        return f_nextIce;
    }

	public float GetNextElectric() {
        return f_nextElectric;
    }

    public float GetCurrentHealth() {
        return f_playerHealth;
    }

    //public void SetCurrentHealth(float f_healthIn) {
    //    f_playerHealth = f_healthIn;
    //}

    void Awake() {
        p_player = ReInput.players.GetPlayer(i_playerNumber);
    }

    void Start() {
        f_playerHealth = Constants.PlayerStats.C_MaxHealth;
        col_originalColor = go_playerCapsule.GetComponent<MeshRenderer>().material.color;
		f_canMove = 1;

		f_nextMagicMissile = 0;
		f_nextWind = 0;
		f_nextIce = 0;
		f_nextElectric = 0;
		f_nextCast = 0;

		if (transform.position.x > 0)
			e_Side = Constants.Global.Side.RIGHT;
		else
			e_Side = Constants.Global.Side.LEFT;
	}

	void FixedUpdate() {
		Move();
		f_nextMagicMissile += Time.deltaTime;
		f_nextWind += Time.deltaTime;
        f_nextIce += Time.deltaTime;
		f_nextElectric += Time.deltaTime;
		f_nextCast += Time.deltaTime;
        f_projectileSize = Constants.SpellStats.C_PlayerProjectileSize;

		// spells
		if (!go_flagObj && !isWisp) {
            // Magic Missile
            if (p_player.GetButtonDown("MagicMissile") && f_nextMagicMissile > Constants.SpellStats.C_MagicMissileCooldown) {   // checks for fire button and if time delay has passed
                f_nextMagicMissile = 0;
				GameObject go_spell = Instantiate(go_magicMissileShot, t_spellSpawn.position, t_spellSpawn.rotation);
				go_spell.GetComponent<SpellController>().e_color = e_Color;
				go_spell.transform.localScale = new Vector3(f_projectileSize, f_projectileSize, f_projectileSize);
				go_spell.GetComponent<Rigidbody>().velocity = transform.forward * Constants.SpellStats.C_MagicMissileSpeed;
			}
            // Wind Spell
            if (p_player.GetButtonDown("WindSpell") && f_nextWind > Constants.SpellStats.C_WindCooldown && f_nextCast > Constants.SpellStats.C_NextSpellDelay) {   // checks for fire button and if time delay has passed
                f_nextWind = 0;
				f_nextCast = 0;
				GameObject go_spell = Instantiate(go_windShot, t_spellSpawn.position, t_spellSpawn.rotation);
				go_spell.GetComponent<SpellController>().e_color = e_Color;
				go_spell.transform.localScale = new Vector3(f_projectileSize, f_projectileSize, f_projectileSize);
				Debug.Log(transform.forward.normalized);
				go_spell.GetComponent<Rigidbody>().velocity = transform.forward * Constants.SpellStats.C_WindSpeed;
			}
            // Ice Spell
            if (p_player.GetButtonDown("IceSpell") && f_nextIce > Constants.SpellStats.C_IceCooldown && f_nextCast > Constants.SpellStats.C_NextSpellDelay) {   // checks for fire button and if time delay has passed
				f_nextIce = 0;
				f_nextCast = 0;
				GameObject go_spell = Instantiate(go_iceShot, t_spellSpawn.position, t_spellSpawn.rotation);
				go_spell.transform.localScale = new Vector3(f_projectileSize, f_projectileSize, f_projectileSize);
				go_spell.GetComponent<SpellController>().e_color = e_Color;
				go_spell.GetComponent<Rigidbody>().velocity = transform.forward * Constants.SpellStats.C_IceSpeed;
			}
            // Electric Spell
            if (p_player.GetButtonDown("ElectricitySpell") && f_nextElectric > Constants.SpellStats.C_ElectricCooldown && f_nextCast > Constants.SpellStats.C_NextSpellDelay) {   // checks for fire button and if time delay has passed
				f_nextElectric = 0;
				f_nextCast = 0;
				GameObject go_spell = Instantiate(go_electricShot, t_spellSpawn.position, t_spellSpawn.rotation);
				go_spell.transform.localScale = new Vector3(f_projectileSize, f_projectileSize, f_projectileSize);
				go_spell.GetComponent<SpellController>().e_color = e_Color;
				go_spell.GetComponent<Rigidbody>().velocity = transform.forward * Constants.SpellStats.C_ElectricSpeed;
			}
            //Parry
            if (p_player.GetButtonDown("Parry")) {
                go_parryShield.SetActive(true);
                Invoke("TurnOffParryShield", 0.25f);
            }
		}
	}

    private void TurnOffParryShield() {
        go_parryShield.SetActive(false);
    }

	void Update() {
        if (p_player.GetButtonDown("Interact") && !isWisp){
            if (go_flagObj) {
				Drop();
			}
			else {
                go_interactCollider.SetActive(true);
            }
		}

        if (p_player.GetButtonUp("Interact")){
            TurnOff();
        }

        if (transform.position.x > 0)
			e_Side = Constants.Global.Side.RIGHT;
		else
			e_Side = Constants.Global.Side.LEFT;
	}

    private void TurnOff()
    {
        go_interactCollider.transform.localPosition = new Vector3(go_interactCollider.transform.localPosition.x, -1000.0f, go_interactCollider.transform.localPosition.z);
        Invoke("MoveBack", 0.05f);
    }

    private void MoveBack() {
        go_interactCollider.SetActive(false);
        go_interactCollider.transform.localPosition = new Vector3(go_interactCollider.transform.localPosition.x, transform.position.y, go_interactCollider.transform.localPosition.z);
        
    }

	void OnTriggerEnter(Collider other) {
		if (other.tag == "Rift") {
			TakeDamage(f_playerHealth);
            while (!isWisp) {
                Debug.Log("I'm waiting for the player to be a wisp, because then they will have dropped the flag and I can move them across the rift.");
            }
			transform.position = transform.position + (int)e_Side * new Vector3(-2, 0, 0);
		}

        if (other.tag == "ParryShield")
            Physics.IgnoreCollision(GetComponent<Collider>(), other.gameObject.GetComponent<Collider>());
    }

    private void OnCollisionEnter(Collision collision)
    {
        if (collision.gameObject.tag == "Player") {
            //ignores any collision detection between any Player
            Physics.IgnoreCollision(GetComponent<Collider>(), collision.gameObject.GetComponent<Collider>());
        }
    }
}
