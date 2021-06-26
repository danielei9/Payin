using PayIn.Domain.Transport.Eige;
using Xp.Domain.Transport.MifareClassic;

namespace PayIn.Application.Transport.Scripts.Mobilis
{
	public interface ITransportCardGetValidationMaxInternalTransfersScript : IMifareClassicScript<EigeCard>
	{
	}
	public static class TransportCardGetValidationMaxInternalTransfersScriptExtension
	{
		public static void AddRequest(this ITransportCardGetValidationMaxInternalTransfersScript that)
		{
			that.Read(that.Request, x => x.Validacion.ContadorTransbordosInternos);
		}
	}
}
