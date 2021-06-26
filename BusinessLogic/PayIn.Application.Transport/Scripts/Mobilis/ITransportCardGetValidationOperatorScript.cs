using PayIn.Domain.Transport.Eige;
using Xp.Domain.Transport.MifareClassic;

namespace PayIn.Application.Transport.Scripts.Mobilis
{
	public interface ITransportCardGetValidationOperatorScript : IMifareClassicScript<EigeCard>
	{
	}
	public static class TransportCardGetValidationOperatorScriptExtension
	{
		public static void AddRequest(this ITransportCardGetValidationOperatorScript that)
		{
			that.Read(that.Request, x => x.Tarjeta.CodigoEntorno);
			that.Read(that.Request, x => x.Historico.EmpresaOperadora1);
			that.Read(that.Request, x => x.Historico.EmpresaOperadora2);
			that.Read(that.Request, x => x.Historico.EmpresaOperadora3);
			that.Read(that.Request, x => x.Historico.EmpresaOperadora4);
			that.Read(that.Request, x => x.Historico.EmpresaOperadora5);
			that.Read(that.Request, x => x.Historico.EmpresaOperadora6);
			that.Read(that.Request, x => x.Historico.EmpresaOperadora7);
			that.Read(that.Request, x => x.Historico.EmpresaOperadora8);
			that.Read(that.Request, x => x.Validacion.EmpresaOperadora);
		}
	}
}
