using System.Reflection;
using MicroOrm.Data.Infra.Mappings;

namespace MicroOrm.Data.Infra.Repositories.Ado.QueryGenerators.SqlServer
{
  public class SqlServerQueryFormatter<T> : GeneralQueryFormatter<T>
  {
    public SqlServerQueryFormatter(IDbMapper<T> mapper)
      : base(mapper)
    {
    }

    protected override string Format(PropertyInfo property, string value)
    {
      var formattedValue = value;

      if (property.PropertyType == typeof(string))
      {
        formattedValue = value == null ? "null" : string.Format("'{0}'", value);
      }

      return formattedValue;
    }
  }
}