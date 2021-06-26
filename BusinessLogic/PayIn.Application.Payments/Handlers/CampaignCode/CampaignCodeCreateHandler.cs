using Microsoft.Practices.Unity;
using PayIn.Application.Dto.Payments.Arguments;
using PayIn.Common;
using PayIn.Domain.Payments;
using PayIn.Infrastructure.Public.Repositories;
using System;
using System.Linq;
using System.Threading.Tasks;
using Xp.Application.Dto;

namespace PayIn.Application.Payments.Handlers
{
    public class CampaignCodeCreateHandler :
        IServiceBaseHandler<PublicCampaignCodeCreateArguments>
    {
        private static Random RandomGenerator = new Random();
        [Dependency] public CampaignCodeRepository CampaignCodeRepository { get; set; }

        #region ExecuteAsync
        public async Task<dynamic> ExecuteAsync(PublicCampaignCodeCreateArguments arguments)
        {
            // Devolver puede haber uno por campaña y usuario
            var item = (await CampaignCodeRepository.GetAsync())
                .Where(x =>
                    x.CampaignId == arguments.CampaignId &&
                    x.Login == arguments.Login
                )
                .FirstOrDefault();
            if (item != null)
                return item;

            // Generar código aleatorio
            var code = RandomGenerator.Next(0, int.MaxValue);
            while ((await CampaignCodeRepository.GetAsync())
                .Any(x =>
                    x.Code == code
                )
            )
                code = RandomGenerator.Next(0, int.MaxValue);

            // Crear código
            item = new CampaignCode
            {
                CampaignId = arguments.CampaignId,
                Login = arguments.Login,
                Code = code,
                State = CampaignCodeState.Active
            };
            await CampaignCodeRepository.AddAsync(item);

            return item;
        }
        #endregion ExecuteAsync
    }
}
