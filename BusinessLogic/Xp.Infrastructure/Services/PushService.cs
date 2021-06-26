using PayIn.Common;
using PayIn.Domain.Public;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Xp.Domain;
using Xp.Infrastructure.Repositories;

namespace Xp.Infrastructure.Services
{
    public class PushService : IPushService
    {
        private readonly IEntityRepository<Device> DeviceRepository;
        public readonly List<IPushSpecificService> PushServices;

        #region Constructors
        public PushService(
            IEntityRepository<Device> deviceRepository,
            IPushAndroidService pushAndroidService,
            IPushExpoService pushExpoService,
            IPushSignalRService pushSignalRService
        )
        {
            DeviceRepository = deviceRepository ?? throw new ArgumentNullException(nameof(deviceRepository));

            PushServices = new List<IPushSpecificService>
            {
                pushAndroidService,
                pushExpoService,
                pushSignalRService
            };
        }
        #endregion

        #region SendNotification
        public async Task<string> SendNotification(IEnumerable<string> targetIds, NotificationType type, NotificationState state, string message, string relatedName, string relatedId, int notificationId)
        {
            try
            {
                if ((targetIds != null) && (targetIds.Count() == 0))
                    throw new ArgumentNullException("targetIds");

                var devices = (await DeviceRepository.GetAsync("Platform"))
                    .Where(x => targetIds.Contains(x.Login))
                    .GroupBy(x => x.Platform, (x, g) => new
                    {
                        x.Type,
                        x.PushId,
                        x.PushCertificate,
                        Devices = g.Select(y => y.Token)
                    });
                if (devices == null)
                    throw new ArgumentNullException("targetIds");

                foreach (var item in devices)
                {
                    if (item.Devices.Count() == 0)
                        continue;

                    foreach (var repository in PushServices.Where(x => x.Type == item.Type))
                        await repository.SendNotification(
                            item.PushId,
                            item.PushCertificate,
                            item.Devices,
                            type,
                            state,
                            message,
                            relatedName,
                            relatedId,
                            notificationId,
                            0,
                            null);
                }
            }
            catch { }

            return "";
        }
        #endregion
    }
}
