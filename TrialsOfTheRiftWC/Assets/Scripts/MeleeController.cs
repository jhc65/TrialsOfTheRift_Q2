using UnityEngine;
using System.Collections;

public class MeleeController : EnemyController {
	
	private GameObject go_closestTarget;

	protected override void ChildEnterStateChase() {
		
    }

    protected override void ChildUpdateChase() {
		go_closestTarget = null;
		float f_minDistance = 9999f;
		float f_currentDistance;
		for(int i = 0; i < Constants.C_Players.Length; i++){	
			f_currentDistance = Vector3.Distance(Constants.C_Players[i].transform.position,transform.position);
			if(Constants.C_Players[i].GetComponent<PlayerController>().e_Side == e_Side && f_currentDistance < f_minDistance && Constants.C_Players[i].GetComponent<PlayerController>().isWisp == false){
				go_closestTarget = Constants.C_Players[i];
				f_minDistance = f_currentDistance;
			}
		}
		
		
		if(go_closestTarget){
			nma_agent.isStopped = false;
			nma_agent.SetDestination(go_closestTarget.transform.position);
			if(Vector3.Distance(transform.position,go_closestTarget.transform.position) < Constants.EnviroStats.C_EnemyAttackRange)
				EnterStateAttack();
		}
		else{
			nma_agent.isStopped = true;
		}
			
    }

    protected override void ChildEnterStateAttack() {
        GetComponent<Animator>().Play("placeholder_enemy_attack");
    }

    protected override void ChildUpdateAttack() {

    }

    protected override void ChildDoAttack() {
		go_closestTarget.GetComponent<PlayerController>().TakeDamage(Constants.EnviroStats.C_EnemyDamage);
    }

    protected override void ChildEnterStateDie() {
    }

    protected override void ChildUpdateDie() {
    }
	
	protected override void ChildEnterStateFrozen(){
	}
	
	protected override void ChildUpdateFrozen(){
	}
	
	protected override void ChildEnterStateSlowed(){
	}
	
	protected override void ChildUpdateSlowed(){
		if(Vector3.Distance(transform.position,go_closestTarget.transform.position) < Constants.EnviroStats.C_EnemyAttackRange)
				EnterStateAttack();
	}
}