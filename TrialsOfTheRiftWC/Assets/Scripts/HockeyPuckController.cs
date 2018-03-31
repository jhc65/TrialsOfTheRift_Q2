/*  Hockey Puck Controller - Dana Thompson
 * 
 *  Desc:   Controls changes to Ice Hockey Objective's Puck movement and speed
 * 
 */
using System.Collections;
using UnityEngine;

public class HockeyPuckController : SpellTarget {
#region Variables and Declarations
    [SerializeField] private IceHockeyObjective iho_owner;    // identifies objective puck is a part of
#endregion

#region HockeyPuckController Methods
    override public void ApplySpellEffect(Constants.SpellStats.SpellType spell, Constants.Global.Color color, float damage, Vector3 direction) {

        if (spell != Constants.SpellStats.SpellType.MAGICMISSILE) {
            if (rb.isKinematic == true) {
                rb.isKinematic = false;
            }

            CancelInvoke();     // reset slowdown invoke
            InvokeRepeating("DecreaseSpeed", Constants.ObjectiveStats.C_PuckSpeedDecayDelay, Constants.ObjectiveStats.C_PuckSpeedDecayRate);
        }

        switch (spell) {
            case Constants.SpellStats.SpellType.WIND:
                rb.AddForce(direction * Constants.SpellStats.C_WindForce);
                f_speed += Constants.ObjectiveStats.C_PuckSpeedHitIncrease;
                transform.Rotate(direction);
                break;
            case Constants.SpellStats.SpellType.ICE:
                rb.isKinematic = true;
                rb.velocity = Vector3.zero;
                rb.angularVelocity = Vector3.zero;
                f_speed = Constants.ObjectiveStats.C_PuckBaseSpeed;
                break;
            case Constants.SpellStats.SpellType.ELECTRICITYAOE:
                f_speed = 0.5f * Constants.ObjectiveStats.C_PuckBaseSpeed;
                break;
        }
    }

    public void ResetPuckPosition() {
        if (e_color == Constants.Global.Color.RED) {
            transform.localPosition = Constants.ObjectiveStats.C_RedPuckSpawn;
        }
        else {
            transform.localPosition = Constants.ObjectiveStats.C_BluePuckSpawn;
        }

        //stop its movement entirely
        rb.isKinematic = true;
        rb.velocity = Vector3.zero;
        rb.angularVelocity = Vector3.zero;
        f_speed = Constants.ObjectiveStats.C_PuckBaseSpeed;
    }

    private void DecreaseSpeed() {
        f_speed -= Constants.ObjectiveStats.C_PuckSpeedDecreaseAmount;
    }

    ////if the puck gets stuck in the portal, move it over from it and reset its speed
    //private void PuckIsStuckInPortal() {

    //    Vector3 v3_rightPortal = new Vector3(37.25f, 0.5f, -15.25f);
    //    Vector3 v3_leftPortal = new Vector3(-37.25f, 0.5f, 15.25f);
    //    if (transform.position == v3_rightPortal) {
    //        transform.position = new Vector3(32.25f, 0.5f, -15.25f);
    //    }
    //    if (transform.position == v3_leftPortal) {
    //        transform.position = new Vector3(-32.25f, 0.5f, 15.25f);
    //    }

    //    isPuckStuck = false;
    //}

#endregion

#region Unity Overrides
    void Start() {
        f_speed = Constants.ObjectiveStats.C_PuckBaseSpeed;     // cannot read from Constants.cs in initialization at top
    }

    void Update() {
        // resets speed if it goes over threshold
        if (f_speed > Constants.ObjectiveStats.C_PuckMaxSpeed) {
            f_speed = Constants.ObjectiveStats.C_PuckMaxSpeed;
        } else if (f_speed < Constants.ObjectiveStats.C_PuckBaseSpeed && f_speed != (0.5f * Constants.ObjectiveStats.C_PuckBaseSpeed)) {
            CancelInvoke("DecreaseSpeed");
            f_speed = Constants.ObjectiveStats.C_PuckBaseSpeed;
        }

        Vector3 v3_dir = rb.velocity.normalized;
        rb.velocity = v3_dir * f_speed;
    }

    void OnCollisionEnter(Collision collision) {
        if (collision.gameObject.CompareTag("Enemy") || collision.gameObject.CompareTag("Player")) {
            Physics.IgnoreCollision(GetComponent<Collider>(), collision.gameObject.GetComponent<Collider>());
            StartCoroutine("ApplyDamage", collision.gameObject);
        } else if (!collision.gameObject.CompareTag("Rift") && !collision.gameObject.CompareTag("Portal") && !collision.gameObject.CompareTag("Spell")) {
            // Reflect puck on collision
            // https://youtube.com/watch?v=u_p50wENBY
            Vector3 v = Vector3.Reflect(transform.forward, collision.contacts[0].normal);
            float rot = 90 - Mathf.Atan2(v.z, v.x) * Mathf.Rad2Deg;
            transform.eulerAngles = new Vector3(90, rot, 0);
            transform.Rotate(v);
            rb.velocity = transform.forward * f_speed;
        }
    }

    void OnTriggerEnter(Collider other) {
        if (other.tag == "HockeyGoal") {   // player scoring with puck TODO: look at layers and tags
            if (other.GetComponent<GoalController>().Color != e_color)
            {
                iho_owner.UpdatePuckScore();
                ResetPuckPosition();
            }
        } else if (other.tag == "ParryShield") {
            if (rb.isKinematic == true)
            {
                rb.isKinematic = false;
            }

            // Reset slowdown invoke
            CancelInvoke();
            InvokeRepeating("DecreaseSpeed", Constants.ObjectiveStats.C_PuckSpeedDecayDelay, Constants.ObjectiveStats.C_PuckSpeedDecayRate);

            Vector3 facingDirection = other.gameObject.transform.forward.normalized;
            transform.Rotate(facingDirection);
            rb.velocity = facingDirection * f_speed;
        }
    }

    void OnTriggerExit(Collider other) {
        StopCoroutine("ApplyDamage");
    }

    public IEnumerator ApplyDamage(GameObject go_target) {
        if (go_target.GetComponent<PlayerController>()) {
            go_target.GetComponent<PlayerController>().TakeDamage(Constants.ObjectiveStats.C_PuckDamage, Constants.Global.DamageType.PUCK);
        } else if (go_target.GetComponent<EnemyController>()) {
            go_target.GetComponent<EnemyController>().TakeDamage(Constants.ObjectiveStats.C_PuckDamage);
        }

        yield return new WaitForSeconds(1);
        StartCoroutine("ApplyDamage", go_target);
    }
    #endregion
}
