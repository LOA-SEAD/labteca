using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using System.Text;

//! Holds the values to be compared as the result of phases
public class ResultVerifier {

	private Dictionary<int, Dictionary<string, string>> phases; // < numer of phase, Dictionary< name of variable, variable's value as a string > >
	
	// Singleton
	private static ResultVerifier instance;		
	public static ResultVerifier GetInstance () {
		if (instance == null) {
			instance = new ResultVerifier ();
		}
		return instance;
	}
	
	private ResultVerifier () {
		phases = new Dictionary<int, Dictionary<string, string>> ();
		
		//Load the phases from file into memory
		phases = PhasesSaver.LoadPhases ();

		/*foreach (string c in CompoundFactory.GetInstance().Collection.Values) {
			foreach (ReactionClass re in reactions.Values) {
				if(re.reagent1 == c.Formula) {
					table[re.reagent1, re.reagent2] = re.name;
					table[re.reagent2, re.reagent1] = re.name;
				}
			}
		}*/
	}
	
	//! Returns whether the content of the glassware is correct or not
	public bool VerifyResult (int lvl, object content) {
		Dictionary<string, string> currentPhase = phases [lvl]; //Dictionary<name of variable, value.ToString()>
		float maxError = 0.2f;

		bool flag = false;
		if (content is Mixture) {
			if((content as Mixture).ProductFormula() != "") { //Making sure the product exists
				if((content as Mixture).Leftover1Molarity() > maxError || (content as Mixture).Leftover2Molarity() > maxError) { //There's too much leftover to be correct
					return false;
				}
				else {
					foreach (string k in currentPhase.Keys) {
						switch(k) {
						case "name":
							if(currentPhase[k] == (content as Mixture).ProductName()) {
								flag = true;
							}
							else {
								return false;
							}
							break;
						case "formula":
							if(currentPhase[k] == (content as Mixture).ProductFormula()) {
								flag = true;
							}
							else {
								return false;
							}
							break;
						case "molarity":
							if(float.Parse(currentPhase[k]) == (content as Mixture).ProductMolarity()) {
								flag = true;
							}
							else {
								return false;
							}
							break;
						case "volume":
							if(float.Parse (currentPhase[k]) == (content as Mixture).Volume) {
								flag = true;
							}
							else {
								return false;
							}
							break;
						}
					}
					if(flag) {
						return true;
					}
				}
			}
		} else if (content is Compound) {
			foreach (string k in currentPhase.Keys) {
				switch(k) {
				case "name":
					if(currentPhase[k] == (content as Compound).Name) {
						flag = true;
					}
					else {
						return false;
					}
					break;
				case "formula":
					if(currentPhase[k] == (content as Compound).Formula) {
						flag = true;
					}
					else {
						return false;
					}
					break;
				case "molarity":
					if(float.Parse(currentPhase[k]) == (content as Compound).Molarity) {
						flag = true;
					}
					else {
						return false;
					}
					break;
				case "volume":
					if(float.Parse (currentPhase[k]) == (content as Compound).Volume) {
						flag = true;
					}
					else {
						return false;
					}
					break;
				}
			}
			if(flag) {
				return true;
			}
		}
		return false;
	}

	//Overload for testing the first phase
	public bool VerifyResult (object content) {
		bool flag = false;
		float maxError = 0.1f;
		float desiredMolarity = 1.0f;
		float minimumSolutionVolume = 25.0f;

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
