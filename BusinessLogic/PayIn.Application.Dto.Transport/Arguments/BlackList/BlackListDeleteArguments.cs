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
	public class BlackListDeleteArguments : IArgumentsBase
	{
		public long   Id    { get; private set; }
		public long Uid   { get; private set; }

		#region Constructors
		public BlackListDeleteArguments(long id, long uid)
		{
			Id    = id;
			Uid   = uid;
		}
		#endregion Constructors
	}
}
