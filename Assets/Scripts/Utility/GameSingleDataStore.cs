﻿using UnityEngine;
using System.Collections;

public class GameSingleDataStore {
	private static GameSingleDataStore instance;
	public static GameSingleDataStore Instance {
		get {
			if(instance == null) {
				instance = new GameSingleDataStore();
			}
			return instance;
		}
	}

}
