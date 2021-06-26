using PayIn.Common;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using Xp.Domain;

namespace PayIn.Domain.Payments
{
	public class Exhibitor : Entity
	{
		[Required(AllowEmptyStrings = false)]       public string           Name           { get; set; }
		[Required(AllowEmptyStrings = true)]        public string           Address        { get; set; }
		[Required(AllowEmptyStrings = true)]        public string           Email          { get; set; }
        [Required(AllowEmptyStrings = true)]        public string           Phone          { get; set; }
        [Required(AllowEmptyStrings = true)]        public string           InvitationCode { get; set; }
                                                    public ExhibitorState   State          { get; set; }
        [Required(AllowEmptyStrings = true)]        public string           Fax            { get; set; } = "";
        [Required(AllowEmptyStrings = true)]        public string           PostalCode     { get; set; } = "";
        [Required(AllowEmptyStrings = true)]        public string           City           { get; set; } = "";
        [Required(AllowEmptyStrings = true)]        public string           Province       { get; set; } = "";
        [Required(AllowEmptyStrings = true)]        public string           Country        { get; set; } = "";
        [Required(AllowEmptyStrings = true)]        public string           Url            { get; set; } = "";
        [Required(AllowEmptyStrings = true)]        public string           Pavilion       { get; set; } = "";
        [Required(AllowEmptyStrings = true)]        public string           Stand          { get; set; } = "";
        [Required(AllowEmptyStrings = true)]        public string           Code           { get; set; } = "";
        [Required(AllowEmptyStrings = true)]        public string           Contact        { get; set; } = "";
        [Required(AllowEmptyStrings = true)]        public string           ContactEmail   { get; set; } = "";

        #region PaymentConcession
        public int? PaymentConcessionId { get; set; }
		[ForeignKey("PaymentConcessionId")]
		public PaymentConcession PaymentConcession { get; set; }
        #endregion PaymentConcession

        #region Events
        public ICollection<Event> Events { get; set; } = new List<Event>();
		#endregion Events

		#region Contact
		[InverseProperty("Exhibitor")]
		public ICollection<Contact> Contacts { get; set; } = new List<Contact>();
		#endregion Contact

		#region Constructors
		public Exhibitor()
		{ }
		public Exhibitor(string name, string address, string email, string phone, string invitationCode, string code, 
			string contact, string postalCode, string city, string province, string country, string fax, 
			string url, string contactEmail, string pavilion, string stand)
		{
			Name = name;
			Address = address;
			Email = email;
			Phone = phone;
			InvitationCode = invitationCode;
			State = ExhibitorState.Active;

			Code = code;
			Contact = contact;
			PostalCode = postalCode;
			City = city;
			Province = province;
			Country = country;
			Fax = fax;
			Url = url;
			ContactEmail = contactEmail;
			Pavilion = pavilion;
			Stand = stand;
		}
		#endregion Constructors
	}
}
