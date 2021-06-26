using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Xp.Common.Dto.Arguments;

namespace PayIn.Application.Dto.Transport.Arguments.TransportCardSupport
{
	public class TransportCardSupportGetAllArguments : IArgumentsBase
	{
		public string Filter { get; set; }
		public int OwnerId { get; set; }
		

		#region Constructors
		public TransportCardSupportGetAllArguments(string filter, int ownerId, string titleId)
		{
			Filter = filter ?? "";
			OwnerId = ownerId;
			
        }
		#endregion Constructors
	}
}
