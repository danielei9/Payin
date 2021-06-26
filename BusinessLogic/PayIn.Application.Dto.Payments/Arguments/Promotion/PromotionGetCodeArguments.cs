using PayIn.Common;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Xp.Common.Dto.Arguments;

namespace PayIn.Application.Dto.Payments.Arguments.Promotion
{
	public class PromotionGetCodeArguments : IArgumentsBase
	{
		public int Id { get; private set; }


		#region Constructors
		public PromotionGetCodeArguments(int id)
		{
			Id = id;
		}
		#endregion Constructors
	}
}
