using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class FrozenController : MonoBehaviour 
{
	public List<Frozen> FreezableObjects = new List<Frozen>();
	public AudioSource EffectSource;
	public AudioClip EffectSound;
	public bool PlaySound;
	public float EffectLength = 3.0f;
    public bool  IsFrozen = false;
	
	// Use this for initialization
	void Start () 
	{
		if (FreezableObjects.Count == 0)
		{
			FreezableObjects.AddRange(GetComponentsInChildren<Frozen>());
		}
	}
	
	private float StartEffect()
	{
		if (PlaySound)
		{
			if (EffectSource != null && EffectSound != null)
			{
				EffectSource.PlayOneShot(EffectSound);
				return EffectSound.length;
			}
		}
		return EffectLength;
	}	
	
	public void Freeze()
	{
	
		if (IsFrozen)
			return;
		IsFrozen = true;
		
		float length = StartEffect();
		foreach(Frozen o in FreezableObjects)
		{
			o.Freeze(length);
		}
	}

	public void Thaw()
	{
		if (!IsFrozen)
			return;
		IsFrozen = false;

		float length = StartEffect();
		foreach(Frozen o in FreezableObjects)
		{
			o.Thaw(length);
		}
	}
}