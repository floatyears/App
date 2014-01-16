//#if UNITY_EDITOR
namespace UnitTesting
{
	[System.Serializable]
	public class TestingException : System.Exception
	{
		public string rawMessage { get; private set; }

		public TestingException() : base() { }

		public TestingException(string message) : base(message)
		{
			rawMessage = message;
		}

		public TestingException(string message, System.Exception inner) : base(message, inner)
		{
			rawMessage = message;
		}
	}
}
//#endif