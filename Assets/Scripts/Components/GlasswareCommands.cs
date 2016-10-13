using UnityEngine;
using UnityEngine.UI;
using System.Collections;

public class GlasswareCommands
{
	public GlasswareCommandsEnum commandType;
	public float volume;
	public Compound compound;

	public GlasswareCommands(GlasswareCommandsEnum type, float v, Compound c){
		this.commandType = type;
		this.volume = v;
		this.compound = c;
	}
}

public enum GlasswareCommandsEnum{
	Add,
	RemoveLiquid,
	RemoveSolid
}

