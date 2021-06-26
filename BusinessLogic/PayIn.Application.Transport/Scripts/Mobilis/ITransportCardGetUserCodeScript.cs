using PayIn.Domain.Transport.Eige;
using Xp.Domain.Transport.MifareClassic;

namespace PayIn.Application.Transport.Scripts.Mobilis
{
	public interface ITransportCardGetUserCodeScript : IMifareClassicScript<EigeCard>
	{
	}
	public static class TransportCardGetUserCodeScriptExtension
	{
		public static void AddRequest(this ITransportCardGetUserCodeScript that)
		{
			that.Read(that.Request, x => x.Usuario.CodigoViajero);
		}
	}
}
