/*  Camera Facing Billboard - Sam Caulker
 * 
 *  Desc:   Makes player and enemy indicators follow their targets and face the requested camera
 *  
 */

using UnityEngine;

public class CameraFacingBillboard : MonoBehaviour {
#region Variables and Declarations
    [SerializeField] private Camera cam_Camera;
    [SerializeField] private GameObject go_trackedObject;
    [SerializeField] private Vector3 v3_offset;
#endregion

#region Camera Facing Billboard Methods
    public void Init(Camera cam, GameObject target)
    {
        cam_Camera = cam;
        go_trackedObject = target;
    }
#endregion

#region Unity Overrides
    void Start() {
        if (go_trackedObject.GetComponent<PlayerController>() == null) {
            Destroy(gameObject, 1.0f);  // enemy indicators do not persist after spawn
        }
    }

    void Update() {
        transform.LookAt(transform.position + cam_Camera.transform.rotation * Vector3.forward,
            cam_Camera.transform.rotation * Vector3.up);
        transform.position = go_trackedObject.transform.position + v3_offset;
    }
#endregion
}