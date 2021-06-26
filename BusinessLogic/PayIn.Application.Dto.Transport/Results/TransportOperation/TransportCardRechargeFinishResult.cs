using PayIn.Domain.Transport.Eige.Enums;
using PayIn.Domain.Transport.Eige.Types;
using Xp.Common;

namespace PayIn.Application.Dto.Transport.Results.TransportCard
{
	public class TransportCardRechargeFinishResult
	{
		public int Code { get; set; }
		public bool InBlackList1 { get; set; }
		public bool InBlackList2 { get; set; }
		public EigeBool TituloEnAmpliacion1 { get; set; }
		public EigeBool TituloEnAmpliacion2 { get; set; }
		public EigeInt8 SaldoViaje1 { get; set; }
		public EigeInt8 SaldoViaje2 { get; set; }
	}
}
