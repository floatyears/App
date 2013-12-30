using UnityEngine;
using System.Collections.Generic;

public interface INetwork  {
	void SendRequest (byte[] data = null);
}
