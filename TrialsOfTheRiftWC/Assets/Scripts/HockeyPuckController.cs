using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class HockeyPuckController : MonoBehaviour {

    public Constants.Global.Color e_color; // identifies owning team
    public Constants.Global.Side e_startSide;
    public Constants.Global.Side e_Side;
    public float f_speed = Constants.ObjectiveStats.C_PuckBaseSpeed;
    public bool b_scored = false;   // identifies when the puck has been used to score
    private Vector3 v3_home;        // location of flag in players' base
    private Rigidbody rb;

    void Start()
    {
        v3_home = transform.position;
        rb = GetComponent<Rigidbody>();

        SetVelocitySide();
    }

    private void Update()
    {
        //if the Puck hasn't been hit by a spell or parry, start the speed cooldown
        InvokeRepeating("DecreaseSpeed", Constants.ObjectiveStats.C_SpeedDecayDelay, Constants.ObjectiveStats.C_SpeedDecayRate);

        if (transform.position.x > 0)
            e_Side = Constants.Global.Side.RIGHT;
        else
            e_Side = Constants.Global.Side.LEFT;

        if (f_speed < Constants.ObjectiveStats.C_PuckBaseSpeed) {
            CancelInvoke();
            f_speed = Constants.ObjectiveStats.C_PuckBaseSpeed;
        }

        Vector3 v3_dir = rb.velocity.normalized;
        rb.velocity = v3_dir * f_speed;
    }

    void OnTriggerEnter(Collider other)
    {
        if (other.tag == "HockeyGoal")
        {   // player scoring with puck
            if (e_startSide == Constants.Global.Side.LEFT && rb.velocity.x > 0 
                && other.GetComponent<GoalController>().GetColor() != e_color)
            {
                b_scored = true;
                transform.position = v3_home;
                SetVelocitySide();
            }
            if (e_startSide == Constants.Global.Side.RIGHT && rb.velocity.x < 0 
                && other.GetComponent<GoalController>().GetColor() != e_color)
            {
                b_scored = true;
                transform.position = v3_home;
                SetVelocitySide();
            }
        }

        if (other.tag == "Rift") {
            //ignores any collision detection between the Rift and puck
            Physics.IgnoreCollision(GetComponent<Collider>(), other);
        }

        if (other.tag == "ParryShield")
        {
            //we need to get the direction the player is facing, so that's why v3_direction is verbose
            CancelInvoke();
            f_speed += Constants.ObjectiveStats.C_HitIncreaseSpeed;
            Vector3 v3_direction = other.gameObject.transform.parent.gameObject.transform.forward.normalized;
            transform.rotation = Quaternion.LookRotation(other.gameObject.transform.parent.gameObject.transform.forward);
            gameObject.GetComponent<Rigidbody>().velocity = v3_direction * f_speed;
        }
    }

    private void OnCollisionEnter(Collision collision)
    {
        if (collision.gameObject.tag == "Enemy") {
            collision.gameObject.GetComponent<EnemyController>().TakeDamage(Constants.EnemyStats.C_EnemyHealth);
        }
        if (collision.gameObject.tag == "Player") {
            collision.gameObject.GetComponent<PlayerController>().TakeDamage(Constants.ObjectiveStats.C_PuckDamage);
        }
        if (collision.gameObject.tag == "Spell") {
            CancelInvoke();
        }
        if (collision.gameObject.tag == "Floor")
        {
            //ignores any collision detection between the floor and puck
            Physics.IgnoreCollision(GetComponent<Collider>(), collision.gameObject.GetComponent<Collider>());
        }
        else if (collision.gameObject.tag != "Portal" || collision.gameObject.tag != "Rift" || collision.gameObject.tag != "Spell") {
            Vector3 v = Vector3.Reflect(transform.forward, collision.contacts[0].normal);
            float rot = 90 - Mathf.Atan2(v.z, v.x) * Mathf.Rad2Deg;
            transform.eulerAngles = new Vector3(0, rot, 0);
            GetComponent<Rigidbody>().velocity = transform.forward * f_speed;
        }

    }

    public void ResetHome()
    {
        transform.position = v3_home;
    }

    //completely sets the pucks default state
    private void SetVelocitySide() {
        if (v3_home.x < 0)
        {
            e_startSide = Constants.Global.Side.LEFT;
            e_Side = e_startSide;
            rb.velocity = new Vector3(0.0f, 0.0f, -1.0f) * f_speed;
        }
        else
        {
            e_startSide = Constants.Global.Side.RIGHT;
            e_Side = e_startSide;
            rb.velocity = new Vector3(0.0f, 0.0f, 1.0f) * f_speed;
        }
    }

    //used to decrement the speed every second after 3 seconds
    private void DecreaseSpeed() {
        f_speed -= Constants.ObjectiveStats.C_SpeedDecreaseRate;
    }
}
