using Microsoft.Practices.Unity;
using PayIn.Application.Dto.Payments.Arguments;
using PayIn.Domain.Payments;
using System;
using System.Linq;
using System.Threading.Tasks;
using Xp.Application.Dto;
using Xp.Domain;

namespace PayIn.Application.Payments.Handlers
{
	public class EventUpdateHandler :
		IServiceBaseHandler<EventUpdateArguments>
	{
		[Dependency] public IEntityRepository<Event> Repository { get; set; }
		
		#region ExecuteAsync
		public async Task<dynamic> ExecuteAsync(EventUpdateArguments arguments)
		{
			var item = (await Repository.GetAsync())
				.Where(x => x.Id == arguments.Id)
				.FirstOrDefault();
			if (item == null)
				throw new ArgumentNullException("id");

			//// Reemplazos para corregir errores en el markdown a guardar
			//item.Description = item.Description.Replace(" **", "** ");
			//item.Description = item.Description.Replace(" *", "* ");
			//item.Description = item.Description.Replace(" ~~", "~~ ");
			//item.Description = item.Description.Replace(" __", "__ ");

			item.Longitude = arguments.Longitude;
			item.Latitude = arguments.Latitude;
			item.Name = arguments.Name;
			item.Description = arguments.Description;
			item.Place = arguments.Place;
			item.Capacity = arguments.Capacity;
			item.EventStart = arguments.EventStart;
			item.EventEnd = arguments.EventEnd;
			item.CheckInStart = arguments.CheckInStart;
			item.CheckInEnd = arguments.CheckInEnd;
			item.State = arguments.State;
			item.EntranceSystemId = arguments.EntranceSystemId;
			item.IsVisible = arguments.IsVisible;
            item.Code = arguments.Code;
            item.Conditions =arguments.Conditions;
            item.ShortDescription = arguments.ShortDescription;
			item.Visibility = arguments.Visibility;
			item.ProfileId = arguments.ProfileId;
			item.MaxEntrancesPerCard = arguments.MaxEntrancesPerCard ?? int.MaxValue;
			item.MaxAmountToSpend = arguments.MaxAmountToSpend;

			return item;
		}
		#endregion ExecuteAsync
	}
}
