using PayIn.Application.Dto.Arguments.ControlItem;
using PayIn.Domain.Public;
using System;
using System.Threading.Tasks;
using Xp.Application.Dto;
using Xp.Common;
using Xp.Domain;

namespace PayIn.Application.Public
{
	public class ControlItemUpdateHandler :
		IServiceBaseHandler<ControlItemUpdateArguments>
	{
		private readonly IEntityRepository<ControlItem> _Repository;

		#region Constructors
		public ControlItemUpdateHandler(IEntityRepository<ControlItem> repository)
		{
			if (repository == null)
				throw new ArgumentNullException("repository");
			_Repository = repository;
		}
		#endregion Constructors

		#region ExecuteAsync
		async Task<dynamic> IServiceBaseHandler<ControlItemUpdateArguments>.ExecuteAsync(ControlItemUpdateArguments arguments)
		{

			var trackFrecuency = arguments.TrackFrecuency;			
			XpDuration min = XpDuration.FromString("0:00:00");

			if (min.Value == trackFrecuency.Value)
				trackFrecuency = new DateTime(1900, 1, 1, 0, 0, 10);

			var item = await _Repository.GetAsync(arguments.Id);
			item.Name = arguments.Name ?? "";
			item.Observations = arguments.Observations ?? "";
			item.SaveTrack = arguments.SaveTrack;
			item.SaveFacialRecognition = arguments.SaveFacialRecognition;
			item.CheckTimetable = arguments.CheckTimetable;
			item.TrackFrecuency = trackFrecuency;

			return item;
		}
		#endregion ExecuteAsync
	}
}