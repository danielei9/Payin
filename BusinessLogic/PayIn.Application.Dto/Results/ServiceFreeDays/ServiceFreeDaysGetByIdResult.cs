using PayIn.Common;
using System;
using System.Collections.Generic;

namespace PayIn.Application.Dto.Results.ServiceFreeDays
{
				public partial class ServiceFreeDaysGetByIdResult
				{
								public int Id { get; set; }
								public string Name { get; set; }
								public int ConcessionId { get; set; }
								public string ConcessionName { get; set; }
								public DateTime From { get; set; }
								public DateTime Until { get; set; }
				}
}
