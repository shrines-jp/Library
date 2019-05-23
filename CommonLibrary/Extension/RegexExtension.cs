using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Text.RegularExpressions;

namespace CommonLibrary.Extension
{
	public class RegexExtension
	{
		/// <summary>
		/// 휴대폰번호 정규식 검사
		/// </summary>
		/// <param name="strIn">휴대폰번호</param>
		/// <returns></returns>
		public static bool IsValidMobile(String mobileNumber)
		{
			return Regex.IsMatch(mobileNumber, @"01[016789]\-?\d{2,4}\-?\d{3,4}");
			//디비에 넣을때는 [휴대폰번호].Replace("-","");
		}


		/// <summary>
		/// 이메일 정규식 체크
		/// </summary>
		/// <param name="emailAddress"></param>
		/// <returns></returns>
		public static bool IsValidEmail(String emailAddress)
		{
			///이메일 정규식 체크
			return Regex.IsMatch(emailAddress, @"^([a-zA-Z0-9]+)([\._-]?[a-zA-Z0-9]+)*@([a-zA-Z0-9]+)([\._-]?[a-zA-Z0-9]+)*([\.]{1}[a-zA-Z0-9]{2,})+$");
		}
	}
}
