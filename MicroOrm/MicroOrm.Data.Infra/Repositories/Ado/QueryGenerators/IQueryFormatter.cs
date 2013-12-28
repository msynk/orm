using System;
using System.Collections.Generic;
using System.Reflection;
using MicroOrm.Data.Infra.Mappings;

namespace MicroOrm.Data.Infra.Repositories.Ado.QueryGenerators
{
  public interface IQueryFormatter<T>
  {
    IDbMapper<T> Mapper { get; }
    string TableName { get; }
    string PrimaryKey { get; }

    IEnumerable<PropertyInfo> AllProperties { get; }
    IEnumerable<string> AllColumns { get; }

    IEnumerable<string> FormatValues(T entity,
                                     Func<string, bool> columnPredicate = null,
                                     Func<string, bool> valuePredicate = null,
                                     Func<IMapping<T>, bool> mappingPredicate = null);

    IEnumerable<string> GetColumnValueStatements(T entity,
                                                 Func<string, bool> columnPredicate = null,
                                                 Func<string, bool> valuePredicate = null,
                                                 Func<IMapping<T>, bool> mappingPredicate = null);

    IEnumerable<KeyValuePair<string, string>> ColumnValues(T entity,
                                                           Func<string, bool> columnPredicate = null,
                                                           Func<string, bool> valuePredicate = null,
                                                           Func<IMapping<T>, bool> mappingPredicate = null);

    IEnumerable<KeyValuePair<string, string>> ColumnFormattedValues(T entity,
                                                                    Func<string, bool> columnPredicate = null,
                                                                    Func<string, bool> valuePredicate = null,
                                                                    Func<IMapping<T>, bool> mappingPredicate = null);
  }
}
