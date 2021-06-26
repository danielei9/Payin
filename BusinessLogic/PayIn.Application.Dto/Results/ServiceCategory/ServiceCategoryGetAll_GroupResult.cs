using PayIn.Domain.Public;
using System.Collections.Generic;

namespace PayIn.Application.Dto.Results
{
	public partial class ServiceCategoryGetAll_GroupResult
	{
		public int Id { get; set; }
		public string Name { get; set; }
		public int MembersCount { get; set; }
	}
}