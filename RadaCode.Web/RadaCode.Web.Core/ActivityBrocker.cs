using System;
using RadaCode.Web.Data.EF;
using RadaCode.Web.Data.Entities;

namespace RadaCode.Web.Core
{
    public class ActivityBrocker
    {
        private RadaCodeWebStoreContext _context;

        public ActivityBrocker(RadaCodeWebStoreContext context)
        {
            _context = context;
        }

        public void RegisterUserLogonActivity(WebUser visitor, string IP)
        {
            var loginActivity = _context.UserActivities.Add(new UserLogin
            {
                CreateTime = DateTime.UtcNow,
                CreatorID = visitor.UserName,
                CreatorIP = IP
            });

            visitor.Activities.Add(loginActivity);

            try
            {
                _context.SaveChanges();
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }
    }
}
