namespace MicroOrm.Data.Infra.Mappings.Converters
{
  /// <summary>
  /// Used to convert from a table column type to a property type
  /// </summary>
  public interface IMappingConverter
  {
    /// <summary>
    /// Convert from db value to property value
    /// </summary>
    /// <param name="dbColumnValue">Value in the db column</param>
    /// <returns>Value which can be assigned to the property</returns>
    object ConvertFromDb(object dbColumnValue);

    /// <summary>
    /// Convert to db value from property value
    /// </summary>
    /// <param name="propertyValue">Value of the property</param>
    /// <returns>Value which can be stored in the db column</returns>
    object ConvertToDb(object propertyValue);
  }
}