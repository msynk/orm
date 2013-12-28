using System;

namespace MicroOrm.Data.Infra.Mappings.Converters
{
  /// <summary>
  /// Uses <see cref="System.Convert.ChangeType(object, Type)"/> for the conversion.
  /// </summary>
  /// <typeparam name="T">Property type</typeparam>
  /// <remarks>Uses <c>Activator.CreateInstance</c> to create default values if the db value is null. Not very effecient, but the
  /// best I could come up with. (primitives is the problem)</remarks>
  public class DotNetMappingConverter<T> : IMappingConverter
  {
    /// <summary>
    /// Convert from db value to property value
    /// </summary>
    /// <param name="dbColumnValue">Value in the db column</param>
    /// <returns>Value which can be assigned to the property</returns>
    public object ConvertFromDb(object dbColumnValue)
    {
      if (dbColumnValue == null || dbColumnValue == DBNull.Value) return default(T);
      return System.Convert.ChangeType(dbColumnValue, typeof(T));
    }

    public object ConvertToDb(object propertyValue)
    {
      if (propertyValue == null) return DBNull.Value;
      return propertyValue;
      //return System.Convert.ChangeType(propertyValue, typeof(T));
    }
  }

}