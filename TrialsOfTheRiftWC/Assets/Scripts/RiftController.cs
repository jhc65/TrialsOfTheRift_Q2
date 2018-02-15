/*  Rift Controller - Joe Chew
 * 
 *  Desc:   Facilitates Rift volatility and enemy spawn.
 * 
 */

using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

public sealed class RiftController : MonoBehaviour {

    [SerializeField] private GameObject go_riftDeathBolt;
    public GameObject[] go_playerReferences;
    //public GameObject go_PocketRift;
    //public Vector3[] v3_PocketRiftLocations;
    //public Camera mainCamera;

    // enemies
    [SerializeField] private GameObject go_enemyPrefab;
    private int i_leftEnemies = 0;
    private int i_rightEnemies = 0;
    private Vector3[] v3_rightEnemySpawnPositions = new Vector3[] {
        new Vector3(18f, 0.5f, 0f),
        new Vector3(18f, 0.5f, -16.5f),
        new Vector3(18f, 0.5f, 14f),
        new Vector3(5f, 0.5f, 14f),
        new Vector3(5f, 0.5f, -16.5f)
    };
    private Vector3[] v3_leftEnemySpawnPositions = new Vector3[] {
        new Vector3(-18f, 0.5f, 0f),
        new Vector3(-18f, 0.5f, -16.5f),
        new Vector3(-18f, 0.5f, 14f),
        new Vector3(-5f, 0.5f, 14f),
        new Vector3(-5f, 0.5f, -16.5f)
    };
    private float f_enemySpeed;

    private float f_volatility;
    private float f_volatilityMultiplier;
    private Constants.RiftStats.Volatility e_currentVolatilityLevel;
    private Maestro maestro;     // reference to audio singleton

    // Singleton
    private static RiftController instance;
    public static RiftController Instance {
        get { return instance; }
    }

    // Getters
    public GameObject[] PlayerReferences {
        get { return go_playerReferences; }
    }

    /*/////////////////////////////////////////////////////////////////////////////////////////////////////////////*/

    public void IncreaseVolatility(float volatilityUp) {
        Debug.Log("Volatility increased!");

        volatilityUp += (volatilityUp * f_volatilityMultiplier);
        f_volatility += volatilityUp;

        if (f_volatility >= 100.0f && e_currentVolatilityLevel != Constants.RiftStats.Volatility.ONEHUNDRED) {
            e_currentVolatilityLevel = Constants.RiftStats.Volatility.ONEHUNDRED;
            Invoke("ResetVolatility", Constants.RiftStats.C_VolatilityResetTime);
            BoardClear();
            //InvertScreen();
        }
        else if (f_volatility >= 75.0f && e_currentVolatilityLevel != Constants.RiftStats.Volatility.SEVENTYFIVE) {
            e_currentVolatilityLevel = Constants.RiftStats.Volatility.SEVENTYFIVE;
            EnterNewVolatilityLevel(4);
            //InvertControls();
        }
        else if (f_volatility >= 65.0f && e_currentVolatilityLevel != Constants.RiftStats.Volatility.SIXTYFIVE) {
            e_currentVolatilityLevel = Constants.RiftStats.Volatility.SIXTYFIVE;
            EnterNewVolatilityLevel(3);
            for (int i = 0; i < 5; i++) {
                SpawnEnemies();
            }
            //SpawnPocketRifts();
        }
        else if (f_volatility >= 50.0f && e_currentVolatilityLevel != Constants.RiftStats.Volatility.FIFTY) {
            e_currentVolatilityLevel = Constants.RiftStats.Volatility.FIFTY;
            EnterNewVolatilityLevel(3);
            //SpawnNecromancers();
        }
        else if (f_volatility >= 35.0f && e_currentVolatilityLevel != Constants.RiftStats.Volatility.THIRTYFIVE) {
            e_currentVolatilityLevel = Constants.RiftStats.Volatility.THIRTYFIVE;
            EnterNewVolatilityLevel(2);
            f_enemySpeed += 1.0f;
        }
        else if (f_volatility >= 25.0f && e_currentVolatilityLevel != Constants.RiftStats.Volatility.TWENTYFIVE) {
            e_currentVolatilityLevel = Constants.RiftStats.Volatility.TWENTYFIVE;
            EnterNewVolatilityLevel(2);
            FireDeathBolts(Constants.Global.Color.RED);
            FireDeathBolts(Constants.Global.Color.BLUE);
        }
        else if (f_volatility >= 5.0f && e_currentVolatilityLevel != Constants.RiftStats.Volatility.FIVE) {
            e_currentVolatilityLevel = Constants.RiftStats.Volatility.FIVE;
            EnterNewVolatilityLevel(1);
            InvokeRepeating("SpawnEnemies", 0.0f, Constants.RiftStats.C_VolatilityEnemySpawnTimer);
        }
        else if (f_volatility < 5.0f) {
            EnterNewVolatilityLevel(0);
        }
    }

