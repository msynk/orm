using System.Collections.Generic;
using System.Linq;
using MicroOrm.Data.Infra.Contexts;
using MicroOrm.Data.Infra.Contexts.Ado;
using MicroOrm.Data.Infra.Mappings.Ado;
using MicroOrm.Data.Infra.Repositories.Ado.QueryGenerators;

namespace MicroOrm.Data.Infra.Repositories.Ado
{
  public abstract class AdoRepository<T> : IAdoRepository<T> where T : new()
  {
    #region AdoRepository

    private readonly IQueryGenerator<T> _queryGenerator;

    protected AdoRepository(IAdoContext context, IAdoMapper<T> mapper)
    {
      Context = context;
      Mapper = mapper;

      Uow = context;
      _queryGenerator = context.QueryGenerator(mapper);
    }

    private int ExecuteQuery(string query)
    {
      using (var command = Context.CreateCommand())
      {
        command.CommandText = query;
        var result = command.ExecuteNonQuery();
        if (AutoCommit)
        {
          Context.SaveChanges();
        }
        return result;
      }
    }

    #endregion

    #region IAdoRepository<T> implementation

    public IAdoContext Context { get; private set; }

    public IAdoMapper<T> Mapper { get; private set; }

    #endregion

    #region IRepository<T> implementation

    public IUnitOfWork Uow { get; private set; }
    public bool AutoCommit { get; set; }

    public IList<T> GetAll()
    {
      return Get(_queryGenerator.Select());
    }

    public IList<T> GetAll(string whereCluase)
    {
      return
        Get(string.Format("{0} where {1}", _queryGenerator.Select(),
                          whereCluase.Replace("where", "").Replace("WHERE", "")));
    }

    public T GetByKey(object key)
    {
      var entity = new T();
      Mapper.Mappings.Single(m => m.ColumnName == Mapper.PrimaryKey).Set(entity, key);
      return Get(entity);
    }

    public T Get(T entity)
    {
      return Get(_queryGenerator.Select(entity)).SingleOrDefault();
    }

    private IList<T> Get(string query)
    {
      using (var command = Context.CreateCommand())
      {
        command.CommandText = query;
        return new QueryResult<T>(command, Mapper).Items;
      }
    }


    public virtual void Add(T entity)
    {
      ExecuteQuery(_queryGenerator.Insert(entity));
    }

    public virtual void Edit(T entity)
    {
      ExecuteQuery(_queryGenerator.Update(entity));
    }

    public virtual void Delete(T entity)
    {
      ExecuteQuery(_queryGenerator.Delete(entity));
    }

    #endregion
  }
}
