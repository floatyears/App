using System.Collections.Generic;

/// <summary>
/// origin data model. package protobuf data
/// </summary>
public interface IOriginModel {	
	/// <summary>
	/// write data to store
	/// </summary>
	/// <returns><c>true</c>, if data was set, <c>false</c> otherwise.</returns>
	/// <param name="instance">Instance.</param>
	ErrorMsg SerializeData(object instance);

	/// <summary>
	/// Gets the data instance.
	/// </summary>
	/// <returns>The data.</returns>
	object DeserializeData();
}

/// <summary>
/// model save data interface
/// </summary>
public interface IModelSave {
	/// <summary>
	/// write data to disk.
	/// </summary>
	/// <returns><c>true</c>, if data was writed, <c>false</c> otherwise.</returns>
	bool WriteData();

	/// <summary>
	/// Reads the data from disk.
	/// </summary>
	/// <returns><c>true</c>, if data was  read, <c>false</c> otherwise.</returns>
	object ReadData();
}

/// <summary>
/// net interface
/// </summary>
public interface INet {
	void Send();
	void Receive();
}

public interface INormalSkill {
	bool CalculateCard (List<uint> count);
}