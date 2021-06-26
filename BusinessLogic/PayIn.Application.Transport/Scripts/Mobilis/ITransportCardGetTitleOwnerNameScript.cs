using PayIn.Domain.Transport.Eige;
using Xp.Domain.Transport.MifareClassic;

namespace PayIn.Application.Transport.Scripts.Mobilis
{
	public interface ITransportCardGetTitleOwnerNameScript : IMifareClassicScript<EigeCard>
	{
	}
	public static class TransportCardGetTitleOwnerNameScriptExtension
	{
		public static void AddRequest(this ITransportCardGetTitleOwnerNameScript that)
		{
			that.Read(that.Request, x => x.Tarjeta.CodigoEntorno);
			that.Read(that.Request, x => x.Titulo.EmpresaPropietaria1);
			that.Read(that.Request, x => x.Titulo.EmpresaPropietaria2);
			that.Read(that.Request, x => x.Titulo.EmpresaPropietariaM);
			that.Read(that.Request, x => x.Titulo.EmpresaPropietariaB);
		}
	}
}
