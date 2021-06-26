using System.Collections.Generic;
using PayIn.Domain.Public;

namespace PayIn.Application.Dto.Results
{
	public partial class ServiceGroupGetResult
	{
		public int	Id				{ get; set; }
		public string Name			{ get; set; }
		public string CategoryName	{ get; set; }
		public int CategoryId		{ get; set; }
		public IEnumerable<ServiceGroupGetUserResult> Users	{ get; set; }
	}
}