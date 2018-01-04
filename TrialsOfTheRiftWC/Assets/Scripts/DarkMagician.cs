using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

public class DarkMagician : MonoBehaviour {

    public GameObject[] go_objectivesList;
	public GameObject go_enemy;
	public float f_enemySpawnTime = Constants.EnviroStats.C_EnemySpawnTime;             // [Param Fix]

    //Singleton
    static DarkMagician instance;

    public static DarkMagician GetInstance()
    {
        return instance;
    }

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

    public Vector3[] GetRightEnemySpawnPositions() {
        return v3_rightEnemySpawnPositions;
    }

    public Vector3[] GetLeftEnemySpawnPositions() {
        return v3_leftEnemySpawnPositions;
    }

    public Objective objv_redObjective, objv_blueObjective; 

	private Objective GetNextObjective(Objective o) {
        Debug.Log("Volatility Increase by 5%");
        RiftController.GetInstance().IncreaseVolatility(Constants.RiftStats.C_VolatilityIncrease_RoomAdvance);
		int newObjectiveNumber = o.i_numberInList;
		//if(newObjectiveNumber == 5)
		//{ game over}

		// temporary, alternates between two objectives indefinitely instead of counting up to 5 and ending
		if (newObjectiveNumber == 2) {
			newObjectiveNumber = 1;
		}
		else {
			newObjectiveNumber += 1;
		}

		GameObject go_newObjective = Instantiate(go_objectivesList[newObjectiveNumber - 1]);	// objectiveNumber starts with 1 but array is 0-based
		go_newObjective.GetComponent<Objective>().Set(o.e_color, newObjectiveNumber);
		Destroy(o.gameObject);
		return go_newObjective.GetComponent<Objective>();
	}

	public void SpawnEnemies() {
		int randLeft = Random.Range(0, v3_leftEnemySpawnPositions.Length);
		int randRight = Random.Range(0, v3_rightEnemySpawnPositions.Length);
		GameObject g1 = Instantiate(go_enemy, v3_leftEnemySpawnPositions[randLeft], new Quaternion(0, 0, 0, 0));
		g1.GetComponent<EnemyController>().e_Side = Constants.Side.LEFT;
        g1.GetComponent<NavMeshAgent>().speed = Constants.EnviroStats.C_EnemySpeed;             // [Param Fix]
        g1.GetComponent<MeleeController>().SetHealth(Constants.EnviroStats.C_EnemyHealth);      // [Param Fix]
		GameObject g2 = Instantiate(go_enemy, v3_rightEnemySpawnPositions[randRight], new Quaternion(0, 0, 0, 0));
		g2.GetComponent<EnemyController>().e_Side = Constants.Side.RIGHT;
        g2.GetComponent<NavMeshAgent>().speed = Constants.EnviroStats.C_EnemySpeed;             // [Param Fix]
        g2.GetComponent<MeleeController>().SetHealth(Constants.EnviroStats.C_EnemyHealth);      // [Param Fix]
	}

    //instanties an enemy at a location on a particular side
    public void SpawnEnemies(Constants.Side side, Vector3 position) {
        GameObject g1 = Instantiate(go_enemy, position, new Quaternion(0, 0, 0, 0));
        g1.GetComponent<EnemyController>().e_Side = side;
        g1.GetComponent<NavMeshAgent>().speed = Constants.EnviroStats.C_EnemySpeed;             // [Param Fix]
        g1.GetComponent<MeleeController>().SetHealth(Constants.EnviroStats.C_EnemyHealth);      // [Param Fix]
    }

	void Awake() {  // parameter screen dictates that we do this before its Start() is called

        if (instance == null)
        {
            instance = this;
        }

        if (instance != null && instance != this)
        {
            Debug.Log("Destroying DM.");
            Destroy(this);
        }

        Time.timeScale = 0;

        objv_redObjective = Instantiate(go_objectivesList[0]).GetComponent<Objective>();
		objv_blueObjective = Instantiate(go_objectivesList[0]).GetComponent<Objective>();
		objv_redObjective.Set(Constants.Color.RED, 1);
		objv_blueObjective.Set(Constants.Color.BLUE, 1);

		// enemies, TODO: this not here
		//for(int i=0; i< v3_leftEnemySpawnPositions.Length; i++) {
		//	GameObject g1 = Instantiate(go_enemy, v3_leftEnemySpawnPositions[i], new Quaternion(0, 0, 0, 0));
		//	g1.GetComponent<EnemyController>().e_Side = Constants.Side.LEFT;
		//	GameObject g2 = Instantiate(go_enemy, v3_rightEnemySpawnPositions[i], new Quaternion(0, 0, 0, 0));
		//	g2.GetComponent<EnemyController>().e_Side = Constants.Side.RIGHT;
		//}

		//InvokeRepeating("SpawnEnemies", 7.0f, f_enemySpawnTime);
	}

	void Update() {
		// check for completion of objectives
		if (objv_redObjective.b_complete) {
			objv_redObjective = GetNextObjective(objv_redObjective);
		}
		if(objv_blueObjective.b_complete) {
			objv_blueObjective = GetNextObjective(objv_blueObjective);
		}
	}

    // [Param Fix]
    public void ResetEnemySpawnRate() {
        //CancelInvoke();
        //InvokeRepeating("SpawnEnemies", 7.0f, f_enemySpawnTime);
    }
}
