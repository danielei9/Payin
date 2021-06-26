using PayIn.Domain.Transport.Eige;
using Xp.Domain.Transport.MifareClassic;

namespace PayIn.Application.Transport.Scripts.Mobilis
{
	public interface ITransportCardGetValidationExternalTransfersScript : IMifareClassicScript<EigeCard>
	{
	}
	public static class TransportCardGetValidationExternalTransfersScriptExtension
	{
		public static void AddRequest(this ITransportCardGetValidationExternalTransfersScript that)
		{
			that.Read(that.Request, x => x.Validacion.ContadorTransbordosExternos);
		}
	}
}
