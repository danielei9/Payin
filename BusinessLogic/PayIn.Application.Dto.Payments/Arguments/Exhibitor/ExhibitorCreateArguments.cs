using System.ComponentModel.DataAnnotations;
using Xp.Common.Dto.Arguments;

namespace PayIn.Application.Dto.Payments.Arguments
{
	public partial class ExhibitorCreateArguments : IArgumentsBase
	{
		public int Id { get; set; }

		[Display(Name = "resources.exhibitor.code")]
		[Required(AllowEmptyStrings = false)]
		public string Code { get; set; }

		[Display(Name = "resources.exhibitor.name")]
		[Required(AllowEmptyStrings = false)]
		public string Name { get; set; }

		[Display(Name = "resources.exhibitor.contact")]
		public string Contact { get; set; }

		[Display(Name = "resources.exhibitor.address")]
		public string Address { get; set; }

		[Display(Name = "resources.exhibitor.postalCode")]
		public string PostalCode { get; set; }

		[Display(Name = "resources.exhibitor.city")]
		public string City { get; set; }

		[Display(Name = "resources.exhibitor.province")]
		public string Province { get; set; }

		[Display(Name = "resources.exhibitor.country")]
		public string Country { get; set; }

        [Display(Name = "resources.exhibitor.phone")]
		public string Phone { get; set; }

		[Display(Name = "resources.exhibitor.fax")]
		public string Fax { get; set; }

        [Display(Name = "resources.exhibitor.email")]
        public string Email { get; set; }

		[Display(Name = "resources.exhibitor.url")]
		public string Url { get; set; }

		[Display(Name = "resources.exhibitor.contactEmail")]
		public string ContactEmail { get; set; }

		[Display(Name = "resources.exhibitor.pavilion")]
		public string Pavilion { get; set; }


		[Display(Name = "resources.exhibitor.stand")]
		public string Stand { get; set; }

		[Display(Name = "resources.exhibitor.invitationCode")]
		public string InvitationCode { get; set; }



		[Display(Name = "resources.exhibitor.concession")]
		public int? PaymentConcessionId { get; set; }
        public int EventId              { get; set; }


		#region Constructors
		public ExhibitorCreateArguments(int id, string code, string name, string contact, 
			string address, string postalCode, string city, string province, string country,
			string email, string contactEmail, string url, 
			string pavilion, string stand, 
			int? concessionId, string phone, string fax, int eventId,
			string invitationCode)
		{
			Id = id;
			Code = code;
			Name = name;
			Contact = contact;

			Address = address ?? "";
			PostalCode = postalCode ?? "";
			City = city ?? "";
			Province = province ?? "";
			Country = country ?? "";

			Email = email ?? "";
			ContactEmail = contactEmail ?? "";
			Url = url ?? "";

			Pavilion = pavilion ?? "";
			Stand = stand ?? "";

			Phone = phone ?? "";
			Fax = fax ?? "";

			InvitationCode = invitationCode;

			PaymentConcessionId = concessionId;
            EventId = eventId;
		}
		#endregion Constructors
	}
}
