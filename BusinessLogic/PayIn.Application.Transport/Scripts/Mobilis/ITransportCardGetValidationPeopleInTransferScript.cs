using PayIn.Domain.Transport.Eige;
using Xp.Domain.Transport.MifareClassic;

namespace PayIn.Application.Transport.Scripts.Mobilis
{
	public interface ITransportCardGetValidationPeopleInTransferScript : IMifareClassicScript<EigeCard>
	{
	}
	public static class TransportCardGetValidationPeopleInTransferScriptExtension
	{
		public static void AddRequest(this ITransportCardGetValidationPeopleInTransferScript that)
		{
			that.Read(that.Request, x => x.Validacion.NumeroPersonasTrasbordo);
			that.Read(that.Request, x => x.Validacion.ContadorViajerosSalida);
		}
	}
}
