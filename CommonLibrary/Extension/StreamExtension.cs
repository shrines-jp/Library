using System.IO;
using System.Runtime.Serialization.Formatters.Binary;

namespace CommonLibrary.Extension
{
	public static class StreamExtension
	{
		public static void SerializeTo<T>(this T o, Stream stream)
		{
			new BinaryFormatter().Serialize(stream, o);  // serialize o not typeof(T)
		}

		public static T Deserialize<T>(this Stream stream)
		{
			return (T)new BinaryFormatter().Deserialize(stream);
		}
	}
}
