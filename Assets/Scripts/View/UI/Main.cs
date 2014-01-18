using System.Collections.Generic;
using UnityEngine;
using ProtoBuf;

/// <summary>
/// main will always exist until the game close
/// </summary>
using System.Collections;

public class Main : MonoBehaviour 
{
	public GameObject uiRoot;

	private static Main mainScrpit;

	public static Main Instance
	{
		get
		{
			if(mainScrpit == null)
				mainScrpit = (Main)FindObjectOfType(typeof(Main));

			return mainScrpit;
		}
	}

	private GameInput gInput;
	public GameInput GInput {
		get{return gInput;}
	}

	private GameTimer gameTimer;
	public GameTimer GTTimer {
		get {return gameTimer;}
	}

	private const float screenWidth = 640;

	private static float texScale = 0f;

	public static float TexScale
	{
		get{ return texScale; }
	}

	private static byte globalDataSeed = 0;

	public static byte GlobalDataSeed
	{
		get
		{
			return globalDataSeed;
		}
	}

	private UILabel label;

	void Awake(){
		mainScrpit = this;
		globalDataSeed = (byte)Random.Range (0, 255);
		gInput = gameObject.AddComponent<GameInput> ();
		gameObject.AddComponent<GameTimer>();
		gameTimer = GameTimer.GetInstance ();
		DontDestroyOnLoad (gameObject);
		texScale = screenWidth / Screen.width;
		// init manager class
		ViewManager.Instance.Init (uiRoot);
		ModelManager.Instance.Init ();
	}

	/// <summary>
	/// start game
	/// </summary>
	void OnEnable()
	{
		ControllerManager.Instance.ChangeScene (SceneEnum.Fight);


//		for (int i = 1; i < 5; i++) {
//			Debug.LogError ((int)Time.timeSinceLevelLoad);
//			gameTimer.AddCountDown (i, CD);
//				}

	}
//
//	void CD() {
//		Debug.LogWarning ((int)Time.timeSinceLevelLoad);
//	}

}

//public class TestSort : IComparer {
//	public int a = 0;
//	public int index = -1;
//	public int Compare (object x, object y)
//	{
//		TestSort ts1 = (TestSort)x;
//		TestSort ts2 = (TestSort)y;
//		return ts1.a.CompareTo (ts2.a);
//	}
//
//			List<TestSort> aa = new List<TestSort> ();
//			for (int i = 0; i < 5; i++) {
//				TestSort ts = new TestSort();
//				ts.a = Random.Range(0,10);
//				ts.index = i;
//				aa.Add(ts);
//			}
//			for (int i = 0; i < aa.Count; i++) {
//				Debug.Log(aa[i].a + "  index : " + aa[i].index);
//			}
//	
//			DGTools.InsertSort<TestSort,IComparer> (aa, new TestSort ());
//	
//			for (int i = 0; i < aa.Count; i++) {
//				Debug.LogError(aa[i].a + "  index : " + aa[i].index);
//			}
//	
//			DGTools.InsertSort<TestSort,IComparer> (aa, new TestSort (), false);
//			for (int i = 0; i < aa.Count; i++) {
//				Debug.LogWarning(aa[i].a + "  index : " + aa[i].index);
//			}
//
//}
