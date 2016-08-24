using UnityEngine;
using System.Collections;

public interface WorkbenchInteractive {
	public string hoverName { get; }

	public void OnClick();
}
