using System;
using System.Collections.Generic;
using System.Linq;
using System.Data;
using System.Reflection;

namespace Object.ExtensionMethods
{
    /// <summary>
    /// 扩展方法
    /// </summary>
    public static class ExtensionMethods
    {

        #region DataTable

        /// <summary>
        /// 转换为字典集合
        /// </summary>
        /// <param name="dt"></param>
        /// <returns></returns>
        public static List<Dictionary<string, object>> ToDictionaryList(this DataTable dt) {
            List<Dictionary<string, object>> table = new List<Dictionary<string, object>>();
            foreach (DataRow dr in dt.Rows)
            {
                Dictionary<string, object> row = new Dictionary<string, object>();
                foreach (DataColumn dc in dt.Columns)
                {
                    row[dc.ColumnName] = dr[dc.ColumnName];
                }
                table.Add(row);
            }
            return table;
        }

        /// <summary>
        /// DataTable 转换为字典
        /// </summary>
        /// <param name="dt"></param>
        /// <param name="majorKeyColumn"></param>
        /// <returns></returns>
        public static Dictionary<string, T> ToDictionary<T>(this DataTable dt,string majorKeyColumn) where T:class,new()
        {
            Type T_type = typeof(T);

            List<PropertyInfo> plist = new List<PropertyInfo>(T_type.GetProperties());
            List<FieldInfo> flist = new List<FieldInfo>(T_type.GetFields());

            Dictionary<string, T> table = new Dictionary<string, T>();
            foreach (DataRow dr in dt.Rows)
            {
                string majorKeyValue = Convert.ToString(dr[majorKeyColumn]);
                T row = new T();
                foreach (DataColumn dc in dt.Columns)
                {

                    //属性
                    PropertyInfo pInfo = plist.Find(p => p.Name == dc.ColumnName);
                    if (pInfo != null)
                    {
                        try
                        {
                            var pValue = dr[dc.ColumnName];
                            if (!Convert.IsDBNull(pValue))
                            {
                                object pVal = null;
                                if (pInfo.PropertyType.ToString().Contains("System.Nullable"))
                                {
                                    pVal = Convert.ChangeType(pValue, Nullable.GetUnderlyingType(pInfo.PropertyType));
                                }
                                else
                                {
                                    pVal = Convert.ChangeType(pValue, pInfo.PropertyType);
                                }
                                pInfo.SetValue(row, pVal);
                            }
                        }
                        catch (Exception ex)
                        {
                            throw new Exception("属性[" + pInfo.Name + "]转换出错," + ex.Message, ex);
                        }
                    }
                    else
                    {
                        //字段
                        FieldInfo fInfo = flist.Find(f => f.Name == dc.ColumnName);
                        if (fInfo != null)
                        {
                            try
                            {
                                var fValue = dr[dc.ColumnName];
                                if (!Convert.IsDBNull(fValue))
                                {
                                    object fVal = null;
                                    if (fInfo.FieldType.ToString().Contains("System.Nullable"))
                                    {
                                        fVal = Convert.ChangeType(fValue, Nullable.GetUnderlyingType(fInfo.FieldType));
                                    }
                                    else
                                    {
                                        fVal = Convert.ChangeType(fValue, fInfo.FieldType);
                                    }
                                    fInfo.SetValue(row, fVal);
                                }
                            }
                            catch (Exception ex)
                            {
                                throw new Exception("字段[" + fInfo.Name + "]转换出错," + ex.Message, ex);
                            }
                        }
                    }

                }
                table.Add(majorKeyValue,row);
            }
            return table;
        }

        /// <summary>
        /// DataTable 转换为字典
        /// </summary>
        /// <param name="dt"></param>
        /// <param name="majorKeyColumn"></param>
        /// <returns></returns>
        public static Dictionary<string, Dictionary<string, object>> ToDictionary(this DataTable dt, string majorKeyColumn)
        {
            Dictionary<string, Dictionary<string, object>> table = new Dictionary<string, Dictionary<string, object>>();
            foreach (DataRow dr in dt.Rows)
            {
                string majorKeyValue = Convert.ToString(dr[majorKeyColumn]);
                Dictionary<string, object> row = new Dictionary<string, object>();
                foreach (DataColumn dc in dt.Columns)
                {
                    row[dc.ColumnName] = dr[dc.ColumnName];
                }
                table.Add(majorKeyValue, row);
            }
            return table;
        }

