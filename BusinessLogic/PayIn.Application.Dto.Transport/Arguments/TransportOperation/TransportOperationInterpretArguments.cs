using PayIn.Domain.Transport.MifareClassic.Operations;
using System;
using Xp.Common.Dto.Arguments;

namespace PayIn.Application.Dto.Transport.Arguments.TransportOperation
{
    public class TransportOperationInterpretArguments : IArgumentsBase
	{
        public OperationInfo Operation { get; set; }
        public MifareOperationResultArguments[] Script { get; set; }
		public int? OperationId { get { return Operation.Id; } }
        public DateTime Now { get; set; }

        #region Constructors
        public TransportOperationInterpretArguments(
            OperationInfo operation,
            MifareOperationResultArguments[] script
#if DEBUG || TEST || HOMO
			, DateTime? now = null
#endif
		)
		{
            Operation = operation;
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
