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
        int timer = Constants.EnviroStats.C_CompletionTimer;
        CreateReverseFlagObject(timer);
    }

    override public void Complete()
    {
        // destroy prefab
        //adjust UI Elements
        b_complete = true;
        GameController.GetInstance().SelfDestructProgress(e_color, Constants.EnviroStats.C_SelfDestructThreshold);
        GameController.GetInstance().CompletionProgress(e_color, Constants.EnviroStats.C_CompletionTimer);
        Destroy(go_activePotato);
    }

    //if the completionTimer hits 30 seconds, complete the objective
    //if selfDestruct timer reaches 0, then spawn enemies and recreate the objective
    void Update()
    {
        if (go_activePotato.GetComponent<PotatoController>().getCompletionTimer() < 0)
        {
            Complete();
        }
        else if (go_activePotato.GetComponent<PotatoController>().getSelfDestructTimer() < 0)
        {
            Vector3 position = go_activePotato.transform.position;
            int timer = go_activePotato.GetComponent<PotatoController>().getCompletionTimer();
            Debug.Log(timer);

            Destroy(go_activePotato);
            GameController.GetInstance().SelfDestructProgress(e_color, Constants.EnviroStats.C_SelfDestructThreshold);

            //spawns 3 enemies
            for (int i = 0; i < 3; i++) {
                DarkMagician.GetInstance().CircularSpawn(position, e_Side);
            }

            CreateReverseFlagObject(timer);
        }
    }

    private void CreateReverseFlagObject(int timer) {
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

        go_activePotato.GetComponent<PotatoController>().setCompletionTimer(timer);
    }

    // [Param Fix] - Used in Parameters Screen. Will be removed in main game (probably)
    public override void ParamReset(float param)
    {
    }
}
