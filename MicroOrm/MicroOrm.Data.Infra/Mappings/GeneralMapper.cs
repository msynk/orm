using System;
using System.Collections.Generic;
using System.Linq.Expressions;
using MicroOrm.Data.Infra.Mappings.Converters;

namespace MicroOrm.Data.Infra.Mappings
{
  /// <summary>
  /// A very simple entity mapper.
  /// </summary>
  /// Either create and register a mapping manually:
  public abstract class GeneralMapper<T> : IDbMapper<T>
  {
    protected GeneralMapper()
    {
      Mappings = new List<IMapping<T>>();
    }

    /// <summary>
    /// Gets column mappings
    /// </summary>
    public IList<IMapping<T>> Mappings { get; private set; }

    /// <summary>
    ///   Gets table name that this entity type is mapped to
    /// </summary>
    public string Table { get; protected set; }

    /// <summary>
    ///   Gets primary key column of table
    /// </summary>
    public string PrimaryKey { get; protected set; }

    /// <summary>
    /// Add a custom mapping
    /// </summary>
    /// <param name="mapping">Mapping which is used.</param>
    public void Map(IMapping<T> mapping)
    {
      if (mapping == null) throw new ArgumentNullException("mapping");

      Mappings.Add(mapping);
    }

    /// <summary>
    /// Add a column mapping
    /// </summary>
    /// <param name="property">Property to map</param>
    /// <param name="columnName">Column in the table</param>
    /// <param name="mappingConverter">Converter (converts from the column type to the property type)</param>
    public void Map(Expression<Func<T, object>> property, string columnName, IMappingConverter mappingConverter)
    {
      if (property == null) throw new ArgumentNullException("property");
      if (columnName == null) throw new ArgumentNullException("columnName");
      if (mappingConverter == null) throw new ArgumentNullException("mappingConverter");

      Mappings.Add(new GeneralMapping<T>(property, columnName, mappingConverter));
    }

    /// <summary>
    /// Add a column mapping
    /// </summary>
    /// <param name="property">Property to map</param>
    /// <param name="columnName">Column name</param>
    /// <remarks>The column type and the property type must match.</remarks>
    public void Map(Expression<Func<T, object>> property, string columnName)
    {
      if (property == null) throw new ArgumentNullException("property");
      if (columnName == null) throw new ArgumentNullException("columnName");

      Mappings.Add(new GeneralMapping<T>(property, columnName));
    }
  }
}
