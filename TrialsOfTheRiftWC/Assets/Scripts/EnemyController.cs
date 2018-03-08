/*  Necromancer Controller - Noah Nam & Jeff Brown
 * 
 *  Desc:   Defines base functionality of enemy bots
 * 
 */
 
using UnityEngine;
using System.Linq;
using System;
using System.Collections;
[RequireComponent(typeof(Rigidbody))]
[RequireComponent(typeof(UnityEngine.AI.NavMeshAgent))]

public abstract class EnemyController : MonoBehaviour {

	//Added WANDER and FLEE states
    protected enum State {CHASE, ATTACK, FROZEN, SLOWED, DIE, WANDER, FLEE, SUMMONING, DROPPING};
	[SerializeField] public Constants.Global.Side e_side;
	[SerializeField] protected float f_health;
	[SerializeField] protected float f_damage;
	protected State e_state;
	protected State e_previousState; //Used for returning to the state previous to entering the AttackState.
	protected State[] e_statusPriorityList = new State[] {State.FROZEN,State.SLOWED};
	protected Rigidbody rb;
	protected UnityEngine.AI.NavMeshAgent nma_agent;
	protected float f_canMove = 1f;
    protected RiftController riftController;
	protected Maestro maestro;

	//The random destination the bot chooses when wandering
	protected Vector3 v3_destination;

	//The radius of which the bot will pick it's random destination
	protected float f_wanderingRadius = Constants.EnemyStats.C_WanderingRadius;

	//The a f_timer that keeps track of how long a bot has been wandering
	protected float f_timer;

	//The time limit for the bot to wander
	protected float f_timeLimit = 4.0f;

	protected virtual void EnterStateChase() {
		e_state = State.CHASE;
    }

    protected virtual void UpdateChase() {}

	protected virtual void EnterStateFlee() {
		e_state = State.FLEE;
    }

    protected virtual void UpdateFlee() {}

	protected virtual void EnterStateWander() {
		e_state = State.WANDER;
    }

    protected virtual void UpdateWander() {
    }

	protected virtual void EnterStateSummoning() {
		e_state = State.SUMMONING;
    }

    protected virtual void UpdateSummoning() {
    }

	protected virtual void EnterStateDropping() {
		e_state = State.DROPPING;
    }

    protected virtual void UpdateDropping() {
    }

    protected virtual void EnterStateAttack() {
		if(e_state != State.ATTACK)
			e_previousState = e_state;
        e_state = State.ATTACK;
    }

    protected virtual void UpdateAttack() {
    }

    protected virtual void DoAttack() {
    }

    protected void AttackOver() {
        Debug.Log("attackover");
		switch (e_previousState) {
		case State.SLOWED:
			UpdateSlowed();
			break;
		default:
			EnterStateChase ();
			break;
		}
    }

    protected virtual void EnterStateDie() {
		e_state = State.DIE;
		maestro.PlayEnemyDie();
    }

    protected virtual void UpdateDie() {
		EnterStateWander();
        //riftController.DecreaseEnemies(e_side);
		//Destroy(gameObject);
		gameObject.SetActive(false);
    }
	
	public void TakeDamage(float damage){
		maestro.PlayEnemyHit();
		f_health -= damage;
		//Debug.Log(i_health);
		if(f_health <= 0f){
			Debug.Log("death");
			EnterStateDie();
		}
	}
	
	protected virtual void EnterStateFrozen(float f) {
		e_state = State.FROZEN;
		f_canMove = f;
		UpdateSpeed();
		//nma_agent.isStopped = true;
		Invoke("Unfreeze", Constants.SpellStats.C_IceFreezeTime);
    }

    protected virtual void UpdateFrozen() {}
	
	public void Freeze(float f){
		EnterStateFrozen(f);
	}

	private void Unfreeze(){
		f_canMove = 1f;
		UpdateSpeed();
		EnterStateChase();
	}
	
	protected virtual void EnterStateSlowed(float f) {
		if(e_statusPriorityList.Contains(e_state) && Array.IndexOf(e_statusPriorityList,State.SLOWED) > Array.IndexOf(e_statusPriorityList,e_state))
			return;
		e_state = State.SLOWED;
		f_canMove = f;
		UpdateSpeed();
    }

    protected virtual void UpdateSlowed() {
    }
	
	public void Slow(){
		EnterStateSlowed(Constants.SpellStats.C_ElectricAOESlowDownMultiplier);
	}

	public void Unslow(){
		f_canMove = 1f;
		UpdateSpeed();
		EnterStateChase();
	}
	
	private void UpdateSpeed(){
		nma_agent.speed = riftController.EnemySpeed * f_canMove;
		//nma_agent.speed = riftController.f_enemySpeed * f_canMove;
		//nma_agent.acceleration = nma_agent.acceleration* (Constants.EnviroStats.C_EnemySpeed / 3.5f) * f_canMove;
	}

	//If the bot tries to move to a destination that's out of bounds
	//This will reset the destination with in bounds
	protected void CheckOutOfBounds() {
		if (e_side == Constants.Global.Side.LEFT) {
			if (v3_destination.x < -1*Constants.EnemyStats.C_MapBoundryXAxis) {
				v3_destination.x = -1*Constants.EnemyStats.C_MapBoundryXAxis;
			}
			else if (v3_destination.x > -1.0f) {
				v3_destination.x = -1.0f;
			}
		}
		else {
			if (v3_destination.x > Constants.EnemyStats.C_MapBoundryXAxis) {
				v3_destination.x = Constants.EnemyStats.C_MapBoundryXAxis;
			}
			else if (v3_destination.x < 1.0f) {
				v3_destination.x = 1.0f;
			}
		}

		if (v3_destination.z > Constants.EnemyStats.C_MapBoundryZAxis) {
			v3_destination.z = Constants.EnemyStats.C_MapBoundryZAxis;
		}
		else if (v3_destination.z < -1*Constants.EnemyStats.C_MapBoundryZAxis) {
			v3_destination.z = -1*Constants.EnemyStats.C_MapBoundryZAxis;
		}
	}

	public virtual void Init(Constants.Global.Side side) {
		riftController = RiftController.Instance;
		rb = GetComponent<Rigidbody>();
		nma_agent = GetComponent<UnityEngine.AI.NavMeshAgent>();
		e_side = side;
	}

	void Start() {

		//riftController = RiftController.Instance;     // reference to Rift singleton
		//rb = GetComponent<Rigidbody>();
		//nma_agent = GetComponent<UnityEngine.AI.NavMeshAgent>();
		f_health = Constants.EnemyStats.C_EnemyHealth;
		f_damage = Constants.EnemyStats.C_EnemyDamage;
		nma_agent.speed = riftController.EnemySpeed;

		//nma_agent.speed = Constants.EnemyStats.C_EnemyBaseSpeed;

		nma_agent.acceleration = nma_agent.acceleration* (Constants.EnemyStats.C_EnemyBaseSpeed / 3.5f);

		//Initializing the f_timer, destination, and f_wanderingRadius
		f_timer = 0.0f;
		v3_destination = transform.position;

		EnterStateWander ();
		maestro = Maestro.Instance;     // reference to Rift singleton
		maestro.PlayEnemySpawn();			   
    }
	
	// Update is called once per frame
	protected virtual void Update () {
		/*
		if(f_health <= 0f){
			Debug.Log("death");
			EnterStateDie();
		}
		*/

		switch (e_state) {
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
    }
}