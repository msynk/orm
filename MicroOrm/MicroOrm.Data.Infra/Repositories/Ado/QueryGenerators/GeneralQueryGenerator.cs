using MicroOrm.Data.Infra.Mappings;

namespace MicroOrm.Data.Infra.Repositories.Ado.QueryGenerators
{
  public class GeneralQueryGenerator<T> : IQueryGenerator<T>
  {
    public GeneralQueryGenerator(IDbMapper<T> mapper)
    {
      Formatter = new GeneralQueryFormatter<T>(mapper);
    }

    public IQueryFormatter<T> Formatter { get; protected set; }


    public virtual string Select()
    {
      return string.Format("select * from {0}", Formatter.TableName);
    }

    public virtual string Select(T entity)
    {
      return string.Format("{0} where {1}", Select(), Formatter.GetColumnValueStatements(entity));
    }

    public virtual string Insert(T entity)
    {
      return string.Format("insert into {0} ({1}) values ({2})",
                           Formatter.TableName,
                           string.Join(", ", Formatter.AllColumns),
                           string.Join(", ", Formatter.FormatValues(entity)));
    }

    public virtual string Update(T entity, bool ignoreNull = false)
    {
      return string.Format("update {0} set {1} where {2}",
                           Formatter.TableName,
                           string.Join(",", Formatter.GetColumnValueStatements(entity, c => c != Formatter.PrimaryKey, v => ignoreNull && v != null)),
                           string.Join(",", Formatter.GetColumnValueStatements(entity)));
    }

    public virtual string Delete(T entity)
    {
      return string.Format("delete from {0} where {1}",
                           Formatter.TableName,
                           string.Join(",", Formatter.GetColumnValueStatements(entity)));
    }
  }
}
