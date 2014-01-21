#if UNITY_EDITOR
using UnityEditor;
using UnityEngine;
using UnitTesting;

using System.Collections;


public class UnitTestingWindow : EditorWindow
{
	private bool runTests = false;
	private bool runningTests = false;

	IEnumerator testEnumerator;

	TestRunner testRunner = null;


	[MenuItem ("Window/Unit Testing")]
	private static void Init()
	{
		UnitTestingWindow window = UnitTestingWindow.GetWindow<UnitTestingWindow>();

		window.title = "Unit Testing";
	}


	void OnGUI()
	{
		Color oldColor = GUI.color;

		if ( !runTests && !runningTests && !EditorApplication.isCompiling )
		{
			if ( GUILayout.Button("Run", GUILayout.MaxWidth(100)) )
			{
				runTests = true;
			}
		}
		else
		{
			GUILayout.Label(testRunner.currentTest + "/" + testRunner.testCount);
		}

		if ( testRunner != null )
		{
			GUILayout.Label("Passed: " + testRunner.passedCount);

			if ( testRunner.failedCount > 0 )
			{
				GUI.color = new Color(1.0f, 0.4f, 0.4f);
				GUILayout.Label("Failed: " + testRunner.failedCount);

				GUILayout.Space(10);

				foreach ( TestResult result in testRunner.failedResults )
				{
					GUILayout.Label(result.classType.ToString() + "." + result.testMethod.Name);
					GUILayout.Label(result.message);
					GUILayout.Space(10);
				}

				GUI.color = oldColor;
			}
		}
	}


	void Update()
	{
		if ( runTests )
		{
			testRunner = new TestRunner();
			testEnumerator = testRunner.Run();
			runTests = false;
			runningTests = true;
		}
		else if ( runningTests )
		{
			if ( !testEnumerator.MoveNext() )
			{
				runningTests = false;
			}

			this.Repaint();
		}
	}
}
#endif