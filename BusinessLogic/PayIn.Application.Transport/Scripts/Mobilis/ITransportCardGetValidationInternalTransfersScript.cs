using PayIn.Domain.Transport.Eige;
using Xp.Domain.Transport.MifareClassic;

namespace PayIn.Application.Transport.Scripts.Mobilis
{
	public interface ITransportCardGetValidationInternalTransfersScript : IMifareClassicScript<EigeCard>
	{
	}
	public static class TransportCardGeValidationInternalTransfersScriptExtension
	{
		public static void AddRequest(this ITransportCardGetValidationInternalTransfersScript that)
		{
			that.Read(that.Request, x => x.Validacion.ContadorTransbordosInternos);
			that.Read(that.Request, x => x.TituloTuiN.ContadorTransbordosInternos);
		}
	}
}
