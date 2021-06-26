using PayIn.Common;
using PayIn.Domain.Public;
using System.ComponentModel.DataAnnotations;
using Xp.Application.Dto;
using Xp.Common;
using Xp.Common.Dto.Arguments;

namespace PayIn.Application.Dto.Arguments
{
	public partial class ServiceUserUpdateArguments : IUpdateArgumentsBase<ServiceUser>
	{
																	public int Id                       { get; set; }
		[Display(Name = "resources.serviceUser.phone")]				public long? Phone					{ get; set; }
		[Display(Name = "resources.serviceUser.email")]				public string Email					{ get; set; }
		[DataSubType(DataSubType.Image, DataSubType.Pdf)]
		[Display(Name = "resources.serviceUser.assertDoc")]			public string AssertDoc				{ get; set; }
		////[DataType(DataType.Upload)]
		//[DataSubType(DataSubType.Image, DataSubType.Pdf)]
		//[Display(Name = "resources.serviceUser.assertDoc")]			public FileDto AssertDocument		{ get; set; }
		[Required(AllowEmptyStrings = false)]
		[Display(Name = "resources.serviceUser.name")]				public string Name					{ get; set; }
		[Required(AllowEmptyStrings = false)]
		[Display(Name = "resources.serviceUser.lastName")]			public string LastName				{ get; set; }
		[Display(Name = "resources.serviceUser.vatNumber")]			public string VatNumber				{ get; set; }
		[Display(Name = "resources.serviceUser.birthDate")]			public XpDate BirthDate 			{ get; set; }
		[Display(Name = "resources.serviceUser.uid")]				public long Uid						{ get; set; }
		[Display(Name = "resources.serviceUser.cardState")]			public ServiceCardState CardState	{ get; set; }
																	public string Photo					{ get; set; }
		[Display(Name = "resources.serviceUser.code")]				public string Code                  { get; set; }
		[Display(Name = "resources.serviceUser.observations")]		public string Observations          { get; set; }
		[Display(Name = "resources.serviceUser.address")]			public string Address				{ get; set; }

		#region Constructors
		//public ServiceUserUpdateArguments(int id, long phone, string mail, byte[] assertDoc, FileDto assertDocument, XpDate birthDate, long uid, ServiceCardState cardState, string name, string lastname, string photo, string vatNumber, string code, string observations, string address)
		public ServiceUserUpdateArguments(int id, long phone, string mail, string assertDoc, XpDate birthDate, long uid, ServiceCardState cardState, string name, string lastname, string photo, string vatNumber, string code, string observations, string address)
		{
			Id = id;
			Phone = phone;
			Email = Email ?? "";
			AssertDoc = assertDoc;
			//AssertDocument = assertDocument;
			BirthDate = birthDate;
			Uid = uid;
			Name = name;
			LastName = lastname;
			CardState = cardState;
			Photo = photo;
			VatNumber = vatNumber ?? "";
			Code = code ?? "";
			Observations = observations ?? "";
			Address = address ?? "";
		}
		#endregion Constructos
	}
}
