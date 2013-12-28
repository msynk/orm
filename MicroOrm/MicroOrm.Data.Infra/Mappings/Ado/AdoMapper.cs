using System;
using System.Data;

namespace MicroOrm.Data.Infra.Mappings.Ado
{
  /// <summary>
  /// A very simple entity mapper.
  /// </summary>
  /// Either create and register a mapping manually:
  public abstract class AdoMapper<T> : GeneralMapper<T>, IAdoMapper<T>
  {
    /// <summary>
    /// Map a record to a new entity
    /// </summary>
    /// <param name="record">Row from the query result</param>
    /// <returns>Created and populated entity.</returns>
    public T Map(IDataRecord record)
    {
      if (record == null) throw new ArgumentNullException("record");

      var entity = (T)Activator.CreateInstance(typeof(T), true);
      foreach (var mapping in Mappings)
      {
        var value = record[mapping.ColumnName];
        mapping.Set(entity, value);
      }
      return entity;
    }
  }
}
