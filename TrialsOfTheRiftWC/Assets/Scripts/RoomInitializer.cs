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

    [SerializeField] private GameObject[] go_spawnerPool;         // rooms allowed for this objective to be played in
    [SerializeField] private Constants.Global.Side side;
    private RiftController riftController;

    void Awake() {
        riftController = RiftController.Instance;
    }

    void OnEnable() {
        if (side == Constants.Global.Side.RIGHT) {
            riftController.RightEnemySpawners = go_spawnerPool;
        }
        else {
            riftController.LeftEnemySpawners = go_spawnerPool;
        }
    }
}
