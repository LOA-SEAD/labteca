using UnityEngine;
using System.Collections;

//! Class that defines a Reaction.
public class ReactionClass
{
    public string name;     //String with Reaction's key

	public string reagent1;   	 //String with reagent 'A' formula
	public int stoichiometryR1;  //Integer with 'A' multiplier

	public string reagent2;   	 //String with reagent 'B' formula
	public int stoichiometryR2;  //Integer with 'B' multiplier

	public string mainProduct;    		  //String with main product's formula
	public int stoichiometryMainProduct;  //Integer with main product's multiplier

	public string subProduct;    		 //String with sub-product's formula
	public int stoichiometrySubProduct;  //Integer with sub-product's multiplier

	public ReactionClass() {}
	public ReactionClass(ReactionClass re) {
		this.name = re.name;
		this.reagent1 = re.reagent1;
		this.stoichiometryR1 = re.stoichiometryR1;
		this.reagent2 = re.reagent2;
		this.stoichiometryR2 = re.stoichiometryR2;
		this.mainProduct = re.mainProduct;
		this.stoichiometryMainProduct = re.stoichiometryMainProduct;
		this.subProduct = re.subProduct;
		this.stoichiometrySubProduct = re.stoichiometrySubProduct;
	}

	public virtual ReactionClass Clone() {
		return new ReactionClass (this);
	}
}
