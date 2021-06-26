using PayIn.Application.Dto.Transport.Arguments.TransportList;
using PayIn.Domain.Transport;
using System;
using System.Threading.Tasks;
using Xp.Application.Dto;
using Xp.Domain;

namespace PayIn.Application.Transport.Handlers
{
	public class WhiteListManageHandler:
		IServiceBaseHandler<WhiteListManageArguments>
	{
		private readonly IEntityRepository<WhiteList> Repository;

		#region constructors
		public WhiteListManageHandler(
			IEntityRepository<WhiteList> repository)
		{
			if (repository == null) throw new ArgumentNullException("repository");
			Repository = repository;
		}
		#endregion constructors

		#region executeasync
		public async Task<dynamic> ExecuteAsync(WhiteListManageArguments arguments)
		{
			return await Task.Run(() =>
			{
				//await transportCardRechargeFinishHandler.ExecuteAsync(new TransportCardRechargeFinishArguments(script.Card.Titulo, script.Card.Uid.ToString(), script, script.Card, 0));
				//var now = DateTime.Now;
				//if (script.Card.Titulo.TituloEnAmpliacion1.Value && (script.Card.Titulo.FechaValidez1.Value > now || script.Card.Titulo.FechaValidez1.Value == null))
				//	throw new Exception(WhiteListResources.ImpossibleRecharge);
				//else if (script.Card.Titulo.TituloEnAmpliacion1.Value && script.Card.Titulo.FechaValidez1.Value < now)
				//	script.Card.Titulo.FechaValidez1 = new EigeDateTime(now.AddDays(script.Card.Titulo.NumeroUnidadesValidezTemporal1.Value));
				//else if (!script.Card.Titulo.TituloEnAmpliacion1.Value && (script.Card.Titulo.FechaValidez1.Value < now || script.Card.Titulo.FechaValidez1.Value > now || script.Card.Titulo.FechaValidez1.Value == null))
				//	script.Card.Titulo.TituloEnAmpliacion1 = new EigeBool(true);
				return (WhiteListManageArguments)null;
			});
		}
		#endregion executeasync
	}
}