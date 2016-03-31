using UnityEngine;
using System.Collections;
using UnityEngine.UI;

public class JournalUIInfo : MonoBehaviour
{
	public Text nameT,physicalState,molarMassT,densityT,polarityT,conductibilityT,solubilityT,phT,turbidityT,refratometerT;
	public GameObject phG, turbilityG, refratometerG;

	public void setReagent(string name, float molarMass,bool isSolid,float density,float polarity,float conductibility,float solubility,float ph,float turbidity,float refratometer){
		nameT.text = name;
		molarMassT.text = molarMass.ToString ();
		densityT.text = density.ToString ();
		polarityT.text = polarity.ToString ();
		conductibilityT.text = conductibility.ToString ();
		solubilityT.text = solubility.ToString ();
		if (isSolid) {
			phG.SetActive (false);
			turbilityG.SetActive (false);
			refratometerG.SetActive (false);
			physicalState.text = "Solido";
		} else {
			phT.text=ph.ToString();
			turbidityT.text = turbidity.ToString();
			refratometerT.text=refratometer.ToString();
			physicalState.text = "Liquido";
		}
	}
}

