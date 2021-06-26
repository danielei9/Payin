using PayIn.Common;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using Xp.Domain;
using Xp.Domain.Transport;

namespace PayIn.Domain.Transport
{
	public class TransportSystem : Entity
	{
		[Required(AllowEmptyStrings = false)]	public string Name { get; set; }
												public CardType CardType { get; set; }
		[Required()]                            public TransportSystemState State { get; set; }

		#region TransportConcession
		[InverseProperty("TransportSystem")]
		public ICollection<TransportConcession> TransportConcession { get; set; }
		#endregion TransportConcession

		#region TransportCard
		[InverseProperty("TransportSystem")]
		public ICollection<TransportCard> TransportCards { get; set; }
		#endregion TransportCard

		#region TransportCardApplication
		[InverseProperty("TransportSystem")]
		public ICollection<TransportCardApplication> TransportCardApplications { get; set; }
		#endregion TransportCardApplication

		#region Constructors
		public TransportSystem()
		{
			TransportConcession = new List<TransportConcession>();
			TransportCards = new List<TransportCard>();
			TransportCardApplications = new List<TransportCardApplication>();
		}
		#endregion Constructors
	}
}
