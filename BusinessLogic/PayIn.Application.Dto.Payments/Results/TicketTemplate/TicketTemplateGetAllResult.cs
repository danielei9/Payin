using PayIn.Common;
using PayIn.Domain.Payments;
using System;
using System.Collections.Generic;
using Xp.Common;

namespace PayIn.Application.Dto.Payments.Results.TicketTemplate
{
	public class TicketTemplateGetAllResult
	{
		public int Id { get; set; }
		public string Name { get; set; }
		public int ConcessionsCount { get; set; }
		public bool IsGeneric { get; set; }
	}
}
