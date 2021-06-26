using PayIn.Common;
using System;
using System.Collections.Generic;

namespace PayIn.Application.Dto.Results
{
    public class ServiceCardGetResult_Operation
	{
		public int Id;
		public DateTime Date;
		public ServiceOperationType Type;
		public string TypeName;
		public int? Seq;
		public int? ESeq;
		public IEnumerable<ServiceCardGetResult_PurseValue> PurseValues { get; set; }
	}
}
