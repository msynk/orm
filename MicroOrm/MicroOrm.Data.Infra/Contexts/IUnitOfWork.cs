using System;
using MicroOrm.Data.Infra.Repositories;

namespace MicroOrm.Data.Infra.Contexts
{
  public interface IUnitOfWork : IDisposable
  {
    void Close();
    IRepository<T> Get<T>() where T : new();
    void SaveChanges();
  }
}