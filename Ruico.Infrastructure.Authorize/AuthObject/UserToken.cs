using System;

namespace Ruico.Infrastructure.Authorize.AuthObject
{
    public class UserToken
    {
        public Guid UserId { get; set; }

        public string LastLoginToken { get; set; }

        public string GetAuthToken()
        {
            return string.Format("{0}_{1}", this.UserId, this.LastLoginToken);
        }
    }
}
