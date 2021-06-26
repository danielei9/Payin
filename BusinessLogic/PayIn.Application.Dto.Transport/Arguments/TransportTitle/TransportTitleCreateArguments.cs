using PayIn.Domain.Transport.Eige.Enums;
using System.ComponentModel.DataAnnotations;
using Xp.Common.Dto.Arguments;

namespace PayIn.Application.Dto.Transport.Arguments.TransportTitle
{
	public class TransportTitleCreateArguments : IArgumentsBase
	{
		[Display(Name = "resources.transportTitle.code")]
		[Required]
		public int Code { get; set; }

		[Display(Name = "resources.transportTitle.name")]
		[Required(AllowEmptyStrings = false)]
		public string Name { get; set; }

		[Display(Name = "resources.transportTitle.ownerCode")]
		[Required]
		public int OwnerCode { get; set; }

		[Display(Name = "resources.transportTitle.slot")]
		public int? Slot { get; set; }

		[Display(Name = "resources.transportTitle.ownerName")]
		[Required(AllowEmptyStrings = true)]
		public string OwnerName { get; set; }

		[Display(Name = "resources.transportTitle.transportConcessionName")]
		[Required(AllowEmptyStrings = true)]
		public string TransportConcessionName { get; set; }

		[Display(Name = "resources.transportTitle.hasZone")]
		public bool HasZone { get; set; }

		[Display(Name = "resources.transportTitle.maxQuantity")]
		[DataType(DataType.Currency)]
		public decimal? MaxQuantity { get; set; }

		[Display(Name = "resources.transportTitle.operateByPayIn")]
		public bool OperateByPayIn { get; set; }

		[Display(Name = "resources.transportTitle.isYoung")]
		public bool IsYoung { get; set; }
		
		[Display(Name = "resources.transportTitle.environment")]
		[Required]
		public EigeCodigoEntornoTarjetaEnum Environment { get; set; }

		[Display(Name = "resources.transportTitle.isOverwritable")]		
		public bool IsOverWritable { get; set; }		

		[Display(Name = "resources.transportPrice.validityBit")]
		public int? ValidityBit { get; set; }

		[Display(Name = "resources.transportPrice.tableIndex")]
		public int? TableIndex { get; set; }

		[Display(Name = "resources.transportPrice.maxExternalChanges")]
		public int? MaxExternalChanges { get; set; }

		[Display(Name = "resources.transportPrice.maxPeopleChanges")]
		public int? MaxPeopleChanges { get; set; }

		[Display(Name = "resources.transportPrice.activeTitle")]
		public int? ActiveTitle { get; set; }

		[Display(Name = "resources.transportPrice.priority")]
		public int? Priority { get; set; }

		[Display(Name = "resources.transportPrice.units")]
		public int? TemporalUnit { get; set; }

		[Display(Name = "resources.transportPrice.temporalUnities")]
		public EigeTipoUnidadesValidezTemporalEnum? TemporalType { get; set; }

		[Display(Name = "resources.transportPrice.balance")]
		public decimal? Quantity { get; set; }
		[Display(Name = "resources.transportPrice.balanceUnities")]
		public EigeTipoUnidadesSaldoEnum? QuantityType { get; set; }

		#region Constructors
		public TransportTitleCreateArguments(int code, string name, int ownerCode, string ownerName, bool hasZone, decimal? maxQuantity, bool operateByPayIn, bool isYoung, string transportConcessionName, EigeCodigoEntornoTarjetaEnum environment, bool isOverWritable, int? slot, int? operatorContext, int? validityBit, int? tableIndex, int? maxExternalChanges, int? maxPeopleChanges, int? activeTitle, int? priority, int? temporalUnit, EigeTipoUnidadesValidezTemporalEnum? temporalType, int? quantity, EigeTipoUnidadesSaldoEnum? quantityType)
		{
			Code = code;
			Name = name;
			OwnerCode = ownerCode;
			OwnerName = ownerName;
			HasZone = hasZone;
			MaxQuantity = maxQuantity;
			OperateByPayIn = operateByPayIn;
			IsYoung = isYoung;
			TransportConcessionName = transportConcessionName;
			Environment = environment;
			IsOverWritable = isOverWritable;
			Slot = slot;
			ValidityBit = validityBit;
			TableIndex = tableIndex;
			MaxExternalChanges = maxExternalChanges;
			MaxPeopleChanges = maxPeopleChanges;
			ActiveTitle = activeTitle;
			Priority = priority;
			TemporalType = temporalType;
			TemporalUnit = temporalUnit;
			Quantity = quantity;
			QuantityType = quantityType;
		}
		#endregion Constructors
	}
}
