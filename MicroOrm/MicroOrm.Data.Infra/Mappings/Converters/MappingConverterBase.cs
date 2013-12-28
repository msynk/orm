namespace MicroOrm.Data.Infra.Mappings.Converters
{
  /// <summary>
  /// Used to convert the conversion to typed.
  /// </summary>
  /// <typeparam name="TDbValue">Type of value in the db column.</typeparam>
  /// <typeparam name="TEntityValue">Property type</typeparam>
  public abstract class MappingConverterBase<TDbValue, TEntityValue> : IMappingConverter
  {
    object IMappingConverter.ConvertFromDb(object dbColumnValue)
    {
      return ConvertFromInternal((TDbValue)dbColumnValue);
    }

    public object ConvertToDb(object propertyValue)
    {
      return ConvertToInternal((TEntityValue)propertyValue);
    }

    /// <summary>
    /// Convert db value
    /// </summary>
    /// <param name="dbValue">Value in the db</param>
    /// <returns>Value which can be assigned to the property.</returns>
    public abstract TEntityValue ConvertFromInternal(TDbValue dbValue);

    /// <summary>
    /// Convert entity value
    /// </summary>
    /// <param name="entityValue">Value in the entity</param>
    /// <returns>Value which can be stored in the db.</returns>
    public abstract TDbValue ConvertToInternal(TEntityValue entityValue);
  }
}