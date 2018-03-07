/*  Rift Controller - Joe Chew
 * 
 *  Desc:   Facilitates Rift volatility and enemy spawn.
 * 
 */


using System;
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
	[SerializeField] private GameObject[] go_skeletons;
	[SerializeField] private GameObject[] go_necromancers;
	[SerializeField] private GameObject[] go_runes;

    //[SerializeField] private GameObject go_enemyPrefab;
	//[SerializeField] private GameObject go_necromancerPrefab;
	//[SerializeField] private GameObject go_runePrefab;
    [SerializeField] private GameObject go_enemyIndiPrefab;
    [SerializeField] private Camera cam_camera;

	private int i_leftRunes = 0;
	private int i_rightRunes = 0;

	private int i_leftNecromancers = 0;
	private int i_rightNecromancers = 0;

    private int i_leftEnemies = 0;
    private int i_rightEnemies = 0;

    private GameObject[] go_rightEnemySpawners;
    private GameObject[] go_leftEnemySpawners;

    private float f_enemySpeed;

	private int i_volatilityLevel;
    private float f_volatility;
    private float f_volatilityMultiplier;
    private Constants.RiftStats.Volatility e_currentVolatilityLevel;
    private Maestro maestro;     // reference to audio singleton
	
	private System.Random r_random = new System.Random();

    // Singleton
    private static RiftController instance;
    public static RiftController Instance {
        get { return instance; }
    }

    // Getters
    public GameObject[] PlayerReferences {
        get { return go_playerReferences; }
    }

	public float EnemySpeed {
		get { return f_enemySpeed; }
	}

    public GameObject[] RightEnemySpawners {
        set { go_rightEnemySpawners = value; }
    }

    public GameObject[] LeftEnemySpawners {
        set { go_leftEnemySpawners = value; }
    }

    /*/////////////////////////////////////////////////////////////////////////////////////////////////////////////*/

    public void IncreaseVolatility(float volatilityUp) {
        Debug.Log("Volatility increased!");
		maestro.PlayAnnouncementVolatilityUp();
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
			i_volatilityLevel = 4;
            EnterNewVolatilityLevel();
            //InvertControls();
			InvokeRepeating("SpawnNecromancers", 0.0f, Constants.RiftStats.C_VolatilityNecromancerSpawnTimer);
        }
        else if (f_volatility >= 65.0f && e_currentVolatilityLevel != Constants.RiftStats.Volatility.SIXTYFIVE) {
            e_currentVolatilityLevel = Constants.RiftStats.Volatility.SIXTYFIVE;
			i_volatilityLevel = 3;
            EnterNewVolatilityLevel();
            for (int i = 0; i < 5; i++) {
                SpawnEnemies();
            }
            //SpawnPocketRifts();
        }
        else if (f_volatility >= 50.0f && e_currentVolatilityLevel != Constants.RiftStats.Volatility.FIFTY) {
            e_currentVolatilityLevel = Constants.RiftStats.Volatility.FIFTY;
			i_volatilityLevel = 3;
            EnterNewVolatilityLevel();
            //SpawnNecromancers();
        }
        else if (f_volatility >= 35.0f && e_currentVolatilityLevel != Constants.RiftStats.Volatility.THIRTYFIVE) {
            e_currentVolatilityLevel = Constants.RiftStats.Volatility.THIRTYFIVE;
			i_volatilityLevel = 2;
            EnterNewVolatilityLevel();
            f_enemySpeed += 1.0f;
        }
        else if (f_volatility >= 25.0f && e_currentVolatilityLevel != Constants.RiftStats.Volatility.TWENTYFIVE) {
            e_currentVolatilityLevel = Constants.RiftStats.Volatility.TWENTYFIVE;
			i_volatilityLevel = 2;
            EnterNewVolatilityLevel();
            FireDeathBolts(Constants.Global.Color.RED);
            FireDeathBolts(Constants.Global.Color.BLUE);
        }
        else if (f_volatility >= 5.0f && e_currentVolatilityLevel != Constants.RiftStats.Volatility.FIVE) {
            e_currentVolatilityLevel = Constants.RiftStats.Volatility.FIVE;
			i_volatilityLevel = 1;
            EnterNewVolatilityLevel();
            InvokeRepeating("SpawnEnemies", 0.0f, Constants.RiftStats.C_VolatilityEnemySpawnTimer);
        }
        else if (f_volatility < 5.0f) {
			i_volatilityLevel = 0;
            EnterNewVolatilityLevel();
        }
    }

    private void EnterNewVolatilityLevel() {
        maestro.PlayVolatilityAmbience(i_volatilityLevel);
        switch (i_volatilityLevel) {
            case 0:
                // Change rift visual to L0
                e_currentVolatilityLevel = Constants.RiftStats.Volatility.ZERO;
                f_volatilityMultiplier = Constants.RiftStats.C_VolatilityMultiplier_L1;     // there is no L0, L1 is already 0
                CancelInvoke("SpawnEnemies");
				CancelInvoke("SpawnNecromancers");
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
		i_volatilityLevel = 0;
        EnterNewVolatilityLevel();
    }

    //----------------------------
    // Rift Effects
    private void BoardClear() {
        foreach (GameObject player in go_playerReferences) {
            player.GetComponent<PlayerController>().TakeDamage(Constants.PlayerStats.C_MaxHealth,Constants.Global.DamageType.RIFT);
        }

        /* Do shit with the runes once I get Jeff and Dana's shit in
        */

        //TODO: kill all enemies
        for (int i = 0; i < go_skeletons.Length; i++) {
			if (go_skeletons[i].activeSelf)
				go_skeletons[i].SetActive(false);
        }

        for (int i = 0; i < go_necromancers.Length; i++) {
			if (go_necromancers[i].activeSelf)
				go_necromancers[i].SetActive(false);
        }

        for (int i = 0; i < go_runes.Length; i++) {
			if (go_runes[i].activeSelf)
				go_runes[i].SetActive(false);
        }
    }

    //TODO: revisit enemy spawn with pooling
	public void ActivateEnemy(Vector3 position) {
	    for (int i = 0; i < go_skeletons.Length; i++) {
			if (!(go_skeletons[i].activeSelf)) {

				GameObject enemyIndi = Instantiate(go_enemyIndiPrefab, position, Quaternion.identity);
				CameraFacingBillboard cfb_this = enemyIndi.GetComponent<CameraFacingBillboard>();
				cfb_this.cam_Camera = cam_camera;

				if (position.x < 0f) {
					go_skeletons[i].transform.position = position;
					go_skeletons[i].GetComponent<SkeletonController>().Init(Constants.Global.Side.LEFT);
					go_skeletons[i].SetActive(true);
				}
				else {
					go_skeletons[i].transform.position = position;
					go_skeletons[i].GetComponent<SkeletonController>().Init(Constants.Global.Side.RIGHT);
					go_skeletons[i].SetActive(true);
				}

				cfb_this.go_trackedObject = go_skeletons[i];
				break;
			}
        }
	}

	public void ActivateNecromancer(Vector3 position) {
	    for (int i = 0; i < go_necromancers.Length; i++) {
			if (!(go_necromancers[i].activeSelf)) {

				GameObject enemyIndi = Instantiate(go_enemyIndiPrefab, position, Quaternion.identity);
				CameraFacingBillboard cfb_this = enemyIndi.GetComponent<CameraFacingBillboard>();
				cfb_this.cam_Camera = cam_camera;

				if (position.x < 0f) {
					go_necromancers[i].transform.position = position;
					go_necromancers[i].GetComponent<NecromancerController>().Init(Constants.Global.Side.LEFT);
					go_necromancers[i].SetActive(true);
				}
				else {
					go_necromancers[i].transform.position = position;
					go_necromancers[i].GetComponent<NecromancerController>().Init(Constants.Global.Side.RIGHT);
					go_necromancers[i].SetActive(true);
				}

				cfb_this.go_trackedObject = go_necromancers[i];
				break;
			}
        }
	}

	public void ActivateRune(Vector3 position) {
	    for (int i = 0; i < go_runes.Length; i++) {
			if (!(go_runes[i].activeSelf)) {

				if (position.x < 0f) {
					go_runes[i].transform.position = position;
					go_runes[i].SetActive(true);
				}
				else {
					go_runes[i].transform.position = position;
					go_runes[i].SetActive(true);
				}
				break;
			}
        }
	}

    // Spawns one enemy on either side of the Rift, randomly chosen position
    public void SpawnEnemies() {
        int randLeft = UnityEngine.Random.Range(0, go_leftEnemySpawners.Length);
        int randRight = UnityEngine.Random.Range(0, go_rightEnemySpawners.Length);

        if (i_leftEnemies < Constants.EnemyStats.C_EnemySpawnCapPerSide) {
            Vector3 pos = go_leftEnemySpawners[randLeft].transform.position;
            CircularEnemySpawn(pos, Constants.Global.Side.LEFT);
        }
        if (i_rightEnemies < Constants.EnemyStats.C_EnemySpawnCapPerSide) {
            Vector3 pos = go_rightEnemySpawners[randRight].transform.position;
            CircularEnemySpawn(pos, Constants.Global.Side.RIGHT);
        }
    }

    // Spawns one enemy on either side of the Rift, randomly chosen position
    public void SpawnRunes() {
        int randLeft = UnityEngine.Random.Range(0, go_leftEnemySpawners.Length);
        int randRight = UnityEngine.Random.Range(0, go_rightEnemySpawners.Length);

        if (i_leftRunes < Constants.RiftStats.C_RuneSpawnCapPerSide) {
            //GameObject leftEnemy = Instantiate(go_runePrefab, v3_leftEnemySpawnPositions[randLeft], Quaternion.identity);
            ActivateRune(go_leftEnemySpawners[randLeft].transform.position);
			i_leftRunes++;
        }
        if (i_rightRunes < Constants.RiftStats.C_RuneSpawnCapPerSide) {
            //GameObject rightEnemy = Instantiate(go_runePrefab, v3_rightEnemySpawnPositions[randRight], Quaternion.identity);
            ActivateRune(go_rightEnemySpawners[randRight].transform.position);
			i_rightRunes++;
        }
    }

    // Spawns one necromancers on either side of the Rift, randomly chosen position
    public void SpawnNecromancers() {
        int randLeft = UnityEngine.Random.Range(0, go_leftEnemySpawners.Length);
        int randRight = UnityEngine.Random.Range(0, go_rightEnemySpawners.Length);

        if (i_leftNecromancers < Constants.EnemyStats.C_NecromancerSpawnCapPerSide) {
            Vector3 pos = go_leftEnemySpawners[randLeft].transform.position;
            pos = new Vector3(pos.x - 1.0f, pos.y, pos.z);
            //GameObject leftEnemy = Instantiate(go_necromancerPrefab, pos, Quaternion.identity);
            //leftEnemy.GetComponent<EnemyController>().e_Side = Constants.Global.Side.LEFT;  //TODO: is there a better way to set-up enemies?
            //leftEnemy.GetComponent<NavMeshAgent>().speed = f_enemySpeed;
            //leftEnemy.GetComponent<NecromancerController>().SetHealth(Constants.EnemyStats.C_EnemyHealth);

            //GameObject enemyIndi = Instantiate(go_enemyIndiPrefab, pos, Quaternion.identity);
            //CameraFacingBillboard cfb_this = enemyIndi.GetComponent<CameraFacingBillboard>();
            //cfb_this.cam_Camera = cam_camera;
            //cfb_this.go_trackedObject = leftEnemy;

			ActivateNecromancer(pos);
			i_leftNecromancers++;
        }
        if (i_rightNecromancers < Constants.EnemyStats.C_NecromancerSpawnCapPerSide) {
            Vector3 pos = go_rightEnemySpawners[randRight].transform.position;
            pos = new Vector3(pos.x + 1.0f, pos.y, pos.z);
            //GameObject rightEnemy = Instantiate(go_necromancerPrefab, pos, Quaternion.identity);
            //rightEnemy.GetComponent<EnemyController>().e_Side = Constants.Global.Side.RIGHT;
            //rightEnemy.GetComponent<NavMeshAgent>().speed = f_enemySpeed;
            //rightEnemy.GetComponent<NecromancerController>().SetHealth(Constants.EnemyStats.C_EnemyHealth);


            //GameObject enemyIndi = Instantiate(go_enemyIndiPrefab, pos, Quaternion.identity);
            //CameraFacingBillboard cfb_this = enemyIndi.GetComponent<CameraFacingBillboard>();
            //cfb_this.cam_Camera = cam_camera;
            //cfb_this.go_trackedObject = rightEnemy;

            ActivateNecromancer(pos);
			i_rightNecromancers++;
        }
    }

    // Spawns an enemy at a specified position
    public void SpawnEnemy(Vector3 position, Constants.Global.Side side) {
        // only spawn if below enemy side cap TODO: is this expected behavior?
        if (side == Constants.Global.Side.LEFT && i_leftEnemies < Constants.EnemyStats.C_EnemySpawnCapPerSide) {
            //GameObject enemy = Instantiate(go_enemyPrefab, position, Quaternion.identity);
            //enemy.GetComponent<EnemyController>().e_Side = side;        //TODO: is there a better way to set-up enemies?
            //enemy.GetComponent<NavMeshAgent>().speed = f_enemySpeed;
            //enemy.GetComponent<MeleeController>().SetHealth(Constants.EnemyStats.C_EnemyHealth);

            //GameObject enemyIndi = Instantiate(go_enemyIndiPrefab, position, Quaternion.identity);
            //CameraFacingBillboard cfb_this = enemyIndi.GetComponent<CameraFacingBillboard>();
            //cfb_this.cam_Camera = cam_camera;
            //cfb_this.go_trackedObject = enemy;

			ActivateEnemy(position);
            i_leftEnemies++;
        } else if (side == Constants.Global.Side.RIGHT && i_rightEnemies < Constants.EnemyStats.C_EnemySpawnCapPerSide) {
            //GameObject enemy = Instantiate(go_enemyPrefab, position, Quaternion.identity);
            //enemy.GetComponent<EnemyController>().e_Side = side;        //TODO: is there a better way to set-up enemies?
            //enemy.GetComponent<NavMeshAgent>().speed = f_enemySpeed;
            //enemy.GetComponent<MeleeController>().SetHealth(Constants.EnemyStats.C_EnemyHealth);

            //GameObject enemyIndi = Instantiate(go_enemyIndiPrefab, position, Quaternion.identity);
            //CameraFacingBillboard cfb_this = enemyIndi.GetComponent<CameraFacingBillboard>();
            //cfb_this.cam_Camera = cam_camera;
            //cfb_this.go_trackedObject = enemy;

			ActivateEnemy(position);
            i_rightEnemies++;
        }
    }

    // Spawns an enemy within a radius when a valid position is selected
    public void CircularEnemySpawn(Vector3 center, Constants.Global.Side side) {
        Vector3 spawnPos = RandomCircle(center, side, Constants.EnemyStats.C_SpawnRadius);

        // Checks to see if the spawn position is already occupied by anything with a collider
        // If it is, find a new spawn position for the enemy
        Debug.Log(spawnPos);
        var hitColliders = Physics.OverlapSphere(spawnPos, 0.0005f);
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

    public void DecreaseNecromancers(Constants.Global.Side side) {
        if (side == Constants.Global.Side.LEFT) {
            i_leftNecromancers--; }
        else {
            i_rightNecromancers--;
        }
    }

    // Gets a random Vector3 position within a given radius
    private Vector3 RandomCircle(Vector3 center, Constants.Global.Side side, float radius) {
        float ang = UnityEngine.Random.value * 360;
        Vector3 pos;

        // by absolute valueing the x position, we can tell the enemy which side it should of the map it should be on
        int s = (int)side;
        pos.x = s * Mathf.Abs(center.x + radius * Mathf.Sin(ang * Mathf.Deg2Rad));
        pos.y = center.y;
        pos.z = center.z + radius * Mathf.Cos(ang * Mathf.Deg2Rad);

        // Reposition the enemies if they spawn outside of the map TODO: revisit once map is scaled, and these should be Constants anyway
        if (pos.z >= Constants.EnemyStats.C_MapBoundryZAxis) {
            float diff = pos.z - Constants.EnemyStats.C_MapBoundryZAxis;
            pos.z = pos.z - diff - 1;
        }
        else if (pos.z <= (-1.0f * Constants.EnemyStats.C_MapBoundryZAxis)) {
            float diff = pos.z + Constants.EnemyStats.C_MapBoundryZAxis;
            pos.z = pos.z - diff + 1;
        }

        if (pos.x >= Constants.EnemyStats.C_MapBoundryXAxis) {
            float diff = pos.x - Constants.EnemyStats.C_MapBoundryXAxis;
            pos.x = pos.x - diff - 1;
        }
        else if (pos.x <= (-1.0f * Constants.EnemyStats.C_MapBoundryXAxis)) {
            float diff = pos.x + Constants.EnemyStats.C_MapBoundryXAxis;
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
	
	void PlayNoise(){
		maestro.PlayVolatilityNoise(i_volatilityLevel);
		Invoke("PlayNoise", r_random.Next(0,10));
	}

    void Awake() {
        instance = this;
    }

    void Start() {
		maestro = Maestro.Instance;
        ResetVolatility();
		Invoke("PlayNoise", r_random.Next(0,10));
    }
}
