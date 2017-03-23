using UnityEngine;
using UnityEditor;
using System.Collections.Generic;

public class CreateReaction : EditorWindow  
{
	private string name;
	
	private int aMultipler;
	private int bMultipler;
	private int cMultipler;
	private int dMultipler;
	
	private string aName = "";
	private string bName = "";
	private string cName = "";
	private string dName = "";

	private Vector2 scrollPosition;

	[MenuItem ("Reacao/Criar Reacao")]
	static void CreateWindow () 
	{
		CreateReaction window = EditorWindow.GetWindow(typeof(CreateReaction),true,"Criar Reacao") as CreateReaction;
		window.minSize = new Vector2(900f,300f);
		window.maxSize = new Vector2(900f,300f);
	}

	void OnGUI()
	{
		EditorGUILayout.BeginVertical();
		scrollPosition = EditorGUILayout.BeginScrollView(scrollPosition, GUILayout.Width(0), GUILayout.Height(0));

		EditorGUILayout.LabelField("Criar Reaçao:");
		
		EditorGUILayout.Space();
		EditorGUILayout.Space();
		
		name = EditorGUILayout.TextField("Nome da Reaçao:",name);

		EditorGUILayout.Space();
		EditorGUILayout.Space();


		EditorGUILayout.LabelField ("        'X'                    A        +        'Y'                    B        =        'W'                 C          +       'Z'                  D");


		EditorGUILayout.BeginHorizontal();

		aMultipler = EditorGUILayout.IntField (aMultipler);
		aName = EditorGUILayout.TextField (aName);

		bMultipler = EditorGUILayout.IntField (bMultipler);
		bName = EditorGUILayout.TextField(bName);

		cMultipler = EditorGUILayout.IntField (cMultipler);
		cName = EditorGUILayout.TextField(cName);

		dMultipler = EditorGUILayout.IntField (dMultipler);
		dName = EditorGUILayout.TextField(dName);

		EditorGUILayout.EndHorizontal();

		EditorGUILayout.Space();

		Dictionary<string, Compound> reagents = CompoundFactory.GetInstance ().CupboardCollection;

		EditorGUILayout.Space();

		EditorGUILayout.BeginHorizontal();
		//TODO: Add the isSolid component when defining the components
		/*if(aName != "")
		{
			if (reagents.ContainsKey(aName)) 
			{
				EditorGUILayout.BeginVertical ();

				EditorGUILayout.LabelField ("Nome: " + reagents [aName].Formula);
				EditorGUILayout.LabelField ("MassaMolar: " + reagents [aName].MolarMass);
				EditorGUILayout.LabelField ("Densidade: " + reagents [aName].Density);
				EditorGUILayout.LabelField ("pH: " + reagents [aName].PH);
				EditorGUILayout.ColorField ("Cor: ", reagents [aName].compoundColor);

				EditorGUILayout.EndVertical ();
			} 
			else 
			{
				EditorGUILayout.BeginVertical ();
				
				EditorGUILayout.LabelField ("Nome: ");
				EditorGUILayout.LabelField ("MassaMolar: ");
				EditorGUILayout.LabelField ("Densidade: ");
				EditorGUILayout.LabelField ("pH: ");
				EditorGUILayout.ColorField ("Cor: ", Color.black);
				
				EditorGUILayout.EndVertical ();	
			}
		}
		else 
		{
			EditorGUILayout.BeginVertical ();
			
			EditorGUILayout.LabelField ("Nome: ");
			EditorGUILayout.LabelField ("MassaMolar: ");
			EditorGUILayout.LabelField ("Densidade: ");
			EditorGUILayout.LabelField ("pH: ");
			EditorGUILayout.ColorField ("Cor: ", Color.black);
			
			EditorGUILayout.EndVertical ();	
		}
	
		if(bName != "")
		{
			if (reagents.ContainsKey (bName)) 
			{
				EditorGUILayout.BeginVertical ();
				
				EditorGUILayout.LabelField ("Nome: " + reagents [bName].Formula);
				EditorGUILayout.LabelField ("MassaMolar: " + reagents [bName].MolarMass);
				EditorGUILayout.LabelField ("Densidade: " + reagents [bName].Density);
				EditorGUILayout.LabelField ("pH: " + (reagents [bName] as Compound).PH);
				EditorGUILayout.ColorField ("Cor: ", reagents [bName].compoundColor);
				
				EditorGUILayout.EndVertical ();
			} 
			else 
			{
				EditorGUILayout.BeginVertical ();
				
				EditorGUILayout.LabelField ("Nome: ");
				EditorGUILayout.LabelField ("MassaMolar: ");
				EditorGUILayout.LabelField ("Densidade: ");
				EditorGUILayout.LabelField ("pH: ");
				EditorGUILayout.ColorField ("Cor: ", Color.black);
				
				EditorGUILayout.EndVertical ();	
			}
		}
		else 
		{
			EditorGUILayout.BeginVertical ();
			
			EditorGUILayout.LabelField ("Nome: ");
			EditorGUILayout.LabelField ("MassaMolar: ");
			EditorGUILayout.LabelField ("Densidade: ");
			EditorGUILayout.LabelField ("pH: ");
			EditorGUILayout.ColorField ("Cor: ", Color.black);
			
			EditorGUILayout.EndVertical ();	
		}


		if(cName != "")
		{
			if (reagents.ContainsKey (cName)) 
			{
				EditorGUILayout.BeginVertical ();
				
				EditorGUILayout.LabelField ("Nome: " + reagents [cName].Formula);
				EditorGUILayout.LabelField ("MassaMolar: " + reagents [cName].MolarMass);
				EditorGUILayout.LabelField ("Densidade: " + reagents [cName].Density);
				EditorGUILayout.LabelField ("pH: " + (reagents [cName] as Compound).PH);
				EditorGUILayout.ColorField ("Cor: ", reagents [cName].compoundColor);
				
				EditorGUILayout.EndVertical ();
			} 
			else 
			{
				EditorGUILayout.BeginVertical ();
				
				EditorGUILayout.LabelField ("Nome: ");
				EditorGUILayout.LabelField ("MassaMolar: ");
				EditorGUILayout.LabelField ("Densidade: ");
				EditorGUILayout.LabelField ("ph: ");
				EditorGUILayout.ColorField ("Cor: ", Color.black);
				
				EditorGUILayout.EndVertical ();	
			}
		}
		else 
		{
			EditorGUILayout.BeginVertical ();
			
			EditorGUILayout.LabelField ("Nome: ");
			EditorGUILayout.LabelField ("MassaMolar: ");
			EditorGUILayout.LabelField ("Densidade: ");
			EditorGUILayout.LabelField ("pH: ");
			EditorGUILayout.ColorField ("Cor: ", Color.black);
			
			EditorGUILayout.EndVertical ();	
		}


		if(dName != "")
		{
			if (reagents.ContainsKey (dName)) 
			{
				EditorGUILayout.BeginVertical ();
				
				EditorGUILayout.LabelField ("Nome: " + reagents [dName].Formula);
				EditorGUILayout.LabelField ("MassaMolar: " + reagents [dName].MolarMass);
				EditorGUILayout.LabelField ("Densidade: " + reagents [dName].Density);
				EditorGUILayout.LabelField ("pH: " + (reagents [dName] as Compound).PH);
				EditorGUILayout.ColorField ("Cor: ", reagents [dName].compoundColor);
				
				EditorGUILayout.EndVertical ();
			} 
			else 
			{
				EditorGUILayout.BeginVertical ();
				
				EditorGUILayout.LabelField ("Nome: ");
				EditorGUILayout.LabelField ("MassaMolar: ");
				EditorGUILayout.LabelField ("Densidade: ");
				EditorGUILayout.LabelField ("pH: ");
				EditorGUILayout.ColorField ("Cor: ", Color.black);
				
				EditorGUILayout.EndVertical ();	
			}
		}
		else 
		{
			EditorGUILayout.BeginVertical ();
			
			EditorGUILayout.LabelField ("Nome: ");
			EditorGUILayout.LabelField ("MassaMolar: ");
			EditorGUILayout.LabelField ("Densidade: ");
			EditorGUILayout.LabelField ("pH: ");
			//EditorGUILayout.ColorField ("Cor: ", Color.black);
			
			EditorGUILayout.EndVertical ();	
		}

		EditorGUILayout.EndHorizontal();


		bool aOk = false;
		bool bOk = false;
		bool cOk = false;
		bool dOk = false;

		if (aName != "" && aMultipler != 0) 
		{
			if (reagents.ContainsKey (aName)) 
			{
				aOk = true;
			}
			else 
			{
				aOk = false;
			}
		} 
		else 
		{
			aOk = false;
		}

		if (cName != "" && cMultipler != 0) 
		{
			if(reagents.ContainsKey (cName))
			{
				cOk = true;
			}
			else 
			{
				cOk = false;
			}
		} 
		else 
		{
			cOk = false;
		}

		if (bName == "") 
		{
			if(bMultipler == 0)
			{
				bOk = true;
			}
			else
			{
				bOk = false;
			}
		}
		else
		{
			if(bMultipler != 0)
			{
				if(reagents.ContainsKey (bName))
				{
					bOk = true;
				}
				else
				{
					bOk = false;
				}
			}
			else
			{
				bOk = false;
			}
		}

		if (dName == "") 
		{
			if(dMultipler == 0)
			{
				dOk = true;
			}
			else
			{
				dOk = false;
			}
		}
		else
		{
			if(dMultipler != 0)
			{
				if(reagents.ContainsKey (dName))
				{
					dOk = true;
				}
				else
				{
					dOk = false;
				}
			}
			else
			{
				dOk = false;
			}
		}


		if (aOk && bOk && cOk && dOk && name != "") 
		{
			if(GUILayout.Button("Salvar"))
			{
				ReactionsSaver.SaveReactionsFromEditor(name, aMultipler, aName, bMultipler, bName, cMultipler, cName, dMultipler, dName);
				this.Close();
			}
		}*/

		if (GUILayout.Button ("Salvar")) {
			//ReactionsSaver.SaveReactionsFromEditor (name, aMultipler, aName, bMultipler, bName, cMultipler, cName, dMultipler, dName);
			this.Close ();
		} 

		EditorGUILayout.EndScrollView();
		EditorGUILayout.EndVertical();
	}
}
