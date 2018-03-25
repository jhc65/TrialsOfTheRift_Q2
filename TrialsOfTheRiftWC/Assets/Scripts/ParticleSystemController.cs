/*  Particle System Controller - Joe Chew
 * 
 *  Desc:   Destroys spell particles a set time after creation
 * 
 */

using UnityEngine;

public class ParticleSystemController : MonoBehaviour {
#region Unity Overrides
    void Start() {
        Destroy(gameObject, Constants.SpellStats.C_SpellParticlesLiveTime);
    }
#endregion
}
