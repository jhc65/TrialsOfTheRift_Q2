using UnityEngine;
using System.Collections;
using UnityEngine.AI;

public class NecromancerController : EnemyController {

	private float f_runeTimer = 8.0f;
	private float f_summoningTimer = 8.0f;
	//[SerializeField] private GameObject go_enemyPrefab;
	//[SerializeField] private GameObject go_runePrefab;

	public override void Init(Constants.Global.Side side) {
		base.Init(side);
		e_side = side;
		nma_agent.speed = Constants.EnemyStats.C_NecromancerBaseSpeed;
		f_health = Constants.EnemyStats.C_NecromancerHealth;
	}

	protected override void Update() {
		base.Update();
		f_runeTimer -= Time.deltaTime;
		f_summoningTimer -= Time.deltaTime;

		if (f_runeTimer < 0 ) {
			EnterStateDropping();
		}

		if (f_summoningTimer < 0) {
			EnterStateSummoning();
		}
	}

    protected override void UpdateWander() {
		base.UpdateWander();
		bool b_playersAvailable = false;
		for(int i = 0; i < riftController.go_playerReferences.Length; i++){	
			if(riftController.go_playerReferences[i].GetComponent<PlayerController>().e_Side == e_side && riftController.go_playerReferences[i].GetComponent<PlayerController>().isWisp == false){
				if (Vector3.Distance(riftController.go_playerReferences[i].transform.position, transform.position) < Constants.EnemyStats.C_NecromancerAvoidDistance) {
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

	protected override void UpdateDropping() {
		base.UpdateDropping();
		f_runeTimer = 2.0f;
		riftController.ActivateRune(transform.position);
		EnterStateWander();
	}

	protected override void UpdateSummoning() {
		base.UpdateSummoning();
		f_summoningTimer = 8.0f;

		for (int i = 0; i < 4; i++) {
			riftController.CircularEnemySpawn(transform.position, e_side);
		}

		EnterStateWander();
	}

	private void Wander() {
		f_timer += Time.deltaTime;

        if (f_timer >= f_timeLimit || Vector3.Distance(transform.position, v3_destination) <= 1.0f ) {

			//bool b_isDestinationValid = false;

			//while(b_isDestinationValid == false) {
				v3_destination = GetWanderPos(transform.position, f_wanderingRadius);
				CheckOutOfBounds();
				//b_isDestinationValid = IsWithinBounds(transform.position, e_side);
			//}

            nma_agent.SetDestination(v3_destination);

            f_timer = 0;
        }
	}

    private static Vector3 GetWanderPos(Vector3 transform, float f_wanderingRadius) {
 
		float angle = Random.Range(0, 2 * Mathf.PI);
		float deltaZ = Mathf.Sin(angle)*f_wanderingRadius;
		float deltaX = Mathf.Cos(angle)*f_wanderingRadius;
		Vector3 position = new Vector3(transform.x + deltaX, 0, transform.z + deltaZ);
 
        NavMeshHit navHit;
 
        NavMesh.SamplePosition (position, out navHit, f_wanderingRadius, -1);
 
        return navHit.position;
    }
	
	protected override void UpdateFlee() {

		base.UpdateFlee();

		int count = 0;

		float angle = 0.0f;
		float sumAngle = 0.0f;

		Vector3 dir;


		for(int i = 0; i < riftController.go_playerReferences.Length; i++){

			if(riftController.go_playerReferences[i].GetComponent<PlayerController>().e_Side == e_side && riftController.go_playerReferences[i].GetComponent<PlayerController>().isWisp == false) {

				if (Vector3.Distance(riftController.go_playerReferences[i].transform.position, transform.position) < Constants.EnemyStats.C_NecromancerAvoidDistance) {

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

			float deltaZ = Mathf.Sin((angle * Mathf.PI)/180)*Constants.EnemyStats.C_NecromancerAvoidDistance;
			float deltaX = Mathf.Cos((angle * Mathf.PI)/180)*Constants.EnemyStats.C_NecromancerAvoidDistance;

			v3_destination = new Vector3(transform.position.x + deltaX, 0, transform.position.z + deltaZ);

			
			CheckOutOfBounds();

			nma_agent.SetDestination(v3_destination);
		}
		else {
			f_timer = f_timeLimit;
			EnterStateWander();
		}

	}

	protected override void EnterStateWander() {
		base.EnterStateWander();
		//Reset f_timer
		f_timer = f_timeLimit;
	}

	protected override void UpdateDie() {
		base.UpdateDie();
		riftController.DecreaseNecromancers(e_side);
	}

	private void DropRune() {
		riftController.ActivateRune(transform.position);
	}

}