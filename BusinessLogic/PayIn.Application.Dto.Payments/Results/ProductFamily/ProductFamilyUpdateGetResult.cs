namespace PayIn.Application.Dto.Payments.Results
{
    public partial class ProductFamilyUpdateGetResult
	{
		public int Id                     { get; set; }
		public string Name                { get; set; }
		public string Description         { get; set; }
        public int? ParentId              { get; set; }
    }
}
