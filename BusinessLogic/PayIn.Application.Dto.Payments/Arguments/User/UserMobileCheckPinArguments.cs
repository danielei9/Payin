using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Text;
using Xp.Common.Dto.Arguments;

namespace PayIn.Application.Dto.Payments.Arguments.User
{
	public class UserMobileCheckPinArguments : IArgumentsBase
	{
		[Required(AllowEmptyStrings = false)] public string Pin { get; set; }
		
		#region Constructors
		public UserMobileCheckPinArguments(string pin)
		{
			Pin = pin;
		}
		#endregion Constructors
	}
}
