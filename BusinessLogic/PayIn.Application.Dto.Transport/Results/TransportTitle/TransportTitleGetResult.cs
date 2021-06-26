using PayIn.Domain.Transport.Eige.Enums;

namespace PayIn.Application.Dto.Transport.Results.TransportTitle
{
	public class TransportTitleGetResult
	{
		public int Id { get; set; }
        public int Code { get; set; }
        public string Name { get; set; }
        public int State { get; set; }
        public int OwnerCode { get; set; }
		public string OwnerName { get; set; }
        public EigeCodigoEntornoTarjetaEnum? Environment { get; set; }

        public bool OperateByPayIn { get; set; }
        public bool IsOverWritable { get; set; }
        public int? Slot { get; set; }
        public bool HasZone { get; set; }

        public int? TemporalUnit { get; set; }
        public string TemporalTypeAlias { get; set; }
        public EigeTipoUnidadesValidezTemporalEnum? TemporalType { get; set; }
        public decimal? Quantity { get; set; }
        public string QuantityTypeAlias { get; set; }
        public EigeTipoUnidadesSaldoEnum? QuantityType { get; set; }

        public decimal? RechargeMin { get; set; }
        public decimal? RechargeStep { get; set; }
        public decimal? MaxQuantity { get; set; }

		public int? ValidityBit { get; set; }
		public int? TableIndex { get; set; }
		public int? MaxExternalChanges { get; set; }
		public int? MaxPeopleChanges { get; set; }
		public int? ActiveTitle { get; set; }
		public int? Priority { get; set; }
	}
}
