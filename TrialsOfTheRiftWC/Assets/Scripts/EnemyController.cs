using UnityEngine;
using System.Linq;
using System;
using System.Collections;
[RequireComponent(typeof(Rigidbody))]
[RequireComponent(typeof(UnityEngine.AI.NavMeshAgent))]

public abstract class EnemyController : MonoBehaviour {

    protected enum State {CHASE, ATTACK, FROZEN, SLOWED, DIE};
	[SerializeField] public Constants.Side e_Side;
	[SerializeField] protected int i_health;
	[SerializeField] protected float f_damage;
	protected State e_State;
	protected State e_previousState; //Used for returning to the state previous to entering the AttackState.
	protected State[] e_statusPriorityList = new State[] {State.FROZEN,State.SLOWED};
	protected Rigidbody r_rigidbody;
	protected UnityEngine.AI.NavMeshAgent nma_agent;
	public float f_canMove = 1f;
	
	protected void Start() {
		r_rigidbody = GetComponent<Rigidbody>();
		nma_agent = GetComponent<UnityEngine.AI.NavMeshAgent>();
		i_health = Constants.EnviroStats.C_EnemyHealth;
		f_damage = Constants.EnviroStats.C_EnemyDamage;
		nma_agent.speed = Constants.EnviroStats.C_EnemySpeed;
		nma_agent.acceleration = nma_agent.acceleration* (Constants.EnviroStats.C_EnemySpeed / 3.5f);
		EnterStateChase ();
	}
	
	// Update is called once per frame
	protected void Update () {
		if(i_health <= 0f){
			Debug.Log("death");
			EnterStateDie();
		}
		
		switch (e_State) {
		case State.CHASE:
			UpdateChase ();
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

	protected void EnterStateChase() {
		e_State = State.CHASE;
		ChildEnterStateChase();
    }
	protected abstract void ChildEnterStateChase();

    protected void UpdateChase() {
		ChildUpdateChase();
    }
	protected abstract void ChildUpdateChase();

    protected void EnterStateAttack() {
		if(e_State != State.ATTACK)
			e_previousState = e_State;
        e_State = State.ATTACK;
		ChildEnterStateAttack();
    }
	protected abstract void ChildEnterStateAttack();

    protected void UpdateAttack() {
		ChildUpdateAttack();
    }
	protected abstract void ChildUpdateAttack();

    protected void DoAttack() {
		ChildDoAttack();
    }
	protected abstract void ChildDoAttack();

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
		ChildEnterStateDie();
    }
	protected abstract void ChildEnterStateDie();

    protected void UpdateDie() {
		ChildUpdateDie();
		Destroy(gameObject);
    }
	protected abstract void ChildUpdateDie();
	
	public void TakeDamage(float damage){
		i_health -= (int)damage;
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
	protected abstract void ChildEnterStateFrozen();

    protected void UpdateFrozen() {
		ChildUpdateFrozen();
    }
	protected abstract void ChildUpdateFrozen();
	
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
	protected abstract void ChildEnterStateSlowed();

    protected void UpdateSlowed() {
		ChildUpdateSlowed();
    }
	protected abstract void ChildUpdateSlowed();
	
	public void Slow(float f){
		EnterStateSlowed(f);
	}

	public void Unslow(){
		f_canMove = 1f;
		UpdateSpeed();
		EnterStateChase();
	}

    public int GetHealth() {
        return i_health;
    }

    public void SetHealth(int healthIn) {
        i_health = healthIn;
    }
	
	private void UpdateSpeed(){
		nma_agent.speed = Constants.EnviroStats.C_EnemySpeed * f_canMove;
		//nma_agent.acceleration = nma_agent.acceleration* (Constants.EnviroStats.C_EnemySpeed / 3.5f) * f_canMove;
	}
}