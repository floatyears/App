#if UNITY_EDITOR
using UnityEngine;
using UnityEditor;

using System;
using System.IO;
using System.Reflection;
using System.Collections;
using System.Collections.Generic;


namespace UnitTesting
{
	public class TestRunner
	{
		const string scriptAssembliesFolder = "/../Library/ScriptAssemblies/";

		public int currentTest { get; private set; }
		public int testCount
		{
			get
			{
				if ( testMethods != null )
				{
					return testMethods.Count;
				}

				return 0;
			}
		}

		public int passedCount { get; private set; }
		public int failedCount { get; private set; }

		Dictionary<MethodInfo, object> testMethods = new Dictionary<MethodInfo, object>();
		Dictionary<Type, MethodInfo> setupMethods = new Dictionary<Type, MethodInfo>();
		Dictionary<Type, MethodInfo> teardownMethods = new Dictionary<Type, MethodInfo>();

		public List<TestResult> failedResults = new List<TestResult>();


		public TestRunner()
		{
			IterateThroughAssemblies();
		}


		public IEnumerator Run()
		{
			string currentScene = EditorApplication.currentScene;

			if ( !EditorApplication.SaveCurrentSceneIfUserWantsTo() )
				yield break;

			EditorApplication.NewScene();

			foreach ( Transform tfm in GameObject.FindObjectsOfType(typeof(Transform)) )
			{
				GameObject.DestroyImmediate(tfm.gameObject);
			}

			currentTest = 0;
			passedCount = 0;
			failedCount = 0;

			foreach ( MethodInfo method in testMethods.Keys )
			{
				object testObject = testMethods[method];

				currentTest++;

				try
				{
					if ( setupMethods.ContainsKey(method.DeclaringType) )
					{
						setupMethods[method.DeclaringType].Invoke(testObject,  new object[] {});
					}

					method.Invoke(testObject, new object[] {});
					passedCount++;
				}
				catch ( Exception e )
				{
					TestingException testException = e.InnerException as TestingException;

					failedCount++;

					if ( testException != null )
					{
						failedResults.Add(new TestResult(method.DeclaringType, method, testException.rawMessage, testException));
						Debug.LogError(method.DeclaringType.ToString() + "." + method.Name + "\n" + testException.rawMessage);
					}
					else
					{
						failedResults.Add(new TestResult(method.DeclaringType, method, e.Message, e));
						LogExceptions(e);
					}
				}
				finally
				{
					if ( teardownMethods.ContainsKey(method.DeclaringType) )
					{
						teardownMethods[method.DeclaringType].Invoke(testObject,  new object[] {});
					}
				}

				yield return null;
			}

			EditorApplication.OpenScene(currentScene);
		}


		void IterateThroughAssemblies()
		{
			string assembliesFolder = Application.dataPath + scriptAssembliesFolder;
			Assembly assembly;

			foreach ( string file in Directory.GetFiles(assembliesFolder) )
			{
				if ( file.EndsWith(".dll") )
				{
					assembly = Assembly.LoadFile(file);
					IterateThroughTypes(assembly.GetTypes());
				}
			}
		}


		void LogExceptions(Exception e)
		{
			Debug.LogError(GetExceptionString(e, ""));
		}


		string GetExceptionString(Exception e, string baseMessage)
		{
			baseMessage += e.Message + "\n";

			if ( e.InnerException != null )
			{
				return GetExceptionString(e.InnerException, baseMessage);
			}

			return baseMessage += e.StackTrace;
		}


		void IterateThroughTypes(Type[] types)
		{
			passedCount = 0;
			failedCount = 0;

			foreach ( Type type in types )
			{
				MethodInfo[] methods = type.GetMethods();

				foreach ( MethodInfo method in methods )
				{
					Test testAttr = Attribute.GetCustomAttribute(method, typeof(Test), false) as Test;
					SetUp setupAttr = Attribute.GetCustomAttribute(method, typeof(SetUp), false) as SetUp;
					TearDown teardownAttr = Attribute.GetCustomAttribute(method, typeof(TearDown), false) as TearDown;

					if ( testAttr != null )
					{
						try
						{
							ConstructorInfo[] constructors = type.GetConstructors();
							object curTestObject = null;

							if ( constructors.Length > 0 )
							{
								curTestObject = constructors[0].Invoke(null);
							}

							if ( curTestObject != null )
							{
								testMethods[method] = curTestObject;
							}
						}
						catch ( Exception e )
						{
							Debug.LogError("*** Error in testing harness ***");
							Debug.LogError(e.Message);
						}
					}
					else if ( setupAttr != null )
					{
						setupMethods.Add(type, method);
					}
					else if ( teardownAttr != null )
					{
						teardownMethods.Add(type, method);
					}
				}
			}
		}
	}


	public class TestResult
	{
		public Type classType { get; private set; }
		public MethodInfo testMethod { get; private set; }
		public string message { get; private set; }
		public Exception exception;


		public TestResult(Type classType, MethodInfo testMethod, string message, Exception exception)
		{
			this.classType = classType;
			this.testMethod = testMethod;
			this.message = message;
			this.exception = exception;
		}
	}
}
#endif