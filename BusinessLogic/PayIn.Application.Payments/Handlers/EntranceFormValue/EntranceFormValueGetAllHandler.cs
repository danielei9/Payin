using PayIn.Application.Dto.Payments.Arguments;
using PayIn.Application.Dto.Payments.Results;
using PayIn.BusinessLogic.Common;
using PayIn.Common;
using PayIn.Domain.Payments;
using PayIn.Domain.Public;
using System;
using System.Linq;
using System.Threading.Tasks;
using Xp.Application.Dto;
using Xp.Common;
using Xp.Domain;

namespace PayIn.Application.Payments.Handlers
{
	class EntranceFormValueGetAllHandler : 
		IQueryBaseHandler<EntranceFormValueGetAllArguments, EntranceFormValueGetAllResult>
	{
		private readonly IEntityRepository<Entrance> Repository;
		private readonly IEntityRepository<ControlFormValue> ControlFormValueRepository;
		private readonly ISessionData SessionData;

		#region Constructors
		public EntranceFormValueGetAllHandler(
			IEntityRepository<Entrance> repository,
			IEntityRepository<ControlFormValue> controlFormValueRepository,
			ISessionData sessionData
			)
		{
			if (repository == null) throw new ArgumentNullException("repository");
			if (controlFormValueRepository == null) throw new ArgumentNullException("controlFormValueRepository");
			if (sessionData == null) throw new ArgumentNullException("sessionData");

			Repository = repository;
			SessionData = sessionData;
			ControlFormValueRepository = controlFormValueRepository;
		}
		#endregion Constructors

		#region ExecuteAsync
		public async Task<ResultBase<EntranceFormValueGetAllResult>> ExecuteAsync(EntranceFormValueGetAllArguments arguments)
		{
			var entrance = (await Repository.GetAsync())
				.Where(x => 
					x.Id == arguments.EntranceId
				)
				.Select(x => new {
					UserName = x.UserName + " " + x.LastName,
					EventName = x.EntranceType.Event.Name,
					Values = x.FormValues
						.Select(y => y.FormValueId)
				})
				.FirstOrDefault();

			var result = (await ControlFormValueRepository.GetAsync())
				.Where(x =>
					entrance.Values.Contains(x.Id)
				)
				.Select(x => new
				{
					Id = x.Id,
					ArgumentName = x.Argument.Name,
					ValueString = x.ValueString,
					ValueNumeric = x.ValueNumeric,
					ValueBool = x.ValueBool,
					ValueDateTime = x.ValueDateTime,
					ValueOptions = x.ValueOptions
						.Select(y => new EntranceFormValueGetAllResult_Option
						{
							Text = y.Text
						})
				})
				.ToList()
				.Select(x => new EntranceFormValueGetAllResult
				{
					Id = x.Id,
					ArgumentName = x.ArgumentName,
					ValueString = x.ValueString,
					ValueNumeric = x.ValueNumeric,
					ValueBool = x.ValueBool,
					ValueDateTime = (x.ValueDateTime == XpDateTime.MinValue)? (DateTime?)null : x.ValueDateTime.ToUTC(),
					ValueOptions = x.ValueOptions
				});

			return new EntranceFormValueGetAllResultBase {
				UserName = entrance.UserName,
				EventName = entrance.EventName,
				Data = result
			};
		}
		#endregion ExecuteAsync
		
	}
}
