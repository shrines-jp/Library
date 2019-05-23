using System;
using System.IO;
using System.Text;
using System.Security.Cryptography;

namespace CommonLibrary.Security
{
    public class HashingService
    {
        #region enum, constants and fields
        /// <summary>
        /// 가능한 해슁 멤버
        /// </summary>
        public enum HashingTypes
        {
            SHA, SHA256, SHA384, SHA512, MD5
        }
        #endregion

        #region static members
        /// <summary>
        /// 입력 텍스트를 해슁 알고리즘으로 암호화한다(해슁 알고리즘 MD5)
        /// </summary>
        /// <param name="inputText">암호화할 텍스트</param>
        /// <returns>암호화된 텍스트</returns>
        public static string Hash(String inputText)
        {
            return ComputeHash(inputText, HashingTypes.MD5);
        }

        /// <summary>
        /// 입력 텍스트를 사용자가 지정한 해슁 알고리즘으로 암호화한다.
        /// </summary>
        /// <param name="inputText">암호화할 텍스트</param>
        /// <param name="hashingType">해슁 알고리즘</param>
        ///<returns>암호화된 텍스트</returns>
        public static string Hash(String inputText, HashingTypes hashingType)
        {
            return ComputeHash(inputText, hashingType);
        }

        /// <summary>
        /// 입력 텍스트와 해쉬된 텍스트가 같은지 여부를 비교한다.
        /// </summary>
        /// <param name="inputText">해쉬되지 않은 입력 텍스트</param>
        /// <param name="hashText">해쉬된 텍스트</param>
        /// <returns>비교 결과</returns>
        public static bool isHashEqual(string inputText, string hashText)
        {
            return (Hash(inputText) == hashText);
        }

        /// <summary>
        /// 사용자가 지정한 해쉬 알고리즘으로 입력 텍스트와 해쉬된 텍스트가 같은지 여부를 비교한다.
        /// </summary>
        /// <param name="inputText">해쉬되지 않은 입력 텍스트</param>
        /// <param name="hashText">해쉬된 텍스트</param>
        /// <param name="hashingType">사용자가 지정한 해슁 타입</param>
        /// <returns>비교 결과</returns>
        public static bool isHashEqual(string inputText, string hashText, HashingTypes hashingType)
        {
            return (Hash(inputText, hashingType) == hashText);
        }
        #endregion

        #region Hashing Engine
        /// <summary>
        /// 해쉬 코드를 계산해서 스트링으로 변환함
        /// </summary>
        /// <param name="inputText">해쉬코드로 변환할 스트링</param>
        /// <param name="hashingType">사용할 해슁 타입</param>
        /// <returns>hashed string</returns>
        private static string ComputeHash(string inputText, HashingTypes hashingType)
        {
            HashAlgorithm HA = getHashAlgorithm(hashingType);

            //declare a new encoder
            UTF8Encoding UTF8Encoder = new UTF8Encoding();
            //get byte representation of input text
            byte[] inputBytes = UTF8Encoder.GetBytes(inputText);

            //hash the input byte array
            byte[] output = HA.ComputeHash(inputBytes);

            //convert output byte array to a string
            return Convert.ToBase64String(output);
        }

        /// <summary>
        /// 특정한 해슁 알고리즘을 리턴함

        /// </summary>
        /// <param name="hashingType">사용할 해슁 알고리즘</param>
        /// <returns>HashAlgorithm</returns>
        private static HashAlgorithm getHashAlgorithm(HashingTypes hashingType)
        {
            switch (hashingType)
            {
                case HashingTypes.MD5:
                    return new MD5CryptoServiceProvider();

                case HashingTypes.SHA:
                    return new SHA1CryptoServiceProvider();

                case HashingTypes.SHA256:
                    return new SHA256Managed();

                case HashingTypes.SHA384:
                    return new SHA384Managed();

                case HashingTypes.SHA512:
                    return new SHA512Managed();

                default:
                    return new MD5CryptoServiceProvider();
            }
        }
        #endregion
    }
}
