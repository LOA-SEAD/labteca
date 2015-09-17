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

		Dictionary<string, ReagentsBaseClass> reagents = ComponentsSaver.LoadReagents();

		EditorGUILayout.Space();

		EditorGUILayout.BeginHorizontal();

		if(aName != "")
		{
			if (reagents.ContainsKey(aName)) 
			{
				EditorGUILayout.BeginVertical ();

				EditorGUILayout.LabelField ("Nome: " + reagents [aName].name);
				EditorGUILayout.LabelField ("MassaMolar: " + reagents [aName].molarMass);
				EditorGUILayout.LabelField ("Densidade: " + reagents [aName].density);
				if(reagents is ReagentsLiquidClass)
					EditorGUILayout.LabelField ("ph: " + (reagents [aName] as ReagentsLiquidClass).ph);
				EditorGUILayout.ColorField ("Cor: ", reagents [aName].color);

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
			EditorGUILayout.LabelField ("ph: ");
			EditorGUILayout.ColorField ("Cor: ", Color.black);
			
			EditorGUILayout.EndVertical ();	
		}
	
		if(bName != "")
		{
			if (reagents.ContainsKey (bName)) 
			{
				EditorGUILayout.BeginVertical ();
				
				EditorGUILayout.LabelField ("Nome: " + reagents [bName].name);
				EditorGUILayout.LabelField ("MassaMolar: " + reagents [bName].molarMass);
				EditorGUILayout.LabelField ("Densidade: " + reagents [bName].density);
				EditorGUILayout.LabelField ("ph: " + (reagents [bName] as ReagentsLiquidClass).ph);
				EditorGUILayout.ColorField ("Cor: ", reagents [bName].color);
				
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
			EditorGUILayout.LabelField ("ph: ");
			EditorGUILayout.ColorField ("Cor: ", Color.black);
			
			EditorGUILayout.EndVertical ();	
		}


		if(cName != "")
		{
			if (reagents.ContainsKey (cName)) 
			{
				EditorGUILayout.BeginVertical ();
				
				EditorGUILayout.LabelField ("Nome: " + reagents [cName].name);
				EditorGUILayout.LabelField ("MassaMolar: " + reagents [cName].molarMass);
				EditorGUILayout.LabelField ("Densidade: " + reagents [cName].density);
				EditorGUILayout.LabelField ("ph: " + (reagents [cName] as ReagentsLiquidClass).ph);
				EditorGUILayout.ColorField ("Cor: ", reagents [cName].color);
				
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
			EditorGUILayout.LabelField ("ph: ");
			EditorGUILayout.ColorField ("Cor: ", Color.black);
			
			EditorGUILayout.EndVertical ();	
		}


		if(dName != "")
		{
			if (reagents.ContainsKey (dName)) 
			{
				EditorGUILayout.BeginVertical ();
				
				EditorGUILayout.LabelField ("Nome: " + reagents [dName].name);
				EditorGUILayout.LabelField ("MassaMolar: " + reagents [dName].molarMass);
				EditorGUILayout.LabelField ("Densidade: " + reagents [dName].density);
				EditorGUILayout.LabelField ("ph: " + (reagents [dName] as ReagentsLiquidClass).ph);
				EditorGUILayout.ColorField ("Cor: ", reagents [dName].color);
				
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
			EditorGUILayout.LabelField ("ph: ");
			EditorGUILayout.ColorField ("Cor: ", Color.black);
			
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
		}

		EditorGUILayout.EndScrollView();
		EditorGUILayout.EndVertical();
	}
}
