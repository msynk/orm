using System.Collections.Generic;

namespace MicroOrm.Data.Infra.Mappings
{
  public interface IDbMapper<T>
  {
    /// <summary>
    ///   Gets table name that this entity type is mapped to
    /// </summary>
    string Table { get; }

    /// <summary>
    ///   Gets primary key column of table
    /// </summary>
    string PrimaryKey { get; }

    /// <summary>
    /// Add a custom mapping
    /// </summary>
    /// <param name="mapping">Mapping which is used.</param>
    void Map(IMapping<T> mapping);

    /// <summary>
    /// Gets column mappings
    /// </summary>
    IList<IMapping<T>> Mappings { get; }

  }
}
