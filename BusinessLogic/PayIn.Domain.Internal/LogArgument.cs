using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Xp.Domain;

namespace PayIn.Domain.Internal
{
	public class LogArgument : Entity
	{
		public string Name { get; set; }
		public string Value { get; set; }

		#region Log
		public int LogId { get; set; }
		public Log Log { get; set; }
		#endregion Log
	}
}
