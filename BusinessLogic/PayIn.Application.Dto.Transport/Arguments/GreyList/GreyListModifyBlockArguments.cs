using PayIn.Common;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Xp.Common.Dto.Arguments;

namespace PayIn.Application.Dto.Transport.Arguments.GreyList
{
	public class GreyListModifyBlockArguments : IArgumentsBase
	{
		public long Id { get; private set; }
		public dynamic Items { get; private set; }

		#region Constructors
		public GreyListModifyBlockArguments(long id, dynamic items)
		{
			Id    = id;
			Items = items;
		}
		#endregion Constructors
	}
}
