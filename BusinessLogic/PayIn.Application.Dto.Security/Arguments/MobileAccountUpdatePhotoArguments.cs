using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Xp.Common;


namespace PayIn.Application.Dto.Security.Arguments
{
	public class MobileAccountUpdatePhotoArguments
	{
		//[DataSubType(DataSubType.Image)] public byte[] Avatar { get; set; }
		[DataType(DataType.ImageUrl)]
		public string Avatar { get; set; }
	}
}
