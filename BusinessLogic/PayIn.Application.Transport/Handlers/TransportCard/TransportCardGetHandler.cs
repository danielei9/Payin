using PayIn.Application.Dto.Transport.Arguments.TransportCard;
using PayIn.Application.Dto.Transport.Results.TransportCard;
using PayIn.BusinessLogic.Common;
using PayIn.Common;
using PayIn.Domain.Public;
using PayIn.Domain.Transport;
using System;
using System.Linq;
using System.Threading.Tasks;
using Xp.Application.Dto;
using Xp.Domain;

namespace PayIn.Application.Public.Handlers
{
	public class TransportCardGetHandler :
		IQueryBaseHandler<TransportCardGetArguments, TransportCardGetResult>
	{
		private readonly IEntityRepository<TransportCard> Repository;

		#region Constructor
		public TransportCardGetHandler(
			IEntityRepository<TransportCard> repository
			)		
		{
			if (repository == null) throw new ArgumentNullException("TransportCard");		

			Repository = repository;		
		}
		#endregion Constructor

		#region ExecuteAsync
		public async Task<ResultBase<TransportCardGetResult>> ExecuteAsync(TransportCardGetArguments arguments)
		{
			var items = (await Repository.GetAsync())
					   .Where(x => x.DeviceAddress == arguments.DeviceAddress);

			if (arguments.DeviceEntry != null)
			{
				items = items
					.Where(x => x.Entry == arguments.DeviceEntry);
			}
						
			var result = items
					.Select(x => new
					{
						Id = x.Id,
						DeviceName = x.Name,
						DeviceEntry = x.Entry,
						DeviceType = x.DeviceType,
						DeviceAddress = x.DeviceAddress,
						Uid = x.Uid,
						LastUse = x.LastUse,
						ExpiryDate = x.ExpiryDate,
						Login = x.Login,
						State = x.State,
						ImageUrl = x.ImageUrl
					})
					.ToList()
					.Select(x => new TransportCardGetResult
					{
						Id = x.Id,
						DeviceName = x.DeviceName,
						DeviceEntry = x.DeviceEntry,
						DeviceType = x.DeviceType,
						DeviceAddress = x.DeviceAddress,
						Uid = x.Uid,
						LastUse = x.LastUse,
						ExpiryDate = x.ExpiryDate,
						Login = x.Login,
						State = x.State,
						ImageUrl = x.ImageUrl
					});

			return new ResultBase<TransportCardGetResult> { Data = result };

		}
		#endregion ExecuteAsync
	}
}
