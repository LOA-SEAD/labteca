using UnityEngine;
using System.Collections;

public interface IPhysicochemical {

	//name
	string Name {
		get;
		set;
	}
	//realMass
	float RealMass {
		get;
		set;
	}
	//density
	float Density {
		get;
		set;
	}
	//solubility
	float Solubility {
		get;
		set;
	}
	//is solid?
	bool IsSolid {
		get;
		set;
	}
	//volume
	float Volume {
		get;
		set;
	}
	//pH
	float PH {
		get;
		set;
	}
	//conductibility
	float Conductibility {
		get;
		set;
	}
	//turbidity
	float Turbidity {
		get;
		set;
	}
	//polarizability
	float Polarizability {
		get;
		set;
	}
	//refratometer
	float Refratometer {
		get;
		set;
	}
}
