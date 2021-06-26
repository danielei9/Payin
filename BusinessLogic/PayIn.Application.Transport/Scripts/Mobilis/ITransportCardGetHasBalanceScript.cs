using PayIn.Domain.Transport.Eige;
using Xp.Domain.Transport.MifareClassic;

namespace PayIn.Application.Transport.Scripts.Mobilis
{
	public interface ITransportCardGetHasBalanceScript : IMifareClassicScript<EigeCard>
	{
	}
	public static class TransportCardGetHasBalanceScriptExtension
	{
		public static void AddRequest(this ITransportCardGetHasBalanceScript that)
		{
			that.Read(that.Request, x => x.Titulo.UsoDelCampoSaldoViajes1);
			that.Read(that.Request, x => x.Titulo.CodigoTitulo1);
			that.Read(that.Request, x => x.Titulo.UsoDelCampoSaldoViajes2);
			that.Read(that.Request, x => x.Titulo.CodigoTitulo2);
		}
	}
}
