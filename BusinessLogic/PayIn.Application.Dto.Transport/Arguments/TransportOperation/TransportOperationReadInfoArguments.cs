using PayIn.Domain.Transport.MifareClassic.Operations;
using System;
using System.ComponentModel.DataAnnotations;
using Xp.Common.Dto.Arguments;
using Xp.Domain.Transport;

namespace PayIn.Application.Dto.Transport.Arguments.TransportOperation
{
	public class TransportOperationReadInfoArguments : IArgumentsBase
	{
		[Required]
		public string MifareClassicCards { get; set; }
		public MifareOperationResultArguments[] Script { get; set; }
		public long? Uids
		{
			get
			{
				return MifareClassicCards.FromHexadecimal().ToInt32().Value;
			}
		}
		public DateTime Now { get; set; }
		public int OperationId { get; set; }
		public bool? IsRead { get; set; }
        public CardSystem CardSystem { get; set; }

		#region Constructors
		public TransportOperationReadInfoArguments(
			string cardId,
            CardSystem? cardSystem,
			MifareOperationResultArguments[] script
#if DEBUG || TEST || HOMO
			, DateTime? now = null
#endif
		)
		{
			MifareClassicCards = cardId;
            CardSystem = cardSystem ?? CardSystem.Mobilis;
            Script = script;
#if DEBUG || TEST || HOMO
			Now = now ?? DateTime.Now;
#else
			Now = DateTime.Now;
#endif
		}
		#endregion Constructors
	}
}
