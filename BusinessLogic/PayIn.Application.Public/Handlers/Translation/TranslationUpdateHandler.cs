using Microsoft.Practices.Unity;
using PayIn.Application.Dto.Arguments;
using PayIn.Domain.Payments;
using System;
using System.Linq;
using System.Threading.Tasks;
using Xp.Application.Dto;
using Xp.Domain;

namespace PayIn.Application.Public.Handlers
{
	public class TranslationUpdateHandler : IServiceBaseHandler<TranslationUpdateArguments>
    {
		[Dependency] public IEntityRepository<Translation> Repository { get; set; }

		#region ExecuteAsync
		public async Task<dynamic> ExecuteAsync(TranslationUpdateArguments arguments)
		{
			var translation = (await Repository.GetAsync())
				.Where(x =>
						x.Id == arguments.Id
				)
				.FirstOrDefault();

			if (translation == null)
				throw new ArgumentException("translationId");

			translation.Text = arguments.TranslatedText;

			return translation;
		}
		#endregion ExecuteAsync
	}
}

