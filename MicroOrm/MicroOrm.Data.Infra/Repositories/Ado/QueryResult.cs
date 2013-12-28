using System.Collections.Generic;
using System.Data;
using MicroOrm.Data.Infra.Mappings.Ado;

namespace MicroOrm.Data.Infra.Repositories.Ado
{
  public class QueryResult<T>
  {
    public IDbCommand Command { get; set; }
    public IAdoMapper<T> Mapper { get; set; }

    public QueryResult(IDbCommand command, IAdoMapper<T> mapper)
    {
      Command = command;
      Mapper = mapper;
      
      MapToList();
    }

    private void MapToList()
    {
      using (var reader = Command.ExecuteReader())
      {
        var items = new List<T>();
        while (reader != null && reader.Read())
        {
          items.Add(Mapper.Map(reader));
        }
        Items = items;
      }
    }

    public IList<T> Items { get; private set; }
  }
}
