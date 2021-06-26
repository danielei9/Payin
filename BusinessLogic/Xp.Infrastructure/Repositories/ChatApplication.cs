using Autoescuelas.Application.Dto.Dtos;
using Autoescuelas.Core;
using Autoescuelas.Core.Constants;
using Autoescuelas.Domain.Entities;
using Autoescuelas.Infrastructure.Interfaces.Repositories;
using Autoescuelas.Others.DI.Config;
using Microsoft.AspNet.SignalR;
using System;
using System.Collections.Generic;
using System.Data.Entity.SqlServer;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using Xp.Domain;

namespace Autoescuelas.DistributedServices.RealTime.Hubs
{
	public class ChatApplication : IChatRepository
	{
		private readonly IRepository<Usuario> _RepositoryUsuario;
		private readonly IRepository<Dispositivo> _RepositoryDispositivo;
		private readonly ISessionData _SessionData;
		private readonly IUnitOfWork _UnitOfWork;

		#region Constructors
		public ChatApplication(IRepository<Usuario> usuarioRepository, IRepository<Dispositivo> dispositivoRepository,
			ISessionData sessionData,
			IUnitOfWork unitOfWork)
		{
			if (usuarioRepository == null)
				throw new ArgumentNullException("usuarioRepository");
			_RepositoryUsuario = usuarioRepository;

			if (dispositivoRepository == null)
				throw new ArgumentNullException("dispositivoRepository");
			_RepositoryDispositivo = dispositivoRepository;

			if (sessionData == null)
				throw new ArgumentNullException("sessionData");
			_SessionData = sessionData;

			if (unitOfWork == null)
				throw new ArgumentNullException("unitOfWork");
			_UnitOfWork = unitOfWork;
		}
		#endregion Constructors

		#region Add
		public async Task Add(string connectionId)
		{
			var usuario = _RepositoryUsuario.Get("Dispositivos")
				.Where(x => x.Email == _SessionData.Email)
				.FirstOrDefault();

			foreach (var dispositivo in usuario.Dispositivos.OrderByDescending(x => x.FechaRegistro).Skip(10))
			{
				_RepositoryDispositivo.Delete(dispositivo);
			}

			_RepositoryDispositivo.Add(new Dispositivo
			{
				FechaRegistro = DateTime.UtcNow,
				Tipo = DispositivoTipo.Web,
				Token = connectionId,
				UsuarioId = usuario.Id
			});

			await _UnitOfWork.CommitAsync();
		}
		#endregion Add

		#region Remove
		public async Task Remove(string connectionId)
		{
			var usuario = _RepositoryUsuario.Get()
				.Where(x => x.Email == _SessionData.Email)
				.FirstOrDefault();

			var items = _RepositoryDispositivo.Get()
				.Where(x =>
					x.Token == connectionId &&
					x.UsuarioId == usuario.Id
				);

			foreach (var item in items)
				_RepositoryDispositivo.Delete(item);

			await _UnitOfWork.CommitAsync();
		}
		#endregion Remove

		public DispositivoTipo DispositivoTipo { get { return DispositivoTipo.Web; } }

		public string SendNotification(string pushId, string pushCertificate, IEnumerable<string> targetIds, string message, string relatedName, string realtedId, int notificationId, int sourceId, string sourceNombre)
		{
			// No se puede usar directamente el hub
			// http://www.asp.net/signalr/overview/signalr-20/hubs-api/hubs-api-guide-server#callfromoutsidehub

			var context = GlobalHost.ConnectionManager.GetHubContext<ChatHub>() as IHubContext;
			var clientes = context.Clients.Clients(targetIds.ToList());
			clientes.SendMessage(new NotificationDto
			{
				Id = notificationId,
				Texto = message,
				DestinoId = null,
				FechaPublicacion = DateTime.UtcNow,
				OrigenId = sourceId,
				OrigenNombre = sourceNombre,
			}.JsonFormat());

			return "Ok";
		}
	}
}