        /// <summary>
        /// DataTable 转成 List
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="dt"></param>
        /// <returns></returns>
        public static List<T> ToList<T>(this DataTable dt)
        {
            //代码收集修改
             
            var list = new List<T>();
            Type type = typeof(T);
            var plist = new List<PropertyInfo>(type.GetProperties());
            var flist = new List<FieldInfo>(type.GetFields());
            foreach (DataRow item in dt.Rows)
            {
                T s = Activator.CreateInstance<T>();
                foreach (DataColumn dc in dt.Columns) {

                    PropertyInfo pInfo = plist.Find(p => p.Name == dc.ColumnName);
                    if (pInfo != null)
                    {
                        try
                        {
                            var pValue = item[dc.ColumnName];
                            if (!Convert.IsDBNull(pValue))
                            {
                                object pVal = null;
                                if (pInfo.PropertyType.ToString().Contains("System.Nullable"))
                                {
                                    pVal = Convert.ChangeType(pValue, Nullable.GetUnderlyingType(pInfo.PropertyType));
                                }
                                else
                                {
                                    pVal = Convert.ChangeType(pValue, pInfo.PropertyType);
                                }
                                pInfo.SetValue(s, pVal);
                            }
                        }
                        catch (Exception ex)
                        {
                            throw new Exception("属性[" + pInfo.Name + "]转换出错," + ex.Message, ex);
                        }
                    }


                    FieldInfo fInfo = flist.Find(f => f.Name == dc.ColumnName);
                    if (fInfo != null)
                    {
                        try
                        {
                            var fValue = item[dc.ColumnName];
                            if (!Convert.IsDBNull(fValue))
                            {
                                object fVal = null;
                                if (fInfo.FieldType.ToString().Contains("System.Nullable"))
                                {
                                    fVal = Convert.ChangeType(fValue, Nullable.GetUnderlyingType(fInfo.FieldType));
                                }
                                else
                                {
                                    fVal = Convert.ChangeType(fValue, fInfo.FieldType);
                                }
                                fInfo.SetValue(s, fVal);
                            }
                        }
                        catch (Exception ex)
                        {
                            throw new Exception("字段[" + fInfo.Name + "]转换出错," + ex.Message, ex);
                        }
                    }




                }
                
                list.Add(s);
            }
            return list;
        }


        /// <summary>
        /// 生成分页数据
        /// </summary>
        /// <param name="dt"></param>
        /// <param name="PageSize">每页大小</param>
        /// <param name="pageNumber">页码</param>
        /// <returns></returns>
        public static PagingListData<DataRow> ToPagingData(this DataTable dt, int PageSize, int pageNumber) {
            PagingListData<DataRow> pd = new PagingListData<DataRow>
            {
                pagenumber = pageNumber,
                pagesize = PageSize,
                total = dt.Rows.Count
            };

            if (pageNumber < 1)
            {
                pageNumber = 1;
            }
            /*
            int minIndex = PageSize * (pageNumber - 1) + 1;
            int maxIndex = PageSize * pageNumber; 
            */
            int minIndex = PageSize * (pageNumber - 1);//从零开始
            int maxIndex = PageSize * pageNumber - 1;
            int rowMaxIndex = (pd.total - 1) >= maxIndex ? maxIndex : (pd.total - 1);

            pd.rows = new List<DataRow>();
            for (int i = minIndex; i <= rowMaxIndex; i++)
            {
                pd.rows.Add(dt.Rows[i]);
            }

            return pd;
        }

        /// <summary>
        /// 查询返回DataTable
        /// </summary>
        /// <param name="dt"></param>
        /// <returns></returns>
        public static DataTable SelectToDataTable(this DataTable dt,string whereString)
        {
            if (string.IsNullOrEmpty(whereString)) {
                return dt;
            }

            DataTable tmplTable = dt.Clone();
            DataRow[] tmplRows = dt.Select(whereString);
            foreach (DataRow dr in tmplRows)
            {
                DataRow tRow = tmplTable.NewRow();
                foreach (DataColumn dc in tmplTable.Columns)
                {
                    tRow[dc.ColumnName] = dr[dc.ColumnName];
                }
                tmplTable.Rows.Add(tRow);
            }

            return tmplTable;
        }

        #endregion

        #region DataSet

