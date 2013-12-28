using MicroOrm.Data.Infra.Mappings;

namespace MicroOrm.Data.Infra.Repositories.Ado.QueryGenerators.Oracle
{
  public class OracleQueryFormatter<T> : GeneralQueryFormatter<T>
  {
    public OracleQueryFormatter(IDbMapper<T> mapper)
      : base(mapper)
    {
    }

  }
}