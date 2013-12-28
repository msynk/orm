using System.Data;
using MicroOrm.Data.Infra.Mappings;
using MicroOrm.Data.Infra.Repositories.Ado;
using MicroOrm.Data.Infra.Repositories.Ado.QueryGenerators;

namespace MicroOrm.Data.Infra.Contexts.Ado
{
  public interface IAdoContext : IUnitOfWork
  {
    bool AutoCommit { get; set; }
    IDbCommand CreateCommand();
    IDbDataAdapter CreateDbDataAdapter();

    QueryResult<T> ExecuteQuery<T>(string query) where T : new();
    DataSet ExecuteDataSet(string query);
    int ExecuteNonQuery(string query);
    object ExecuteScalar(string query);
    
    void Resgister<TEntity, TRepository>();
    IAdoRepository<T> GetReporitory<T>() where T : new();

    IQueryGenerator<T> QueryGenerator<T>(IDbMapper<T> mapper);

  }
}