        /// <summary>
        /// 转换为字典集合
        /// </summary>
        /// <param name="dt"></param>
        /// <returns></returns>
        public static List<List<Dictionary<string, object>>> ToDictionaryList(this DataSet ds)
        {
            try
            {
                List<List<Dictionary<string, object>>> tables = new List<List<Dictionary<string, object>>>();

                foreach (DataTable dt in ds.Tables)
                {
                    List<Dictionary<string, object>> table = new List<Dictionary<string, object>>();
                    foreach (DataRow dr in dt.Rows)
                    {
                        Dictionary<string, object> row = new Dictionary<string, object>();
                        foreach (DataColumn dc in dt.Columns)
                        {
                            row[dc.ColumnName] = dr[dc.ColumnName];
                        }
                        table.Add(row);
                    }
                    tables.Add(table);
                }

                return tables;
            }
            catch (Exception exp) {
                throw new Exception("DataSet 转换为 List<List<Dictionary<string, object>>> 类型异常。" + exp.Message, exp);
            }
        }
        
        #endregion

        #region List<T>

        /// <summary>
        /// 生成分页数据
        /// </summary>
        /// <param name="list"></param>
        /// <param name="PageSize">每页大小</param>
        /// <param name="pageNumber">页码</param>
        /// <returns></returns>
        public static PagingListData<T> ToPagingData<T>(this List<T> list, int PageSize, int pageNumber) where T : class, new()
        {
            if (pageNumber < 1)
            {
                pageNumber = 1;
            }
            if (PageSize < 1)
            {
                PageSize = 10;
            }

            PagingListData<T> pld = new PagingListData<T>();
            pld.pagenumber = pageNumber;
            pld.pagesize = PageSize;
            pld.total = list.Count;
            
            
            /*
            int minIndex = PageSize * (pageNumber - 1) + 1;
            int maxIndex = PageSize * pageNumber; 
            */

            int minIndex = PageSize * (pageNumber - 1);//从零开始
            int maxIndex = PageSize * pageNumber - 1;//1210
            int rowMaxIndex = (pld.total - 1) >= maxIndex ? maxIndex : (pld.total - 1);
            List<T> pageList = new List<T>();
            for (int i = minIndex; i <= rowMaxIndex; i++)
            {
                pageList.Add(list[i]);
            }
            pld.rows = pageList;
            
            return pld;
        }


        #region ToDataTable 

        //代码收集

        /// <summary>
        /// 将实体集合转换为DataTable
        /// </summary>
        /// <typeparam name="T">实体类型</typeparam>
        /// <param name="entities">实体集合</param>
        public static DataTable ToDataTable<T>(List<T> entities)
        {
            try
            {
                var result = CreateTable<T>();
                FillData(result, entities);
                return result;
            }
            catch (Exception exp) {
                throw new Exception("将实体集合转换为 DataTable 异常。", exp);
            }
        }

        /// <summary>
        /// 创建表
        /// </summary>
        private static DataTable CreateTable<T>()
        {
            var result = new DataTable();
            var type = typeof(T);
            foreach (var property in type.GetProperties(BindingFlags.Public | BindingFlags.Instance))
            {
                var propertyType = property.PropertyType;
                if ((propertyType.IsGenericType) && (propertyType.GetGenericTypeDefinition() == typeof(Nullable<>)))
                    propertyType = propertyType.GetGenericArguments()[0];
                result.Columns.Add(property.Name, propertyType);
            }
            return result;
        }

        /// <summary>
        /// 填充数据
        /// </summary>
        private static void FillData<T>(DataTable dt, IEnumerable<T> entities)
        {
            foreach (var entity in entities)
            {
                dt.Rows.Add(CreateRow(dt, entity));
            }
        }

        /// <summary>
        /// 创建行
        /// </summary>
        private static DataRow CreateRow<T>(DataTable dt, T entity)
        {
            DataRow row = dt.NewRow();
            var type = typeof(T);
            foreach (var property in type.GetProperties(BindingFlags.Public | BindingFlags.Instance))
            {
                row[property.Name] = property.GetValue(entity) ?? DBNull.Value;
            }
            return row;
        }

        #endregion

        
        #region SortBy
        
