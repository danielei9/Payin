using PayIn.Domain.Transport.Eige;
using Xp.Domain.Transport.MifareClassic;

namespace PayIn.Application.Transport.Scripts
{
	public interface ITransportCardCheckGreyListScript : IMifareClassicScript<EigeCard>
	{
	}
	public static class TransportCardCheckGreyListScriptExtension
	{
		public static void AddRequest(this ITransportCardCheckGreyListScript that)
		{
			that.Read(that.Request, x => x.Validacion.ListaGris);
		}
	}
}
