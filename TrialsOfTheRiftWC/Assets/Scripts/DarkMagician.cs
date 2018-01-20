using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;
using UnityEngine.UI;

public class DarkMagician : MonoBehaviour {

    public GameObject[] go_objectivesList;
	public GameObject go_enemy;
    public int leftEnemies = 0;
    public int rightEnemies = 0;
	public float f_enemySpawnTime = Constants.EnviroStats.C_EnemySpawnTime;             // [Param Fix]
	public Text txt_winMsg;
	private bool b_gameOver;

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
		if(newObjectiveNumber == go_objectivesList.Length) {
			//	game over
			b_gameOver = true;
			txt_winMsg.text = o.e_color + " team won!";
			Destroy(o.gameObject);
			return null;
		}
		else {
			// temporary, alternates between two objectives indefinitely instead of counting up to 5 and ending
			newObjectiveNumber += 1;

			GameObject go_newObjective = Instantiate(go_objectivesList[newObjectiveNumber - 1]);	// objectiveNumber starts with 1 but array is 0-based
			go_newObjective.GetComponent<Objective>().Set(o.e_color, newObjectiveNumber);
			Destroy(o.gameObject);
			return go_newObjective.GetComponent<Objective>();
		}
	}

	public void SpawnEnemies() {
		int randLeft = Random.Range(0, v3_leftEnemySpawnPositions.Length);
		int randRight = Random.Range(0, v3_rightEnemySpawnPositions.Length);

        if (leftEnemies < Constants.EnviroStats.C_EnemySpawnCap) {
            GameObject g1 = Instantiate(go_enemy, v3_leftEnemySpawnPositions[randLeft], new Quaternion(0, 0, 0, 0));
            g1.GetComponent<EnemyController>().e_Side = Constants.Side.LEFT;
            g1.GetComponent<NavMeshAgent>().speed = Constants.EnviroStats.C_EnemySpeed;             // [Param Fix]
            g1.GetComponent<MeleeController>().SetHealth(Constants.EnviroStats.C_EnemyHealth);      // [Param Fix]
            leftEnemies++;
        } else {
            Debug.Log("Left Side MAX.");
        }

        if (rightEnemies < Constants.EnviroStats.C_EnemySpawnCap) {
            GameObject g2 = Instantiate(go_enemy, v3_rightEnemySpawnPositions[randRight], new Quaternion(0, 0, 0, 0));
            g2.GetComponent<EnemyController>().e_Side = Constants.Side.RIGHT;
            g2.GetComponent<NavMeshAgent>().speed = Constants.EnviroStats.C_EnemySpeed;             // [Param Fix]
            g2.GetComponent<MeleeController>().SetHealth(Constants.EnviroStats.C_EnemyHealth);      // [Param Fix]
            rightEnemies++;
        } else {
            Debug.Log("Right Side MAX.");
        }
	}

    //instanties an enemy at a location on a particular side
    public void SpawnEnemies(Constants.Side side, Vector3 position) {
        if (side == Constants.Side.LEFT && leftEnemies < Constants.EnviroStats.C_EnemySpawnCap ||
            side == Constants.Side.RIGHT && rightEnemies < Constants.EnviroStats.C_EnemySpawnCap) {  //I'm aware of how ugly this looks, but it works.

            GameObject g1 = Instantiate(go_enemy, position, new Quaternion(0, 0, 0, 0));
            g1.GetComponent<EnemyController>().e_Side = side;
            g1.GetComponent<NavMeshAgent>().speed = Constants.EnviroStats.C_EnemySpeed;             // [Param Fix]
            g1.GetComponent<MeleeController>().SetHealth(Constants.EnviroStats.C_EnemyHealth);      // [Param Fix]
        }
        
    }

    //instantiates an enemy within a radius when a valid position is selected
    public void CircularSpawn(Vector3 center, Constants.Side side)
    {
        Vector3 pos = RandomCircle(center, side, 3.0f);

        //checks to see if the position is occupied by anything with a collider
        //if there is, then find a new position for the enemy
        var hitColliders = Physics.OverlapSphere(pos, 0.005f);
        if (hitColliders.Length > 0)
        {
            Debug.Log("Papa Bless");
            CircularSpawn(center, side);
        }
        else
        {
            SpawnEnemies(side, pos);
        }
    }

    //gets a random Vector3 position within the certain radius from the potato
    private Vector3 RandomCircle(Vector3 center, Constants.Side side, float radius)
    {
        float ang = Random.value * 360;
        Vector3 pos;

        //by absolute valueing the x position, we can tell the enemy which side it should of the map it should be on
        int s = (int)side;
        pos.x = s * Mathf.Abs(center.x + radius * Mathf.Sin(ang * Mathf.Deg2Rad));
        pos.y = center.y;
        pos.z = center.z + radius * Mathf.Cos(ang * Mathf.Deg2Rad);

        //reposition the enemies if they spawn outside of the map
        if (pos.z >= 22)
        {
            float diff = pos.z - 22;
            pos.z = pos.z - diff - 1;
        }
        else if (pos.z <= -22)
        {
            Debug.Log(pos.z);
            float diff = pos.z + 22;
            pos.z = pos.z - diff + 1;
            Debug.Log(pos.z);
        }

        if (pos.x >= 40)
        {
            float diff = pos.x - 40;
            pos.x = pos.x - diff - 1;
        }
        else if (pos.x <= -40)
        {
            float diff = pos.x + 40;
            pos.x = pos.x - diff + 1;
        }

        return pos;
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

		txt_winMsg.enabled = false;
		b_gameOver = false;
		ObjectiveShuffle();
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

	void ObjectiveShuffle() {
        for (int i = 0; i < go_objectivesList.Length-1; i++ )
        {
            GameObject tmp = go_objectivesList[i];
            int j = Random.Range(i, go_objectivesList.Length-1);
            go_objectivesList[i] = go_objectivesList[j];
            go_objectivesList[j] = tmp;
        }
	}

	void Update() {

		// check for completion of objectives
		if (b_gameOver) {
			txt_winMsg.enabled = true;
			Time.timeScale = 0;
		}
		else {
			if (objv_redObjective.b_complete) {
				objv_redObjective = GetNextObjective(objv_redObjective);
			}
			if(objv_blueObjective.b_complete) {
				objv_blueObjective = GetNextObjective(objv_blueObjective);
			}
		}
	}

    // [Param Fix]
    public void ResetEnemySpawnRate() {
        //CancelInvoke();
        //InvokeRepeating("SpawnEnemies", 7.0f, f_enemySpawnTime);
    }
}
