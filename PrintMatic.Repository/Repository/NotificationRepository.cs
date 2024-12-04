using Microsoft.AspNetCore.Identity;
using PrintMatic.Core;
using PrintMatic.Core.Entities;
using PrintMatic.Core.Entities.Identity;
using PrintMatic.Core.Repository.Contract;
using PrintMatic.Repository.Data;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using FirebaseAdmin.Messaging;
namespace PrintMatic.Repository.Repository
{
    public class NotificationRepository : GenericRepository<NotificationMessage>, INotificationRepository
    {
        private readonly PrintMaticContext _context;
        private readonly UserManager<AppUser> _user;
        private readonly UnitOfWork _unitOfWork;

        public NotificationRepository(PrintMaticContext context,UserManager<AppUser> user,UnitOfWork unitOfWork):base(context)
        {
            _context = context;
            _user = user;
            _unitOfWork = unitOfWork;
        }

        public async Task SendNotification(string userId , string content,string title)
        {
            var user = await _user.FindByIdAsync(userId);
            string fcm = user.FCMToken;

            var message = new Message()
            {
                Notification = new Notification
                {
                    Title = title,
                    Body = content
                },
                Token = fcm 
            };
            var messaging = FirebaseMessaging.DefaultInstance;
            var result = await messaging.SendAsync(message);
            NotificationMessage notification = new NotificationMessage()
            {
                Content = content,
               UserId = userId,
            };
            _context.NotificationMessage.Add(notification);
           var count = await _unitOfWork.Complet();
        }

       
    }
}
