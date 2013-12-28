using MicroOrm.Data.Infra.Mappings;

namespace MicroOrm.Data.Infra.Repositories.Ado.QueryGenerators.SqlServer
{
  public class SqlServerQueryGenerator<T> : GeneralQueryGenerator<T>
  {
    public SqlServerQueryGenerator(IDbMapper<T> mapper)
      : base(mapper)
    {
      Formatter = new SqlServerQueryFormatter<T>(mapper);
    }
  }
}
