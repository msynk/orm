using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using MicroOrm.Data.Infra.Mappings;

namespace MicroOrm.Data.Infra.Repositories.Ado.QueryGenerators
{
  public class GeneralQueryFormatter<T> : IQueryFormatter<T>
  {
    private string _tableName;
    private string _primaryKey;
    private IEnumerable<string> _allColumns;
    private IEnumerable<PropertyInfo> _allProperties;

    public GeneralQueryFormatter(IDbMapper<T> mapper)
    {
      Mapper = mapper;
    }

    public IDbMapper<T> Mapper { get; private set; }



    public virtual string TableName { get { return _tableName ?? (_tableName = ExtractTableName()); } }

    public virtual string PrimaryKey { get { return _primaryKey ?? (_primaryKey = ExtractPrimaryKey()); } }

    public virtual IEnumerable<PropertyInfo> AllProperties { get { return _allProperties ?? (_allProperties = ExtractAllProperties()); } }

    public virtual IEnumerable<string> AllColumns { get { return _allColumns ?? (_allColumns = ExtractAllColumns()); } }

    public virtual IEnumerable<string> FormatValues(T entity,
                                            Func<string, bool> columnPredicate = null,
                                            Func<string, bool> valuePredicate = null,
                                            Func<IMapping<T>, bool> mappingPredicate = null)
    {
      return ColumnFormattedValues(entity, columnPredicate, valuePredicate, mappingPredicate).Select(cv => cv.Value);
    }

    public virtual IEnumerable<string> GetColumnValueStatements(T entity,
                                                                Func<string, bool> columnPredicate = null,
                                                                Func<string, bool> valuePredicate = null,
                                                                Func<IMapping<T>, bool> mappingPredicate = null)
    {

      return
        ColumnFormattedValues(entity, columnPredicate, valuePredicate, mappingPredicate)
          .Select(cv => string.Format("{0} = {1}", cv.Key, cv.Value));
    }

    public virtual IEnumerable<KeyValuePair<string, string>> ColumnValues(T entity,
                                                                             Func<string, bool> columnPredicate = null,
                                                                             Func<string, bool> valuePredicate = null,
                                                                             Func<IMapping<T>, bool> mappingPredicate = null)
    {
      var columns = mappingPredicate != null
                      ? ExtractAllColumns(Mapper.Mappings.Where(mappingPredicate).ToList())
                      : AllColumns;
      if (columnPredicate != null)
      {
        columns = columns.Where(columnPredicate);
      }
      var columnValues = new List<KeyValuePair<string, string>>();
      foreach (var column in columns)
      {
        string value;
        var mapping = Mapper.Mappings.SingleOrDefault(m => m.ColumnName == column);
        if (mapping == null)
        {
          var property = GetPropertyFromColumn(column);
          var obj = property.GetValue(entity);
          value = obj == null ? null : obj.ToString();
        }
        else
        {
          var obj = mapping.Get(entity);
          value = obj == null ? null : obj.ToString();
        }
        if (valuePredicate != null && !valuePredicate(value)) continue;
        columnValues.Add(new KeyValuePair<string, string>(column, value));
      }
      return columnValues;
    }

    public virtual IEnumerable<KeyValuePair<string, string>> ColumnFormattedValues(T entity,
                                                                             Func<string, bool> columnPredicate = null,
                                                                             Func<string, bool> valuePredicate = null,
                                                                             Func<IMapping<T>, bool> mappingPredicate = null)
    {
      var columnFormattedValues = new List<KeyValuePair<string, string>>();
      var columnValues = ColumnValues(entity, columnPredicate, valuePredicate, mappingPredicate).ToList();
      foreach (var columnValue in columnValues)
      {
        var column = columnValue.Key;
        var value = columnValue.Value;
        var property = GetPropertyFromColumn(column);
        columnFormattedValues.Add(new KeyValuePair<string, string>(column, Format(property, value)));
      }
      return columnFormattedValues;
    }




    private string ExtractTableName()
    {
      var tableName = Mapper.Table;
      if (string.IsNullOrEmpty(tableName))
      {
        tableName = typeof(T).Name;
      }
      return tableName;
    }

    private string ExtractPrimaryKey()
    {
      var primaryKey = Mapper.PrimaryKey;
      if (!string.IsNullOrEmpty(primaryKey)) return primaryKey;

      var name = TableName;
      var ids = new[] { "Id", "ID", "id" };
      var keys = new List<string>();
      foreach (var id in ids)
      {
        keys.Add(id);
        keys.Add(name + id);
        keys.Add(name + "_" + id);
        keys.Add("tbl_" + id);
        keys.Add("table_" + id);
      }
      primaryKey = keys.FirstOrDefault(PropertyExist);
      //throw new InvalidOperationException("No PrimaryKey found.");
      return primaryKey ?? string.Empty;
    }

    private bool PropertyExist(string name)
    {
      var type = typeof(T);
      return type.GetProperty(name, BindingFlags.Public | BindingFlags.Instance) != null;
    }

    private IEnumerable<string> ExtractAllColumns(IList<IMapping<T>> mappings = null)
    {
      if (mappings == null) mappings = Mapper.Mappings;
      var columns = new List<string>();
      foreach (var property in AllProperties)
      {
        var column = property.Name;
        var mapping = mappings.SingleOrDefault(m => m.PropertyName == column);
        if (mapping != null) column = mapping.ColumnName;
        columns.Add(column);
      }
      //_allColumns = columns;
      return columns;
    }

    private IEnumerable<PropertyInfo> ExtractAllProperties()
    {
      return typeof(T).GetProperties(BindingFlags.Public | BindingFlags.Instance);
    }

    private PropertyInfo GetPropertyFromColumn(string column)
    {
      var mapping = Mapper.Mappings.SingleOrDefault(m => m.ColumnName == column);
      return mapping == null
               ? AllProperties.SingleOrDefault(p => p.Name == column)
               : AllProperties.SingleOrDefault(p => p.Name == mapping.PropertyName);
    }



    protected virtual string Format(PropertyInfo property, string value)
    {
      var formattedValue = value;

      if (property.PropertyType == typeof(string))
      {
        formattedValue = string.Format("'{0}'", value);
      }

      return formattedValue;
    }
  }
}