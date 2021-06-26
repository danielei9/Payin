using PayIn.Application.Dto.Arguments.ControlItem;
using PayIn.Domain.Public;
using System;
using System.Threading.Tasks;
using Xp.Application.Dto;
using Xp.Domain;
using Xp.Common;

namespace PayIn.Application.Public
{
	public class ControlItemCreateHandler :
		IServiceBaseHandler<ControlItemCreateArguments>
	{
		private readonly IEntityRepository<ControlItem> Repository;

		#region Constructors
		public ControlItemCreateHandler(IEntityRepository<ControlItem> repository)
		{
			if (repository == null) throw new ArgumentNullException("repository");
			Repository = repository;
		}
		#endregion Constructors

		#region ExecuteAsync
		async Task<dynamic> IServiceBaseHandler<ControlItemCreateArguments>.ExecuteAsync(ControlItemCreateArguments arguments)
		{

			var trackFrecuency = arguments.TrackFrecuency;
			XpDuration min = XpDuration.FromString("0:00:00");

			if (trackFrecuency == null || (min.Value == trackFrecuency.Value))			
				trackFrecuency = new DateTime(1900, 1, 1, 0, 0, 10);
			

			var itemControlItem = new ControlItem
			{
				Name = arguments.Name ?? "",
				Observations = arguments.Observations ?? "",
				ConcessionId = arguments.ConcessionId,
				SaveTrack = arguments.SaveTrack,
				SaveFacialRecognition = arguments.SaveFacialRecognition,
				CheckTimetable = arguments.CheckTimetable,
				TrackFrecuency = trackFrecuency
			};
			await Repository.AddAsync(itemControlItem);

			return itemControlItem;
		}
		#endregion ExecuteAsync
	}
}