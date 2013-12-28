using System;
using MicroOrm.Data.Infra.Contexts.Ado;
using MicroOrm.Data.Repositories.Ado;
using MicroOrm.Models;

namespace MicroOrm.Data.Contexts.Ado
{
  public class TestAdoContext : AdoContext
  {
    public TestAdoContext(IAdoDbFactory adoDbFactory, Action<AdoContext> rolledBack = null, Action<AdoContext> committed = null)
      : base(adoDbFactory, rolledBack, committed)
    {
      RegisterRepositories();
    }

    private void RegisterRepositories()
    {
      Resgister<User, UserRepository>();
    }
  }
}
