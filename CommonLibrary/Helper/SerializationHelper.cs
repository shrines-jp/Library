using System;
using System.IO;
using System.Runtime.Serialization.Formatters.Binary;

///System.Runtime.Serialization.Formatters.Soap.dll assembly.
using System.Runtime.Serialization.Formatters.Soap;

namespace CommonLibrary.Helper
{
    /// <summary>
    /// Serialize할 유형입니다.
    /// </summary>
    public enum SerializeType
    {
        /// <summary>
        /// BinaryFormatter
        /// </summary>
        BinaryFormatter,
        /// <summary>
        /// SoapFormatter
        /// </summary>
        SoapFormatter
    }

    /// <summary>
    /// Serialize와 Deserialize를 편리하게 이용하는 Static Class
    /// </summary>
    public static class SerializationHelper
    {
        /// <summary>
        /// 직렬화합니다.
        /// </summary>
        /// <param name="serializeType">Serialize 유형</param>
        /// <param name="filePath">Write할 파일경로</param>
        /// <param name="graph">그래프의 루트에 있는 serialize할 개체입니다.</param>
        public static void Serialize(SerializeType serializeType, string filePath, object graph)
        {
            try
            {
                switch (serializeType)
                {
                    case SerializeType.BinaryFormatter: BinaryFormatterSerialize(filePath, graph); break;
                    case SerializeType.SoapFormatter: SoapFormatterSerialize(filePath, graph); break;
                }
            }
            catch (Exception e)
            {
                throw e;
            }
        }

        /// <summary>
        /// 역직렬화합니다.
        /// </summary>
        /// <param name="serializeType">Serialize 유형</param>
        /// <param name="filePath">Read할 파일경로</param>
        /// <param name="graph">그래프의 루트에 있는 Deserialize할 개체입니다.</param>
        public static object Deserialize(SerializeType serializeType, string filePath, object graph)
        {
            try
            {
                switch (serializeType)
                {
                    case SerializeType.BinaryFormatter: return BinaryFormatterDeserialize(filePath, graph);
                    case SerializeType.SoapFormatter: return SoapFormatterDeserialize(filePath, graph);
                }
                throw new NotSupportedException("지원하지 않는 형식입니다.");
            }
            catch (Exception e)
            {
                throw e;
            }
        }
        /// <summary>
        /// BinaryFormatter를 이용해 Serialize합니다.
        /// </summary>
        /// <param name="filePath"></param>
        /// <param name="graph"></param>
        private static object BinaryFormatterDeserialize(string filePath, object graph)
        {
            FileStream fs = null;
            try
            {
                fs = new FileStream(filePath, FileMode.Open, FileAccess.Read);
                BinaryFormatter bf = new BinaryFormatter();
                object obj = bf.Deserialize(fs);
                fs.Close();
                return obj;
            }
            catch (Exception e)
            {
                throw e;
            }
            finally
            {

                if (fs != null)
                    fs.Close();
            }
        }


        /// <summary>
        /// SoapFormatter를 이용해 Serialize합니다.
        /// </summary>
        /// <param name="filePath"></param>
        /// <param name="graph"></param>
        private static object SoapFormatterDeserialize(string filePath, object graph)
        {
            FileStream fs = null;
            try
            {
                fs = new FileStream(filePath, FileMode.Open, FileAccess.Read);
                SoapFormatter sf = new SoapFormatter();
                object obj = sf.Deserialize(fs);
                fs.Close();
                return obj;
            }
            catch (Exception e)
            {
                throw e;
            }
            finally
            {
                if (fs != null)
                    fs.Close();
            }
        }


        /// <summary>
        /// BinaryFormatter를 이용해 Serialize합니다.
        /// </summary>
        /// <param name="filePath"></param>
        /// <param name="graph"></param>
        private static void BinaryFormatterSerialize(string filePath, object graph)
        {
            FileStream fs = null;
            try
            {
                fs = new FileStream(filePath, FileMode.Create, FileAccess.Write);
                BinaryFormatter bf = new BinaryFormatter();
                bf.Serialize(fs, graph);
                fs.Close();
            }
            catch (Exception e)
            {
                throw e;
            }
            finally
            {
                if (fs != null)
                    fs.Close();
            }
        }


        /// <summary>
        /// SoapFormatter를 이용해 Serialize합니다.
        /// </summary>
        /// <param name="filePath"></param>
        /// <param name="graph"></param>
        private static void SoapFormatterSerialize(string filePath, object graph)
        {
            FileStream fs = null;
            try
            {
                fs = new FileStream(filePath, FileMode.Create, FileAccess.Write);
                SoapFormatter sf = new SoapFormatter();
                sf.Serialize(fs, graph);
                fs.Close();
            }
            catch (Exception e)
            {
                throw e;
            }
            finally
            {
                if (fs != null)
                    fs.Close();
            }
        }

    }
}
