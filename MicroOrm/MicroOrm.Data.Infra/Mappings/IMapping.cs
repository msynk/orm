using MicroOrm.Data.Infra.Mappings.Converters;

namespace MicroOrm.Data.Infra.Mappings
{
  public interface IMapping<T>
  {
    /// <summary>
    /// Gets column name
    /// </summary>
    string ColumnName { get; }

    /// <summary>
    /// Gets property name
    /// </summary>
    string PropertyName { get; }

    /// <summary>
    ///   Gets the converter instance of mapping
    /// </summary>
    IMappingConverter MappingConverter { get; }

    object Get(T entity);
    T Set(T entity, object value);
  }
}
