using PayIn.Domain.Transport.Eige;
using Xp.Domain.Transport.MifareClassic;

namespace PayIn.Application.Transport.Scripts.Mobilis
{
	public interface ITransportCardGetIsPersonalizedScript : IMifareClassicScript<EigeCard>
	{
	}
	public static class TransportCardGetIsPersonalizedScriptExtension
	{
		public static void AddRequest(this ITransportCardGetIsPersonalizedScript that)
		{
			that.Read(that.Request, x => x.Tarjeta.Tipo);
			that.Read(that.Request, x => x.Tarjeta.Subtipo);
		}
	}
}
