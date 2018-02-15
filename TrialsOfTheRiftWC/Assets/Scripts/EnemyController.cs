using UnityEngine;
using System.Linq;
using System;
using System.Collections;
[RequireComponent(typeof(Rigidbody))]
[RequireComponent(typeof(UnityEngine.AI.NavMeshAgent))]

public abstract class EnemyController : MonoBehaviour {

	//Added WANDER and FLEE states
    protected enum State {CHASE, ATTACK, FROZEN, SLOWED, DIE, WANDER, FLEE, SUMMONING, DROPPING};
	[SerializeField] public Constants.Global.Side e_Side;
	[SerializeField] protected float f_health;
	[SerializeField] protected float f_damage;
	protected State e_State;
	protected State e_previousState; //Used for returning to the state previous to entering the AttackState.
	protected State[] e_statusPriorityList = new State[] {State.FROZEN,State.SLOWED};
	protected Rigidbody r_rigidbody;
	protected UnityEngine.AI.NavMeshAgent nma_agent;
	public float f_canMove = 1f;
    protected RiftController riftController;
	protected Maestro maestro;

	//The random destination the bot chooses when wandering
	protected Vector3 destination;

	//The radius of which the bot will pick it's random destination
	protected float wanderingRadius = 10.0f;

	//The a timer that keeps track of how long a bot has been wandering
	protected float timer;

	//The time limit for the bot to wander
	protected float timeLimit = 4.0f;

	protected void Start() {
		r_rigidbody = GetComponent<Rigidbody>();
		nma_agent = GetComponent<UnityEngine.AI.NavMeshAgent>();
		f_health = Constants.EnemyStats.C_EnemyHealth;
		f_damage = Constants.EnemyStats.C_EnemyDamage;
		nma_agent.speed = Constants.EnemyStats.C_EnemyBaseSpeed;
		nma_agent.acceleration = nma_agent.acceleration* (Constants.EnemyStats.C_EnemyBaseSpeed / 3.5f);

		//Initializing the timer, destination, and wanderingRadius
		timer = 0.0f;
		destination = transform.position;

		EnterStateWander ();
        riftController = RiftController.Instance;     // reference to Rift singleton
		maestro = Maestro.Instance;     // reference to Rift singleton
		maestro.PlayEnemySpawn();			   
    }
	
	// Update is called once per frame
	protected void Update () {
		if(f_health <= 0f){
			Debug.Log("death");
			EnterStateDie();
		}
		
		switch (e_State) {
		case State.CHASE:
			UpdateChase ();
			break;
		case State.WANDER:
			UpdateWander ();
			break;
		case State.FLEE:
			UpdateFlee ();
			break;
		case State.SUMMONING:
			UpdateSummoning ();
			break;
		case State.DROPPING:
			UpdateDropping ();
			break;
		case State.ATTACK:
			UpdateAttack();
			break;
		case State.FROZEN:
			UpdateFrozen();
			break;
		case State.SLOWED:
			UpdateSlowed();
			break;
		case State.DIE:
			UpdateDie ();
			break;
		}

		ChildUpdate();
    }

	protected virtual void ChildUpdate(){}

	protected void EnterStateChase() {
		e_State = State.CHASE;
		ChildEnterStateChase();
    }
	protected virtual void ChildEnterStateChase(){}

    protected void UpdateChase() {
		ChildUpdateChase();
    }
	protected virtual void ChildUpdateChase(){}

	protected void EnterStateFlee() {
		e_State = State.FLEE;
		ChildEnterStateFlee();
    }

	protected virtual void ChildEnterStateFlee(){}

    protected void UpdateFlee() {
		ChildUpdateFlee();
    }
	protected virtual void ChildUpdateFlee(){}

	protected void EnterStateWander() {
		e_State = State.WANDER;
		ChildEnterStateWander();
    }

	protected virtual void ChildEnterStateWander(){}

    protected void UpdateWander() {
		ChildUpdateWander();
    }
	protected virtual void ChildUpdateWander(){}

	protected void EnterStateSummoning() {
		e_State = State.SUMMONING;
		ChildEnterStateSummoning();
    }

	protected virtual void ChildEnterStateSummoning(){}

    protected void UpdateSummoning() {
		ChildUpdateSummoning();
    }
	protected virtual void ChildUpdateSummoning(){}

	protected void EnterStateDropping() {
		e_State = State.DROPPING;
		ChildEnterStateDropping();
    }

	protected virtual void ChildEnterStateDropping(){}

    protected void UpdateDropping() {
		ChildUpdateDropping();
    }
	protected virtual void ChildUpdateDropping(){}

