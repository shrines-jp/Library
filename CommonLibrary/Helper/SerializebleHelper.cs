using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.IO;
using System.Linq;
using System.Runtime.Serialization.Formatters.Binary;
using System.Text;
using System.Xml;
using System.Xml.Serialization;

namespace CommonLibrary.Helper
{

	/// <summary>
	/// SerializebleHelper
	/// DataSet 또는 DataTable을 Serialize, Deserialize 하는 헬퍼클래스
	/// </summary>
	public static class SerializebleHelper
	{

		#region Serialize With SerializeType
		/// <summary>
		/// SerializeType
		/// </summary>
		public enum SerializeType
		{
			[Description("DataSet")]
			DataSet = 1,
			[Description("DataTable")]
			DataTable = 2
		}

		
		/// <summary>
		/// Serialize
		/// Object을 byte[]로 시리얼라이즈 한다.
		/// </summary>
		/// <param name="type">Serialize Type</param>
		/// <param name="obj"></param>
		/// <returns></returns>
		public static byte[] Serialize(SerializeType type, Object obj)
		{
			System.IO.MemoryStream stream = new System.IO.MemoryStream();
			System.Runtime.Serialization.IFormatter formatter = new System.Runtime.Serialization.Formatters.Binary.BinaryFormatter();

			switch (type)
			{
				case SerializeType.DataSet:
					DataSet ds = (DataSet)obj;
					ds.RemotingFormat = SerializationFormat.Binary;
					formatter.Serialize(stream, ds);
					break;
				case SerializeType.DataTable:
					DataTable dt = (DataTable)obj;
					dt.RemotingFormat = SerializationFormat.Binary;
					formatter.Serialize(stream, dt);
					break;
			}

			return stream.ToArray();
		}



		/// <summary>
		/// Deserialize
		/// 시리얼라이즈된 byte[]를 Object로 디시리얼라이즈한다. (Casting 필수)
		/// </summary>
		/// <param name="buffer"></param>
		/// <returns></returns>
		public static Object Deserialize(SerializeType type, byte[] buffer)
		{
			System.IO.MemoryStream stream = new System.IO.MemoryStream(buffer);
			System.Runtime.Serialization.IFormatter formatter = new BinaryFormatter();

			Object obj = null;

			switch (type)
			{
				case SerializeType.DataSet:
					obj = formatter.Deserialize(stream) as DataSet;
					break;
				case SerializeType.DataTable:
					obj = formatter.Deserialize(stream) as DataTable;
					break;
			}

			return obj;
		}

		#endregion


		#region DataTable Serialze
		/// <summary>
		/// Serialize
		/// DataTable을 byte[]로 시리얼라이즈 한다.
		/// </summary>
		/// <param name="dt"></param>
		/// <returns></returns>
		public static byte[] Serialize(DataTable dt)
		{
			System.IO.MemoryStream stream = new System.IO.MemoryStream();
			System.Runtime.Serialization.IFormatter formatter = new System.Runtime.Serialization.Formatters.Binary.BinaryFormatter();
			formatter.Serialize(stream, dt);
			return stream.ToArray();
		}


		/// <summary>
		/// Deserialize
		/// 시리얼라이즈된 byte[]를 DataTable로 디시리얼라이즈한다.
		/// </summary>
		/// <param name="buffer"></param>
		/// <returns></returns>
		public static DataTable Deserialize(byte[] buffer)
		{
			System.IO.MemoryStream stream = new System.IO.MemoryStream(buffer);
			System.Runtime.Serialization.IFormatter formatter = new BinaryFormatter();

			return formatter.Deserialize(stream) as DataTable;
		}
		#endregion


		#region DataTable Serialize With TableSchemaName
		/// <summary>
		/// LightWeightSerialize
		/// </summary>
		/// <param name="dt">DataTable</param>
		/// <param name="serializedTableData">byte[]로 Serialize</param>
		/// <param name="tableSchema">테이블 스키마</param>
		public static void LightWeightSerialize(DataTable dt, out byte[] serializedTableData, out string tableSchema)
		{
			//Get all row values as jagged object array
			object[][] tableItems = new object[dt.Rows.Count][];
			for (int rowIndex = 0; rowIndex < dt.Rows.Count; rowIndex++)
				tableItems[rowIndex] = dt.Rows[rowIndex].ItemArray;

			//binary serialize jagged object array
			BinaryFormatter serializationFormatter = new BinaryFormatter();
			MemoryStream buffer = new MemoryStream();
			serializationFormatter.Serialize(buffer, tableItems);
			serializedTableData = buffer.ToArray();


			//Get table schema
			///DataTable의 dt.TableName이 없으면 오류발생.
		
			StringBuilder tableSchemaBuilder = new StringBuilder();
			dt.WriteXmlSchema(new StringWriter(tableSchemaBuilder));
			tableSchema = tableSchemaBuilder.ToString();
		}


