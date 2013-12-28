using System;
using System.Collections.Generic;
using System.Data;
using MicroOrm.Data.Infra.Mappings;
using MicroOrm.Data.Infra.Repositories;
using MicroOrm.Data.Infra.Repositories.Ado;
using MicroOrm.Data.Infra.Repositories.Ado.QueryGenerators;
using MicroOrm.Data.Infra.Repositories.Ado.QueryGenerators.Oracle;
using MicroOrm.Data.Infra.Repositories.Ado.QueryGenerators.SqlServer;

namespace MicroOrm.Data.Infra.Contexts.Ado
{
  public abstract class AdoContext : IAdoContext
  {
    #region Members

    private readonly Action<AdoContext> _rolledBack;
    private readonly Action<AdoContext> _committed;

    private readonly IAdoDbFactory _adoDbFactory;
    private readonly IDbConnection _connection;
    private IDbTransaction _transaction;

    private static readonly object ContainerLock = new object();
    private static Dictionary<Type, Type> _repositoryContainer;

    #endregion

    #region AdoContext

    protected AdoContext(IAdoDbFactory adoDbFactory, Action<AdoContext> rolledBack = null, Action<AdoContext> committed = null)
    {
      _adoDbFactory = adoDbFactory;
      _rolledBack = rolledBack;
      _committed = committed;

      _connection = adoDbFactory.CreateConnection();
      _transaction = _connection.BeginTransaction();
      _repositoryContainer = new Dictionary<Type, Type>();
    }

    #endregion


    #region IDbContext implementation

    public bool AutoCommit { get; set; }

    public IDbCommand CreateCommand()
    {
      var cmd = _connection.CreateCommand();
      cmd.Transaction = _transaction;
      return cmd;
    }

    public IDbDataAdapter CreateDbDataAdapter()
    {
      return _adoDbFactory.CreateDbDataAdapter();
    }

    public QueryResult<T> ExecuteQuery<T>(string query) where T : new()
    {
      using (var command = CreateCommand())
      {
        command.CommandText = query;
        return new QueryResult<T>(command, GetReporitory<T>().Mapper);
      }
    }

    public DataSet ExecuteDataSet(string query)
    {
      using (var command = CreateCommand())
      {
        var ds = new DataSet();
        var adapter = CreateDbDataAdapter();

        command.CommandText = query;
        adapter.SelectCommand = command;
        adapter.Fill(ds);
        return ds;
      }
    }

    public int ExecuteNonQuery(string query)
    {
      using (var command = CreateCommand())
      {
        command.CommandText = query;
        var result = command.ExecuteNonQuery();
        if (AutoCommit)
        {
          ((IUnitOfWork)this).SaveChanges();
        }
        return result;
      }
    }

    public object ExecuteScalar(string query)
    {
      using (var command = CreateCommand())
      {
        command.CommandText = query;
        var result = command.ExecuteScalar();
        return result;
      }
    }

    public void Resgister<TEntity, TRepository>()
    {
      lock (ContainerLock)
      {
        var entityType = typeof(TEntity);
        if (_repositoryContainer.ContainsKey(entityType))
        {
          _repositoryContainer.Remove(entityType);
        }
        _repositoryContainer.Add(entityType, typeof(TRepository));
      }
    }

    public IAdoRepository<T> GetReporitory<T>() where T : new()
    {
      return (AdoRepository<T>)Activator.CreateInstance(_repositoryContainer[typeof(T)], this);
    }

    public IQueryGenerator<T> QueryGenerator<T>(IDbMapper<T> mapper)
    {
      if (_adoDbFactory.DatabaseType == DatabaseType.SqlServer)
        return new SqlServerQueryGenerator<T>(mapper);

      if (_adoDbFactory.DatabaseType == DatabaseType.Oracle)
        return new OracleQueryGenerator<T>(mapper);

      return new GeneralQueryGenerator<T>(mapper);
    }

    #endregion

    #region IUnitOfWork implementation

    void IDisposable.Dispose()
    {
      ((IUnitOfWork)this).Close();
    }

    void IUnitOfWork.Close()
    {
      using (_transaction)
      {
        _transaction.Rollback();
        if (_rolledBack != null) _rolledBack(this);
      }
      _transaction = _connection.BeginTransaction();
    }

    void IUnitOfWork.SaveChanges()
    {
      using (_transaction)
      {
        _transaction.Commit();
        if (_committed != null) _committed(this);
      }
      _transaction = _connection.BeginTransaction();
    }

    IRepository<T> IUnitOfWork.Get<T>()
    {
      return GetReporitory<T>();
    }

    #endregion
  }
}
