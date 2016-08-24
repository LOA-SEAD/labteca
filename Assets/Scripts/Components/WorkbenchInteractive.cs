using UnityEngine;
using System.Collections;

public interface WorkbenchInteractive {
	string hoverName { get; }

	void OnClick();
}
