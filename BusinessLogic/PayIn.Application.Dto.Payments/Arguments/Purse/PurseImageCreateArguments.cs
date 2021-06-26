using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Xp.Common.Dto.Arguments;

namespace PayIn.Application.Dto.Payments.Arguments.Purse
{
	public class PurseImageCreateArguments : IArgumentsBase
	{
		[Required]   public byte [] Image { get; set; }
					 public int PurseId { get; set; }	


		#region Constructor
		public PurseImageCreateArguments(byte[] image, int purseId)
		{
			Image = image;
			PurseId = purseId;
		}
		#endregion Constructor
	}
}
