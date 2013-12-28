using MicroOrm.Data.Infra.Contexts.Ado;
using MicroOrm.Data.Infra.Repositories.Ado;
using MicroOrm.Data.Mappers;
using MicroOrm.Models;

namespace MicroOrm.Data.Repositories.Ado
{
  public class UserRepository : AdoRepository<User>
  {
    public UserRepository(IAdoContext context)
      : base(context, new UserMapper())
    {

    }
  }
}
