namespace PayIn.Application.Dto.Payments.Results
{
    public partial class ProductFamilyGetResult
	{
		public int Id					{ get; set; }
		public string Name				{ get; set; }
		public string Description		{ get; set; }
		public string PhotoUrl			{ get; set; }
		public int? ParentId			{ get; set; }
        public string FamilyName		{ get; set; }
		public bool IsVisible			{ get; set; }
        public string Code              { get; set; }
    }
}
