using PayIn.Application.Dto.Arguments;
using PayIn.Domain.Payments;
using System;
using System.Threading.Tasks;
using Xp.Application.Dto;
using Xp.Domain;

namespace PayIn.Application.Public.Handlers
{
	public class TranslationDeleteHandler :
		IServiceBaseHandler<TranslationDeleteArguments>
	{
		private readonly IEntityRepository<Translation> Repository;

		#region Constructors
		public TranslationDeleteHandler(IEntityRepository<Translation> repository)
		{
			if (repository == null) throw new ArgumentNullException("repository");
			Repository = repository;
		}
		#endregion Constructors

		#region ExecuteAsync
		async Task<dynamic> IServiceBaseHandler<TranslationDeleteArguments>.ExecuteAsync(TranslationDeleteArguments arguments)
		{
			var item = await Repository.GetAsync(arguments.Id);
			if (item == null)
				throw new ArgumentException("ID no encontrado");

			await Repository.DeleteAsync(item);
			return null;
		}
		#endregion ExecuteAsync
	}
}
