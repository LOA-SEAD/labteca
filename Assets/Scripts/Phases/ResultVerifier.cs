using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using System.Text;

//! Holds the values of expected results of each step, to be compared in the LIA State
public class ResultVerifier {


	private Dictionary<string, string> step;
	private TypeOfStep currentType;
	//ProgressController progressController;

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
		//progressController = GameObject.Find ("ProgressController").GetComponent<ProgressController> ();
	}

	public void SetVerificationStep(TypeOfStep stepType, Dictionary<string, string> newStep) {
		step = newStep;
		currentType = stepType;
		Debug.Log ("Settando step = " + currentType);
		//LIAState.SetVerificationInterface();
	}
	
	//! Returns whether the content of the glassware is correct or not
	public bool VerifyResult (int lvl, object content) {
		float maxError = float.Parse(step["maxError"]);

		bool flag = false;
		//The "Parse" function is used in every comparison to convert the strings into the correct types
		if (content is Mixture) { //TODO: Check if this is not checked before already
			if((content as Mixture).ProductFormula() != "") { //Making sure the product exists
				if((content as Mixture).Leftover1Molarity() > maxError || (content as Mixture).Leftover2Molarity() > maxError) { //There's too much leftover to be correct
					return false;
				}
				else {
					foreach (string k in step.Keys) {
						if(step[k] != "null") {
							switch(k) {
							case "productFormula":
								if(step[k] == (content as Mixture).ProductFormula()) {
									Debug.Log ((content as Mixture).ProductFormula());
									flag = true;
								}
								else {
									return false;
								}
								break;
							case "molarity":
								if(((content as Mixture).ProductMolarity() < float.Parse (step[k]) + maxError) &&
								   ((content as Mixture).ProductMolarity() > float.Parse (step[k]) - maxError)) {
									flag = true;
								}
								else {
									return false;
								}
								break;
							case "minVolume":
								if(((content as Mixture).Volume > float.Parse (step[k]) - maxError)) {
									flag = true;
								}
								else {
									return false;
								}
								break;
							case "density":
								if(((content as Mixture).Density < float.Parse (step[k]) + maxError) &&
								   ((content as Mixture).Density > float.Parse (step[k]) - maxError)) {
									flag = true;
								}
								else {
									return false;
								}
								break;
							case "turbidity":
								if(((content as Mixture).Turbidity < float.Parse (step[k]) + maxError) &&
								   ((content as Mixture).Turbidity > float.Parse (step[k]) - maxError)) {
									flag = true;
								}
								else {
									return false;
								}
								break;
							case "conductibility":
								if(((content as Mixture).Conductibility < float.Parse (step[k]) + maxError) &&
								   ((content as Mixture).Conductibility > float.Parse (step[k]) - maxError)) {
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

	/// <summary>
	/// Returns whether the content of the glassware is correct or not.
	/// The function handles the type of verification according to the 
	/// </summary>
	/// <returns> True, if correct. False otherwise. </returns>
	/// <param name="glassware">Glassware.</param>
	/*public bool VerifyResult(ref Glassware glassware) {
		switch(currentType) {
		case TypeOfStep.CompoundClass:
			
			break;
		case TypeOfStep.WhatCompound:
			
			break;
		case TypeOfStep.MolarityCheck:
			
			break;
		case TypeOfStep.GlasswareCheck:
			
			break;
		}
	}
*/
	public bool VerifyCheckBox(int option){ //TODO:check exactly how the answer will be defined
		bool answer;
		if (option == int.Parse(step ["correctAnswer"])) {
			answer = true;
		} else {
			answer = false;
		}
		return answer;
	}
	public bool VerifyTextBox(string textAnswer){
		bool answer = false;

		if (currentType == TypeOfStep.WhatCompound) {
			if (textAnswer == step ["compoundFormula"]) {
				answer = true;
			} else {
				answer = false;
			}
		} else if (currentType == TypeOfStep.MolarityCheck) {
			float value;
			if(float.TryParse(textAnswer, out value)) {
				if ( (value <= (float.Parse(step["molarity"]) + float.Parse(step["maxError"])))
				  && (value >= (float.Parse(step["molarity"]) - float.Parse(step["maxError"]))) ) {
					answer = true;
				}
				else {
					answer = false;
				}
			}
			else {
				answer = false;
			}
		}
		return answer;
	}
	public bool VerifyGlassware(Glassware glassware) {
		Debug.Log ("Verifying glassware");
		bool flag = false;
		float maxError = float.Parse (step ["maxError"]);

		if (glassware.content is Mixture) { //TODO: Check if this is not checked before already
			Debug.Log ("Entrou mixture");
			if ((glassware.content as Mixture).ProductFormula () != "") { //Making sure the product exists
				Debug.Log ("Entrou formula");
				if ((glassware.content as Mixture).Leftover1Molarity () > maxError || (glassware.content as Mixture).Leftover2Molarity () > maxError) { //There's too much leftover to be correct
					Debug.Log ("Entrou leftover");
					return false;
				} else {
					Debug.Log ("Else leftover");
					foreach (string k in step.Keys) {
						if (step [k] != "null") {
							switch (k) {
							case "productFormula":
								if (step [k] == (glassware.content as Mixture).ProductFormula ()) {
									Debug.Log ((glassware.content as Mixture).ProductFormula ());
									flag = true;
								} else {
									return false;
								}
								break;
							case "molarity":
								if (((glassware.content as Mixture).ProductMolarity () < float.Parse (step [k]) + maxError) &&
									((glassware.content as Mixture).ProductMolarity () > float.Parse (step [k]) - maxError)) {
									flag = true;
								} else {
									return false;
								}
								break;
							case "minVolume":
								if (((glassware.content as Mixture).Volume > float.Parse (step [k]) - maxError)) {
									flag = true;
								} else {
									return false;
								}
								break;
							/*case "density":
								if (((glassware.content as Mixture).Density < float.Parse (step [k]) + maxError) &&
									((glassware.content as Mixture).Density > float.Parse (step [k]) - maxError)) {
									flag = true;
								} else {
									return false;
								}
								break;
							case "turbidity":
								if (((glassware.content as Mixture).Turbidity < float.Parse (step [k]) + maxError) &&
									((glassware.content as Mixture).Turbidity > float.Parse (step [k]) - maxError)) {
									flag = true;
								} else {
									return false;
								}
								break;
							case "conductibility":
								if (((glassware.content as Mixture).Conductibility < float.Parse (step [k]) + maxError) &&
									((glassware.content as Mixture).Conductibility > float.Parse (step [k]) - maxError)) {
									flag = true;
								} else {
									return false;
								}
								break;*/
							}
						}
					}
					Debug.Log("Sera que deu bom?");
					if (flag) {
						Debug.Log ("Deu booom!");
						return true;
					}
				}
			}
		}
		else {
			Debug.Log ("Entrou nao mistura");
			foreach (string k in step.Keys) {
				if (step [k] != "null") {
					Debug.Log ("Pre switch");
					switch (k) {
					case "compoundFormula":
						Debug.Log ("Caso formula");
						if (step [k] == (glassware.content as Compound).Formula) {
							Debug.Log ((glassware.content as Compound).Formula);
							flag = true;
						} else {
							return false;
							}
						break;
					/*case "molarity": //TODO: reativar isso depois que tivermos as informaçoes da Teca
						Debug.Log ("Caso molaridade");
						if (((glassware.content as Compound).Molarity < float.Parse (step [k]) + maxError) &&
						    ((glassware.content as Compound).Molarity > float.Parse (step [k]) - maxError)) {
							Debug.Log ("Caso molaridade true");
							flag = true;
						} else {
							Debug.Log ("Caso molaridade false");
							return false;
							}
						break;*/
					case "minVolume":
						Debug.Log ("Caso min volume");
						if (((glassware.content as Compound).Volume > float.Parse (step [k]) - maxError)) {
							flag = true;
						} else {
							return false;
						}
						break;
					/*case "density":
						if (((glassware.content as Compound).Density < float.Parse (step [k]) + maxError) &&
						    ((glassware.content as Compound).Density > float.Parse (step [k]) - maxError)) {
							flag = true;
						} else {
							return false;
						}
						break;
					case "turbidity":
						if (((glassware.content as Compound).Turbidity < float.Parse (step [k]) + maxError) &&
						    ((glassware.content as Compound).Turbidity > float.Parse (step [k]) - maxError)) {
							flag = true;
						} else {
							return false;
						}
						break;
					case "conductibility":
						if (((glassware.content as Compound).Conductibility < float.Parse (step [k]) + maxError) &&
						    ((glassware.content as Compound).Conductibility > float.Parse (step [k]) - maxError)) {
							flag = true;
						} else {
							return false;
						}
						break;*/
					}
				}
			}
			Debug.Log("Sera que deu bom no composto?");
			if (flag) {
				Debug.Log ("Deu booom!");
				return true;
			}
		}
		return false;
	}
}
