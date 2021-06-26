namespace PayIn.Application.Dto.Payments.Results
{
    public partial class ApiAccessControlGetResult
	{
		public int Id						{ get; set; }
		public string Name					{ get; set; }
		public string Schedule				{ get; set; }
		public string Map					{ get; set; }
		public int CurrentCapacity			{ get; set; }
		public int MaxCapacity				{ get; set; }
	}
}