    protected void EnterStateAttack() {
		if(e_State != State.ATTACK)
			e_previousState = e_State;
        e_State = State.ATTACK;
		ChildEnterStateAttack();
    }
	protected virtual void ChildEnterStateAttack(){}

    protected void UpdateAttack() {
		ChildUpdateAttack();
    }
	protected virtual void ChildUpdateAttack(){}

    protected void DoAttack() {
		ChildDoAttack();
    }
	protected virtual void ChildDoAttack(){}

    protected void AttackOver() {
		switch (e_previousState) {
		case State.SLOWED:
			UpdateSlowed();
			break;
		default:
			EnterStateChase ();
			break;
		}
    }

    protected void EnterStateDie() {
		e_State = State.DIE;
		maestro.PlayEnemyDie();
		ChildEnterStateDie();
    }
	protected virtual void ChildEnterStateDie(){}

    protected void UpdateDie() {
		ChildUpdateDie();
        //riftController.DecreaseEnemies(e_Side);
		Destroy(gameObject);
    }
	protected virtual void ChildUpdateDie(){}
	
	public void TakeDamage(float damage){
		maestro.PlayEnemyHit();
		f_health -= (int)damage;
		//Debug.Log(i_health);
		//if(i_health <= 0f){
		//	Debug.Log("death");
		//	EnterStateDie();
		//}
	}
	
	protected void EnterStateFrozen(float f) {
		e_State = State.FROZEN;
		f_canMove = f;
		UpdateSpeed();
		//nma_agent.isStopped = true;
		Invoke("Unfreeze", Constants.SpellStats.C_IceFreezeTime);
		ChildEnterStateFrozen();
    }
	protected virtual void ChildEnterStateFrozen(){}

    protected void UpdateFrozen() {
		ChildUpdateFrozen();
    }
	protected virtual void ChildUpdateFrozen(){}
	
	public void Freeze(float f){
		EnterStateFrozen(f);
	}

	private void Unfreeze(){
		f_canMove = 1f;
		UpdateSpeed();
		EnterStateChase();
	}
	
	protected void EnterStateSlowed(float f) {
		if(e_statusPriorityList.Contains(e_State) && Array.IndexOf(e_statusPriorityList,State.SLOWED) > Array.IndexOf(e_statusPriorityList,e_State))
			return;
		e_State = State.SLOWED;
		f_canMove = f;
		UpdateSpeed();
		ChildEnterStateSlowed();
    }
	protected virtual void ChildEnterStateSlowed(){}

    protected void UpdateSlowed() {
		ChildUpdateSlowed();
    }
	protected virtual void ChildUpdateSlowed(){}
	
	public void Slow(float f){
		EnterStateSlowed(f);
	}

	public void Unslow(){
		f_canMove = 1f;
		UpdateSpeed();
		EnterStateChase();
	}

    public float GetHealth() {
        return f_health;
    }

    public void SetHealth(float healthIn) {
        f_health = healthIn;
    }
	
	private void UpdateSpeed(){
		nma_agent.speed = Constants.EnemyStats.C_EnemyBaseSpeed * f_canMove;
		//nma_agent.acceleration = nma_agent.acceleration* (Constants.EnviroStats.C_EnemySpeed / 3.5f) * f_canMove;
	}

	//If the bot tries to move to a destination that's out of bounds
	//This will reset the destination with in bounds
	protected void CheckOutOfBounds() {
		if (e_Side == Constants.Global.Side.LEFT) {
			if (destination.x < -37.0f) {
				destination.x = -37.0f;
			}
			else if (destination.x > -1.0f) {
				destination.x = -1.0f;
			}
		}
		else {
			if (destination.x > 37.0f) {
				destination.x = 37.0f;
			}
			else if (destination.x < 1.0f) {
				destination.x = 1.0f;
			}
		}

		if (destination.z > 21.0f) {
			destination.z = 21.0f;
		}
		else if (destination.z < -21.0f) {
			destination.z = -21.0f;
		}
	}

	protected bool IsWithinBounds(Vector3 position, Constants.Global.Side side) {
		if (side == Constants.Global.Side.LEFT) {
			if (position.x <= -39.0f) {
				return false;
			}
			else if (position.x >= 1.0f) {
				return false;
			}
		}
		else {
			if (position.x >= 39.0f) {
				return false;
			}
			else if (position.x <= 1.0f) {
				return false;
			}
		}

		if (position.z >= 21.0f) {
			return false;
		}
		else if (position.z <= -21.0f) {
			return false;
		}

		return true;
	}
}