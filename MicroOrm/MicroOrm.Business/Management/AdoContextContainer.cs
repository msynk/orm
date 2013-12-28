using System;
using System.Collections.Generic;
using MicroOrm.Data.Contexts.Ado;
using MicroOrm.Data.Infra;
using MicroOrm.Data.Infra.Contexts.Ado;

namespace MicroOrm.Business.Management
{
  public static class AdoContextContainer
  {
    private static bool _initialized;
    private static string _oracleConnectionString;
    private static string _sqlServerConnectionString;

    private static readonly object SqlServerLock = new object();
    private static readonly object OracleLock = new object();
    private readonly static List<TestAdoContext> SqlServerContexts = new List<TestAdoContext>();
    private readonly static List<TestAdoContext> OracleContexts = new List<TestAdoContext>();

    private static int _size = 10;
    public static int Size
    {
      get { return _size; }
      set
      {
        if (value > 10) value = 10;
        _size = value;
        Reinitialize();
      }
    }

    public static void Initialize(string oracleConnectionString, string sqlServerConnectionString)
    {
      _oracleConnectionString = oracleConnectionString;
      _sqlServerConnectionString = sqlServerConnectionString;

      if (SqlServerContexts.Count > 0)
        SqlServerContexts.Clear();
      for (int i = 0; i < _size; i++)
      {
        var factory = new GeneralAdoDbFactory(DatabaseType.SqlServer, sqlServerConnectionString);
        var context = new TestAdoContext(factory, ReleaseSqlServerContext, ReleaseSqlServerContext);
        SqlServerContexts.Add(context);
      }

      if (OracleContexts.Count > 0)
        OracleContexts.Clear();
      for (int i = 0; i < _size; i++)
      {
        var factory = new GeneralAdoDbFactory(DatabaseType.Oracle, oracleConnectionString);
        var context = new TestAdoContext(factory, ReleaseOracleContext, ReleaseOracleContext);
        OracleContexts.Add(context);
      }
      _initialized = true;
    }
    public static void Reinitialize()
    {
      CheckInitialize();
      Initialize(_oracleConnectionString, _sqlServerConnectionString);
    }
    private static void CheckInitialize()
    {
      if(!_initialized)
        throw new InvalidOperationException("The container is not initialized yet.");
    }
    
    public static IAdoContext GetSqlServer()
    {
      CheckInitialize();
      lock (SqlServerLock)
      {
        if (SqlServerContexts.Count == 0)
          throw new InvalidOperationException("There is no active context");
        var context = SqlServerContexts[0];
        SqlServerContexts.RemoveAt(0);
        return context;
      }
    }

    public static IAdoContext GetOracle()
    {
      CheckInitialize();
      lock (OracleLock)
      {
        if (OracleContexts.Count == 0)
          throw new InvalidOperationException("There is no active context");
        var context = OracleContexts[0];
        OracleContexts.RemoveAt(0);
        return context;
      }
    }


    private static void ReleaseSqlServerContext(AdoContext context)
    {
      lock (SqlServerLock)
      {
        SqlServerContexts.Add(context as TestAdoContext);
      }
    }
    private static void ReleaseOracleContext(AdoContext context)
    {
      lock (OracleLock)
      {
        OracleContexts.Add(context as TestAdoContext);
      }
    }
  }
}
