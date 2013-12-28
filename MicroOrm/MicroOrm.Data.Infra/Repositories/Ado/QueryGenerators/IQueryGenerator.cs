namespace MicroOrm.Data.Infra.Repositories.Ado.QueryGenerators
{
  public interface IQueryGenerator<T>
  {
    IQueryFormatter<T> Formatter { get; }
    string Select();
    string Select(T entity);
    string Insert(T entity);
    string Update(T entity, bool ignoreNull = false);
    string Delete(T entity);
  }
}
