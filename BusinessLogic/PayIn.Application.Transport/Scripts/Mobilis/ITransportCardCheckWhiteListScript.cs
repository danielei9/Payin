using PayIn.Domain.Transport.Eige;
using Xp.Domain.Transport.MifareClassic;

namespace PayIn.Application.Transport.Scripts.Mobilis
{
	public interface ITransportCardCheckWhiteListScript : IMifareClassicScript<EigeCard>
	{
	}
	public static class TransportCardCheckWhiteListScriptExtension
	{
		public static void AddRequest(this ITransportCardCheckWhiteListScript that)
		{
			that.Read(that.Request, x => x.Validacion.ListaBlanca);
		}
	}
}
