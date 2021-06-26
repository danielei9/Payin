namespace PayIn.Application.Dto.Results
{
	public partial class ServiceGroupGetAllResult
	{
		public int	Id					{ get; set; }
		public string Name				{ get; set; }
		public int	CategoryId			{ get; set; }
		public int  State				{ get; set; }
		public int? CounterGroupAfiliates { get; set; }
	}
}