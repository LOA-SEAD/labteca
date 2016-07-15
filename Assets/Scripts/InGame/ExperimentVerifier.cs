using UnityEngine;
using System.Collections;

public class ExperimentVerifier {
	//Singleton
	private ExperimentVerifier instance;
	public ExperimentVerifier GetInstance() {
		if (instance == null) {
			instance = new ExperimentVerifier ();
		}
		return instance;
	}


	private ExperimentVerifier() {
	
	}
	
	public void VerifyExperiment(int expNumber) {

	}
}
