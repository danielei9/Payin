using PayIn.Domain.Transport.Eige;
using Xp.Domain.Transport.MifareClassic;

namespace PayIn.Application.Transport.Scripts.Mobilis
{
	public interface ITransportCardGetHistoricoIndiceScript : IMifareClassicScript<EigeCard>
	{
	}
	public static class TransportCardGetHistoricoIndiceScriptExtension
	{
		public static void AddRequest(this ITransportCardGetHistoricoIndiceScript that)
		{
			that.Read(that.Request, x => x.Titulo.CodigoTitulo1);
			that.Read(that.Request, x => x.Titulo.CodigoTitulo2);
			that.Read(that.Request, x => x.Historico.IndiceTransaccion);
			that.Read(that.Request, x => x.TituloTuiN.IndiceTransaccion);
		}
	}
}
