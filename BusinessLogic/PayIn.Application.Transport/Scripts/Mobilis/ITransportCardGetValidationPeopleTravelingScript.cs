using PayIn.Domain.Transport.Eige;
using Xp.Domain.Transport.MifareClassic;

namespace PayIn.Application.Transport.Scripts.Mobilis
{
	public interface ITransportCardGetValidationPeopleTravelingScript : IMifareClassicScript<EigeCard>
	{
	}
	public static class TransportCardGetValidationPeopleTravelingScriptExtension
	{
		public static void AddRequest(this ITransportCardGetValidationPeopleTravelingScript that)
		{
			that.Read(that.Request, x => x.Validacion.NumeroPersonasViaje);
			that.Read(that.Request, x => x.TituloTuiN.NumeroPersonasViajando);
		}
	}
}
