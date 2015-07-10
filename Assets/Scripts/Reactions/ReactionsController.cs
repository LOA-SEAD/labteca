using UnityEngine;
using System.Collections;
using System.Collections.Generic;

//! Define reaction properties.
/*! */
// TODO: Verificar o funcionamento disse junto com os scripts dentro de Editor.
public class ReactionsController : MonoBehaviour 
{
	public string nameReaction;     /*!< String for name of reaction. */
    public float aConcentration;    /*!< Float for 'A' concentration. */
    public float bConcentration;    /*!< Float for 'B' concentration. */

    public bool makeReaction = false;   /*!< Bool to decide make reaction. */

    public float ph;                /*!< Float for Ph. */
    public float polarizibility;    /*!< Foat for polarizability. */    // TODO: refatorar: polarizability
    public float condutibility;     /*!< Float for conductibility. */   // TODO: refatorar: conductibility 
    public float solubility;        /*!< Float for solubility. */
    public float turbility;         /*!< Float for turbidity. */    // TODO: turbidity?

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