		/// <summary>
		/// LightWeightDeserialize
		/// </summary>
		/// <param name="serializedTableData">시리얼라이즈된 byte[]</param>
		/// <param name="tableSchema">테이블 스키마</param>
		/// <returns></returns>
		public static DataTable LightWeightDeserialize(byte[] serializedTableData, string tableSchema)
		{
			DataTable table = new DataTable();
			table.ReadXmlSchema(new StringReader(tableSchema));

			BinaryFormatter serializationFormatter = new BinaryFormatter();
			MemoryStream buffer = new MemoryStream(serializedTableData);
			object[][] itemArrayForRows = (object[][])serializationFormatter.Deserialize(buffer);

			table.MinimumCapacity = itemArrayForRows.Length;
			table.BeginLoadData();
			for (int rowIndex = 0; rowIndex < itemArrayForRows.Length; rowIndex++)
				table.Rows.Add(itemArrayForRows[rowIndex]);
			table.EndLoadData();

			return table;
		}

		#endregion


		#region String <--> ByteArray Serialize
		/// <summary>
		/// To convert a Byte Array of Unicode values (UTF-8 encoded) to a complete String.
		/// </summary>
		/// <param name="characters">Unicode Byte Array to be converted to String</param>
		/// <returns>String converted from Unicode Byte Array</returns>
		private static string UTF8ByteArrayToString(byte[] characters)
		{
			UTF8Encoding encoding = new UTF8Encoding();
			string constructedString = encoding.GetString(characters);
			return (constructedString);
		}

		/// <summary>
		/// Converts the String to UTF8 Byte array and is used in De serialization
		/// </summary>
		/// <param name="pXmlString"></param>
		/// <returns></returns>
		private static Byte[] StringToUTF8ByteArray(string pXmlString)
		{
			UTF8Encoding encoding = new UTF8Encoding();
			byte[] byteArray = encoding.GetBytes(pXmlString);
			return byteArray;
		}

		/// <summary>
		/// Serialize an object into an XML string
		/// </summary>
		/// <typeparam name="T"></typeparam>
		/// <param name="obj"></param>
		/// <returns></returns>
		public static string SerializeObject<T>(T obj)
		{
			try
			{
				string xmlString = null;
				MemoryStream memoryStream = new MemoryStream();
				XmlSerializer xs = new XmlSerializer(typeof(T));
				XmlTextWriter xmlTextWriter = new XmlTextWriter(memoryStream, Encoding.UTF8);
				xs.Serialize(xmlTextWriter, obj);
				memoryStream = (MemoryStream)xmlTextWriter.BaseStream;
				xmlString = UTF8ByteArrayToString(memoryStream.ToArray()); return xmlString;
			}
			catch
			{
				return string.Empty;
			}
		}

		/// <summary>
		/// Serialize
		/// </summary>
		/// <typeparam name="T"></typeparam>
		/// <param name="obj"></param>
		/// <returns></returns>
		public static byte[] Serialize<T>(T obj)
		{
			try
			{
				MemoryStream memoryStream = new MemoryStream();
				XmlSerializer xs = new XmlSerializer(typeof(T));
				XmlTextWriter xmlTextWriter = new XmlTextWriter(memoryStream, Encoding.UTF8);
				xs.Serialize(xmlTextWriter, obj);
				memoryStream = (MemoryStream)xmlTextWriter.BaseStream;
				return memoryStream.ToArray(); 
			}
			catch
			{
				return null;
			}
		}

		/// <summary>
		/// Reconstruct an object from an XML string
		/// </summary>
		/// <param name="xml"></param>
		/// <returns></returns>
		public static T DeserializeObject<T>(string xml)
		{
			XmlSerializer xs = new XmlSerializer(typeof(T));
			MemoryStream memoryStream = new MemoryStream(StringToUTF8ByteArray(xml));
			XmlTextWriter xmlTextWriter = new XmlTextWriter(memoryStream, Encoding.UTF8);
			return (T)xs.Deserialize(memoryStream);
		}


		/// <summary>
		/// DeserializeObject
		/// </summary>
		/// <typeparam name="T"></typeparam>
		/// <param name="serializedArray"></param>
		/// <returns></returns>
		public static T DeserializeObject<T>(byte[] serializedArray)
		{
			XmlSerializer xs = new XmlSerializer(typeof(T));
			MemoryStream memoryStream = new MemoryStream(serializedArray);
			XmlTextWriter xmlTextWriter = new XmlTextWriter(memoryStream, Encoding.UTF8);
			return (T)xs.Deserialize(memoryStream);
		}

		#endregion
	}
}
