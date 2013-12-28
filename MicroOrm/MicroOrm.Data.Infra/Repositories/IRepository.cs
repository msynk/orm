using System.Collections.Generic;
using MicroOrm.Data.Infra.Contexts;

namespace MicroOrm.Data.Infra.Repositories
{
  public interface IRepository<T>
  {
    IUnitOfWork Uow { get; }
    bool AutoCommit { get; set; }

    IList<T> GetAll();
    IList<T> GetAll(string whereCluase);
    T GetByKey(object key);
    T Get(T entity);

    void Add(T entity);
    void Edit(T entity);
    void Delete(T entity);
  }
}
