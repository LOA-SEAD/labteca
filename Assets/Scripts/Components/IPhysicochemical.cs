using UnityEngine;
using System.Collections;

public interface IPhysicochemical {

	//name
	void SetName(string name);
	string GetName();
	//realMass
	void SetRealMass(float mass);
	float GetRealMass();
	//density
	void SetDensity(float density);
	float GetDensity();
	//solubility
	void SetSolubilitye(float solubility);
	float GetSolubility();

	
	//pH
	void SetPh (float pH);
	float GetPh();
	//conductibility
	void SetConductibility (float conductibility);
	float GetConductibility();
	//turbidity
	void SetTurbidity (float turbidity);
	float GetTurbidity();
	//polarizability
	void SetPolarizability (float polarizability);
	float GetPolarizability();
	//refratometer
	void SetRefratometer (float refratometer);
	float GetRefratometer(); 
}
