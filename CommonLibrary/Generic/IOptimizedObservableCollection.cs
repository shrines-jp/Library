using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace CommonLibrary.Generic
{
	interface IOptimizedObservableCollection<T>
	{
		void AddRange(System.Collections.Generic.IEnumerable<T> items);
	}
}
