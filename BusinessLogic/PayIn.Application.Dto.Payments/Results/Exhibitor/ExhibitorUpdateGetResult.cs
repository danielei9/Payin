namespace PayIn.Application.Dto.Payments.Results
{
    public partial class ExhibitorUpdateGetResult
	{
		public int Id                     { get; set; }
		public string Name                { get; set; }
        public string Address             { get; set; }
        public string Email               { get; set; }
        public int? ConcessionId          { get; set; }
        public string Phone               { get; set; }
    }
}
