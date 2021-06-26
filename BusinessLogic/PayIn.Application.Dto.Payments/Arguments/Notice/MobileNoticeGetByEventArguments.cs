﻿using Xp.Common.Dto.Arguments;

namespace PayIn.Application.Dto.Payments.Arguments
{
	public class MobileNoticeGetByEventArguments : IArgumentsBase
	{
		public int Id { get; set; }

		#region Constructors
		public MobileNoticeGetByEventArguments(int id)
		{
			Id = id;
		}
		#endregion Constructors
	}
}