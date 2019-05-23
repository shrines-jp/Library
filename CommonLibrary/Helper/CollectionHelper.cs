using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Reflection;

namespace CommonLibrary.Helper
{
	/// <summary>
	/// CollectionHelper
	/// 박정환
	/// </summary>
	public class CollectionHelper
	{
		private CollectionHelper()
		{
		}


		#region DataTable 관련 라이브러리
		/// <summary>
		/// IList를 DataTable로 변경해준다.
		/// </summary>
		/// <typeparam name="T"></typeparam>
		/// <param name="list"></param>
		/// <returns></returns>
		public static DataTable ConvertTo<T>(IList<T> list)
		{
			DataTable table = CreateTable<T>();
			Type entityType = typeof(T);
			PropertyDescriptorCollection properties = TypeDescriptor.GetProperties(entityType);

			foreach (T item in list)
			{
				DataRow row = table.NewRow();

				foreach (PropertyDescriptor prop in properties)
				{
					row[prop.Name] = prop.GetValue(item);
				}

				table.Rows.Add(row);
			}

			return table;
		}


		/// <summary>
		/// IList<DataRow>형식을 IList<T>형식으로 리턴
		/// </summary>
		/// <typeparam name="T"></typeparam>
		/// <param name="rows"></param>
		/// <returns></returns>
		public static IList<T> ConvertTo<T>(IList<DataRow> rows)
		{
			IList<T> list = null;

			if (rows != null)
			{
				list = new List<T>();

				foreach (DataRow row in rows)
				{
					T item = CreateItem<T>(row);
					list.Add(item);
				}
			}

			return list;
		}


		/// <summary>
		/// DataTable을 IList형식으로 리턴
		/// </summary>
		/// <typeparam name="T"></typeparam>
		/// <param name="table"></param>
		/// <returns></returns>
		public static IList<T> ConvertTo<T>(DataTable table)
		{
			if (table == null)
			{
				return null;
			}

			List<DataRow> rows = new List<DataRow>();

			foreach (DataRow row in table.Rows)
			{
				rows.Add(row);
			}

			return ConvertTo<T>(rows);
		}


		/// <summary>
		/// CreateItem
		/// DataRow를 T형식으로 리턴
		/// </summary>
		/// <typeparam name="T"></typeparam>
		/// <param name="row"></param>
		/// <returns></returns>
		public static T CreateItem<T>(DataRow row)
		{
			T obj = default(T);
			if (row != null)
			{
				obj = Activator.CreateInstance<T>();

				foreach (DataColumn column in row.Table.Columns)
				{
					PropertyInfo prop = obj.GetType().GetProperty(column.ColumnName);
					try
					{
						object value = row[column.ColumnName];
						prop.SetValue(obj, value, null);
					}
					catch
					{
						// You can log something here
						throw;
					}
				}
			}

			return obj;
		}


		/// <summary>
		/// CreateTable
		/// T형식을 데이터 테이블 형식으로 변환
		/// </summary>
		/// <typeparam name="T"></typeparam>
		/// <returns></returns>
		public static DataTable CreateTable<T>()
		{
			Type entityType = typeof(T);
			DataTable table = new DataTable(entityType.Name);
			PropertyDescriptorCollection properties = TypeDescriptor.GetProperties(entityType);

			foreach (PropertyDescriptor prop in properties)
			{
				table.Columns.Add(prop.Name, prop.PropertyType);
			}

			return table;
		}


		/// <summary>
		/// ToDataTable
		/// IEnumerable 개체를 받아서, DataTable로 리턴
		/// </summary>
		/// <typeparam name="T"></typeparam>
		/// <param name="collection"></param>
		/// <returns></returns>
		public static DataTable ToDataTable<T>(IEnumerable<T> collection)
		{
			DataTable dt = new DataTable("DataTable");
			Type t = typeof(T);
			PropertyInfo[] pia = t.GetProperties();

			//Inspect the properties and create the columns in the DataTable 
			foreach (PropertyInfo pi in pia)
			{
				Type ColumnType = pi.PropertyType;
				if ((ColumnType.IsGenericType))
				{
					ColumnType = ColumnType.GetGenericArguments()[0];
				}
				dt.Columns.Add(pi.Name, ColumnType);
			}

			//Populate the data table 
			foreach (T item in collection)
			{
				DataRow dr = dt.NewRow();
				dr.BeginEdit();
				foreach (PropertyInfo pi in pia)
				{
					if (pi.GetValue(item, null) != null)
					{
						dr[pi.Name] = pi.GetValue(item, null);
					}
				}
				dr.EndEdit();
				dt.Rows.Add(dr);
			}
			return dt;
		}


		/// <summary>
		/// ConvertToDataTable
		/// List형식을 데이터 테이블 형식으로 변환
		/// </summary>
		/// <typeparam name="T"></typeparam>
		/// <param name="data"></param>
		/// <returns>DataTable</returns>
		public static DataTable ConvertToDataTable<T>(IList<T> data)
		{
			PropertyDescriptorCollection properties = TypeDescriptor.GetProperties(typeof(T));
			DataTable table = new DataTable();
			foreach (PropertyDescriptor prop in properties)
				table.Columns.Add(prop.Name, Nullable.GetUnderlyingType(prop.PropertyType) ?? prop.PropertyType);
			foreach (T item in data)
			{
				DataRow row = table.NewRow();
				foreach (PropertyDescriptor prop in properties)
					row[prop.Name] = prop.GetValue(item) ?? DBNull.Value;
				table.Rows.Add(row);
			}
			return table;

		}


		/// <summary>
		/// DataTable의 원하는 필드를 뽑아서, DataTable로 다시 리턴
		/// ReturnColumn
		/// </summary>
		/// <param name="dataTable"></param>
		/// <param name="fieldNames"></param>
		/// <returns></returns>
		public static DataTable ReturnColumn(DataTable dataTable, ICollection<String> fieldNames)
		{
			if (fieldNames.Count > 0)
			{
				for (int i = 0; i < dataTable.Columns.Count; i++)
				{
					DataColumn col = dataTable.Columns[i];
					if (!fieldNames.Contains(col.ColumnName))
					{
						dataTable.Columns.Remove(col);
						i--;
					}
				}
			}

			return dataTable;
		}

		#endregion
	}
}
