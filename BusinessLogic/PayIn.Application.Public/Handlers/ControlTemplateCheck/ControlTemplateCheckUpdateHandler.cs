using PayIn.Application.Dto.Arguments.ControlTemplateCheck;
using PayIn.Domain.Public;
using System;
using System.Threading.Tasks;
using Xp.Application.Dto;
using Xp.Domain;

namespace PayIn.Application.Public.Handlers
{
	public class ControlTemplateCheckUpdateHandler :
		IServiceBaseHandler<ControlTemplateCheckUpdateArguments>
	{
		private readonly IEntityRepository<ControlTemplateCheck> Repository;

		#region Constructors
		public ControlTemplateCheckUpdateHandler(IEntityRepository<ControlTemplateCheck> repository)
		{
			if (repository == null)
				throw new ArgumentNullException("repository");
			Repository = repository;
		}
		#endregion Constructors

		#region ExecuteAsync
		async Task<dynamic> IServiceBaseHandler<ControlTemplateCheckUpdateArguments>.ExecuteAsync(ControlTemplateCheckUpdateArguments arguments)
		{
			var check = await Repository.GetAsync(arguments.Id);  
			check.Time = arguments.Time;

			return check;
		}
		#endregion ExecuteAsync
	}
}
