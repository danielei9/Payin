using PayIn.Application.Dto.Arguments.ServiceTag;
using PayIn.Application.Dto.Results.ServiceTag;
using PayIn.Domain.Public;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Xp.Application.Dto;
using Xp.Application.Handlers;
using Xp.Domain;

namespace PayIn.Application.Public.Handlers
{
	public class ServiceTagGetHandler :
		GetHandler<ServiceTagGetArguments, ServiceTagGetResult, ServiceTag>
	{
		#region Constructors
		public ServiceTagGetHandler(IEntityRepository<ServiceTag> repository)
			: base(
				repository,
				(arguments, entities) =>
				{
					var properties = typeof(ServiceTagGetResult).GetProperties();

					var result = new List<ServiceTagGetResult>();
					foreach (var item in entities.Where(x => x.Id == arguments.Id))
					{
						var temp = Activator.CreateInstance<ServiceTagGetResult>();
						result.Add(temp);

						foreach (var property in properties)
						{
							var value = item.GetPropertyValue(property.Name);
							temp.SetPropertyValue(property.Name, value);
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
