using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;
using UnityEngine.UI;

public class DarkMagician : MonoBehaviour {

    public Objective[] objv_redObjectiveList;
    public Objective[] objv_blueObjectiveList;
    
    public GameObject go_enemy;
    public int leftEnemies = 0;
    public int rightEnemies = 0;
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

	private void GetNextObjective(Constants.Global.Color c, int objectiveNumber) {
        
        // check for game end
        if (objectiveNumber == objv_redObjectiveList.Length) {
			b_gameOver = true;
			txt_winMsg.text = c + " team won!";
			return;
		}

        Debug.Log("Volatility Increase by 5%");
        RiftController.GetInstance().IncreaseVolatility(Constants.RiftStats.C_VolatilityIncrease_RoomAdvance);
        objectiveNumber++;
        Debug.Log(objectiveNumber);

        if (c == Constants.Global.Color.RED) {
            objv_redObjective.Complete();
            objv_redObjective = objv_redObjectiveList[objectiveNumber-1].Activate(objectiveNumber);	// objectiveNumber starts with 1 but array is 0-based
        }
        else if (c == Constants.Global.Color.BLUE) {
            objv_blueObjective.Complete();
            objv_blueObjective = objv_blueObjectiveList[objectiveNumber - 1].Activate(objectiveNumber);	// objectiveNumber starts with 1 but array is 0-based
		}
	}

    // TODO: move enemy spawn to RiftController
	public void SpawnEnemies() {
		int randLeft = Random.Range(0, v3_leftEnemySpawnPositions.Length);
		int randRight = Random.Range(0, v3_rightEnemySpawnPositions.Length);

        if (leftEnemies < Constants.EnemyStats.C_EnemySpawnCapPerSide) {
            GameObject g1 = Instantiate(go_enemy, v3_leftEnemySpawnPositions[randLeft], new Quaternion(0, 0, 0, 0));
            g1.GetComponent<EnemyController>().e_Side = Constants.Global.Side.LEFT;
            g1.GetComponent<NavMeshAgent>().speed = Constants.EnemyStats.C_EnemySpeed;             // [Param Fix]
            g1.GetComponent<MeleeController>().SetHealth(Constants.EnemyStats.C_EnemyHealth);      // [Param Fix]
            leftEnemies++;
        } else {
            Debug.Log("Left Side MAX.");
        }

        if (rightEnemies < Constants.EnemyStats.C_EnemySpawnCapPerSide) {
            GameObject g2 = Instantiate(go_enemy, v3_rightEnemySpawnPositions[randRight], new Quaternion(0, 0, 0, 0));
            g2.GetComponent<EnemyController>().e_Side = Constants.Global.Side.RIGHT;
            g2.GetComponent<NavMeshAgent>().speed = Constants.EnemyStats.C_EnemySpeed;             // [Param Fix]
            g2.GetComponent<MeleeController>().SetHealth(Constants.EnemyStats.C_EnemyHealth);      // [Param Fix]
            rightEnemies++;
        } else {
            Debug.Log("Right Side MAX.");
        }
	}

    //instanties an enemy at a location on a particular side
    public void SpawnEnemies(Constants.Global.Side side, Vector3 position) {
        if (side == Constants.Global.Side.LEFT && leftEnemies < Constants.EnemyStats.C_EnemySpawnCapPerSide ||
            side == Constants.Global.Side.RIGHT && rightEnemies < Constants.EnemyStats.C_EnemySpawnCapPerSide) {  //I'm aware of how ugly this looks, but it works.

            GameObject g1 = Instantiate(go_enemy, position, new Quaternion(0, 0, 0, 0));
            g1.GetComponent<EnemyController>().e_Side = side;
            g1.GetComponent<NavMeshAgent>().speed = Constants.EnemyStats.C_EnemySpeed;             // [Param Fix]
            g1.GetComponent<MeleeController>().SetHealth(Constants.EnemyStats.C_EnemyHealth);      // [Param Fix]
        }
        
    }

    //instantiates an enemy within a radius when a valid position is selected
    public void CircularSpawn(Vector3 center, Constants.Global.Side side)
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
    private Vector3 RandomCircle(Vector3 center, Constants.Global.Side side, float radius)
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

        if (instance == null) {
            instance = this;
        }

		txt_winMsg.enabled = false;
		b_gameOver = false;
		ShuffleObjectives();
        Time.timeScale = 0;

        objv_redObjective = objv_redObjectiveList[0].Activate(1);
        objv_blueObjective = objv_blueObjectiveList[0].Activate(1);
    }

    // Shuffles the order of both red and blue objective lists in parallel
	void ShuffleObjectives() {
        for (int i = 0; i < objv_redObjectiveList.Length-1; i++ ) {
            Objective tmp1 = objv_redObjectiveList[i];
            Objective tmp2 = objv_blueObjectiveList[i];
            int j = Random.Range(i, objv_redObjectiveList.Length-1);
            objv_redObjectiveList[i] = objv_redObjectiveList[j];
            objv_blueObjectiveList[i] = objv_blueObjectiveList[j];
            objv_redObjectiveList[j] = tmp1;
            objv_blueObjectiveList[j] = tmp2;
        }
	}

	void Update() {

		// check for completion of objectives
		if (b_gameOver) {
			txt_winMsg.enabled = true;
			Time.timeScale = 0;
		}
		else {
			if (objv_redObjective.IsComplete) {
                Debug.Log("getting next obj");
                GetNextObjective(objv_redObjective.Color, objv_redObjective.NumberInList);
			}
			if(objv_blueObjective.IsComplete) {
                Debug.Log("getting next blue obj");
                GetNextObjective(objv_blueObjective.Color, objv_blueObjective.NumberInList);
            }
		}
	}
}
