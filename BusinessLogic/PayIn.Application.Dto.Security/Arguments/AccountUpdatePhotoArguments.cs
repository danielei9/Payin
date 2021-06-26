using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Xp.Common;


namespace PayIn.Application.Dto.Security.Arguments
{
	public class AccountUpdatePhotoArguments
	{
		[DataSubType(DataSubType.Image)] public byte[] Avatar { get; set; }
	}
}
