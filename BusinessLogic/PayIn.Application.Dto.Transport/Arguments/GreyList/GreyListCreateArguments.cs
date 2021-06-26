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
	public class GreyListCreateArguments : IArgumentsBase
	{
		public long   Id    { get; private set; }
		public string Key   { get; private set; }
		public string Value { get; private set; }

		#region Constructors
		public GreyListCreateArguments(long id, string key, string value)
		{
			Id    = id;
			Key   = key;
			Value = value;
		}
		#endregion Constructors
	}
}
