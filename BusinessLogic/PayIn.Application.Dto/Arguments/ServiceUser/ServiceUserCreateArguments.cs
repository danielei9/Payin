using PayIn.Common;
using System.ComponentModel.DataAnnotations;
using Xp.Common;
using Xp.Common.Dto.Arguments;

namespace PayIn.Application.Dto.Arguments
{
	public partial class ServiceUserCreateArguments : IArgumentsBase
	{
		[Display(Name = "resources.serviceUser.phone")]                                              public long? Phone                { get; set; }
		[Display(Name = "resources.serviceUser.email")]                                              public string Email               { get; set; }
		[DataSubType(DataSubType.Image, DataSubType.Pdf)]
		[Display(Name = "resources.serviceUser.assertDoc")]                                          public byte [] AssertDoc          { get; set; }
		[Display(Name = "resources.serviceUser.birthDate")]		                                     public XpDate BirthDate           { get; set; }
		[Display(Name = "resources.serviceUser.uid")]                                                public long? Uid                  { get; set; }
		[Display(Name = "resources.serviceUser.cardState")]                                          public ServiceCardState CardState { get; set; }
		[Display(Name = "resources.serviceUser.name")]         [Required(AllowEmptyStrings = false)] public string Name                { get; set; }
		[Display(Name = "resources.serviceUser.lastName")]     [Required(AllowEmptyStrings = false)] public string LastName            { get; set; }
		[Display(Name = "resources.serviceUser.vatNumber")]                                          public string VatNumber           { get; set; }
		[Display(Name = "resources.serviceUser.systemCard")]   [Required]		                     public int SystemCardId           { get; set; }
		[Display(Name = "resources.serviceUser.code")]		                                         public string Code                { get; set; }
		[Display(Name = "resources.serviceUser.observations")]                                       public string Observations        { get; set; }
		[Display(Name = "resources.serviceUser.address")]                                            public string Address             { get; set; }

		#region Constructors
		public ServiceUserCreateArguments(long phone, string mail, byte[] assertDoc, XpDate birthDate, long? uid, ServiceCardState cardState, string name, string lastname, string vatNumber, int systemCardId, string code, string observations, string address)
		{
			Phone = phone;
			Email = Email ?? "";
			AssertDoc = assertDoc;
			BirthDate = birthDate;
			Uid = uid;
			Name = name;
			LastName = lastname;
			CardState = cardState;
			VatNumber = vatNumber ?? "";
			SystemCardId = systemCardId;
			Code = code ?? "";
			Observations = observations ?? "";
			Address = address ?? "";
		}
		#endregion Constructors
	}
}
