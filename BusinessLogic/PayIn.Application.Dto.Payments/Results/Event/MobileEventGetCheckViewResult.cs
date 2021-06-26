namespace PayIn.Application.Dto.Payments.Results
{
    public partial class MobileEventGetCheckViewResult
	{
		public int Id			 { get; set; }
		public string Name		 { get; set; }
		public string PhotoUrl	 { get; set; }
		public int? Capacity     { get; set; }
		public int Count         { get; set; }
	}
}
