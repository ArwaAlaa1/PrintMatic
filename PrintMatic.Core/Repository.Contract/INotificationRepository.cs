using PrintMatic.Core.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace PrintMatic.Core.Repository.Contract
{
    public interface INotificationRepository: IGenericRepository<NotificationMessage>
    {
        Task SendNotification(string userId, string content, string title);
    }
}
