using PayIn.Common;

namespace PayIn.Application.Dto.Payments.Results
{
    public partial class ExhibitorGetResult
	{
		public int Id                   { get; set; }
		public string Name              { get; set; }
        public string Address           { get; set; }
        public string Email             { get; set; }
        public int? ConcessionId        { get; set; }
        public string concessionName    { get; set; }
        public string Phone             { get; set; }

		public string InvitationCode	{ get; set; }
		public ExhibitorState State		{ get; set; }

		public string Code				{ get; set; }
		public string Contact			{ get; set; }
		public string PostalCode		{ get; set; }
		public string City				{ get; set; }
		public string Province			{ get; set; }
		public string Country			{ get; set; }
		public string Fax				{ get; set; }
		public string Url				{ get; set; }
		public string ContactEmail		{ get; set; }
		public string Pavilion			{ get; set; }
		public string Stand				{ get; set; }
	}
}
