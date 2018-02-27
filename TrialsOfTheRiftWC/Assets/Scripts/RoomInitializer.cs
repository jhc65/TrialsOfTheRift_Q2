/*  RoomInitializer - Dana Thompson
 * 
 *  Desc:   Determines what happens when a Room is set to be active
 * 
 */

using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class RoomInitializer : MonoBehaviour
{

    public GameObject[] go_spawnerPool;         // rooms allowed for this objective to be played in
    public Constants.Global.Side side;

    private void OnEnable()
    {
        if (side == Constants.Global.Side.RIGHT)
        {
            RiftController.Instance.SetRightEnemySpawners(go_spawnerPool);
        }
        else
        {
            RiftController.Instance.SetLeftEnemySpawners(go_spawnerPool);
        }
    }
}
