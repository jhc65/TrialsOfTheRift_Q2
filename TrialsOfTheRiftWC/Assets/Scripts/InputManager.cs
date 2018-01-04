using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public static class InputManager{

	public enum Axes{
		HORIZONTAL, VERTICAL,
		AIMHORIZONTAL, AIMVERTICAL,
		MAGICMISSILE,
		WINDSPELL, ICESPELL,
		ELECTRICSPELL,
		//Ultimate,
		INTERACT,
		MENU, SUBMIT, CANCEL
	}

	public static Dictionary<Axes, string> P1_XBOX = new Dictionary<Axes, string> {
		{Axes.HORIZONTAL,"P1 Horizontal XBOX"},
		{Axes.VERTICAL,"P1 Vertical XBOX"},
		{Axes.AIMHORIZONTAL,"P1 Aim Horizontal XBOX"},
		{Axes.AIMVERTICAL,"P1 Aim Vertical XBOX"},
		{Axes.MAGICMISSILE,"P1 Magic Missile XBOX"},
		{Axes.ELECTRICSPELL, "P1 Electricity Spell XBOX"},
		{Axes.WINDSPELL,"P1 Wind Spell XBOX"},
		{Axes.ICESPELL,"P1 Ice Spell XBOX"},
		//{Axes.Ultimate,"P1 Ultimate XBOX"},
		{Axes.INTERACT,"P1 Interact XBOX"},
		{Axes.MENU,"P1 Menu XBOX"},
		{Axes.SUBMIT,"P1 Submit XBOX"},
		{Axes.CANCEL,"P1 Cancel XBOX"}
	};

	public static Dictionary<Axes, string> P2_XBOX = new Dictionary<Axes, string> {
		{Axes.HORIZONTAL,"P2 Horizontal XBOX"},
		{Axes.VERTICAL,"P2 Vertical XBOX"},
		{Axes.AIMHORIZONTAL,"P2 Aim Horizontal XBOX"},
		{Axes.AIMVERTICAL,"P2 Aim Vertical XBOX"},
		{Axes.MAGICMISSILE,"P2 Magic Missile XBOX"},
		{Axes.ELECTRICSPELL, "P2 Electricity Spell XBOX"},
		{Axes.WINDSPELL,"P2 Wind Spell XBOX"},
		{Axes.ICESPELL,"P2 Ice Spell XBOX"},
		//{Axes.Ultimate,"P2 Ultimate XBOX"},
		{Axes.INTERACT,"P2 Interact XBOX"},
		{Axes.MENU,"P2 Menu XBOX"},
		{Axes.SUBMIT,"P2 Submit XBOX"},
		{Axes.CANCEL,"P2 Cancel XBOX"}
	};

	public static Dictionary<Axes, string> P3_XBOX = new Dictionary<Axes, string> {
		{Axes.HORIZONTAL,"P3 Horizontal XBOX"},
		{Axes.VERTICAL,"P3 Vertical XBOX"},
		{Axes.AIMHORIZONTAL,"P3 Aim Horizontal XBOX"},
		{Axes.AIMVERTICAL,"P3 Aim Vertical XBOX"},
		{Axes.MAGICMISSILE,"P3 Magic Missile XBOX"},
		{Axes.ELECTRICSPELL, "P3 Electricity Spell XBOX"},
		{Axes.WINDSPELL,"P3 Wind Spell XBOX"},
		{Axes.ICESPELL,"P3 Ice Spell XBOX"},
		//{Axes.Ultimate,"P3 Ultimate XBOX"},
		{Axes.INTERACT,"P3 Interact XBOX"},
		{Axes.MENU,"P3 Menu XBOX"},
		{Axes.SUBMIT,"P3 Submit XBOX"},
		{Axes.CANCEL,"P3 Cancel XBOX"}
	};

	public static Dictionary<Axes, string> P4_XBOX = new Dictionary<Axes, string> {
		{Axes.HORIZONTAL,"P4 Horizontal XBOX"},
		{Axes.VERTICAL,"P4 Vertical XBOX"},
		{Axes.AIMHORIZONTAL,"P4 Aim Horizontal XBOX"},
		{Axes.AIMVERTICAL,"P4 Aim Vertical XBOX"},
		{Axes.MAGICMISSILE,"P4 Magic Missile XBOX"},
		{Axes.ELECTRICSPELL, "P4 Electricity Spell XBOX"},
		{Axes.WINDSPELL,"P4 Wind Spell XBOX"},
		{Axes.ICESPELL,"P4 Ice Spell XBOX"},
		//{Axes.Ultimate,"P4 Ultimate XBOX"},
		{Axes.INTERACT,"P4 Interact XBOX"},
		{Axes.MENU,"P4 Menu XBOX"},
		{Axes.SUBMIT,"P4 Submit XBOX"},
		{Axes.CANCEL,"P4 Cancel XBOX"}
	};

	public static Dictionary<Axes, string> P1_PS4 = new Dictionary<Axes, string> {
		{Axes.HORIZONTAL,"P1 Horizontal PS4"},
		{Axes.VERTICAL,"P1 Vertical PS4"},
		{Axes.AIMHORIZONTAL,"P1 Aim Horizontal PS4"},
		{Axes.AIMVERTICAL,"P1 Aim Vertical PS4"},
		{Axes.MAGICMISSILE,"P1 Magic Missile PS4"},
		{Axes.ELECTRICSPELL, "P1 Electricity Spell PS4"},
		{Axes.WINDSPELL,"P1 Wind Spell PS4"},
		{Axes.ICESPELL,"P1 Ice Spell PS4"},
		//{Axes.Ultimate,"P1 Ultimate PS4"},
		{Axes.INTERACT,"P1 Interact PS4"},
		{Axes.MENU,"P1 Menu PS4"},
		{Axes.SUBMIT,"P1 Submit PS4"},
		{Axes.CANCEL,"P1 Cancel PS4"}
	};

	public static Dictionary<Axes, string> P2_PS4 = new Dictionary<Axes, string> {
		{Axes.HORIZONTAL,"P2 Horizontal PS4"},
		{Axes.VERTICAL,"P2 Vertical PS4"},
		{Axes.AIMHORIZONTAL,"P2 Aim Horizontal PS4"},
		{Axes.AIMVERTICAL,"P2 Aim Vertical PS4"},
		{Axes.MAGICMISSILE,"P2 Magic Missile PS4"},
		{Axes.ELECTRICSPELL, "P2 Electricity Spell PS4"},
		{Axes.WINDSPELL,"P2 Wind Spell PS4"},
		{Axes.ICESPELL,"P2 Ice Spell PS4"},
		//{Axes.Ultimate,"P2 Ultimate PS4"},
		{Axes.INTERACT,"P2 Interact PS4"},
		{Axes.MENU,"P2 Menu PS4"},
		{Axes.SUBMIT,"P2 Submit PS4"},
		{Axes.CANCEL,"P2 Cancel PS4"}
	};

	public static Dictionary<Axes, string> P3_PS4 = new Dictionary<Axes, string> {
		{Axes.HORIZONTAL,"P3 Horizontal PS4"},
		{Axes.VERTICAL,"P3 Vertical PS4"},
		{Axes.AIMHORIZONTAL,"P3 Aim Horizontal PS4"},
		{Axes.AIMVERTICAL,"P3 Aim Vertical PS4"},
		{Axes.MAGICMISSILE,"P3 Magic Missile PS4"},
		{Axes.ELECTRICSPELL, "P3 Electricity Spell PS4"},
		{Axes.WINDSPELL,"P3 Wind Spell PS4"},
		{Axes.ICESPELL,"P3 Ice Spell PS4"},
		//{Axes.Ultimate,"P3 Ultimate PS4"},
		{Axes.INTERACT,"P3 Interact PS4"},
		{Axes.MENU,"P3 Menu PS4"},
		{Axes.SUBMIT,"P3 Submit PS4"},
		{Axes.CANCEL,"P3 Cancel PS4"}
	};

	public static Dictionary<Axes, string> P4_PS4 = new Dictionary<Axes, string> {
		{Axes.HORIZONTAL,"P4 Horizontal PS4"},
		{Axes.VERTICAL,"P4 Vertical PS4"},
		{Axes.AIMHORIZONTAL,"P4 Aim Horizontal PS4"},
		{Axes.AIMVERTICAL,"P4 Aim Vertical PS4"},
		{Axes.MAGICMISSILE,"P4 Magic Missile PS4"},
		{Axes.ELECTRICSPELL, "P4 Electricity Spell PS4"},
		{Axes.WINDSPELL,"P4 Wind Spell PS4"},
		{Axes.ICESPELL,"P4 Ice Spell PS4"},
		//{Axes.Ultimate,"P4 Ultimate PS4"},
		{Axes.INTERACT,"P4 Interact PS4"},
		{Axes.MENU,"P4 Menu PS4"},
		{Axes.SUBMIT,"P4 Submit PS4"},
		{Axes.CANCEL,"P4 Cancel PS4"}
	};


	public static Dictionary<Axes, string> P1_Map = P1_PS4;		// P1, P2 default to PS4 for easier testing
	public static Dictionary<Axes, string> P2_Map = P2_PS4;
	public static Dictionary<Axes, string> P3_Map = P3_XBOX;	// P3, P4 default to XBOX to share the keyboard map
	public static Dictionary<Axes, string> P4_Map = P4_XBOX;


	public static float GetAxis(Axes a, int player) {
		switch(player){
			case 1:
				return Input.GetAxis(P1_Map[a]);
			case 2:
				return Input.GetAxis(P2_Map[a]);
			case 3:
				return Input.GetAxis(P3_Map[a]);
			case 4:
				return Input.GetAxis(P4_Map[a]);
			default:
				return 0;	// unreachable
		}
	}

	public static bool GetButtonDown(Axes a, int player) {
		switch (player){
			case 1:
				return Input.GetButtonDown(P1_Map[a]);
			case 2:
				return Input.GetButtonDown(P2_Map[a]);
			case 3:
				return Input.GetButtonDown(P3_Map[a]);
			case 4:
				return Input.GetButtonDown(P4_Map[a]);
			default:
				return false;	// unreachable
		}
	}

    public static bool GetButtonUp(Axes a, int player)
    {
        switch (player)
        {
            case 1:
                return Input.GetButtonUp(P1_Map[a]);
            case 2:
                return Input.GetButtonUp(P2_Map[a]);
            case 3:
                return Input.GetButtonUp(P3_Map[a]);
            case 4:
                return Input.GetButtonUp(P4_Map[a]);
            default:
                return false;   // unreachable
        }
    }

    public static bool GetButton(Axes a, int player) {
		switch (player){
			case 1:
				return Input.GetButton(P1_Map[a]);
			case 2:
				return Input.GetButton(P2_Map[a]);
			case 3:
				return Input.GetButton(P3_Map[a]);
			case 4:
				return Input.GetButton(P4_Map[a]);
			default:
				return false;	// unreachable
		}
	}
}
