using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using Xp.Common.Dto.Arguments;
using Xp.Domain.Transport;

namespace PayIn.Application.Dto.Transport.Arguments.TransportOperation
{
    public class TransportOperationDetectArguments : IArgumentsBase
	{
		[Required]
        public CardInfo Card { get; set; }
		public DeviceInfo Device { get; set; }
        public bool NeedKeys { get; set; } = true;
        public int OperationId { get; set; }

        #region Constructors
        public TransportOperationDetectArguments(
            CardInfo card,
            DeviceInfo device)
		{
            Card = card;
            Device = device;
		}
		#endregion Constructors
	}
}
