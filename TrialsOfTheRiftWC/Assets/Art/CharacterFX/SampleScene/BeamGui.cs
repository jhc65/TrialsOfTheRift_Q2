using UnityEngine;
using System.Collections;

public class BeamGui : MonoBehaviour {
	
	public BeamController Beamer;
	public CustomColorsController Custom;
	public StoneController Stoner;
    public MouseOrbitImproved   Orbiter;
	public Hologram    HoloGuy;
	public GameObject    SpiritGuy;
    public FrozenController    FrozenDude;
    private Rect ButtonRect = new Rect(10,10,360, 600);
	
	void OnGUI () {

        GUILayout.BeginArea(ButtonRect);

        GUILayout.BeginHorizontal();
        if (GUILayout.Button("View Beamer"))
            Orbiter.target = Beamer.gameObject.transform;
        if (GUILayout.Button("Beam Out"))
            Beamer.BeamOut(false);
        if (GUILayout.Button("Beam In"))
            Beamer.BeamIn();
        GUILayout.EndHorizontal();

        GUILayout.BeginHorizontal();
        if (GUILayout.Button("View Custom"))
            Orbiter.target = Custom.gameObject.transform;
        if (GUILayout.Button("Eyes"))
            Custom.SetEyeColor(Random.value, Random.value, Random.value);
        if (GUILayout.Button("\"Hair\""))
            Custom.SetHairColor(Random.value, Random.value, Random.value);
        if (GUILayout.Button("Skin"))
            Custom.SetSkinColor(Random.value, Random.value, Random.value);
        GUILayout.EndHorizontal();

        GUILayout.BeginHorizontal();
        if (GUILayout.Button("View Stoner"))
            Orbiter.target = Stoner.gameObject.transform;
        if (GUILayout.Button("Petrify"))
            Stoner.TurnToStone();
        if (GUILayout.Button("Stone To Flesh"))
            Stoner.StoneToFlesh();
        GUILayout.EndHorizontal();

        GUILayout.BeginHorizontal();
        if (GUILayout.Button("View Hologram"))
            Orbiter.target = HoloGuy.gameObject.transform;
        if (GUILayout.Button("Power Up"))
            HoloGuy.PowerUp();
        if (GUILayout.Button("Power Down"))
            HoloGuy.PowerDown();
        GUILayout.EndHorizontal();

        GUILayout.BeginHorizontal();
		if (GUILayout.Button ("View Spirit"))
            Orbiter.target =  SpiritGuy.transform;
        GUILayout.EndHorizontal();

        GUILayout.BeginHorizontal();
        if (GUILayout.Button("View Frozen"))
            Orbiter.target = FrozenDude.transform;
        if (GUILayout.Button("Freeze Lerpz"))
            FrozenDude.Freeze();
        if (GUILayout.Button("Thaw Lerpz"))
            FrozenDude.Thaw();
        GUILayout.EndHorizontal();

        GUILayout.EndArea();
    }
}
