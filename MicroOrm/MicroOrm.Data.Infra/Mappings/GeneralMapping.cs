using System;
using System.Linq.Expressions;
using System.Reflection;
using MicroOrm.Data.Infra.Mappings.Converters;

namespace MicroOrm.Data.Infra.Mappings
{
  /// <summary>
  /// Mapping for a specific column
  /// </summary>
  /// <typeparam name="T">POCO</typeparam>
  public class GeneralMapping<T> : IMapping<T>
  {
    private readonly PropertyInfo _propertyInfo;

    /// <summary>
    /// Initializes a new instance of the <see cref="GeneralMapping{T}" /> class.
    /// </summary>
    /// <param name="property">The property name that the mapping is for.</param>
    /// <param name="columnName">Name of the column in the table.</param>
    /// <param name="mappingConverter">Used of the column value is not of the same type as the property.</param>
    public GeneralMapping(string property, string columnName, IMappingConverter mappingConverter = null)
      : this(typeof(T).GetProperty(property), columnName, mappingConverter)
    {
    }

    /// <summary>
    /// Initializes a new instance of the <see cref="GeneralMapping{T}" /> class.
    /// </summary>
    /// <param name="property">The property that the mapping is for.</param>
    /// <param name="columnName">Name of the column in the table.</param>
    /// <param name="mappingConverter">Used of the column value is not of the same type as the property.</param>
    public GeneralMapping(Expression<Func<T, object>> property, string columnName, IMappingConverter mappingConverter = null)
      : this(property != null ? (PropertyInfo)property.GetMemberInfo().Member : null, columnName, mappingConverter)
    {
    }

    /// <summary>
    /// Initializes a new instance of the <see cref="GeneralMapping{T}" /> class.
    /// </summary>
    /// <param name="propertyInfo">The property that the mapping is for.</param>
    /// <param name="columnName">Name of the column in the table.</param>
    /// <param name="mappingConverter">Used of the column value is not of the same type as the property.</param>
    public GeneralMapping(PropertyInfo propertyInfo, string columnName, IMappingConverter mappingConverter = null)
    {
      if (propertyInfo == null) throw new ArgumentNullException("propertyInfo");
      if (columnName == null) throw new ArgumentNullException("columnName");

      _propertyInfo = propertyInfo;
      PropertyName = _propertyInfo.Name;
      ColumnName = columnName;
      if (mappingConverter == null)
      {
        mappingConverter = new DefaultMappingConverter();
      }
      MappingConverter = mappingConverter;
    }



    /// <summary>
    /// Gets column name
    /// </summary>
    public string ColumnName { get; private set; }

    /// <summary>
    /// Gets property name
    /// </summary>
    public string PropertyName { get; private set; }

    /// <summary>
    ///   Gets the converter instance of mapping
    /// </summary>
    public IMappingConverter MappingConverter { get; private set; }

    /// <summary>
    ///   Gets the value of current mapping from an entity instance after converting to db value
    /// </summary>
    /// <param name="entity">An entity instance</param>
    /// <returns>The converted value of current mapping to db value</returns>
    public object Get(T entity)
    {
      return MappingConverter.ConvertToDb(_propertyInfo.GetValue(entity));
    }

    /// <summary>
    ///   Sets the value of current mapping into an entity instance after converting from db value
    /// </summary>
    /// <param name="entity">An entity instance</param>
    /// <param name="value">The value of current mapping from db</param>
    public T Set(T entity, object value)
    {
      _propertyInfo.SetValue(entity, MappingConverter.ConvertFromDb(value));
      return entity;
    }
  }
}