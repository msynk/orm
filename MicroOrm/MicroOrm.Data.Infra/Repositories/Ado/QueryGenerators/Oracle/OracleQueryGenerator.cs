using MicroOrm.Data.Infra.Mappings;

namespace MicroOrm.Data.Infra.Repositories.Ado.QueryGenerators.Oracle
{
  public class OracleQueryGenerator<T> : GeneralQueryGenerator<T>
  {
    public OracleQueryGenerator(IDbMapper<T> mapper)
      : base(mapper)
    {
      Formatter = new OracleQueryFormatter<T>(mapper);
    }
  }
}
