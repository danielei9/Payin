using PayIn.Domain.Transport.Eige;
using Xp.Domain.Transport.MifareClassic;

namespace PayIn.Application.Transport.Scripts.Mobilis
{
	public interface ITransportCardGetHistoricoTiene8Script : IMifareClassicScript<EigeCard>
	{
	}
	public static class TransportCardGetHistoricoTiene8ScriptExtension
	{
		public static void AddRequest(this ITransportCardGetHistoricoTiene8Script that)
		{
			that.Read(that.Request, x => x.Titulo.CodigoTitulo1);
			that.Read(that.Request, x => x.Titulo.CodigoTitulo2);
		}
	}
}
