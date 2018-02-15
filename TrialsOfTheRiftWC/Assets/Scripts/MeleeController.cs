using UnityEngine;
using System.Collections;
using UnityEngine.AI;

public class MeleeController : EnemyController {
	
	private GameObject go_closestTarget;

    protected override void ChildUpdateChase() {
		go_closestTarget = null;
		float f_minDistance = 9999f;
		float f_currentDistance;
		for(int i = 0; i < riftController.go_playerReferences.Length; i++){	
			f_currentDistance = Vector3.Distance(riftController.go_playerReferences[i].transform.position,transform.position);
			if(riftController.go_playerReferences[i].GetComponent<PlayerController>().e_Side == e_Side && f_currentDistance < f_minDistance && riftController.go_playerReferences[i].GetComponent<PlayerController>().isWisp == false){
				go_closestTarget = riftController.go_playerReferences[i];
				f_minDistance = f_currentDistance;
			}
		}
		
		
		if(go_closestTarget){
			nma_agent.isStopped = false;
			nma_agent.SetDestination(go_closestTarget.transform.position);
			if(Vector3.Distance(transform.position,go_closestTarget.transform.position) < Constants.EnemyStats.C_EnemyAttackRange)
				EnterStateAttack();
		}
		else{
			EnterStateWander();
			//nma_agent.isStopped = true;
		}
			
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

		//TODO: While loop to keep checking if deltaX and deltaZ are within bounds

		Vector3 position = new Vector3(transform.x + deltaX, 0, transform.z + deltaZ);
 
        NavMeshHit navHit;
 
        NavMesh.SamplePosition (position, out navHit, wanderingRadius, -1);

        return navHit.position;
    }

	protected override void ChildUpdateWander() {
		bool b_playersAvailable = false;

		for(int i = 0; i < riftController.go_playerReferences.Length; i++){	
			if(riftController.go_playerReferences[i].GetComponent<PlayerController>().e_Side == e_Side && riftController.go_playerReferences[i].GetComponent<PlayerController>().isWisp == false){
				b_playersAvailable = true;
				break;
			}
		}

		if (b_playersAvailable) {
			EnterStateChase();
		}
		else {
			 Wander();
		}
	}

	protected override void ChildUpdateDie() {
		riftController.DecreaseEnemies(e_Side);
	}

	protected override void ChildEnterStateWander() {
		//Reset timer
		timer = timeLimit;
	}

    protected override void ChildEnterStateAttack() {
        GetComponent<Animator>().Play("placeholder_enemy_attack");
    }

    protected override void ChildDoAttack() {
		go_closestTarget.GetComponent<PlayerController>().TakeDamage(Constants.EnemyStats.C_EnemyDamage);
    }
	
	protected override void ChildUpdateSlowed(){
		if(Vector3.Distance(transform.position,go_closestTarget.transform.position) < Constants.EnemyStats.C_EnemyAttackRange)
				EnterStateAttack();
	}
}