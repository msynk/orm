using System.Data;

namespace MicroOrm.Data.Infra.Mappings.Ado
{
  public interface IAdoMapper<T> : IDbMapper<T>
  {
    /// <summary>
    ///   Maps data in a IDataRecord into current entity
    /// </summary>
    /// <param name="record"></param>
    /// <returns></returns>
    T Map(IDataRecord record);
  }
}
