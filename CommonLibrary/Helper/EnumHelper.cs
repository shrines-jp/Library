using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Reflection;
using System.Text;

namespace CommonLibrary.Helper
{
	/// <summary>
	/// EnumHelper
	/// 솔루션개발팀 박정환
	/// </summary>
	public static class EnumHelper
	{
		/// <summary>
		/// GetDescriptionFromEnumValue
		/// Enum 타입의 [Description]을 검색한다.
		/// [Description("Bright Pink")]
		/// BrightPink = 2,
		/// </summary>
		/// <param name="value">Enum Value</param>
		/// <returns>Enum Description</returns>
		public static String GetDescriptionFromEnumValue(Enum value)
		{
			DescriptionAttribute attribute = value.GetType()
				.GetField(value.ToString())
				.GetCustomAttributes(typeof(DescriptionAttribute), false)
				.SingleOrDefault() as DescriptionAttribute;
			return attribute == null ? value.ToString() : attribute.Description;
		}


		/// <summary>
		/// GetEnumValueFromDescription
		/// [Description("Bright Pink")]
		/// </summary>
		/// <typeparam name="T">EnumType</typeparam>
		/// <param name="description">Description</param>
		/// <returns></returns>
		public static T GetEnumValueFromDescription<T>(string description)
		{
			var type = typeof(T);
			if (!type.IsEnum)
				throw new ArgumentException();
			FieldInfo[] fields = type.GetFields();
			var field = fields.SelectMany(f => f.GetCustomAttributes(typeof(DescriptionAttribute), false), (f, a) => new { Field = f, Att = a })
							.Where(a => ((DescriptionAttribute)a.Att).Description == description).SingleOrDefault();
			return field == null ? default(T) : (T)field.Field.GetRawConstantValue();
		}


		/// <summary>
		/// GetEnumValueFromEnumString
		/// BrightPink = 2,
		/// </summary>
		/// <typeparam name="T">EnumType</typeparam>
		/// <param name="enumString">EnumString</param>
		/// <returns></returns>
		public static byte? GetEnumValueFromEnumString<T>(string enumString)
		{
			try
			{
				return Convert.ToByte(Enum.Parse(typeof(T), EnumHelper.GetEnumValueFromDescription<T>(enumString).ToString()));
			}
			catch
			{
				return null;
			}
		}


		/// <summary>
		/// EnumToList
		/// Enum을 리스트로 변경해준다.
		/// </summary>
		/// <typeparam name="T"></typeparam>
		/// <returns></returns>
		public static List<T> EnumToList<T>()
		{
			Type enumType = typeof(T);

			// Can't use type constraints on value types, so have to do check like this
			if (enumType.BaseType != typeof(Enum))
				throw new ArgumentException("T must be of type System.Enum");

			Array enumValArray = Enum.GetValues(enumType);

			List<T> enumValList = new List<T>(enumValArray.Length);

			foreach (int val in enumValArray)
			{
				enumValList.Add((T)Enum.Parse(enumType, val.ToString()));
			}

			return enumValList;
		}


		/// <summary>
		/// EnumToListWithoutValueTypes
		/// Enum을 리스트로 변경해준다.
		/// 더 간단함
		/// </summary>
		/// <typeparam name="T"></typeparam>
		/// <returns></returns>
		public static List<T> EnumToListWithoutValueTypes<T>()
		{

			Type enumType = typeof(T);

			// Can't use type constraints on value types, so have to do check like this

			if (enumType.BaseType != typeof(Enum))

				throw new ArgumentException("T must be of type System.Enum");

			return new List<T>(Enum.GetValues(enumType) as IEnumerable<T>);

		}


	}
}
