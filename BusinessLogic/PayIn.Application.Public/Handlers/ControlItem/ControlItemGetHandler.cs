using PayIn.Application.Dto.Arguments.ControlItem;
using PayIn.Application.Dto.Results.ControlItem;
using PayIn.Domain.Public;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Xp.Application.Dto;
using Xp.Application.Handlers;
using Xp.Common;
using Xp.Domain;

namespace PayIn.Application.Public.Handlers
{
	public class ControlItemGetHandler :
		GetHandler<ControlItemGetArguments, ControlItemGetResult, ControlItem>
	{
		#region Constructors
		public ControlItemGetHandler(IEntityRepository<ControlItem> repository)
			: base(
				repository,
				(arguments, entities) =>
				{
					var properties = typeof(ControlItemGetResult).GetProperties();

					var result = new List<ControlItemGetResult>();
					foreach (var item in entities.Where(x => x.Id == arguments.Id))
					{
						var temp = Activator.CreateInstance<ControlItemGetResult>();
						result.Add(temp);

						foreach (var property in properties)
						{
							if (property.Name == "TrackFrecuency") {
								var value = (DateTime) (item.GetPropertyValue(property.Name));													 
								var valueMod = new XpDuration (value);
								temp.SetPropertyValue(property.Name, valueMod);

							}
							else
							{
								var value = item.GetPropertyValue(property.Name);
								temp.SetPropertyValue(property.Name, value);
							}
						}
					}

					return result;
				}
			)
		{
		}
		#endregion Constructors
	}
}
