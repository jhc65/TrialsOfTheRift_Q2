using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class RiftController : MonoBehaviour {
    private static RiftController rc_instance;
    private float f_volatility;
    private float f_volatilityMultiplier;
    private Constants.RiftStats.Volatility V_currentVolatilityLevel = Constants.RiftStats.Volatility.ZERO;
    private float f_enemyStartingSpeed;

    public GameObject go_RiftMagicMissile;
    public GameObject[] go_PlayerReferences;
    public GameObject go_PocketRift;
    public Vector3[] v3_PocketRiftLocations;
    public Camera mainCamera;

    public static RiftController GetInstance()
    {
        return rc_instance;
    }

    void Awake() {
        if (rc_instance == null) {
            rc_instance = this;
        }

        if (rc_instance != null && rc_instance != this) {
            Debug.Log("Destroying RC");
            Destroy(this);
        }
    }

    void Start() {
        if (rc_instance == null) {
            rc_instance = this;
        }

        if (rc_instance != null && rc_instance != this) {
            Debug.Log("Destroying RC");
            Destroy(this);
        }

        f_enemyStartingSpeed = Constants.EnviroStats.C_EnemySpeed;
    }

    public void IncreaseVolatility(float volatilityUp) {
        Debug.Log("Volatility incrased!");

        volatilityUp += (volatilityUp * f_volatilityMultiplier);
        f_volatility += volatilityUp;

        if (f_volatility >= 100.0f && V_currentVolatilityLevel != Constants.RiftStats.Volatility.ONEHUNDRED) {
            V_currentVolatilityLevel = Constants.RiftStats.Volatility.ONEHUNDRED;
            Invoke("Restart", Constants.RiftStats.C_Volatility_ResetTime);
            //InvertScreen();
            HighDamageAttack();
        }
        else if (f_volatility >= 75.0f && V_currentVolatilityLevel != Constants.RiftStats.Volatility.SEVENTYFIVE) {
            // Change Rift material to L4
            V_currentVolatilityLevel = Constants.RiftStats.Volatility.SEVENTYFIVE;
            f_volatilityMultiplier = Constants.RiftStats.C_VolatilityMultiplier_L4;
            InvertControls();
        }
        else if (f_volatility >= 65.0f && V_currentVolatilityLevel != Constants.RiftStats.Volatility.SIXTYFIVE) {
            //SpawnPocketRifts();
            V_currentVolatilityLevel = Constants.RiftStats.Volatility.SIXTYFIVE;
            f_volatilityMultiplier = Constants.RiftStats.C_VolatilityMultiplier_L3;
            SpawnFiveSkeletons();
        }
        else if (f_volatility >= 50.0f && V_currentVolatilityLevel != Constants.RiftStats.Volatility.FIFTY) {
            // Change Rift material to L3
            V_currentVolatilityLevel = Constants.RiftStats.Volatility.FIFTY;
            f_volatilityMultiplier = Constants.RiftStats.C_VolatilityMultiplier_L3;
            SpawnNecromancers();
        }
        else if (f_volatility >= 35.0f && V_currentVolatilityLevel != Constants.RiftStats.Volatility.THIRTYFIVE) {
            V_currentVolatilityLevel = Constants.RiftStats.Volatility.THIRTYFIVE;
            f_volatilityMultiplier = Constants.RiftStats.C_VolatilityMultiplier_L2;
            EnhanceEnemies();
        }
        else if (f_volatility >= 25.0f && V_currentVolatilityLevel != Constants.RiftStats.Volatility.TWENTYFIVE) {
            // Change Rift material to L2
            V_currentVolatilityLevel = Constants.RiftStats.Volatility.TWENTYFIVE;
            f_volatilityMultiplier = Constants.RiftStats.C_VolatilityMultiplier_L2;
            //FireSpells();
        }
        else if (f_volatility >= 5.0f && V_currentVolatilityLevel != Constants.RiftStats.Volatility.FIVE) {
            // Change Rift material to L1
            V_currentVolatilityLevel = Constants.RiftStats.Volatility.FIVE;
            f_volatilityMultiplier = Constants.RiftStats.C_VolatilityMultiplier_L1;
            InvokeRepeating("SpawnEnemies", 0.0f, Constants.RiftStats.C_Volatility_EnemySpawnTimer);
        }
        else if (f_volatility < 5.0f) {
            CancelInvoke("SpawnEnemies");
            ResetEnemies();
            V_currentVolatilityLevel = Constants.RiftStats.Volatility.ZERO;
        }
    }

    public void SpawnEnemies() {
        // Spawn 2 enemies on each side. Idk, only 1 enemy per side seems low and the GDD does not have a number *shrugs*
        for (int i = 0; i < 2; i++) {
            DarkMagician.GetInstance().SpawnEnemies();
        }
        Debug.Log("Rift spawned enemies!");
    }

    public void FireSpells() {
        float f_projectileSize = Constants.SpellStats.C_PlayerProjectileSize;
        List<GameObject> go_riftSpells = new List<GameObject>();

        for (int i = 0; i < 4; i++) {
            GameObject go_spell = Instantiate(go_RiftMagicMissile, gameObject.transform.position, gameObject.transform.rotation);
            go_spell.transform.localScale = new Vector3(f_projectileSize, f_projectileSize, f_projectileSize);
            go_spell.GetComponent<Rigidbody>().velocity = transform.forward * Constants.SpellStats.C_MagicMissileSpeed;

            go_riftSpells.Add(Instantiate(go_RiftMagicMissile, gameObject.transform.position, gameObject.transform.rotation));
        }

        go_riftSpells[0].GetComponent<Rigidbody>().velocity = go_PlayerReferences[0].transform.position * Constants.SpellStats.C_MagicMissileSpeed;
        go_riftSpells[1].GetComponent<Rigidbody>().velocity = go_PlayerReferences[1].transform.position * Constants.SpellStats.C_MagicMissileSpeed;
        go_riftSpells[2].GetComponent<Rigidbody>().velocity = go_PlayerReferences[2].transform.position * Constants.SpellStats.C_MagicMissileSpeed;
        go_riftSpells[3].GetComponent<Rigidbody>().velocity = go_PlayerReferences[3].transform.position * Constants.SpellStats.C_MagicMissileSpeed;
    }

    public void EnhanceEnemies() {
        Constants.EnviroStats.C_EnemySpeed += 1.0f;
        Debug.Log("Rift Boosted Enemies!");
    }

    public void ResetEnemies() {
        Constants.EnviroStats.C_EnemySpeed = f_enemyStartingSpeed;
    }

    public void SpawnNecromancers() {
        Debug.Log("Spawn necromancers when we have them");
    }

    public void SpawnPocketRifts() {

    }

    public void SpawnFiveSkeletons() {
        for (int i = 0; i < 5; i++) {
            DarkMagician.GetInstance().SpawnEnemies();
        }
        Debug.Log("Rift spawned 5 skeletons!");
    }

    public void HighDamageAttack() {
        // if 0, attack RED. if 1, attack BLUE
        int colorToAttack = Random.Range(0, 1);

        if (colorToAttack == 0) {
            foreach (GameObject player in go_PlayerReferences) {
                if (player.GetComponent<PlayerController>().GetColor() == Constants.Color.RED) {
                    player.GetComponent<PlayerController>().TakeDamage((Constants.PlayerStats.C_MaxHealth * 0.25f));
                }
            }
        }
        else if (colorToAttack == 1) {
            foreach (GameObject player in go_PlayerReferences) {
                if (player.GetComponent<PlayerController>().GetColor() == Constants.Color.BLUE) {
                    player.GetComponent<PlayerController>().TakeDamage((Constants.PlayerStats.C_MaxHealth * 0.25f));
                }
            }
        }

        Debug.Log("Rift did a high-damage attack!");
    }

    public void InvertControls() {

    }

    void ReverseInvertControls() {

    }

    public void InvertScreen() {
        mainCamera.transform.Rotate(0.0f, 0.0f, 180.0f);
        Invoke("ReverseInvertScreen", Constants.RiftStats.C_Volatility_CameraFlipTime);
    }

    void ReverseInvertScreen() {
        mainCamera.transform.Rotate(0.0f, 0.0f, 180.0f);
    }

    void Restart() {
        f_volatility = 0.0f;
        V_currentVolatilityLevel = Constants.RiftStats.Volatility.ZERO;
    }

    void OnTriggerEnter(Collider other) {
		if (other.tag == "Player") {
			other.GetComponent<PlayerController>().TakeDamage(Constants.PlayerStats.C_MaxHealth);
			other.transform.position = other.transform.position + (int)other.GetComponent<PlayerController>().e_Side * new Vector3(-4, 0, 0);
		}
	}
}
