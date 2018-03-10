/*  Hockey Puck Controller - Dana Thompson
 * 
 *  Desc:   Controls changes to Ice Hockey Objective's Puck movement and speed
 * 
 */

using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class HockeyPuckController : MonoBehaviour {

    public IceHockeyObjective iho_owner;    // identifies objective puck is a part of
    public GameObject go_riftShield1;
    public GameObject go_riftShield2;
    public GameObject go_riftBossRed;
    public GameObject go_riftBossBlue;
    [SerializeField] private Constants.Global.Color e_color;  // identifies owning team
    [SerializeField] private Constants.Global.Side e_startSide;   // MUST BE SET IN EDITOR!
    private float f_speed;
    private Rigidbody rb;

    // Getters
   public float Speed {
        get { return f_speed; }
        set { f_speed = value; }
    }

    /*/////////////////////////////////////////////////////////////////////////////////////////////////////////////*/

    public void ResetPuckPosition() {
        if (e_color == Constants.Global.Color.RED) {
            transform.localPosition = Constants.ObjectiveStats.C_RedPuckSpawn;
        }
        else {
            transform.localPosition = Constants.ObjectiveStats.C_BluePuckSpawn;
        }

        //stop its movement entirely
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

    /*/////////////////////////////////////////////////////////////////////////////////////////////////////////////*/

    void Start() {
        rb = GetComponent<Rigidbody>();
        f_speed = Constants.ObjectiveStats.C_PuckBaseSpeed;     // cannot read from Constants.cs in initialization at top
        Physics.IgnoreCollision(GetComponent<Collider>(), go_riftShield1.GetComponent<Collider>());
        Physics.IgnoreCollision(GetComponent<Collider>(), go_riftShield2.GetComponent<Collider>());
        Physics.IgnoreCollision(GetComponent<Collider>(), go_riftBossRed.GetComponent<Collider>());
        Physics.IgnoreCollision(GetComponent<Collider>(), go_riftBossBlue.GetComponent<Collider>());
    }

    void Update() {
        //Incase the portal glitch pops up again, keep until portal is known issue
        //Vector3 v3_rightPortal = new Vector3(37.25f, 0.5f, -15.25f);
        //Vector3 v3_leftPortal = new Vector3(-37.25f, 0.5f, 15.25f);

        ////calls the function if the puck is believed to be stuck.  This way of invoking saves frames
        //if (!isPuckStuck && (transform.position == v3_rightPortal || transform.position == v3_leftPortal)) {
        //    Invoke("PuckIsStuckInPortal", 5.0f);
        //    Debug.Log("We're stuck in Portal potentially.");
        //    isPuckStuck = true;
        //}

        //resets speed if it goes over threshold
        if (f_speed > Constants.ObjectiveStats.C_PuckMaxSpeed) {
            f_speed = Constants.ObjectiveStats.C_PuckMaxSpeed;
        }

        // Set speed to base if it gets too low
        if (f_speed < Constants.ObjectiveStats.C_PuckBaseSpeed) {
            CancelInvoke("DecreaseSpeed");
            f_speed = Constants.ObjectiveStats.C_PuckBaseSpeed;
        }

        Vector3 v3_dir = rb.velocity.normalized;
        rb.velocity = v3_dir * f_speed;
    }

    void OnCollisionEnter(Collision collision) {
        if (collision.gameObject.CompareTag("Enemy")) {
            collision.gameObject.GetComponent<EnemyController>().TakeDamage(Constants.EnemyStats.C_EnemyHealth);
        }
        else if (collision.gameObject.CompareTag("Player")) {
            collision.gameObject.GetComponent<PlayerController>().TakeDamage(Constants.ObjectiveStats.C_PuckDamage, Constants.Global.DamageType.PUCK);
        }
        else if (collision.gameObject.CompareTag("Spell")) {
            // Reset slowdown invoke
            CancelInvoke();
            InvokeRepeating("DecreaseSpeed", Constants.ObjectiveStats.C_PuckSpeedDecayDelay, Constants.ObjectiveStats.C_PuckSpeedDecayRate);
        }
        else if (!collision.gameObject.CompareTag("Portal") && !collision.gameObject.CompareTag("Rift")) {
            // Reflect puck on collision
            // https://youtube.com/watch?v=u_p50wENBY
            Vector3 v = Vector3.Reflect(transform.forward, collision.contacts[0].normal);
            float rot = 90 - Mathf.Atan2(v.z, v.x) * Mathf.Rad2Deg;
            transform.eulerAngles = new Vector3(0, rot, 0);
            rb.velocity = transform.forward * f_speed;
        }
    }

    void OnTriggerEnter(Collider other) {
        if (other.tag == "HockeyGoal") {   // player scoring with puck TODO: look at layers and tags
            if(other.GetComponent<GoalController>().Color != e_color &&
                ((e_startSide == Constants.Global.Side.LEFT && rb.velocity.x > 0 ||
                  e_startSide == Constants.Global.Side.RIGHT && rb.velocity.x < 0))) { 
                iho_owner.UpdatePuckScore();
                ResetPuckPosition();
            }
        }
        else if (other.tag == "ParryShield") {
            // Reset slowdown invoke
            CancelInvoke();
            InvokeRepeating("DecreaseSpeed", Constants.ObjectiveStats.C_PuckSpeedDecayDelay, Constants.ObjectiveStats.C_PuckSpeedDecayRate);

            Vector3 facingDirection = other.gameObject.transform.root.forward.normalized;
            transform.rotation = Quaternion.LookRotation(other.gameObject.transform.root.forward);
            rb.velocity = facingDirection * f_speed;
        }
    }
}
