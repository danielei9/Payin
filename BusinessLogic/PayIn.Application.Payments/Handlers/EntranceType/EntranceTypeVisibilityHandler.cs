using Microsoft.Practices.Unity;
using PayIn.Application.Dto.Payments.Arguments;
using PayIn.Domain.Payments;
using System;
using System.Threading.Tasks;
using Xp.Application.Dto;
using Xp.Domain;

namespace PayIn.Application.Payments.Handlers
{
    class EntranceTypeVisibilityHandler :
        IServiceBaseHandler<EntranceTypeVisibilityArguments>
    {
        [Dependency] public IEntityRepository<EntranceType> Repository {get; set;}

		#region ExecuteAsync
		public async Task<dynamic> ExecuteAsync(EntranceTypeVisibilityArguments arguments)
		{
			var item = (await Repository.GetAsync(arguments.Id));

			item.Visibility = arguments.Visibility;

			return item;
		}
		#endregion ExecuteAsync
	}
}
