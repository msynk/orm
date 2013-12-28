using MicroOrm.Business.BLO.Base;
using MicroOrm.Business.Management;
using MicroOrm.Data.Infra.Contexts;
using MicroOrm.Models;

namespace MicroOrm.Business.BLO
{
  public partial class UserBLO : BaseBLO<User>
  {
    public static UserBLO GetInstance()
    {
      return new UserBLO(ObjectManager.GetUow());
    }


    public UserBLO(IUnitOfWork uow) : base(uow) { }
  }
}
