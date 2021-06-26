using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using Xp.Domain;
using PayIn.Common;
using PayIn.Domain.Transport.Eige.Enums;

namespace PayIn.Domain.Transport
{
	public class TransportTitle : Entity
	{
		public int Code { get; set; }
		[Required(AllowEmptyStrings = false)]
		public string Name { get; set; }
		public int OwnerCode { get; set; }
		[Required(AllowEmptyStrings = true)]
		public string OwnerName { get; set; }
		public EigeCodigoEntornoTarjetaEnum? Environment { get; set; }
		[Required(AllowEmptyStrings = true)]
		public string Image { get; set; }
		public bool HasZone { get; set; }
		public TransportTitleState State { get; set; }
		[DataType(DataType.Currency)]
		public decimal? MaxQuantity { get; set; }
		public bool OperateByPayIn { get; set; }
		public bool IsOverWritable { get; set; }
		public int? Slot { get; set; }
		public bool IsPrivate { get; set; }
		public int? ValidityBit { get; set; }
		public int? TableIndex { get; set; }
		public int? MaxExternalChanges { get; set; }
		public int? MaxInternalChanges { get; set; }
		public int? MaxPeopleChanges { get; set; }
		public int? ActiveTitle { get; set; }
		public int? Priority { get; set; }
		public decimal? PriceStep { get; set; }
		public decimal? MinCharge { get; set; }
		public EigeTipoUnidadesValidezTemporalEnum? TemporalType { get; set; }
		public int? TemporalUnit { get; set; }
        //[Precision(9, 6)]
        public decimal? Quantity { get; set; }
		public EigeTipoUnidadesSaldoEnum? QuantityType { get; set; }
		public MeanTransportEnum? MeanTransport { get; set; }

		public bool AskQuantity()
		{
			return QuantityType == EigeTipoUnidadesSaldoEnum.Money;
		}

        // No se usa
        public bool IsYoung { get; set; }

        #region TransportPrice
        [InverseProperty("Title")]
		public ICollection<TransportPrice> Prices { get; set; } = new List<TransportPrice>();
        #endregion TransportPrice

        #region TransportCardSupportTitleCompatibility
        [InverseProperty("TransportTitle")]
		public ICollection<TransportCardSupportTitleCompatibility> TransportCardSupportTitleCompatibility { get; set; }
		#endregion TransportCardSupportTitleCompatibility

		#region TransportSimultaneousTitleCompatibility
		[InverseProperty("TransportTitle")]
		public ICollection<TransportSimultaneousTitleCompatibility> TransportSimultaneousTitleCompatibility { get; set; } = new List<TransportSimultaneousTitleCompatibility>();
        #endregion TransportSimultaneousTitleCompatibility

        #region TransportSimultaneousTitleCompatibility2
        [InverseProperty("TransportTitle2")]
		public ICollection<TransportSimultaneousTitleCompatibility> TransportSimultaneousTitleCompatibility2 { get; set; } = new List<TransportSimultaneousTitleCompatibility>();
        #endregion TransportSimultaneousTitleCompatibility2

        #region TransportConcession
        public int TransportConcessionId { get; set; }
		public TransportConcession TransportConcession { get; set; }
		#endregion TransportConcession

		#region ToTransportTitle
		[InverseProperty("FromTransportTitle")]
		public ICollection<TransportTitle> ToTransportTitle { get; set; } = new List<TransportTitle>();
        #endregion ToTransportTitle

        #region TransportOperationTitle
        [InverseProperty("Title")]
		public ICollection<TransportOperationTitle> TransportOperationTitles { get; set; } = new List<TransportOperationTitle>();
        #endregion TransportOperationTitle

        #region FromTransportTitle
        [InverseProperty("ToTransportTitle")]
        public ICollection<TransportTitle> FromTransportTitle { get; set; } = new List<TransportTitle>();
        #endregion FromTransportTitle
	}
}
