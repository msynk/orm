using System;
using System.Data;
using System.Data.Common;

namespace MicroOrm.Data.Infra.Contexts.Ado
{
  public class GeneralAdoDbFactory : IAdoDbFactory
  {
    private readonly DbProviderFactory _provider;

    public GeneralAdoDbFactory(DatabaseType databaseType, string connectionString)
    {
      DatabaseType = databaseType;
      ConnectionString = connectionString;

      _provider = databaseType == DatabaseType.SqlServer
                    ? DbProviderFactories.GetFactory(DbProviders.SqlServer)
                    : DbProviderFactories.GetFactory(DbProviders.Oracle);
    }

    #region IDbFactory implementation

    public DatabaseType DatabaseType { get; private set; }

    public string ConnectionString { get; private set; }

    public IDbConnection CreateConnection()
    {
      var connection = _provider.CreateConnection();
      if (connection == null)
        throw new Exception(string.Format("Failed to create a connection for the {0} DatabaseType", DatabaseType));

      connection.ConnectionString = ConnectionString;
      connection.Open();
      return connection;
    }

    public IDbDataAdapter CreateDbDataAdapter()
    {
      var adapter = _provider.CreateDataAdapter();
      return adapter;
    }

    #endregion
  }
}
