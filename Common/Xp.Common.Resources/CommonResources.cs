using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Xp.Common.Resources
{
	public class CommonResources
	{
		public GlobalResources Global { get; private set; }

		#region Constructors
		public CommonResources()
		{
			Global = new GlobalResources();
		}
		#endregion Constructors
	}
}
