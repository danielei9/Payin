using PayIn.Domain.Transport.Eige;
using Xp.Domain.Transport.MifareClassic;

namespace PayIn.Application.Transport.Scripts.Mobilis
{
	public interface ITransportCardGetValidationMaxExternalTransfersScript : IMifareClassicScript<EigeCard>
	{
	}
	public static class TransportCardGetValidationMaxExternalTransfersScriptExtension
	{
		public static void AddRequest(this ITransportCardGetValidationMaxExternalTransfersScript that)
		{
			that.Read(that.Request, x => x.Validacion.ContadorTransbordosInternos);
		}
	}
}
