﻿using UnityEngine;
using UnityEditor;

public class CreateComponent : EditorWindow  
{
	private string name;
	private float density;
	private int molarMass;
	private float ph;
	private float polarizability;
	private Texture2D uvSpecter;
	private Texture2D irSpecter;
	private Texture2D flameSpecter;
	private float conductibility;
	private float solubility;
	private float turbidity;

	private Texture2D hplc;
	private float refratometer;

	private Texture2D texture;
	private Color color;


	private Vector2 scrollPosition;

	[MenuItem ("Reagente/Criar Reagente")]
	static void CreateWindow () 
	{
		CreateComponent window = EditorWindow.GetWindow(typeof(CreateComponent),true,"Criar Reagente") as CreateComponent;
		window.minSize = new Vector2(300f,650f);
		window.maxSize = new Vector2(300f,650f);
	}

	void OnGUI()
	{
		EditorGUILayout.BeginVertical();
		scrollPosition = EditorGUILayout.BeginScrollView(scrollPosition, GUILayout.Width(0), GUILayout.Height(0));

		EditorGUILayout.LabelField("Criar Reagente:");

		EditorGUILayout.Space();
		EditorGUILayout.Space();

		name = EditorGUILayout.TextField("Nome:",name);

		EditorGUILayout.Space();

		density = EditorGUILayout.FloatField("Densidade:", density);
		EditorGUILayout.Space();

		molarMass = EditorGUILayout.IntField("Massa Molar:", molarMass);
		EditorGUILayout.Space();

		ph = EditorGUILayout.FloatField("PH:", ph);
		EditorGUILayout.Space();

		polarizability = EditorGUILayout.FloatField("Polaridade:", polarizability);
		EditorGUILayout.Space();


		uvSpecter = EditorGUILayout.ObjectField("Espectro UV:", uvSpecter, typeof(Texture2D)) as Texture2D;
		EditorGUILayout.Space();
		EditorGUILayout.Space();
		EditorGUILayout.Space();

		irSpecter = EditorGUILayout.ObjectField("Espectro IR:", irSpecter, typeof(Texture2D)) as Texture2D;
		EditorGUILayout.Space();
		EditorGUILayout.Space();
		EditorGUILayout.Space();

		flameSpecter = EditorGUILayout.ObjectField("Espectro de Chama:", flameSpecter, typeof(Texture2D)) as Texture2D;
		EditorGUILayout.Space();
		EditorGUILayout.Space();
		EditorGUILayout.Space();

		conductibility = EditorGUILayout.FloatField("Condutividade:", conductibility);
		EditorGUILayout.Space();

		solubility = EditorGUILayout.FloatField("Solubilidade:", solubility);
		EditorGUILayout.Space();

		turbidity = EditorGUILayout.FloatField("Turbilidade:", turbidity);
		EditorGUILayout.Space();

		hplc = EditorGUILayout.ObjectField("HPLC:", hplc, typeof(Texture2D)) as Texture2D;
		EditorGUILayout.Space();
		EditorGUILayout.Space();
		EditorGUILayout.Space();

		refratometer = EditorGUILayout.FloatField("Refratometro:", refratometer);
		EditorGUILayout.Space();

		texture = EditorGUILayout.ObjectField("Textura:", texture, typeof(Texture2D)) as Texture2D;
		EditorGUILayout.Space();
		EditorGUILayout.Space();
		EditorGUILayout.Space();

		color = EditorGUILayout.ColorField ("Cor", color);
		EditorGUILayout.Space();

		if (GUILayout.Button ("SALVAR")) 
		{
			ComponentsSaver.SaveReagentFromEditor(name, molarMass, density, ph, polarizability, uvSpecter, irSpecter, flameSpecter, conductibility, solubility, turbidity, hplc, refratometer, texture, color);
			this.Close();
		}

		EditorGUILayout.EndScrollView();
		EditorGUILayout.EndVertical();
	}
}
