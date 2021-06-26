using PayIn.Domain.Transport.Eige;
using Xp.Domain.Transport.MifareClassic;

namespace PayIn.Application.Transport.Scripts.Mobilis
{
	public interface ITransportCardGetTitleCaducityScript : IMifareClassicScript<EigeCard>
	{
	}
	public static class TransportCardGetTitleCaducityScriptExtension
	{
		public static void AddRequest(this ITransportCardGetTitleCaducityScript that)
		{
			that.Read(that.Request, x => x.Titulo.FechaValidez1);
			that.Read(that.Request, x => x.Titulo.FechaValidez2);
		}
	}
}
