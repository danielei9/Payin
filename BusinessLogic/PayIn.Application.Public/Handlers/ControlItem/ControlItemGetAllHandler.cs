using PayIn.Application.Dto.Arguments.ControlItem;
using PayIn.Application.Dto.Results.ControlItem;
using PayIn.Common;
using PayIn.Domain.Public;
using System;
using System.Linq;
using System.Threading.Tasks;
using Xp.Application.Dto;
using Xp.Application.Handlers;
using Xp.Common;
using Xp.Domain;

namespace PayIn.Application.Public.Handlers
{
	public class ControlItemGetAllHandler :
		GetHandler<ControlItemGetAllArguments, ControlItemGetAllResult, ControlItem>
	{
		#region Constructors
		public ControlItemGetAllHandler(IEntityRepository<ControlItem> repository)
			: base (
				repository,
				(arguments, items) =>
					items.Where(x => (
						string.IsNullOrEmpty(arguments.Filter) ||
						x.Name.Contains(arguments.Filter) ||
						x.Observations.Contains(arguments.Filter)
					))
					.OrderBy(x => new { x.Name })
					.Select(x => new ControlItemGetAllResult
					{
						Id                    = x.Id,
						Name                  = x.Name,
						Observations          = x.Observations,
						EntranceCount         = x.CheckPoints.Where(y => y.Type == CheckPointType.Entrance).Count(),
						ExitCount             = x.CheckPoints.Where(z => z.Type == CheckPointType.Exit).Count(),
						CheckCount            = x.CheckPoints.Where(z => z.Type == CheckPointType.Check).Count(),
						RoundCount            = x.CheckPoints.Where(z => z.Type == CheckPointType.Round).Count(),
						SaveTrack             = x.SaveTrack,
						SaveFacialRecognition = x.SaveFacialRecognition,
						CheckTimetable        = x.CheckTimetable
					})
			)
		{
		}
		#endregion Constructors
	}
}
