
/// <summary>
/// origin data model. package protobuf data
/// </summary>
public interface IOriginModel
{	
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