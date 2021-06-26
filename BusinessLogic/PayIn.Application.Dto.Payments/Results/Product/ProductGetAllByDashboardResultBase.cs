using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Xp.Application.Dto;

namespace PayIn.Application.Dto.Payments.Results
{
	public class ProductGetAllByDashboardResultBase : ResultBase<ProductGetAllByDashboardResult>
	{
		public int LastDay { get; set; }
		public int LastWeek { get; set; }
		public int LastMonth { get; set; }
		public int LastYear { get; set; }
		public int PercentDay { get; set; }
		public int PercentWeek { get; set; }
		public int PercentMonth { get; set; }
		public int PercentYear { get; set; }
	}
}
