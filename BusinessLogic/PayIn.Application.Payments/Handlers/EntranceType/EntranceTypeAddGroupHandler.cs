using Microsoft.Practices.Unity;
using PayIn.Application.Dto.Payments.Arguments;
using PayIn.BusinessLogic.Common;
using PayIn.Common;
using PayIn.Domain.Payments;
using PayIn.Domain.Public;
using System;
using System.Linq;
using System.Threading.Tasks;
using Xp.Application.Dto;
using Xp.Domain;

namespace PayIn.Application.Payments.Handlers
{
	public class EntranceTypeAddGroupHandler :
		IServiceBaseHandler<EntranceTypeAddGroupArguments>
	{
		[Dependency] public IEntityRepository<ServiceGroupEntranceType> Repository { get; set; }

		#region ExecuteAsync
		public async Task<dynamic> ExecuteAsync(EntranceTypeAddGroupArguments arguments)
		{
			var item = new ServiceGroupEntranceType
			{
				EntranceTypeId = arguments.Id,
				GroupId = arguments.GroupId
			};
			await Repository.AddAsync(item);

			return item;	
		}
		#endregion ExecuteAsync
	}
}