        public class ListSortByInfo {
            public string sortName = null;
            public string sortOrder = null;
        }
        /// <summary>
        /// 排序
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="list"></param>
        /// <param name="OrderBy">排序字符串。像SQL 的order by 后面的内容。如：“AA”,“AA asc”，“AA desc,BB”</param>
        /// <returns></returns>
        public static List<T> SortBy<T>(this List<T> list,string OrderBy) where T:class
        {
            if (list.Count < 2) {
                return list;
            }

            List<ListSortByInfo> OrderByInfoList = new List<ListSortByInfo>();
            T row = list[0];
            Type type = row.GetType();
            if (!string.IsNullOrEmpty(OrderBy))
            {
                string[] OrderBys = OrderBy.Trim().Split(new char[] { ',' });
                for (int ind = 0; ind < OrderBys.Length; ind++)
                {
                    string _OrderBy = OrderBys[ind].Trim();
                    string sortName = _OrderBy.Split(new char[] { ' ' })[0];
                    string sortOrder = _OrderBy.Replace(sortName + " ", "").Trim();
                    if (!string.IsNullOrEmpty(sortName) && (type.GetProperty(sortName) != null || type.GetField(sortName) != null))
                    {
                        OrderByInfoList.Add(new ListSortByInfo
                        {
                            sortName = sortName,
                            sortOrder = sortOrder
                        });
                    }
                }
            }
            if (OrderByInfoList.Count > 0)
            {
                ListSortByInfo sort01 = OrderByInfoList[0];
                var sortData = sort01.sortOrder == "desc" ? list.OrderByDescending(row2 => GetObjectValue(row2, sort01.sortName)) 
                    : list.OrderBy(row2 => GetObjectValue(row2, sort01.sortName));
                
                if (OrderByInfoList.Count > 1)
                {
                    for (int ind = 1; ind < OrderByInfoList.Count; ind++)
                    {
                        ListSortByInfo sort02 = OrderByInfoList[ind];
                        sortData = sort01.sortOrder == "desc" ? sortData.ThenByDescending(row2 => GetObjectValue(row2, sort01.sortName)) 
                            : sortData.ThenBy(row2 => GetObjectValue(row2, sort01.sortName));
                    }
                }

                return sortData.ToList();

            }
            return list;
        }

        #endregion


        #endregion

        #region Dictionary<string, T>

        /// <summary>
        /// 字典值集合转List
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="dt"></param>
        /// <returns></returns>
        public static List<T> ToValueList<T>(this Dictionary<string, T> dic) where T : class, new()
        {
            var list = new List<T>();
            if (dic.Count == 0)
            {
                return list;
            }

            foreach (KeyValuePair<string, T> kvp in dic)
            {
                list.Add(kvp.Value);
            }

            return list;
        }

        #endregion


        #region DataRow

        /// <summary>
        /// DataRow 转成 T 对象
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="dt"></param>
        /// <returns></returns>
        public static T ToObject<T>(this DataRow row) where T:class
        {
            DataColumnCollection columns = row.Table.Columns;

            Type type = typeof(T);
            T obj = Activator.CreateInstance<T>();
            foreach (DataColumn dc in columns)
            {

                PropertyInfo pInfo = type.GetProperty(dc.ColumnName);
                if (pInfo != null)
                {
                    try
                    {
                        var pValue = row[dc.ColumnName];
                        if (!Convert.IsDBNull(pValue))
                        {
                            object pVal = null;
                            if (pInfo.PropertyType.ToString().Contains("System.Nullable"))
                            {
                                pVal = Convert.ChangeType(pValue, Nullable.GetUnderlyingType(pInfo.PropertyType));
                            }
                            else
                            {
                                pVal = Convert.ChangeType(pValue, pInfo.PropertyType);
                            }
                            pInfo.SetValue(obj, pVal);
                        }
                    }
                    catch (Exception ex)
                    {
                        throw new Exception("属性[" + pInfo.Name + "]转换出错," + ex.Message, ex);
                    }
                }


                FieldInfo fInfo = type.GetField(dc.ColumnName);
                if (fInfo != null)
                {
                    try
                    {
                        var fValue = row[dc.ColumnName];
                        if (!Convert.IsDBNull(fValue))
                        {
                            object fVal = null;
                            if (fInfo.FieldType.ToString().Contains("System.Nullable"))
                            {
                                fVal = Convert.ChangeType(fValue, Nullable.GetUnderlyingType(fInfo.FieldType));
                            }
                            else
                            {
                                fVal = Convert.ChangeType(fValue, fInfo.FieldType);
                            }
                            fInfo.SetValue(obj, fVal);
                        }
                    }
                    catch (Exception ex)
                    {
                        throw new Exception("字段[" + fInfo.Name + "]转换出错," + ex.Message, ex);
                    }
                }
                
            }
            
            return obj;
        }

