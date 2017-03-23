using UnityEngine;
using UnityEditor;

public class CreateProduct : EditorWindow  
{
	private string name;
	private string formula;
	private bool isSolid = false;
	private float density;
	private float purity;
	private float molarMass;
	private float ph;
	private float polarizability;
	private Texture2D uvSpecter;
	private Texture2D irSpecter;
	private float flameSpecter;
	private float conductibility;
	private float solubility;
	private float turbidity;
	
	private Texture2D hplc;
	private float refratometer;
	
	private bool fumeHoodOnly;
	
	private Texture2D texture;
	private Color color;
	
	
	private Vector2 scrollPosition;
	
	[MenuItem ("Produto/Criar Produto")]
	static void CreateWindow () 
	{
		CreateProduct window = EditorWindow.GetWindow(typeof(CreateProduct),true,"Criar Produto") as CreateProduct;
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
		
		formula = EditorGUILayout.TextField("Formula:",formula);
		EditorGUILayout.Space();
		
		isSolid = GUILayout.Toggle(isSolid, "Solid?");
		EditorGUILayout.Space();
		
		density = EditorGUILayout.FloatField("Densidade:", density);
		EditorGUILayout.Space();
		
		purity = EditorGUILayout.FloatField("Pureza:", purity);
		EditorGUILayout.Space();
		
		molarMass = EditorGUILayout.FloatField("Massa Molar:", molarMass);
		EditorGUILayout.Space();
		
		polarizability = EditorGUILayout.FloatField("Polaridade:", polarizability);
		EditorGUILayout.Space();
		
		conductibility = EditorGUILayout.FloatField("Condutividade:", conductibility);
		EditorGUILayout.Space();
		
		solubility = EditorGUILayout.FloatField("Solubilidade:", solubility);
		EditorGUILayout.Space();
		
		flameSpecter = EditorGUILayout.FloatField ("Espectro de Chama:", flameSpecter);
		EditorGUILayout.Space ();
		
		if (!isSolid) {
			
			ph = EditorGUILayout.FloatField ("PH:", ph);
			EditorGUILayout.Space ();
			
			turbidity = EditorGUILayout.FloatField("Turbilidade:", turbidity);
			EditorGUILayout.Space();
			
			refratometer = EditorGUILayout.FloatField("Refratometro:", refratometer);
			EditorGUILayout.Space();
			
			hplc = EditorGUILayout.ObjectField("HPLC:", hplc, typeof(Texture2D)) as Texture2D;
			EditorGUILayout.Space();
			EditorGUILayout.Space();
			EditorGUILayout.Space();
		}
		
		irSpecter = EditorGUILayout.ObjectField("Espectro IR:", irSpecter, typeof(Texture2D)) as Texture2D;
		EditorGUILayout.Space();
		EditorGUILayout.Space();
		EditorGUILayout.Space();
		
		uvSpecter = EditorGUILayout.ObjectField("Espectro UV:", uvSpecter, typeof(Texture2D)) as Texture2D;
		EditorGUILayout.Space();
		EditorGUILayout.Space();
		EditorGUILayout.Space();
		
		fumeHoodOnly = GUILayout.Toggle(fumeHoodOnly, "Usado apenas na capela: ");
		EditorGUILayout.Space();
		
		texture = EditorGUILayout.ObjectField("Textura:", texture, typeof(Texture2D)) as Texture2D;
		EditorGUILayout.Space();
		EditorGUILayout.Space();
		EditorGUILayout.Space();
		
		color = EditorGUILayout.ColorField ("Cor", color);
		EditorGUILayout.Space();
		
		if (GUILayout.Button ("SALVAR")) 
		{
			//ComponentsSaver.SaveProductFromEditor(name, formula, isSolid, molarMass, purity, density, ph, polarizability, uvSpecter, irSpecter, flameSpecter, conductibility, solubility, turbidity, hplc, refratometer, fumeHoodOnly, texture, color);
			this.Close();
		}
		
		EditorGUILayout.EndScrollView();
		EditorGUILayout.EndVertical();
	}
}
