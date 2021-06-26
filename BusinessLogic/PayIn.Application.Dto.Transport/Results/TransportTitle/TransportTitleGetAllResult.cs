using PayIn.Common;
using PayIn.Domain.Transport.Eige.Enums;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace PayIn.Application.Dto.Transport.Results.TransportTitle
{
	public class TransportTitleGetAllResult
	{
		public int Id { get; set; }
		public int Code { get; set; }
		public string Name { get; set; }
		public int? OwnerCode { get; set; }
		public string OwnerName { get; set; }
		public string Image { get; set; }
		public bool HasZone { get; set; }
		public decimal? Price { get; set; }
		public int? CounterCardSupport { get; set; }
        public int? CounterTitlecompatibilities { get; set; }
        public TransportTitleState State { get; set; }
		public int CounterPricing { get; set; }
		public int? PriceId { get; set; }
		public decimal? MaxQuantity { get; set; }
		public bool OperateByPayIn { get; set; }
		public bool IsYoung { get; set; }
		public EigeCodigoEntornoTarjetaEnum? Environment { get; set; }
		public string EnvironmentAlias { get; set; }
		public bool IsOverWritable { get; set; }
		public int? ValidityBit { get; set; }
		public int? TableIndex { get; set; }
		public int? MaxExternalChanges { get; set; }
		public int? MaxPeopleChanges { get; set; }
		public int? ActiveTitle { get; set; }
		public int? Priority { get; set; }
		public EigeTipoUnidadesValidezTemporalEnum? TemporalType { get; set; }
		public string TemporalTypeAlias { get; set; }
		public int? TemporalUnit { get; set; }
		public EigeTipoUnidadesSaldoEnum? QuantityType { get; set; }
		public string QuantityTypeAlias { get; set; }
		public decimal? Quantity { get; set; }
	}
}