        #endregion

        #region DataRow[]

        /// <summary>
        /// DataRow[] 转成 List<T>
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="dt"></param>
        /// <returns></returns>
        public static List<T> ToList<T>(this DataRow[] rows)
        {
            //代码收集修改

            var list = new List<T>();
            if (rows ==null || rows.Length == 0)
            {
                return list;
            }

            DataColumnCollection columns = rows[0].Table.Columns;

            Type type = typeof(T);
            var plist = new List<PropertyInfo>(type.GetProperties());
            var flist = new List<FieldInfo>(type.GetFields());
            foreach (DataRow item in rows)
            {
                T s = Activator.CreateInstance<T>();
                foreach (DataColumn dc in columns)
                {

                    PropertyInfo pInfo = plist.Find(p => p.Name == dc.ColumnName);
                    if (pInfo != null)
                    {
                        try
                        {
                            var pValue = item[dc.ColumnName];
                            if (!Convert.IsDBNull(pValue))
                            {
                                object pVal = null;
                                if (pInfo.PropertyType.ToString().Contains("System.Nullable"))
                                {
                                    pVal = Convert.ChangeType(pValue, Nullable.GetUnderlyingType(pInfo.PropertyType));
                                }
                                else
                                {
                                    pVal = Convert.ChangeType(pValue, pInfo.PropertyType);
                                }
                                pInfo.SetValue(s, pVal);
                            }
                        }
                        catch (Exception ex)
                        {
                            throw new Exception("属性[" + pInfo.Name + "]转换出错," + ex.Message, ex);
                        }
                    }


                    FieldInfo fInfo = flist.Find(f => f.Name == dc.ColumnName);
                    if (fInfo != null)
                    {
                        try
                        {
                            var fValue = item[dc.ColumnName];
                            if (!Convert.IsDBNull(fValue))
                            {
                                object fVal = null;
                                if (fInfo.FieldType.ToString().Contains("System.Nullable"))
                                {
                                    fVal = Convert.ChangeType(fValue, Nullable.GetUnderlyingType(fInfo.FieldType));
                                }
                                else
                                {
                                    fVal = Convert.ChangeType(fValue, fInfo.FieldType);
                                }
                                fInfo.SetValue(s, fVal);
                            }
                        }
                        catch (Exception ex)
                        {
                            throw new Exception("字段[" + fInfo.Name + "]转换出错," + ex.Message, ex);
                        }
                    }




                }

                list.Add(s);
            }
            return list;
        }

        #endregion

        /// <summary>
        /// 获取类公共字段/属性的值
        /// </summary>
        /// <param name="obj"></param>
        /// <param name="key"></param>
        /// <returns></returns>
        private static object GetObjectValue<T>(T obj, string key) where T : class
        {
            Type type = obj.GetType();
            FieldInfo fieldInfo = type.GetField(key);
            if (fieldInfo != null)
            {
                return fieldInfo.GetValue(obj);
            }
            PropertyInfo propertyInfo = type.GetProperty(key);
            if (propertyInfo != null)
            {
                return propertyInfo.GetValue(obj);
            }
            return null;
        }

    }

    /// <summary>
    ///  分页数据
    /// </summary>
    [Serializable]
    public class PagingListData<T>
    {
        /// <summary>
        /// 页码
        /// </summary>
        public int pagenumber;
        /// <summary>
        /// 每页大小
        /// </summary>
        public int pagesize;
        /// <summary>
        /// 总数据条数
        /// </summary>
        public int total;
        /// <summary>
        /// 数据
        /// </summary>
        public List<T> rows;
        /// <summary>
        /// 
        /// </summary>
        public PagingListData() {
            pagenumber = 0;
            pagesize = 0;
            total = 0;
            rows = new List<T>();
        }
        /// <summary>
        /// 
        /// </summary>
        /// <param name="pagenumber">页码</param>
        /// <param name="pagesize">每页大小</param>
        /// <param name="total">总数据条数</param>
        public PagingListData(int pagenumber, int pagesize, int total = 0)
        {
            this.pagenumber = pagenumber;
            this.pagesize = pagesize;
            this.total = total;
            rows = new List<T>();
        }

    }

    
}