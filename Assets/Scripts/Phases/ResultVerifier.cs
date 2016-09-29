using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using System.Text;

//! Holds the values of expected results of each phase, to be compared in the LIA State
public class ResultVerifier {

	// The phases dictionary uses the index as a key to access the dictionary of values for that phase.
	// The values of the phases are all in string, the conversion has to be done when comparing the values
	private Dictionary<int, Dictionary<string, string>> phases; // < index of phase, Dictionary< name of variable, variable's value as a string > >
	
	// Singleton
	private static ResultVerifier instance;		
	public static ResultVerifier GetInstance () {
		if (instance == null) {
			instance = new ResultVerifier ();
		}
		return instance;
	}

	//Constructor
	private ResultVerifier () {
		//
		phases = new Dictionary<int, Dictionary<string, string>> ();
		
		//Load the phases from file into memory
		phases = PhasesSaver.LoadPhases ();
	}
	
	//! Returns whether the content of the glassware is correct or not
	public bool VerifyResult (int lvl, object content) {
		Dictionary<string, string> currentPhase = phases [lvl]; //Dictionary<name of variable, value.ToString()>
		float maxError = 0.2f;

		bool flag = false;
		//The "Parse" function is used in every comparison to convert the strings into the correct types
		if (content is Mixture) { //TODO: Check if this is not checked before already
			if((content as Mixture).ProductFormula() != "") { //Making sure the product exists
				if((content as Mixture).Leftover1Molarity() > maxError || (content as Mixture).Leftover2Molarity() > maxError) { //There's too much leftover to be correct
					return false;
				}
				else {
					foreach (string k in currentPhase.Keys) {
						if(currentPhase[k] != "null") {
							switch(k) {
							case "productFormula":
								if(currentPhase[k] == (content as Mixture).ProductFormula()) {
									Debug.Log ((content as Mixture).ProductFormula());
									flag = true;
								}
								else {
									return false;
								}
								break;
							case "molarity":
								if(((content as Mixture).ProductMolarity() < float.Parse (currentPhase[k]) + maxError) &&
								   ((content as Mixture).ProductMolarity() > float.Parse (currentPhase[k]) - maxError)) {
									flag = true;
								}
								else {
									return false;
								}
								break;
							case "minVolume":
								if(((content as Mixture).Volume < float.Parse (currentPhase[k]) + maxError) &&
								   ((content as Mixture).Volume > float.Parse (currentPhase[k]) - maxError)) {
									flag = true;
								}
								else {
									return false;
								}
								break;
							case "density":
								if(((content as Mixture).Density < float.Parse (currentPhase[k]) + maxError) &&
								   ((content as Mixture).Density > float.Parse (currentPhase[k]) - maxError)) {
									flag = true;
								}
								else {
									return false;
								}
								break;
							case "turbidity":
								if(((content as Mixture).Turbidity < float.Parse (currentPhase[k]) + maxError) &&
								   ((content as Mixture).Turbidity > float.Parse (currentPhase[k]) - maxError)) {
									flag = true;
								}
								else {
									return false;
								}
								break;
							case "conductibility":
								if(((content as Mixture).Conductibility < float.Parse (currentPhase[k]) + maxError) &&
								   ((content as Mixture).Conductibility > float.Parse (currentPhase[k]) - maxError)) {
									flag = true;
								}
								else {
									return false;
								}
								break;
							}
						}
					}
					if(flag) {
						return true;
					}
				}
			}
		}
		return false;
	}

	//Overload for testing the first phase
	//TODO: DELETE THIS ONE, WHEN THE VERIFICATION IS WORKING CORRECTLY
	public bool VerifyResult (object content) {
		bool flag = false;
		float maxError = 0.1f;
		float desiredMolarity = 1.0f;
		float minimumSolutionVolume = 24.7f;

		if ((content is Mixture)) {
			if ((content as Mixture).ProductFormula () == "NaCl") {
				flag = true;
			} else {
				return false;
			}

			if (((content as Mixture).ProductMolarity () < desiredMolarity + maxError) && ((content as Mixture).ProductMolarity () > desiredMolarity - maxError)) {
				flag = true;
			} else {
				return false;
			}

			if (((content as Mixture).Leftover1Molarity () < 0.0f + maxError) && ((content as Mixture).Leftover1Molarity () > 0.0f - maxError)) {
				flag = true;
			} else {
				return false;
			}

			if (((content as Mixture).Leftover2Molarity () < 0.0f + maxError) && ((content as Mixture).Leftover2Molarity () > 0.0f - maxError)) {
				flag = true;
			} else {
				return false;
			}

			if ((content as Mixture).Volume > minimumSolutionVolume) {
				flag = true;
			} else {
				return false;
			}

			if (flag) {
				return true;
			} else {
				return false;
			}
		} else {
			return false;
		}
	}
}
