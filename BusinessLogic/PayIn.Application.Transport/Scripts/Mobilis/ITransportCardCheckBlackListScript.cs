using PayIn.Domain.Transport.Eige;
using Xp.Domain.Transport.MifareClassic;

namespace PayIn.Application.Transport.Scripts.Mobilis
{
	public interface ITransportCardCheckBlackListScript : IMifareClassicScript<EigeCard>
	{
	}
	public static class TransportCardCheckBlackListScriptExtension
	{
		public static void AddRequest(this ITransportCardCheckBlackListScript that)
		{
			that.Read(that.Request, x => x.Validacion.ListaNegra);
			that.Read(that.Request, x => x.Usuario.DesbloqueoListaNegra);
		}
	}
}
