using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class Hologram : MonoBehaviour {
    public enum HoloState { Off, On, PoweringUp, PoweringDown } 
    public float ClipVariance = 5.0f;
    public float RimVariance = 0.5f;
	public float lightflicker = 0.3f;
    public float MinXZScale = 0.01f;
    public AnimationCurve TurnOnCurve;
    public AnimationCurve TurnOffCurve;

    // public bool IsOn = true;
    

    public Renderer HoloRenderer;

    public HoloState State;
    public Light FlickerLight;	
    private float clippower = 0.0f;
    private float rimpower = 0.0f;
    private float intensity = 0.0f;
	private Material[] HoloMaterials = null;
    private Vector3 OffScale = new Vector3(0, 0, 0);
    private Vector3 OnScale = new Vector3(1, 1, 1);
    private Vector3 TempScale = new Vector3(1, 1, 1);
    private float ProcessingTime = 0.0f;
    private float brightness = 0.0f;
	
    void Start()
    {

    }
	
	void Awake()
	{
        SkinnedMeshRenderer[] renderers = GetComponentsInChildren<SkinnedMeshRenderer>();
        List<Material> Materials = new List<Material>();
        foreach(SkinnedMeshRenderer smr in renderers)
        {
            Material[] mat = smr.materials;
            foreach (Material m in mat)
            {
                if (m.shader.name.Contains("Character/Holo"))
                {
                    Materials.Add(m);
                }
            }
        }

        HoloMaterials = Materials.ToArray();
		
    	clippower = HoloMaterials[0].GetFloat("_ClipPower");
    	rimpower = HoloMaterials[0].GetFloat("_RimPower");
        brightness = HoloMaterials[0].GetFloat("_Brightness");
		if (FlickerLight != null)
			intensity = FlickerLight.intensity;
        float val = TurnOffCurve.Evaluate(1.0f);
        OffScale = new Vector3(val, 0.0f, val);
        val = TurnOnCurve.Evaluate(1.0f);
        OnScale = new Vector3(val, 1.0f, val);

        if (State == HoloState.Off)
        {
            transform.localScale = OffScale;
        }
        else if (State == HoloState.On)
        {
            transform.localScale = OnScale;
        }
    }

    public void EnableScanlines(bool enabled)
	{
		clippower = 301.0f;
	}
	

    public void PowerUp()
    {
        State = HoloState.PoweringUp;
    }

    public void PowerDown()
    {
        State = HoloState.PoweringDown;
    }

    // Update is called every frame, if the
    // MonoBehaviour is enabled.
    void Update () {

        float newbrightness = brightness;

        switch (State)
        {
            case HoloState.Off:
                return;
            case HoloState.On:
                break;
            case HoloState.PoweringDown:
                ProcessingTime += Time.deltaTime;
                if (ProcessingTime > 1.0f)
                {
                    State = HoloState.Off;
                    ProcessingTime = 0.0f;
                    transform.localScale = OffScale;
                }
                else
                {
                    float val = TurnOffCurve.Evaluate(ProcessingTime);
                    TempScale.x = val;
                    TempScale.z = val;
                    transform.localScale = TempScale;
                    newbrightness = val * brightness;
                }
                break;
            case HoloState.PoweringUp:
                ProcessingTime += Time.deltaTime;
                if (ProcessingTime > 1)
                {
                    State = HoloState.On;
                    ProcessingTime = 0.0f;
                    transform.localScale = OnScale;
                }
                else
                {
                    float val = TurnOnCurve.Evaluate(ProcessingTime);
                    TempScale.x = val;
                    TempScale.z = val;
                    transform.localScale = TempScale;
                    newbrightness = val * brightness;
                }

                break;

        }

		// make hologram flicker
		float newclip =(clippower-(ClipVariance/2)) + (Random.value * ClipVariance);
		float rimrandom = Random.value;
		float rimchange = rimrandom * RimVariance;
		float newrim = rimpower-(RimVariance/2);
		if (newclip < 0) newclip = 0;
		if (newrim < 0) newrim = 0;
		
		foreach(Material HoloMaterial in HoloMaterials)
		{
			HoloMaterial.SetFloat("_RimPower",newrim);
			HoloMaterial.SetFloat("_ClipPower",newclip);
            HoloMaterial.SetFloat("_Brightness", newbrightness);
		}

		
		// make light flicker
		if (FlickerLight != null)
		{
			if (rimchange < 0)
			{	
				FlickerLight.intensity = intensity - (intensity * lightflicker * rimrandom);
			}
			else
			{
				FlickerLight.intensity = intensity + (intensity * lightflicker * rimrandom);
			}
		}
	}
	
	void OnDestroy()
	{
		foreach(Material HoloMaterial in HoloMaterials)
		{
			Destroy(HoloMaterial);
		}
	}
}
