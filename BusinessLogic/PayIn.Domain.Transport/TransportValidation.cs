using PayIn.Domain.Payments;
using PayIn.Domain.Transport.Eige.Enums;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using Xp.Domain;
using Xp.Domain.Transport;

namespace PayIn.Domain.Transport
{
	public class TransportValidation : Entity
	{
		public DateTime Date { get; set; }
		public EigeTipoValidacion_TransbordoEnum ValidationType { get; set; }
		public int Line { get; set; }
		public int Station { get; set; }
		public int Vehicle { get; set; }
		public EigeTipoValidacion_SentidoEnum Sense { get; set; }
		public int Travellers { get; set; }
		public decimal Price { get; set; }
		public long Uid { get; set; }

		#region Company	
		public int? CompanyId { get; set; }
		public TransportConcession Company { get; set; }
		#endregion Company

		#region Constructors
		public TransportValidation()
		{			
		}
		#endregion Constructors	
	}
}
