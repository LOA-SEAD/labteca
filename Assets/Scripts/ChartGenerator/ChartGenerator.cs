using UnityEngine;
using System.Collections;

//! Generates a chart.
/*!
 * Contains four methods that generates a chart according to data entered ("Plot the equation").
 * The methods generates charts with pixels, circles and dots connected.
 * Chart of SPCTROPHOTOMETER_UV, SPCTROPHOTOMETER_IR, SPCTROPHOTOMETER_FLAME, HPLC. (GenerateWithTextureConectingDots)
 * 
 */
public class ChartGenerator : MonoBehaviour 
{

	public Texture2D testChart; /*!< Texture handling. */
	public float concentration; /*!<  */
	public float sensibility; /*!<  */
	public int LineSize; /*!<  */
	public int radius; /*!<  */

	public Renderer sampleRender; /*!< Makes an object appear on the screen. */

	//! Generates a chart with pixel only. 
	/*! */
	public static Texture2D GenerateWithTexturePixelOnly(Texture2D originalChart, float factor, float threshould, int lineSize)
	{
		int width = originalChart.width;
		int height = originalChart.height;

		Texture2D chart = new Texture2D (width, height);
		int[] chartArray = new int[width];

		float realThreshould = 1f - threshould;
		int lineThickness =  Mathf.RoundToInt((float)(lineSize) / 2f);
		
		for (int x = 0; x < width; x++) 
		{
			bool findValue = false;
			for (int y = 0; y < height; y++) 
			{
				if(!findValue)
				{
					float graphValue = (originalChart.GetPixel(x,y).r + originalChart.GetPixel(x,y).g + originalChart.GetPixel(x,y).b) / 3f;
					if(graphValue < realThreshould)
					{
						findValue = true;
						chartArray[x] = y;
					}
				}

				//clean chart Texture
				chart.SetPixel(x,y,Color.white);
			}
		}

		for (int i = 0; i < width; i++) 
		{
			int value = Mathf.RoundToInt(chartArray[i] * factor) - Mathf.RoundToInt(factor * height);
			chart.SetPixel(i, value, Color.black);

			for (int j = 0; j < lineThickness; j++) 
			{
				chart.SetPixel(i, value + j, Color.black);
				chart.SetPixel(i, value - j, Color.black);
			}
		}

		chart.Apply ();

		return chart;
	}

	//! Generates a chart with circles
	/*! */
	public static Texture2D GenerateWithTextureCircle(Texture2D originalChart, float factor, float threshould, int radius)
	{
		int width = originalChart.width;
		int height = originalChart.height;
		
		Texture2D chart = new Texture2D (width, height);
		int[] chartArray = new int[width];
		
		float realThreshould = 1f - threshould;
		
		for (int x = 0; x < width; x++) 
		{
			bool findValue = false;
			for (int y = 0; y < height; y++) 
			{
				if(!findValue)
				{
					float graphValue = (originalChart.GetPixel(x,y).r + originalChart.GetPixel(x,y).g + originalChart.GetPixel(x,y).b) / 3f;
					if(graphValue < realThreshould)
					{
						findValue = true;
						chartArray[x] = y;
					}
				}
				
				
				//clean chart Texture
				chart.SetPixel(x,y,Color.white);
			}
		}
		
		for (int i = 0; i < width; i++) 
		{
			int value = Mathf.RoundToInt(chartArray[i] * factor) - Mathf.RoundToInt(factor * height);
			chart.SetPixel(i, value, Color.black);

			float rad = 0;
			float radPass = 360f/(10f*radius);
			while (rad < 2.1f * Mathf.PI) // a little more than 2 pi just to be sure that last pixel was printe=0f;
			{
				for (int r = 0; r < radius; r++) 
				{
					chart.SetPixel(Mathf.RoundToInt(r*Mathf.Cos(rad)) + i, Mathf.RoundToInt(r*Mathf.Sin(rad)) + value, Color.black);
				}

				rad += Mathf.Deg2Rad*radPass;
			}
		}
		
		chart.Apply ();
		
		return chart;
	}

	//! Generates texture corrected.
	/*! */
	public static Texture2D GenerateWithTextureCorrected(Texture2D originalChart, float factor, float threshould)
	{
		int width = originalChart.width;
		int height = originalChart.height;
		
		Texture2D chart = new Texture2D (width, height);
		int[] chartArray = new int[width];
		int[] chartArrayDensity = new int[width];
		
		float realThreshould = 1f - threshould;
		
		for (int x = 0; x < width; x++) 
		{
			for (int y = 0; y < height; y++) 
			{
				float graphValue = (originalChart.GetPixel(x,y).r + originalChart.GetPixel(x,y).g + originalChart.GetPixel(x,y).b) / 3f;
				if(graphValue < realThreshould)
				{
					chartArray[x] = y;
					chartArrayDensity[x]++;
				}
				
				
				//clean chart Texture
				chart.SetPixel(x,y,Color.white);
			}
		}
		
		for (int i = 0; i < width; i++) 
		{
			int value = Mathf.RoundToInt(chartArray[i] * factor) - Mathf.RoundToInt(factor * height);
			int collunSize = Mathf.RoundToInt(chartArrayDensity[i] * factor);
			chart.SetPixel(i, value, Color.black);

			for (int c = 0; c < collunSize; c++) 
			{
				chart.SetPixel(i, value - c, Color.black);
			}
		}
		
		chart.Apply ();
		
		return chart;
	}

	//! Generates a chart with connecting dots.
	/*! */
	public static Texture2D GenerateWithTextureConectingDots(Texture2D originalChart, float factor, float threshould)
	{
		int width = originalChart.width;
		int height = originalChart.height;
		
		Texture2D chart = new Texture2D (width, height);
		int[] chartArray = new int[width];
		int[] chartArrayDensity = new int[width];
		
		float realThreshould = 1f - threshould;
		
		for (int x = 0; x < width; x++) 
		{
			for (int y = 0; y < height; y++) 
			{
				float graphValue = (originalChart.GetPixel(x,y).r + originalChart.GetPixel(x,y).g + originalChart.GetPixel(x,y).b) / 3f;
				if(graphValue < realThreshould)
				{
					chartArray[x] = y;
					chartArrayDensity[x]++;
				}
				
				
				//clean chart Texture
				chart.SetPixel(x,y,Color.white);
			}
		}
		
		for (int i = 0; i < width; i++) 
		{
			int value = Mathf.RoundToInt(chartArray[i] * factor) - Mathf.RoundToInt(factor * height);
			int valueLastInteraction = 0;
			int distance = 0;

			if(i != 0)
			{
				valueLastInteraction = Mathf.RoundToInt(chartArray[i - 1] * factor) - Mathf.RoundToInt(factor * height);
				distance = Mathf.RoundToInt(new Vector2( 1f ,  value - valueLastInteraction).magnitude);
			}
			 
			chart.SetPixel(i, value, Color.black);

			if(i != 0)
			{
				for (int d = 0; d < distance; d++) 
				{
					Vector2 line = Vector2.Lerp(new Vector2(i - 1,valueLastInteraction), new Vector2(i, value), (float)(d)/(float)(distance));
					chart.SetPixel(Mathf.RoundToInt(line.x), Mathf.RoundToInt(line.y), Color.black);
				}
			}
		}
		
		chart.Apply ();
		
		return chart;
	}
}
