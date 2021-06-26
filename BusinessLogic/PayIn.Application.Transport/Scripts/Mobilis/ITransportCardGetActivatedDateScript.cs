using PayIn.Domain.Transport.Eige;
using Xp.Domain.Transport.MifareClassic;

namespace PayIn.Application.Transport.Scripts.Mobilis
{
	public interface ITransportCardGetActivatedDateScript : IMifareClassicScript<EigeCard>
	{
	}
	public static class TransportCardGetActivatedDateScript
	{
		public static void AddRequest(this ITransportCardGetActivatedDateScript that)
		{
			// Titulo 1
			that.Read(that.Request, x => x.Titulo.FechaValidez1);
			// Titulo 2
			that.Read(that.Request, x => x.Titulo.FechaValidez2);
		}
	}
}
