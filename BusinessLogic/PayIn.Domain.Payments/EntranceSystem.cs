using PayIn.Common;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Text.RegularExpressions;
using Xp.Domain;

namespace PayIn.Domain.Payments
{
	public class EntranceSystem : Entity
	{
		[Required(AllowEmptyStrings = false)] public string Name               { get; set; }
		                                      public EntranceSystemType Type   { get; set; }
		[Required]                            public bool IsDefault            { get; set; }
		// Para el QR
		[Required(AllowEmptyStrings = false)] public string Template           { get; set; }
		[Required(AllowEmptyStrings = false)] public string RegEx              { get; set; }
		                                      public int? RegExEventCode       { get; set; }
        [Required]                            public int RegExEntranceCode     { get; set; }
		// Para la introducción manual
		[Required(AllowEmptyStrings = false)] public string TemplateText       { get; set; }
		[Required(AllowEmptyStrings = false)] public string RegExText          { get; set; }
		                                      public int? RegExEventCodeText   { get; set; }
        [Required]                            public int RegExEntranceCodeText { get; set; }

		#region Events
		[InverseProperty("EntranceSystem")]
		public ICollection<Event> Events { get; set; } = new List<Event>();
		#endregion Events

		#region PaymentConcessions
		[InverseProperty("EntranceSystems")]
		public ICollection<PaymentConcession> PaymentConcessions { get; set; } = new List<PaymentConcession>();
		#endregion PaymentConcessions

		public class Codes
		{
			public long? EventCode { get; set; }
			public long EntranceCode { get; set; }
		}

		#region GetEntranceCodeQr
		public Codes GetEntranceCodeQr(string code, long? expectedEventCode)
		{
			var match = new Regex(RegEx)
				.Match(code);
			if (!match.Success)
				return null;

			var eventCode = RegExEventCode != null ?
				Convert.ToInt64(match.Groups[RegExEventCode.Value].Value) :
				(long?)null;
			var entranceCode =
				Convert.ToInt64(match.Groups[RegExEntranceCode].Value);
			
			if ((expectedEventCode != null) && (eventCode != expectedEventCode))
				throw new ApplicationException("Entrada de otro evento");

			return new Codes
			{
				EventCode = eventCode,
				EntranceCode = entranceCode
			};
		}
		#endregion GetEntranceCodeQr

		#region GetEntranceCodeText
		public Codes GetEntranceCodeText(string code, long? expectedEventCode)
		{
			var match = new Regex(RegExText)
				.Match(code);
			if (!match.Success)
				throw new ApplicationException("Formato de la entrada no correcto");

			var eventCode = RegExEventCodeText != null ?
				Convert.ToInt64(match.Groups[RegExEventCodeText.Value].Value) :
				(long?)null;
			var entranceCode =
				Convert.ToInt64(match.Groups[RegExEntranceCodeText].Value);

			if ((expectedEventCode != null) && (eventCode != expectedEventCode))
				throw new ApplicationException("Entrada de otro evento");

			return new Codes
			{
				EventCode = eventCode,
				EntranceCode = entranceCode
			};
		}
		#endregion GetEntranceCodeText
	}
}
