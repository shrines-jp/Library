using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Collections.ObjectModel;
using System.Collections.Specialized;

namespace CommonLibrary.Generic
{
	/// <summary>
	/// OptimizedObservableCollection
	/// .NET 4 에서 사용됨
	/// System.Collections.ObjectModel
	/// System.Collections.Specialized
	/// </summary>
	/// <typeparam name="T"></typeparam>
	public class OptimizedObservableCollection<T> : ObservableCollection<T>, IOptimizedObservableCollection<T>
	{
		private bool suppressOnCollectionChanged;

		public void AddRange(IEnumerable<T> items)
		{
			if (null == items)
			{
				throw new ArgumentNullException("items");
			}

			if (items.Any())
			{
				try
				{
					suppressOnCollectionChanged = true;
					foreach (var item in items)
					{
						Add(item);
					}
				}
				finally
				{
					suppressOnCollectionChanged = false;
					OnCollectionChanged(
						new NotifyCollectionChangedEventArgs(
							NotifyCollectionChangedAction.Reset));
				}
			}
		}


		public List<T> ConvertToList(IEnumerable<T> items)
		{
			List<T> list = new List<T>();

			if (null == items)
			{
				throw new ArgumentNullException("items");
			}

			if (items.Any())
			{
				IEnumerable<T> obsCollection = (IEnumerable<T>)items;
				list = new List<T>(obsCollection);
			}

			return list;
		}


		protected override void OnCollectionChanged(NotifyCollectionChangedEventArgs e)
		{
			if (!suppressOnCollectionChanged)
			{
				base.OnCollectionChanged(e);
			}
		}
	}
}
