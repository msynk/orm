using System;
using System.Linq;

namespace MicroOrm.Data.Infra.Repositories.Ado.QueryGenerators
{
  public static class StringExtensions
  {
    public static string FormatForQuery(this string value)
    {
      return IsNumber(value) ? value : string.Format("'{0}'", value);
    }
    public static bool IsNumber(this string value)
    {
      return !value.ToCharArray().Any(x => !Char.IsDigit(x));
    }
  }
}
