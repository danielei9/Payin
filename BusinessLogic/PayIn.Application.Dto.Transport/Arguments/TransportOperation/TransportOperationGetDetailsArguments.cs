using PayIn.Common;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Xp.Common.Dto.Arguments;

namespace PayIn.Application.Dto.Transport.Arguments.TransportOperation
{
	public class TransportOperationGetDetailsArguments : IArgumentsBase
	{
		public long Id { get; set; }
		public long Uid { get; set; }	

		#region Constructors
		public TransportOperationGetDetailsArguments(long id, long uid)
		{
			Id = id;
			Uid = uid;		
		}
		#endregion Constructors
	}
}
