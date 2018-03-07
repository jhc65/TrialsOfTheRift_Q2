using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Rewired;
using UnityEngine.SceneManagement;

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
    //[SerializeField]private PlayerHUDController phc_hud;    //HUD object  
    [SerializeField] private PauseController pauc_pause;     //For Pausing.
    [SerializeField] private GameObject go_deathOrbPrefab;

    public bool isWisp = false;

    private Player p_player;                // rewired player for input control
    private float f_nextWind;				// time next wind spell can be cast
	private float f_nextIce;                // time next ice spell can be cast
	private float f_nextElectric;           // time next ice spell can be cast
	private float f_nextMagicMissile;       // time next basic attack can be cast

    public bool b_iceboltMode = false;     // player is controlling an icebolt.
    private GameObject go_icebolt;          // The icebolt the player is controlling.

    private float f_mmCharge;               // current charge of magic missile.
    private float f_nextMmCharge;           // time till next charge shot can be fired.
    private float f_iceCharge;              // current charge of ice spell.
    private float f_windCharge;             // current charge of wind spell.
    private float f_electricCharge;         // current charge of electric charge.

    private float f_nextCast;               // time next spell in general can be cast. (not including MagicMissile)
	private float f_playerHealth;           // player's current health value
    public float f_projectileSize;          // size of player projectiles.
    //private Color col_originalColor;        // Color of capsule.
	private Maestro maestro;				// Reference to Maestro singleton.

	[SerializeField] private Animator anim;

	private bool b_stepOk;
	private float f_stepDelay = 0.4f;

    // Getters
    public Animator Animator {
        get { return anim; }
    }

    private void Move() {
        float f_inputX = p_player.GetAxis("MoveHorizontal");
        float f_inputZ = p_player.GetAxis("MoveVertical");
        float f_aimInputX = p_player.GetAxis("AimHorizontal");
        float f_aimInputZ = p_player.GetAxis("AimVertical");
		//float f_lookDirection;

        Vector3 v3_moveDir = new Vector3(f_inputX, 0, f_inputZ).normalized;
		Vector3 v3_aimDir = new Vector3(f_aimInputX, 0, f_aimInputZ).normalized;
		//f_lookDirection = f_inputX + f_aimInputX;

		anim.SetFloat ("runSpeed", v3_moveDir.magnitude);
		anim.SetFloat ("lookDirection", v3_aimDir.magnitude);


		if (v3_aimDir.magnitude > 0) {
			transform.rotation = Quaternion.LookRotation(v3_aimDir);
		}

        if (isWisp) {
			GetComponent<Rigidbody>().velocity = (v3_moveDir * Constants.PlayerStats.C_WispMovementSpeed) * f_canMove;
		}
		else {
			GetComponent<Rigidbody>().velocity = (v3_moveDir * Constants.PlayerStats.C_MovementSpeed) * f_canMove;
			if(v3_moveDir.magnitude > 0 && b_stepOk){
				b_stepOk = false;
				maestro.PlayPlayerFootstep();
				maestro.PlayPlayerClothing();
			}
			
		}
	}

	public void Freeze() {
		f_canMove = 0;
		DropFlag();
		anim.SetTrigger ("freezeTrigger");
		anim.SetBool ("freezeBool", true);
		Invoke("Unfreeze", Constants.SpellStats.C_IceFreezeTime);
	}

	private void Unfreeze() {
		f_canMove = 1;
		anim.SetBool ("freezeBool", false);
    }

	public void Pickup(GameObject flag) {
        if (!isWisp)
        {
            flag.transform.SetParent(t_flagPos);
            flag.transform.localPosition = new Vector3(0, 0, 0);
            go_flagObj = flag;
        }
	}

	public void DropFlag() {
		if(go_flagObj) {
            //this value right here is where the flag is being dropped from the bug
            //tried to change it to the transform of the player, didn't really work, maybe
            //try getting player transform, but setting y to 0
            //go_flagObj.transform.localPosition = new Vector3(0.0f, -1.5f, 0.0f);	// this is relative to t_flagPos

            go_flagObj.GetComponent<FlagController>().DropFlag();
			go_flagObj = null;
		}
	}

	public Constants.Global.Color GetColor() {
		return e_Color;
	}

    private void PlayerDeath()
    {
		maestro.PlayAnnouncementWispGeneric();
        DropFlag();
        TurnOff();
        isWisp = true;
        if(SceneManager.GetActiveScene().name != "WarmUp") {
            Debug.Log("Increase Volatility by 2.5%");
            RiftController.Instance.IncreaseVolatility(Constants.RiftStats.C_VolatilityIncrease_PlayerDeath);
        } 
		maestro.PlayPlayerDie();
        go_playerCapsule.SetActive(false);
		go_playerWisp.SetActive(true);
		f_nextWind = Time.time + (Constants.PlayerStats.C_RespawnTimer + 3.0f);
        f_nextIce = Time.time + (Constants.PlayerStats.C_RespawnTimer + 3.0f);
        //go_playerCapsule.GetComponent<MeshRenderer>().material.color = col_originalColor;
        Invoke("PlayerRespawn", Constants.PlayerStats.C_RespawnTimer);
    }

    private void PlayerRespawn() {
		isWisp = false;
		maestro.PlayPlayerSpawn();
        go_playerCapsule.SetActive(true);
        go_playerWisp.SetActive(false);
        f_playerHealth = Constants.PlayerStats.C_MaxHealth;
        f_nextWind = Time.time;
        f_nextIce = Time.time;
    }

	public void TakeDamage(float damage, Constants.Global.DamageType d) {
		if (!isWisp) {
			maestro.PlayAnnouncmentPlayerHit(i_playerNumber,d);
			maestro.PlayPlayerHit();
			f_playerHealth -= damage;
            //Damage flicker goes here.
            DamageVisualOn();
            //phc_hud.ShakeUI();
			if (f_playerHealth <= 0.0f) {
                PlayerDeath();
			}
		}
	}

    public void DamageVisualOn() {
 //       go_playerCapsule.GetComponent<MeshRenderer>().material.color = Color.yellow;
        //Call screenshake here.
        Invoke("DamageVisualOff", 0.1666f * 2);
    }

    public void DamageVisualOff() {
        //go_playerCapsule.GetComponent<MeshRenderer>().material.color = col_originalColor;
    }

    public void HealVisualOn() {
 //       go_playerCapsule.GetComponent<MeshRenderer>().material.color = Color.green;
        //Call screenshake here.
        Invoke("HealVisualOff", 0.1666f * 2);
    }

    public void HealVisualOff() {
        //go_playerCapsule.GetComponent<MeshRenderer>().material.color = col_originalColor;
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

    //void Awake() {
    //    p_player = ReInput.players.GetPlayer(i_playerNumber);
    //}

    void Start() {
		b_stepOk = true;
		InvokeRepeating("StepDelay",f_stepDelay,f_stepDelay);
        p_player = ReInput.players.GetPlayer(i_playerNumber);
        f_playerHealth = Constants.PlayerStats.C_MaxHealth;
        //col_originalColor = go_playerCapsule.GetComponent<MeshRenderer>().material.color;
		f_canMove = 1;

		f_nextMagicMissile = 0;
		f_nextWind = 0;
		f_nextIce = 0;
		f_nextElectric = 0;
		f_nextCast = 0;
        b_iceboltMode = false;
		
		maestro = Maestro.Instance;     // reference to Rift singleton

		if (transform.position.x > 0)
			e_Side = Constants.Global.Side.RIGHT;
		else
			e_Side = Constants.Global.Side.LEFT;
	}

	void FixedUpdate() {
        if (!b_iceboltMode) {
            Move();
            f_nextIce += Time.deltaTime;
        }

		f_nextMagicMissile += Time.deltaTime;
		f_nextWind += Time.deltaTime;
		f_nextElectric += Time.deltaTime;
		f_nextCast += Time.deltaTime;
        f_nextMmCharge += Time.deltaTime;
        f_projectileSize = Constants.SpellStats.C_PlayerProjectileSize;


        // pause
        if (p_player.GetButtonDown("Menu") && Time.timeScale == 1) {
            pauc_pause.Pause(this);
        }

        // spells
        if (p_player.GetButtonUp("IceSpell")) {
            b_iceboltMode = false;
            Destroy(go_icebolt);
        }
		
		if (!go_flagObj && !isWisp) {
            // Magic Missile (Auto-fire)
            if (f_nextMagicMissile > Constants.SpellStats.C_MagicMissileCooldown) {   // checks for fire button and if time delay has passed
                if (p_player.GetButtonTimePressed("MagicMissile") != 0) {
                    f_mmCharge += p_player.GetButtonTimePressed("MagicMissile");
                }
                if (p_player.GetButton("MagicMissile")) {
					maestro.PlayMagicMissileShoot();
					anim.SetTrigger ("attackTrigger");
                    f_nextMagicMissile = 0;
				    GameObject go_spell = Instantiate(go_magicMissileShot, t_spellSpawn.position, t_spellSpawn.rotation);
				    SpellController sc_firing = go_spell.GetComponent<SpellController>();
                    sc_firing.e_color = e_Color;
				    go_spell.transform.localScale = new Vector3(f_projectileSize, f_projectileSize, f_projectileSize);
				    go_spell.GetComponent<Rigidbody>().velocity = transform.forward * Constants.SpellStats.C_MagicMissileSpeed;
                    sc_firing.Charge(0);
                    sc_firing.pc_owner = this;

                }                
			}
            // Charged Magic Missile (Release)
            if (p_player.GetButtonUp("MagicMissile") && f_nextMmCharge > Constants.SpellStats.C_MagicMissileChargeCooldown) {
				maestro.PlayMagicMissileShoot();
                f_nextMmCharge = 0;
				GameObject go_spell = Instantiate(go_magicMissileShot, t_spellSpawn.position, t_spellSpawn.rotation);
				SpellController sc_firing = go_spell.GetComponent<SpellController>();
                sc_firing.e_color = e_Color;
				go_spell.transform.localScale = new Vector3(f_projectileSize, f_projectileSize, f_projectileSize);
				go_spell.GetComponent<Rigidbody>().velocity = transform.forward * Constants.SpellStats.C_MagicMissileSpeed;
                sc_firing.Charge(f_mmCharge);
                sc_firing.pc_owner = this;
                f_mmCharge = 0;
            }
            // Wind Spell
            if (f_nextWind > Constants.SpellStats.C_WindCooldown && f_nextCast > Constants.SpellStats.C_NextSpellDelay) {   // checks for fire button and if time delay has passed
                if (p_player.GetButtonTimePressed("WindSpell") != 0) {
                    f_windCharge += p_player.GetButtonTimePressed("WindSpell");
					anim.SetTrigger ("windChargeTrigger");
					anim.SetFloat ("windCharge", f_windCharge);
                }
                if (p_player.GetButtonUp("WindSpell")) {
					maestro.PlayWindShoot();
					anim.SetTrigger("windSpellTrigger");
                    f_nextWind = 0;
				    f_nextCast = 0;
                    for (int i = -30; i <= 30; i += 30) {
                        GameObject go_spell = Instantiate(go_windShot, t_spellSpawn.position, t_spellSpawn.rotation);
                        SpellController sc_firing = go_spell.GetComponent<SpellController>();
                        sc_firing.e_color = e_Color;
                        go_spell.transform.eulerAngles = go_spell.transform.eulerAngles + new Vector3(0,i,0);
                        Debug.Log(go_spell.transform.forward);
                        go_spell.transform.localScale = new Vector3(f_projectileSize, f_projectileSize, f_projectileSize);
                        go_spell.GetComponent<Rigidbody>().velocity = go_spell.transform.forward * Constants.SpellStats.C_WindSpeed;
                        sc_firing.Charge(f_windCharge);
                        sc_firing.pc_owner = this;
                    }
                    f_windCharge = 0;
					anim.SetFloat ("windCharge", f_windCharge);
				} 
                
			}
            // Ice Spell
            if (f_nextIce > Constants.SpellStats.C_IceCooldown && f_nextCast > Constants.SpellStats.C_NextSpellDelay) {   // checks for fire button and if time delay has passed
                if (p_player.GetButtonDown("IceSpell")) {
					maestro.PlayIceShoot();
                    b_iceboltMode = true;
                    f_nextIce = 0;
                    f_nextCast = 0;
                    GameObject go_spell = Instantiate(go_iceShot, t_spellSpawn.position, t_spellSpawn.rotation);
                    go_spell.transform.localScale = new Vector3(f_projectileSize, f_projectileSize, f_projectileSize);
                    IceController sc_firing = go_spell.GetComponent<IceController>();
                    sc_firing.SetPlayer(this);
                    sc_firing.pc_owner = this;
                    sc_firing.e_color = e_Color;
                    go_spell.GetComponent<Rigidbody>().velocity = transform.forward * Constants.SpellStats.C_IceSpeed;
                    go_icebolt = go_spell;
                }
				
			}
            // Electric Spell
            if (f_nextElectric > Constants.SpellStats.C_ElectricCooldown && f_nextCast > Constants.SpellStats.C_NextSpellDelay) {   // checks for fire button and if time delay has passed
                if (p_player.GetButtonTimePressed("ElectricitySpell") != 0) {
                    f_electricCharge += p_player.GetButtonTimePressed("ElectricitySpell");
					anim.SetFloat ("gooCharge", f_electricCharge);
                }
                if (p_player.GetButtonUp("ElectricitySpell")) {
					maestro.PlayElectricShoot();
					anim.SetTrigger ("goospellTrigger");
                    f_nextElectric = 0;
				    f_nextCast = 0;
				    GameObject go_spell = Instantiate(go_electricShot, t_spellSpawn.position, t_spellSpawn.rotation);
				    go_spell.transform.localScale = new Vector3(f_projectileSize, f_projectileSize, f_projectileSize);
				    SpellController sc_firing = go_spell.GetComponent<SpellController>();
                    sc_firing.e_color = e_Color;
				    go_spell.GetComponent<Rigidbody>().velocity = transform.forward * Constants.SpellStats.C_ElectricSpeed;
                    sc_firing.Charge(f_electricCharge);
                    f_electricCharge = 0;
                    sc_firing.pc_owner = this;
					anim.SetFloat ("gooCharge", f_electricCharge);
                }
                
			}
            //Parry
            if (p_player.GetButtonDown("Parry"))
            {
                go_parryShield.SetActive(true);
                Invoke("TurnOffParryShield", 0.25f);
            }
        }
	}

    private void TurnOffParryShield()
    {
        go_parryShield.SetActive(false);
    }

    void Update() {
        if (p_player.GetButtonDown("Interact") && !isWisp){
            if (go_flagObj) {
				DropFlag();
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

    private void TeleportPlayer() {
        transform.position = transform.position + (int)e_Side * Constants.RiftStats.C_RiftTeleportOffset;
        go_deathOrbPrefab.gameObject.SetActive(false);
        gameObject.SetActive(true);
    }

	void OnTriggerEnter(Collider other) {
		if (other.tag == "Rift") {
            DropFlag();
			TakeDamage(f_playerHealth,Constants.Global.DamageType.RIFT);
            while (!isWisp) {
                Debug.Log("I'm waiting for the player to be a wisp, because then they will have dropped the flag and I can move them across the rift.");
            }

            //moves the death orb assigned to the position they were swallowed to indicate they are in the Rift
            go_deathOrbPrefab.transform.position = new Vector3(0.0f, transform.position.y, transform.position.z);
            go_deathOrbPrefab.gameObject.SetActive(true);
            gameObject.SetActive(false);
            Invoke("TeleportPlayer", Constants.RiftStats.C_RiftTeleportDelay);
		}
    }

    private void OnCollisionEnter(Collision collision)
    {
        if (collision.gameObject.tag == "Player") {
            //ignores any collision detection between any Player
            Physics.IgnoreCollision(GetComponent<Collider>(), collision.gameObject.GetComponent<Collider>());
        }
    }
	
	private void StepDelay(){
		b_stepOk = true;
	}
}