    private void EnterNewVolatilityLevel(int i) {
        maestro.PlayVolatility(i);
        switch (i) {
            case 0:
                // Change rift visual to L0
                e_currentVolatilityLevel = Constants.RiftStats.Volatility.ZERO;
                f_volatilityMultiplier = Constants.RiftStats.C_VolatilityMultiplier_L1;     // there is no L0, L1 is already 0
                CancelInvoke("SpawnEnemies");
                f_enemySpeed = Constants.EnemyStats.C_EnemyBaseSpeed;
                break;
            case 1:
                // Change rift visual to L1
                f_volatilityMultiplier = Constants.RiftStats.C_VolatilityMultiplier_L1;
                break;
            case 2:
                // Change rift visual to L2
                f_volatilityMultiplier = Constants.RiftStats.C_VolatilityMultiplier_L2;
                break;
            case 3:
                // Change rift visual to L3
                f_volatilityMultiplier = Constants.RiftStats.C_VolatilityMultiplier_L3;
                break;
            case 4:
                // Change rift visual to L4
                f_volatilityMultiplier = Constants.RiftStats.C_VolatilityMultiplier_L4;
                break;
        }
    }

    public void ResetVolatility() {
        f_volatility = 0.0f;
        EnterNewVolatilityLevel(0);
    }

    //----------------------------
    // Rift Effects
    private void BoardClear() {
        foreach (GameObject player in go_playerReferences) {
            player.GetComponent<PlayerController>().TakeDamage(Constants.PlayerStats.C_MaxHealth);
        }

        /* Do shit with the runes once I get Jeff and Dana's shit in
        */

        GameObject[] go_allEnemies = GameObject.FindGameObjectsWithTag("Enemy");
        for (int i = 0; i < go_allEnemies.Length; i++) {
            Destroy(go_allEnemies[i]);
        }
    }

    //TODO: revisit enemy spawn with pooling

    // Spawns one enemy on either side of the Rift, randomly chosen position
    public void SpawnEnemies() {
        int randLeft = Random.Range(0, v3_leftEnemySpawnPositions.Length);
        int randRight = Random.Range(0, v3_rightEnemySpawnPositions.Length);

        if (i_leftEnemies < Constants.EnemyStats.C_EnemySpawnCapPerSide) {
            GameObject leftEnemy = Instantiate(go_enemyPrefab, v3_leftEnemySpawnPositions[randLeft], Quaternion.identity);
            leftEnemy.GetComponent<EnemyController>().e_Side = Constants.Global.Side.LEFT;  //TODO: is there a better way to set-up enemies?
            leftEnemy.GetComponent<NavMeshAgent>().speed = f_enemySpeed;
            leftEnemy.GetComponent<MeleeController>().SetHealth(Constants.EnemyStats.C_EnemyHealth);
            i_leftEnemies++;
        }
        if (i_rightEnemies < Constants.EnemyStats.C_EnemySpawnCapPerSide) {
            GameObject rightEnemy = Instantiate(go_enemyPrefab, v3_rightEnemySpawnPositions[randRight], Quaternion.identity);
            rightEnemy.GetComponent<EnemyController>().e_Side = Constants.Global.Side.RIGHT;
            rightEnemy.GetComponent<NavMeshAgent>().speed = f_enemySpeed;
            rightEnemy.GetComponent<MeleeController>().SetHealth(Constants.EnemyStats.C_EnemyHealth);
            i_rightEnemies++;
        }
    }

