using System.Collections.Generic;
using MicroOrm.Data.Infra.Contexts;
using MicroOrm.Data.Infra.Repositories;

namespace MicroOrm.Business.BLO.Base
{
  public partial class BaseBLO<T> where T : class, new()
  {
    protected readonly IRepository<T> Repository;
    protected readonly IUnitOfWork Uow;

    public BaseBLO(IUnitOfWork uow)
    {
      Uow = uow;
      Repository = uow.Get<T>();
      Repository.AutoCommit = true;
    }

    /// <summary>
    ///   Returns a list of current entity from all records in db table
    /// </summary>
    /// <returns>A list of all current entity</returns>
    public IList<T> GetAll()
    {
      return Repository.GetAll();
    }

    /// <summary>
    ///   Returns a list of current entity from all records with a where clause in current table in db
    /// </summary>
    /// <param name="whereClause">Where clause</param>
    /// <returns>A list of all current entity</returns>
    public IList<T> GetAll(string whereClause)
    {
      return Repository.GetAll(whereClause);
    }

    /// <summary>
    ///   Returns an entity by a key object from db table
    /// </summary>
    /// <param name="key">A key object</param>
    /// <returns>An entity</returns>
    public T GetByKey(object key)
    {
      return Repository.GetByKey(key);
    }

    /// <summary>
    ///   Returns an entity by creating equality where clause from all not-null values in a provided entity
    /// </summary>
    /// <param name="entity">The provided entity for creating a where clause</param>
    /// <returns>An entity</returns>
    public T Get(T entity)
    {
      return Repository.Get(entity);
    }

    /// <summary>
    ///   Adds an entity to db
    /// </summary>
    /// <param name="entity">An entity</param>
    public void Add(T entity)
    {
      Repository.Add(entity);
    }

    /// <summary>
    ///   Update an entity in db
    /// </summary>
    /// <param name="entity">An entity</param>
    public void Edit(T entity)
    {
      Repository.Edit(entity);
    }

    /// <summary>
    ///   Deletes an entity from database with auto creation of where clause from not-null values in a provided entity
    /// </summary>
    /// <param name="entity">An </param>
    public void Delete(T entity)
    {
      Repository.Delete(entity);
    }

  }
}
