using UnityEngine;
using System.Collections.Generic;

public class EffectPath {
	private static EffectPath effectPath;
	public static EffectPath Instance {
		get {
			if(effectPath == null) {
				effectPath = new EffectPath();
			}
			return effectPath;
		}
	}

	private EffectPath () {
		mapEffectPath.Add ("Q_ENEMY", "Effect/Enconuterenemy");
		mapEffectPath.Add ("Q_TRAP", "Effect/Trap");
	}

	private Dictionary<string,string> mapEffectPath = new Dictionary<string, string>();

	public string GetEffectPath(string name) {
		string path = string.Empty;
		mapEffectPath.TryGetValue (name, out path);
		return path;	
	}
}
