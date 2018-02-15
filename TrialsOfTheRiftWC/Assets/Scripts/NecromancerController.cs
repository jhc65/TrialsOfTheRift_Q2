using UnityEngine;
using System.Collections;
using UnityEngine.AI;

public class NecromancerController : EnemyController {

	private float avoidDistance = 10.0f;
	private float runeTimer = 8.0f;
	private float summoningTimer = 8.0f;
	[SerializeField] private GameObject go_enemyPrefab;
	[SerializeField] private GameObject go_runePrefab;

	protected override void ChildUpdate() {
		runeTimer -= Time.deltaTime;
		summoningTimer -= Time.deltaTime;

		if (runeTimer < 0 ) {
			EnterStateDropping();
		}

		if (summoningTimer < 0) {
			EnterStateSummoning();
		}
	}

    protected override void ChildUpdateWander() {

		bool b_playersAvailable = false;
		for(int i = 0; i < riftController.go_playerReferences.Length; i++){	
			if(riftController.go_playerReferences[i].GetComponent<PlayerController>().e_Side == e_Side && riftController.go_playerReferences[i].GetComponent<PlayerController>().isWisp == false){
				if (Vector3.Distance(riftController.go_playerReferences[i].transform.position, transform.position) < avoidDistance) {
					b_playersAvailable = true;
					break;
				}
			}
		}

		if (b_playersAvailable) {
			EnterStateFlee();
		}
		else {
			 Wander();
		}
    }

	protected override void ChildUpdateDropping() {
		runeTimer = 2.0f;
		DropRune();
		EnterStateWander();
	}

	protected override void ChildUpdateSummoning() {
		summoningTimer = 8.0f;
		riftController.SummonEnemiesAroundNecromancer(transform.position, e_Side);
		EnterStateWander();
	}

	private void Wander() {
		timer += Time.deltaTime;

        if (timer >= timeLimit || Vector3.Distance(transform.position, destination) <= 1.0f ) {

			//bool b_isDestinationValid = false;

			//while(b_isDestinationValid == false) {
				destination = GetWanderPos(transform.position, wanderingRadius);
				CheckOutOfBounds();
				//b_isDestinationValid = IsWithinBounds(transform.position, e_Side);
			//}

            nma_agent.SetDestination(destination);

            timer = 0;
        }
	}

    private static Vector3 GetWanderPos(Vector3 transform, float wanderingRadius) {
 
		float angle = Random.Range(0, 2 * Mathf.PI);
		float deltaZ = Mathf.Sin(angle)*wanderingRadius;
		float deltaX = Mathf.Cos(angle)*wanderingRadius;
		Vector3 position = new Vector3(transform.x + deltaX, 0, transform.z + deltaZ);
 
        NavMeshHit navHit;
 
        NavMesh.SamplePosition (position, out navHit, wanderingRadius, -1);
 
        return navHit.position;
    }
	
	protected override void ChildUpdateFlee() {

		int count = 0;

		float deltaX = 0.0f;
		float deltaZ = 0.0f;

		float angle = 0.0f;
		float sumAngle = 0.0f;

		Vector3 dir;


		for(int i = 0; i < riftController.go_playerReferences.Length; i++){

			if(riftController.go_playerReferences[i].GetComponent<PlayerController>().e_Side == e_Side && riftController.go_playerReferences[i].GetComponent<PlayerController>().isWisp == false) {

				if (Vector3.Distance(riftController.go_playerReferences[i].transform.position, transform.position) < avoidDistance) {

					count = count + 1;

					dir = riftController.go_playerReferences[i].transform.InverseTransformDirection((riftController.go_playerReferences[i].transform.position - transform.position));

					angle = (Mathf.Atan2(dir.z, dir.x) * Mathf.Rad2Deg)+180.0f;

					if (angle >= 360)
						angle = angle-360.0f;

					sumAngle = sumAngle + angle;
				}

			}
		}

		if (count > 0) {

			angle = sumAngle/count;

			deltaZ = Mathf.Sin((angle * Mathf.PI)/180)*avoidDistance;
			deltaX = Mathf.Cos((angle * Mathf.PI)/180)*avoidDistance;

			destination = new Vector3(transform.position.x + deltaX, 0, transform.position.z + deltaZ);

			
			CheckOutOfBounds();

			nma_agent.SetDestination(destination);
		}
		else {
			timer = timeLimit;
			EnterStateWander();
		}

	}

	protected override void ChildEnterStateWander() {
		//Reset timer
		timer = timeLimit;
	}

	protected override void ChildUpdateDie() {
		riftController.DecreaseNecromancers(e_Side);
	}

	private void DropRune() {
		//Vector3 dropPos = RandomCircle(transform.position, e_Side, 1.0f);
            
		// If it is, find a new spawn position for the enemy
        //var hitColliders = Physics.OverlapSphere(dropPos, 0.005f);
        //if (hitColliders.Length > 0) {
        //    DropRune();
        //}
        //else {
            Instantiate(go_runePrefab, transform.position, Quaternion.identity);
        //}
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

}