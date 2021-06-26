using PayIn.Common;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Xp.Common.Dto.Arguments;

namespace PayIn.Application.Dto.Transport.Arguments.BlackList
{
	public class BlackListCreateArguments : IArgumentsBase
	{
		public long   Id    { get; private set; }
		public long Uid   { get; private set; }

		#region Constructors
		public BlackListCreateArguments(long id, long uid)
		{
			Id    = id;
			Uid   = uid;
		}
		#endregion Constructors
	}
}
