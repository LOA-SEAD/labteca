using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class ReactionsController : MonoBehaviour 
{
	public string nameReaction;
	public float aConcentration;
	public float bConcentration;

	public bool makeReaction = false;

	public float ph;
	public float polarizibility;
	public float condutibility;
	public float solubility;
	public float turbility;

	public Color color;


	// Use this for initialization
	void Start () {
	
	}
	
	// Update is called once per frame
	void Update () 
	{
		if (makeReaction) 
		{
			makeReaction = false;
			MakeReaction(nameReaction, aConcentration, bConcentration);
		}
	}


	public void MakeReaction(string reactionName, float concentrationA, float concentrationB)
	{
		ReactionClass reaction = ReactionsSaver.LoadReactions () [reactionName];

		float realConcentrationA;
		float realConcentrationB;

		float realConcentrationC;
		float realConcentrationD;

		realConcentrationB = concentrationB; // limitant!!! 

		realConcentrationA = realConcentrationB * reaction.aMultipler;

		realConcentrationC = realConcentrationA * reaction.cMultipler;

		realConcentrationD = realConcentrationA * reaction.dMultipler;

		Dictionary<string, ReagentsLiquidClass> reagents = ComponentsSaver.LoadReagents();

		ph = (reagents [reaction.cName].ph * realConcentrationC + reagents [reaction.dName].ph * realConcentrationD) / (realConcentrationC + realConcentrationD);
		polarizibility = (reagents [reaction.cName].polarizibility * realConcentrationC + reagents [reaction.dName].polarizibility * realConcentrationD) / (realConcentrationC + realConcentrationD);
		condutibility = (reagents [reaction.cName].condutibility * realConcentrationC + reagents [reaction.dName].condutibility * realConcentrationD) / (realConcentrationC + realConcentrationD);
		solubility = (reagents [reaction.cName].solubility * realConcentrationC + reagents [reaction.dName].solubility * realConcentrationD) / (realConcentrationC + realConcentrationD);
		turbility = (reagents [reaction.cName].turbility * realConcentrationC + reagents [reaction.dName].turbility * realConcentrationD) / (realConcentrationC + realConcentrationD);

		float colorR = (reagents [reaction.cName].color.r * realConcentrationC + reagents [reaction.dName].color.r * realConcentrationD) / (realConcentrationC + realConcentrationD);
		float colorG = (reagents [reaction.cName].color.g * realConcentrationC + reagents [reaction.dName].color.g * realConcentrationD) / (realConcentrationC + realConcentrationD);
		float colorB = (reagents [reaction.cName].color.b * realConcentrationC + reagents [reaction.dName].color.b * realConcentrationD) / (realConcentrationC + realConcentrationD);
		float colorA = (reagents [reaction.cName].color.a * realConcentrationC + reagents [reaction.dName].color.a * realConcentrationD) / (realConcentrationC + realConcentrationD);

		color = new Color (colorR, colorG, colorB, colorA);
	}
}