    // Spawns an enemy at a specified position
    public void SpawnEnemy(Vector3 position, Constants.Global.Side side) {
        // only spawn if below enemy side cap TODO: is this expected behavior?
        if ((side == Constants.Global.Side.LEFT && i_leftEnemies < Constants.EnemyStats.C_EnemySpawnCapPerSide) ||
            (side == Constants.Global.Side.RIGHT && i_rightEnemies < Constants.EnemyStats.C_EnemySpawnCapPerSide)) {
            GameObject enemy = Instantiate(go_enemyPrefab, position, Quaternion.identity);
            enemy.GetComponent<EnemyController>().e_Side = side;        //TODO: is there a better way to set-up enemies?
            enemy.GetComponent<NavMeshAgent>().speed = f_enemySpeed;
            enemy.GetComponent<MeleeController>().SetHealth(Constants.EnemyStats.C_EnemyHealth);
        }
    }

    // Spawns an enemy within a radius when a valid position is selected
    public void CircularEnemySpawn(Vector3 center, Constants.Global.Side side) {
        Vector3 spawnPos = RandomCircle(center, side, 3.0f);

        // Checks to see if the spawn position is already occupied by anything with a collider
            // If it is, find a new spawn position for the enemy
        var hitColliders = Physics.OverlapSphere(spawnPos, 0.005f);
        if (hitColliders.Length > 0) {
            CircularEnemySpawn(center, side);
        }
        else {
            SpawnEnemy(spawnPos, side);
        }
    }

    // Decrease enemy count per side on enemy death
    public void DecreaseEnemies(Constants.Global.Side side) {
        if (side == Constants.Global.Side.LEFT) {
            i_leftEnemies--; }
        else {
            i_rightEnemies--;
        }
    }

    // Gets a random Vector3 position within a given radius
    private Vector3 RandomCircle(Vector3 center, Constants.Global.Side side, float radius) {
        float ang = Random.value * 360;
        Vector3 pos;

        // by absolute valueing the x position, we can tell the enemy which side it should of the map it should be on
        int s = (int)side;
        pos.x = s * Mathf.Abs(center.x + radius * Mathf.Sin(ang * Mathf.Deg2Rad));
        pos.y = center.y;
        pos.z = center.z + radius * Mathf.Cos(ang * Mathf.Deg2Rad);

        // Reposition the enemies if they spawn outside of the map TODO: revisit once map is scaled, and these should be Constants anyway
        if (pos.z >= 22) {
            float diff = pos.z - 22;
            pos.z = pos.z - diff - 1;
        }
        else if (pos.z <= -22) {
            Debug.Log(pos.z);
            float diff = pos.z + 22;
            pos.z = pos.z - diff + 1;
            Debug.Log(pos.z);
        }

        if (pos.x >= 40) {
            float diff = pos.x - 40;
            pos.x = pos.x - diff - 1;
        }
        else if (pos.x <= -40) {
            float diff = pos.x + 40;
            pos.x = pos.x - diff + 1;
        }

        return pos;
    }

    // @Joe get this working
    // Joe: NO BITCH, I DO WHAT I WANT! >:(
    public void FireDeathBolts(Constants.Global.Color c) {
     // Only shoot at players of Color c, and don't collide with anything EXCEPT players (layer matrix, probably)

        float f_projectileSize = Constants.SpellStats.C_PlayerProjectileSize;
        List<GameObject> go_riftSpells = new List<GameObject>();

        for (int i = 0; i < 4; i++) {
            if (go_playerReferences[i].GetComponent<PlayerController>().GetColor() == c) {
                GameObject go_spell = Instantiate(go_riftDeathBolt, gameObject.transform.position, gameObject.transform.rotation);
                go_spell.transform.localScale = new Vector3(f_projectileSize, f_projectileSize, f_projectileSize);
                go_spell.GetComponent<Rigidbody>().velocity = go_playerReferences[i].transform.position.normalized * Constants.RiftStats.C_VolatilityDeathboltSpeed;
            }
        }
    }

    /*public void SpawnNecromancers() {
        Debug.Log("Spawn necromancers when we have them");
    }
    
    public void InvertControls() {

    }

    void ReverseInvertControls() {

    }

    public void SpawnPocketRifts() {

    }

    public void InvertScreen() {
        mainCamera.transform.Rotate(0.0f, 0.0f, 180.0f);
        Invoke("ReverseInvertScreen", Constants.RiftStats.C_Volatility_CameraFlipTime);
    }

    void ReverseInvertScreen() {
        mainCamera.transform.Rotate(0.0f, 0.0f, 180.0f);
    }*/

    /*/////////////////////////////////////////////////////////////////////////////////////////////////////////////*/

    void Awake() {
        instance = this;
    }

    void Start() {
        maestro = Maestro.Instance;
        ResetVolatility();
    }
}
