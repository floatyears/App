#if UNITY_EDITOR
using System;
using UnityEngine;

namespace UnitTesting
{
	public class Assert
	{
		// === Pass and Fail === //

		public static void Pass()
		{
		}


		public static void Fail()
		{
			throw new TestingException("Failed");
		}


		public static void Fail(string message)
		{
			if ( message == string.Empty )
			{
				message = "string.Empty";
			}

			throw new TestingException(message);
		}


		// === AreEqual === //

		public static void AreEqual(IComparable expectedResult, IComparable actualResult, string message = "")
		{
			if ( !CheckIfTwoValuesAreEqual(expectedResult, actualResult) )
			{
				ThrowException(expectedResult, actualResult, message);
			}
		}


		public static void AreEqual(string[] expectedResult, string[] actualResult)
		{
			ArraysAreIdentical(expectedResult, actualResult);
		}


		// === AreNotEqual === //

		public static void AreNotEqual(IComparable expectedResult, IComparable actualResult)
		{
			if ( CheckIfTwoValuesAreEqual(expectedResult, actualResult) )
			{
				ThrowException(expectedResult, actualResult);
			}
		}


		public static void AreNotEqual(object expectedResult, object actualResult)
		{
			if ( expectedResult == actualResult )
			{
				ThrowException(expectedResult, actualResult);
			}
		}


		// === Approx === //

		public static void Approx (float expectedResult, float actualResult, float error)
		{
			if ( actualResult > expectedResult + error || actualResult < expectedResult - error )
			{
				ThrowException(expectedResult + " +/- " + error, actualResult);
			}
		}


		private static bool CheckIfTwoValuesAreEqual(IComparable expectedResult, IComparable actualResult)
		{
			if ( expectedResult == null )
			{
				return actualResult == null;
			}

			return expectedResult.Equals(actualResult);
		}


		// === Less === //

		public static void Less (IComparable leftValue, IComparable rightValue)
		{
			if ( leftValue.CompareTo(rightValue) != -1 )
			{
				ThrowLessThanException(leftValue, rightValue);
			}
		}


		public static void ThrowLessThanException(object leftValue, object rightValue)
		{
			throw new TestingException(leftValue + " should be less than " + rightValue + " but is not.");
		}


		// === Greater === //

		public static void Greater (IComparable leftValue, IComparable rightValue)
		{
			if ( leftValue.CompareTo(rightValue) != 1 )
			{
				ThrowGreaterThanException(leftValue, rightValue);
			}
		}


		private static void ThrowGreaterThanException(object leftValue, object rightValue)
		{
			throw new TestingException(leftValue + " should be greater than " + rightValue + " but is not.");
		}


		// === AreNotSame === //

		public static void AreNotSame (object expected, object actual)
		{
			bool passed = true;

			if ( expected == null && actual == null )
			{
				passed = false;
			}
			else if ( expected != null && actual != null )
			{
				if ( object.ReferenceEquals (expected, actual) )
				{
					passed = false;
				}
			}

			if ( !passed )
			{
				ThrowException("Not Same", "Same");
			}
		}


		// === ArraysAreIdentical === //

		public static void ArraysAreIdentical<T>(T[] expectedResult, T[] actualResult)
		{
			if ( expectedResult == actualResult )
			{
				return;
			}

			if ( expectedResult == null && actualResult != null )
			{
				ThrowException("Array is null", "Array is not null");
			}

			if ( expectedResult != null && actualResult == null )
			{
				ThrowException("Array is not null", "Array is null");
			}

			if ( expectedResult.Length != actualResult.Length )
			{
				ThrowException("Array of length = " + expectedResult.Length, "Array of length = " + actualResult.Length);
			}

			for ( int i = 0; i < expectedResult.Length; i++ )
			{
				if ( expectedResult[i] == null )
				{
					if ( actualResult[i] != null )
					{
						ThrowException(null, "Index " + i + " = " + actualResult[i]);
					}
				}
				else if ( expectedResult[i].Equals(actualResult[i]) == false )
				{
					object objValue = actualResult[i];

					if ( objValue == null )
					{
						objValue = "null";
					}

					ThrowException("Index " + i + " = " + expectedResult[i], "Index " + i + " = " + objValue);
				}
			}
		}


		public static void IsEmpty (object[] result)
		{
			if ( result != null && result.Length != 0 )
			{
				ThrowException("Empty", "Not Empty");
			}
		}


		public static void IsNotEmpty (object[] result)
		{
			if ( result == null || result.Length == 0 )
			{
				ThrowException("Not Empty", "Empty");
			}
		}


		// === IsTrue and InFalse === //

		public static void IsTrue(bool result, string message = "")
		{
			if ( result == false )
			{
				ThrowException(true, result, message);
			}
		}


		public static void IsFalse(bool result)
		{
			if ( result == true )
			{
				ThrowException(false, result);
			}
		}


		// === IsNotNull and IsNull === //

		public static void IsNotNull(object result)
		{
			if ( result == null || result.Equals(null) )
			{
				ThrowException("Not null", result);
			}
		}


		public static void IsNotNullOrEmpty(string result)
		{
			if ( string.IsNullOrEmpty(result) )
			{
				ThrowException("Not null", result);
			}
		}


		public static void IsNull(object result)
		{
			if ( result != null )
			{
				ThrowException(null, result);
			}
		}


		public static void NoNullArrayElements(object[] results)
		{
			if ( results == null )
			{
				ThrowException("The array should not be null", "The array is null");
			}

			for ( int i = 0; i < results.Length; i++ )
			{
				object result = results[i];

				if ( result == null || result.Equals(null) )
				{
					ThrowException("No array elements should be null", "Array element " + i + " is null");
				}
			}
		}


		// === ThrowException === //

		public static void ThrowException(object expectedResult, object actualResult, string message = "")
		{
			string exception = "";
			string expectedValue = GetObjectStringValue(expectedResult);
			string actualValue = GetObjectStringValue(actualResult);

			if ( !string.IsNullOrEmpty(message) )
			{
				exception += message + "\n";
			}

			exception += "Expected: " + expectedValue;
			exception += " - Actual: " + actualValue;

			throw new TestingException(exception);
		}


		private static string GetObjectStringValue(object valueObject)
		{
			if ( valueObject != null )
			{
				string result = valueObject.ToString();

				if ( valueObject is string && result == string.Empty )
				{
					return "string.Empty";
				}

				return result;
			}

			return "null";
		}
	}
}
#endif