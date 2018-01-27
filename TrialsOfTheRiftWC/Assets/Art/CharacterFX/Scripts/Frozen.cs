using UnityEngine;
using System.Collections;

public class Frozen : MonoBehaviour
{
	public AudioSource FrozenSource;
	public AudioClip FrozenSound; //crackle
	public Renderer SourceRenderer;

	public Animator SourceAnimator;
	private float AnimSpeed = 1.0f;

	public Animation SourceAnimation;
	
	private bool  IsFrozen = false;
	private Material[] EffectMaterials;	
	public float EffectLength = 0.0f;	
	
	void Start () 
	{
		EffectMaterials = SourceRenderer.materials;
	}
		
	public void OnDestroy()
	{
		foreach(Material m in EffectMaterials)
		{
			Destroy (m);
		}
	}
	
	private void SetMaterialParms(float amount)
	{
		foreach(Material m in EffectMaterials)
		{
			if (m.shader.name.Contains("Character/Crystal"))
			{
				m.SetFloat("_DiffuseAmount",amount);
			}
		}	
	}	
	
	private float StartEffect()
	{
		if (FrozenSource != null && FrozenSound != null)
		{
			EffectLength = FrozenSound.length;
			FrozenSource.PlayOneShot(FrozenSound);
		}
		return EffectLength;
	}
	
	private IEnumerator DoFreeze()
	{
		if (!IsFrozen) 
		{
			IsFrozen = true;
			if (SourceAnimator != null)
			{
				AnimSpeed = SourceAnimator.speed;
				SourceAnimator.speed = 0;
			}
			if (SourceAnimation != null)
			{
            Animation anim = SourceAnimation.GetComponent<Animation>();

            if (anim != null)
            {
               foreach(AnimationState ast in anim)
               {
                  ast.speed = 0.0f;
               }
            }
			}
		
			float LengthLeft = StartEffect();
				
			while(LengthLeft > 0.0f)
	    	{
				float pos = LengthLeft / EffectLength;
				SetMaterialParms(pos);
    	    	yield return null;
				LengthLeft -= Time.deltaTime;
    		}
			SetMaterialParms(0.0f);
		}
	}

	private IEnumerator DoThaw()
	{
		if (IsFrozen) 
		{
			float LengthLeft = StartEffect();
				
			while(LengthLeft > 0.0f)
    		{
				float pos = 1.0f - (LengthLeft / EffectLength);
				SetMaterialParms (pos);
        		yield return null;
				LengthLeft -= Time.deltaTime;
    		}
			SetMaterialParms(1.0f);
			if (SourceAnimator != null)
			{
				SourceAnimator.speed = AnimSpeed;
			}
			if (SourceAnimation != null)
			{
            Animation anim = SourceAnimation.GetComponent<Animation>();

            if (anim != null)
            {
               foreach(AnimationState ast in anim)
				   {
				   	ast.speed = 1.0f;
				   }
            }
			}
			
			IsFrozen = false;
		}
	}

	public void Freeze()
	{
		StartCoroutine(DoFreeze());
	}

	public void Thaw()
	{
		StartCoroutine(DoThaw());
	}
	
	public void Freeze(float length)
	{
		EffectLength = length;
		Freeze();
	}

	public void Thaw(float length)
	{
		EffectLength = length;
		Thaw();
	}
}

