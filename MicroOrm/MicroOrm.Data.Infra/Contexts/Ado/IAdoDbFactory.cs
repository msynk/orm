using System.Data;

namespace MicroOrm.Data.Infra.Contexts.Ado
{
  public interface IAdoDbFactory
  {
    DatabaseType DatabaseType { get; }
    string ConnectionString { get; }

    IDbConnection CreateConnection();
    IDbDataAdapter CreateDbDataAdapter();
  }
}