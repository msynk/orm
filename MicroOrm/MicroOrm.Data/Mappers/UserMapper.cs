using MicroOrm.Data.Infra.Mappings.Ado;
using MicroOrm.Models;

namespace MicroOrm.Data.Mappers
{
  public class UserMapper : AdoMapper<User>
  {
    public UserMapper()
    {
      Table = "USER";
      PrimaryKey = "ID";

      Map(t => t.Id, "ID");
      Map(t => t.FirstName, "FIRST_NAME");
      Map(t => t.LastName, "LAST_NAME");
      Map(t => t.Username, "USERNAME");
      Map(t => t.Password, "PASSWORD");
    }
  }
}
