using MicroOrm.Data.Infra;
using MicroOrm.Data.Infra.Contexts;

namespace MicroOrm.Business.Management
{
  public static class ObjectManager
  {
    static DatabaseType _databaseType = DatabaseType.SqlServer;
    public static void Initialize()
    {
      var oracleConnectionString = "";
      var sqlServerConnectionString = "";
      AdoContextContainer.Initialize(oracleConnectionString, sqlServerConnectionString);
    }

    public static IUnitOfWork GetUow()
    {
      return _databaseType == DatabaseType.SqlServer
               ? AdoContextContainer.GetSqlServer()
               : AdoContextContainer.GetOracle();
    }
  }
}
