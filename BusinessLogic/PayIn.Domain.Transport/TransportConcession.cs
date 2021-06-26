using System.ComponentModel.DataAnnotations;
using Xp.Domain;
using Xp.Common;
using PayIn.Common;
using PayIn.Domain.Payments;
using System.ComponentModel.DataAnnotations.Schema;
using System.Collections.Generic;

namespace PayIn.Domain.Transport
{ 
	public class TransportConcession : Entity
	{
													public TransportCardType CardType {get; set;}
		[Required(AllowEmptyStrings = true)]		public string UrlServer { get; set; }
													public TransportType TransportType {get; set;}

		#region Concession
		public int ConcessionId { get; set; }
		public PaymentConcession Concession { get; set; }
		#endregion Concession

		#region PaymentConcession
		public int? PaymentConcessionId { get; set; }
		public PaymentConcession PaymentConcession { get; set; }
		#endregion PaymentConcession


		#region TransportSystem
		public int? TransportSystemId { get; set; }
		public TransportSystem TransportSystem { get; set; }
		#endregion TransportSystem

		#region TransportTitle
		[InverseProperty("TransportConcession")]
		public ICollection<TransportTitle> Titles { get; set; }
		#endregion TransportTitle

		#region TransportCard
		[InverseProperty("TransportConcession")]
		public ICollection<TransportCard> TransportCards { get; set; }
		#endregion TransportCard

		#region TransportValidation
		[InverseProperty("Company")]
		public ICollection<TransportValidation> TransportValidations { get; set; }
		#endregion TransportValidation

		//#region TransportCardSupport
		//[InverseProperty("TransportConcession")]
		//public ICollection<TransportCardSupport> Supports { get; set; }
		//#endregion TransportCardSupport

		#region Constructors
		public TransportConcession()
		{
			Titles = new List<TransportTitle>();
			TransportCards = new List<TransportCard>();
			TransportValidations = new List<TransportValidation>();
			//Supports = new List<TransportCardSupport>();
		}
		#endregion Constructors

	}
}
