using PayIn.Common;

namespace PayIn.Application.Dto.Results
{
	public class ServiceGroupServiceUsersGetAllResult
	{
		public int Id { get; set; }
		public string Name { get; set; }
		public string LastName { get; set; }
		public string Email { get; set; }
		public string VatNumber { get; set; }
		public ServiceUserState State { get; set; }
	}
}
