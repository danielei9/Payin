using System;

namespace PayIn.Application.Dto.Results.ServiceFreeDays
{
				public partial class ServiceFreeDaysGetAllByConcessionResult
				{
								public int Id { get; set; }
								public string Name { get; set; }
								public int ConcessionId { get; set; }
								public string ConcessionName { get; set; }
								public DateTime From { get; set; }
								public DateTime Until { get; set; }

								public ServiceFreeDaysGetAllByConcessionResult()
								{
								}
				}
}