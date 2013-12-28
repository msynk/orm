using MicroOrm.Data.Infra.Contexts.Ado;
using MicroOrm.Data.Infra.Mappings.Ado;

namespace MicroOrm.Data.Infra.Repositories.Ado
{
  public interface IAdoRepository<T> : IRepository<T>
  {
    IAdoContext Context { get; }
    IAdoMapper<T> Mapper { get; }
  }
}
