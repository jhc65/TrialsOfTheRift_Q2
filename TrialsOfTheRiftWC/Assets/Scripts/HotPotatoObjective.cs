using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class HotPotatoObjective : Objective
{

    public GameObject go_redPotato, go_bluePotato;    // referenced potato objects
    private GameObject go_activePotato;    // active object specific to this objective instance
    private Constants.Side e_Side;

    override public void Instantiate()
    {
        // instantiate prefab based on color
        if (e_color == Constants.Color.RED)
        {
            go_activePotato = Instantiate(go_redPotato, Constants.C_RedHotCrystalSpawn, new Quaternion(0, 0, 0, 0));
            e_Side = Constants.Side.LEFT;
        }
        else
        {
            go_activePotato = Instantiate(go_bluePotato, Constants.C_BlueHotCrystalSpawn, new Quaternion(0, 0, 0, 0));
            e_Side = Constants.Side.RIGHT;
        }
    }

    override public void Complete()
    {
        // destroy prefab
        //adjust UI Elements
        b_complete = true;
        GameController.GetInstance().SelfDestructProgress(e_color, Constants.EnviroStats.C_SelfDestructThreshold);
        GameController.GetInstance().CompletionProgress(e_color, 0);
        Destroy(go_activePotato);
    }

    //if the completionTimer hits 30 seconds, complete the objective
    //if selfDestruct timer reaches 0, then spawn enemies and recreate the objective
    void Update()
    {
        if (go_activePotato.GetComponent<PotatoController>().getCompletionTimer() >= Constants.EnviroStats.C_CompletionTimer)
        {
            Complete();
        }
        else if (go_activePotato.GetComponent<PotatoController>().getSelfDestructTimer() < 0)
        {
            Vector3 position = go_activePotato.transform.position;
            Destroy(go_activePotato);
            GameController.GetInstance().SelfDestructProgress(e_color, Constants.EnviroStats.C_SelfDestructThreshold);

            //spawns 3 enemies
            for (int i = 0; i < 3; i++) {
                CircularSpawn(position);
            }

            Instantiate();
        }
    }

    //instantiates an enemy when a valid position is selected
    private void CircularSpawn(Vector3 center) {
        Vector3 pos = RandomCircle(center, 3.0f);
        //Quaternion rot = Quaternion.FromToRotation(Vector3.forward, center - pos);

        //checks to see if the position is occupied by anything with a collider
        //if there is, then find a new position for the enemy
        var hitColliders = Physics.OverlapSphere(pos, 0.005f);
        if (hitColliders.Length > 0)
        {
            Debug.Log("Papa Bless");
            CircularSpawn(center);
        }
        else
        {
            DarkMagician.GetInstance().SpawnEnemies(e_Side, pos);
        }
    }

    //gets a random Vector3 position within the certain radius from the potato
    private Vector3 RandomCircle(Vector3 center, float radius)
    {
        float ang = Random.value * 360;
        Vector3 pos;

        //by absolute valueing the x position, we can tell the enemy which side it should of the map it should be on
        int side = (int)e_Side;
        pos.x = side * Mathf.Abs(center.x + radius * Mathf.Sin(ang * Mathf.Deg2Rad));
        pos.y = center.y;
        pos.z = center.z + radius * Mathf.Cos(ang * Mathf.Deg2Rad);

        //reposition the enemies if they spawn outside of the map
        if (pos.z >= 22) {
            float diff = pos.z - 22;
            pos.z = pos.z - diff - 1;
        }
        else if (pos.z <= -22)
        {
            Debug.Log(pos.z);
            float diff = pos.z + 22;
            pos.z = pos.z - diff + 1;
            Debug.Log(pos.z);
        }

        if (pos.x >= 40)
        {
            float diff = pos.x - 40;
            pos.x = pos.x - diff - 1;
        }
        else if (pos.x <= -40)
        {
            float diff = pos.x + 40;
            pos.x = pos.x - diff + 1;
        }

        return pos;
    }

    // [Param Fix]
    public override void ParamReset(float param_in)
    {
    }
}
