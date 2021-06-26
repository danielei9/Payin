using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Xp.Infrastructure.Azure
{
	public abstract class AzureQueueContext<T> : IDisposable
		where T : AzureQueueContext<T>
	{
		#region Dispose
		public void Dispose()
		{
		}
		#endregion Dispose
	}
}
