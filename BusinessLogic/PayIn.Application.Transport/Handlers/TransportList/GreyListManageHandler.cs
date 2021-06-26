using PayIn.Application.Dto.Transport.Arguments.TransportList;
using PayIn.Application.Transport.Scripts;
using PayIn.BusinessLogic.Common;
using PayIn.Common;
using PayIn.Domain.Transport;
using PayIn.Domain.Transport.Eige.Types;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Xp.Application.Dto;
using Xp.Domain;

namespace PayIn.Application.Transport.Handlers
{
	public class GreyListManageHandler :
		IServiceBaseHandler<GreyListManageArguments>
	{
		private readonly ISessionData SessionData;
		private readonly IEntityRepository<GreyList> Repository;

		#region Constructors
		public GreyListManageHandler(
			ISessionData sessionData,
			IEntityRepository<GreyList> repository)
		{
			if (sessionData == null) throw new ArgumentNullException("sessionData");
			if (repository == null) throw new ArgumentNullException("repository");
			SessionData = sessionData;
			Repository = repository;
		}
		#endregion Constructors

		#region ExecuteAsync
		public async Task<dynamic> ExecuteAsync(GreyListManageArguments arguments)
		{
			var now =  DateTime.Now;

			var uid = arguments.CardId.FromHexadecimal().ToInt48().Value;
			var item = (await Repository.GetAsync())
				.Where(x => x.Uid == uid)
				.FirstOrDefault();

			if(item == null)
			{
				var greyList = new GreyList
				{
						Uid = uid,
						RegistrationDate = now,
						Resolved = false,
						ResolutionDate = null,
						//OperationNumber = arguments.OperationNumber,
						Action = arguments.Action,
						//AffectedCamp = arguments.AffectedCamp,
						NewValue = arguments.NewValue, 
						Source = GreyList.GreyListSourceType.SigAPunt
						//EquipmentType = arguments.EquipmentType						
				};
				await Repository.AddAsync(greyList);
			}

			//await Task.Run(() =>
			//{
			//	var script = new TransportCardCheckGreyListScript(SessionData.Login, arguments.Script);				
							
			//});

			return item;
		}
		#endregion ExecuteAsync
	}
}