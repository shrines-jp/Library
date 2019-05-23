using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Web;

namespace CommonLibrary.Helper
{
	public static class CacheHelper
	{
		private const Int32 OneWeekByMinutes = 60 * 24 * 7;

		/// <summary>
		/// Insert value into the cache using
		/// appropriate name/value pairs
		/// </summary>
		/// <typeparam name="T">Type of cached item</typeparam>
		/// <param name="o">Item to be cached</param>
		/// <param name="Key">Name of item</param>
		public static void Add<T>(String Key, T o)
		{
			// NOTE: Apply expiration parameters as you see fit.
			// I typically pull from configuration file.

			// In this example, I want an absolute
			// timeout so changes will always be reflected
			// at that time. Hence, the NoSlidingExpiration.
			HttpContext.Current.Cache.Insert(
				Key,
				o,
				null,
				DateTime.Now.AddMinutes(OneWeekByMinutes),
				System.Web.Caching.Cache.NoSlidingExpiration);
		}

		public static void Add<T>(String Key, T o, bool overWrite)
		{
			Remove(Key);
			Add<T>(Key, o);
		}

		/// <summary>
		/// Remove item from cache
		/// </summary>
		/// <param name="key">Name of cached item</param>
		public static void Clear(String Key)
		{
			HttpContext.Current.Cache.Remove(Key);
		}


		/// <summary>
		/// Check for item in cache
		/// </summary>
		/// <param name="key">Name of cached item</param>
		/// <returns></returns>
		public static bool Exists(String Key)
		{
			return HttpContext.Current.Cache[Key] != null;
		}

		/// <summary>
		/// Retrieve cached item
		/// </summary>
		/// <typeparam name="T">Type of cached item</typeparam>
		/// <param name="key">Name of cached item</param>
		/// <param name="value">Cached value. Default(T) if item doesn't exist.</param>
		/// <returns>Cached item as type</returns>
		public static bool Get<T>(String Key, out T value)
		{
			try
			{
				if (!Exists(Key))
				{
					value = default(T);
					return false;
				}

				value = (T)HttpContext.Current.Cache[Key];
			}
			catch
			{
				value = default(T);
				return false;
			}

			return true;
		}

		public static void Remove(String Key)
		{
			if (Exists(Key))
				HttpContext.Current.Cache.Remove(Key);
		}


		public static void RemoveAll()
		{
			IDictionaryEnumerator cacheEnumerator = HttpContext.Current.Cache.GetEnumerator();
			while (cacheEnumerator.MoveNext())
			{
				HttpContext.Current.Cache.Remove(cacheEnumerator.Key.ToString());
			}

			///Here is the another example of removing cache.
			//foreach (DictionaryEntry dEntry in HttpContext.Current.Cache)
			//{
			//    HttpContext.Current.Cache.Remove(dEntry.Key.ToString());
			//}
		}
	}
}
