using PayIn.Common;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace PayIn.Application.Dto.Results.ServiceWorker
{
	public class WorkerMobileGetAllResult
	{
		public int         Id             { get; set; }
		public string      ConcessionName { get; set; }
		public WorkerState State          { get; set; }
		public WorkerType  Type           { get; set; }
	}
}
