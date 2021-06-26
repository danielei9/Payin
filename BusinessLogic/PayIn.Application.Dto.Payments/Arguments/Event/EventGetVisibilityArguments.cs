﻿using Newtonsoft.Json;
using Xp.Common.Dto.Arguments;

namespace PayIn.Application.Dto.Payments.Arguments
{
	public class EventGetVisibilityArguments : IArgumentsBase
	{
		[JsonIgnore]
		public int Id { get; set; }
	}
}