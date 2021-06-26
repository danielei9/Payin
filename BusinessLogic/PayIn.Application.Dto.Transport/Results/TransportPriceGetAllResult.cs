using PayIn.Domain.Transport;
using PayIn.Domain.Transport.Eige.Enums;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Xp.Common;


namespace PayIn.Application.Dto.Transport.Results
{
	public partial class TransportPriceGetAllResult
	{
		public DateTime Start { get; set; }
		public DateTime End { get; set; }
		public int Version { get; set; }
		public decimal Price { get; set; }
		public int Quantity { get; set; }

		public EigeTipoUnidadesValidezTemporalEnum? TemporalUnities { get; set; }
		#region Title
		public int TransportTitleId { get; set; }
		public PayIn.Domain.Transport.TransportTitle Title { get; set; }
		#endregion
	}
}
