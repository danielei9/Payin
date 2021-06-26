﻿using PayIn.Common;

namespace PayIn.Application.Dto.Results
{
	public class ServiceCardGetAllResult
	{
		public enum ResultType
		{
			NotEmitted = 0,
			Principal = 1,
			Secondary = 2,
			Anonymous = 3,
			Linked = 4
		}

		public int Id { get; set; }
		public long Uid { get; set; }
		public string UidText { get; set; }
		public string BatchName { get; set; }
		public string Name { get; set; }
		public string LastName { get; set; }
		public string Alias { get; set; }
		public ServiceCardState State { get; set; }
		public ResultType Type { get; set; }
		public int LastSeq { get; set; }
		public decimal LastBalance { get; set; }
		public decimal PendingBalance { get; set; }
		public int BlackListCount { get; set; }
	}
}
