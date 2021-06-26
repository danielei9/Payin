using PayIn.Domain.Transport.Eige;
using Xp.Domain.Transport.MifareClassic;

namespace PayIn.Application.Transport.Scripts.Mobilis
{
	public interface ITransportCardGetTitleZoneNameScript : IMifareClassicScript<EigeCard>
	{
	}
	public static class TransportCardGetTitleZoneNameScriptExtension
	{
		public static void AddRequest(this ITransportCardGetTitleZoneNameScript that)
		{
			that.Read(that.Request, x => x.Titulo.ValidezZonal1);
			that.Read(that.Request, x => x.Titulo.ValidezZonal2);
		}
	}
}
