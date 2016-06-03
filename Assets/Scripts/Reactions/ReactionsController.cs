﻿using UnityEngine;
using System.Collections;
using System.Collections.Generic;

//! Define reaction properties.

public class ReactionsController : MonoBehaviour 
{
	public string nameReaction;     /*!< String for name of reaction. */
    public float aConcentration;    /*!< Float for 'A' concentration. */
    public float bConcentration;    /*!< Float for 'B' concentration. */

    public bool makeReaction = false;   /*!< Bool to decide make reaction. */

    public float ph;                /*!< Float for Ph. */
    public float polarizability;    /*!< Float for polarizability. */
    public float conductibility;     /*!< Float for conductibility. */
    public float solubility;        /*!< Float for solubility. */
    public float turbidity;         /*!< Float for turbidity. */

	public Color color;             /*!< Color of Reaction */
	
	void Update () 
	{
		if (makeReaction) 
		{
			makeReaction = false;
			MakeReaction(nameReaction, aConcentration, bConcentration);
		}
	}

    //! Make the reaction using concentration A and B.
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

		float phC = 0, phD = 0;
		float turbidityC = 0, turbidityD = 0;

		Dictionary<string, Compound> reagents = CompoundFactory.GetInstance().Collection;

		if (!reagents [reaction.cName].IsSolid) {
			phC = reagents [reaction.cName].PH;
			turbidityC = reagents [reaction.cName].Turbidity;
		}

		if (!reagents [reaction.dName].IsSolid) {
			phD = reagents [reaction.cName].PH;
			turbidityD = reagents [reaction.cName].Turbidity;
		}

		polarizability = (reagents [reaction.cName].Polarizability * realConcentrationC + reagents [reaction.dName].Polarizability * realConcentrationD) / (realConcentrationC + realConcentrationD);
		conductibility = (reagents [reaction.cName].Conductibility * realConcentrationC + reagents [reaction.dName].Conductibility * realConcentrationD) / (realConcentrationC + realConcentrationD);
		solubility = (reagents [reaction.cName].Solubility * realConcentrationC + reagents [reaction.dName].Solubility * realConcentrationD) / (realConcentrationC + realConcentrationD);

		ph = (phC * realConcentrationC + phD * realConcentrationD) / (realConcentrationC + realConcentrationD);
		turbidity = (turbidityC * realConcentrationC + turbidityD * realConcentrationD) / (realConcentrationC + realConcentrationD);

/*		float colorR = (reagents [reaction.cName].color.r * realConcentrationC + reagents [reaction.dName].color.r * realConcentrationD) / (realConcentrationC + realConcentrationD);
		float colorG = (reagents [reaction.cName].color.g * realConcentrationC + reagents [reaction.dName].color.g * realConcentrationD) / (realConcentrationC + realConcentrationD);
		float colorB = (reagents [reaction.cName].color.b * realConcentrationC + reagents [reaction.dName].color.b * realConcentrationD) / (realConcentrationC + realConcentrationD);
		float colorA = (reagents [reaction.cName].color.a * realConcentrationC + reagents [reaction.dName].color.a * realConcentrationD) / (realConcentrationC + realConcentrationD);

		color = new Color (colorR, colorG, colorB, colorA);*/
	}
}
