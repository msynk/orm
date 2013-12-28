using MicroOrm.Data.Infra.Contexts.Ado;
using MicroOrm.Data.Infra.Repositories.Ado;

namespace MicroOrm.Business.BLO.Base
{
  public partial class BaseBLO<T>
  {
    protected IAdoContext AdoContext { get { return Uow as IAdoContext; } }

    protected IAdoRepository<T> AdoRepository { get { return Repository as IAdoRepository<T>; } }
  }
}
