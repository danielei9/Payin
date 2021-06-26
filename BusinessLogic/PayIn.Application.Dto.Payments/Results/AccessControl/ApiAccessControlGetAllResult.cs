namespace PayIn.Application.Dto.Payments.Results
{
    public partial class ApiAccessControlGetAllResult
	{

		public int Id						{ get; set; }
		public string Name					{ get; set; }
		public int CurrentCapacity			{ get; set; }
		public int MaxCapacity				{ get; set; }
	}
}