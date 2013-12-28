﻿using System;

namespace MicroOrm.Data.Infra.Mappings.Converters
{
  /// <summary>
  /// Will change <c>DBNull.Value</c> to the specified default value (or <c>null</c> if none is specified)
  /// </summary>
  public class DbNullMappingConverter : IMappingConverter
  {
    private readonly object _defaultValue;

    public DbNullMappingConverter()
      : this(null)
    {
    }

    public DbNullMappingConverter(object defaultValue)
    {
      _defaultValue = defaultValue;
    }

    /// <summary>
    /// Convert from db value to property value
    /// </summary>
    /// <param name="dbColumnValue">Value in the db column</param>
    /// <returns>Value which can be assigned to the property</returns>
    public object ConvertFromDb(object dbColumnValue)
    {
      return dbColumnValue == DBNull.Value ? _defaultValue : dbColumnValue;
    }

    /// <summary>
    /// Convert to db value from property value
    /// </summary>
    /// <param name="propertyValue">Value of the property</param>
    /// <returns>Value which can be stored in the db column</returns>
    public object ConvertToDb(object propertyValue)
    {
      return propertyValue ?? DBNull.Value;
    }
  }
}
