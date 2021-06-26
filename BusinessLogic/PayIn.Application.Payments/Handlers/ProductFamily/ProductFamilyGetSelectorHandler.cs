using PayIn.Application.Dto.Payments.Arguments;
using PayIn.Application.Dto.Payments.Results;
using PayIn.BusinessLogic.Common;
using PayIn.Domain.Payments;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Xp.Application.Dto;
using Xp.Domain;

namespace PayIn.Application.Public.Handlers
{
    public class ProductFamilyGetSelectorHandler :
        IQueryBaseHandler<ProductFamilyGetSelectorArguments, ProductFamilyGetSelectorResult>
    {
        private readonly IEntityRepository<ProductFamily> Repository;
        private readonly ISessionData SessionData;

        #region Constructors
        public ProductFamilyGetSelectorHandler(IEntityRepository<ProductFamily> repository, ISessionData sessionData)
        {
            if (repository == null) throw new ArgumentNullException("repository");
			if (sessionData == null) throw new ArgumentNullException("sessionData");
			Repository = repository;
            SessionData = sessionData;
        }
        #endregion Constructors

        #region ExecuteAsync
        public async Task<ResultBase<ProductFamilyGetSelectorResult>> ExecuteAsync(ProductFamilyGetSelectorArguments arguments)
        {
            var items = (await Repository.GetAsync());

			var dictionary = items.ToDictionary((item) => item.Id);

			if (!arguments.Filter.IsNullOrEmpty())
				items = items
					.Where(x =>
						x.Name.Contains(arguments.Filter)
					);

			var result = CheckLoop(items, dictionary, arguments.Id)
				.Select(x => new ProductFamilyGetSelectorResult
				{
					Id = x.Id,
					Value = x.Name
				});

            return new ResultBase<ProductFamilyGetSelectorResult> { Data = result };
        }
		#endregion ExecuteAsync

		#region CheckLoop
		public IEnumerable<ProductFamily> CheckLoop(IEnumerable<ProductFamily> items, Dictionary<int, ProductFamily> dictionary, int? sourceId = null)
		{
			foreach (var item in items)
			{
				var family = CheckLoop(item, dictionary, sourceId);
				if (family != null)
					yield return family;
			}
		}
		public ProductFamily CheckLoop(ProductFamily item, Dictionary<int, ProductFamily> dictionary, int? sourceId = null)
		{
			var family = item;
			var ids = new Dictionary<int, int>();
			while (family != null)
			{
				// Va a montar un bucle
				if (family.Id == sourceId)
					return null;

				// Diccionario para evitar bucles
				if (ids.ContainsKey(family.Id))
					return null;
				ids.Add(family.Id, family.Id);

				// No es bucle
				if (family.SuperFamilyId == null)
					return item;

				family = dictionary[family.SuperFamilyId.Value];
			}

			return null;
		}
		#endregion CheckLoop
	}
}