#if UNITY_EDITOR
using System;
using UnityEngine;

namespace UnitTesting
{
	[AttributeUsage(AttributeTargets.Class)]
	public class TestFixture : System.Attribute
	{
		public TestFixture()
		{
		}
	}


	[AttributeUsage(AttributeTargets.Method)]
	public class Test : System.Attribute
	{
		public Test()
		{
		}
	}


	[AttributeUsage(AttributeTargets.Method)]
	public class SetUp : System.Attribute
	{
		public SetUp()
		{
		}
	}


	[AttributeUsage(AttributeTargets.Method)]
	public class TearDown : System.Attribute
	{
		public TearDown()
		{
		}
	}
}
#endif