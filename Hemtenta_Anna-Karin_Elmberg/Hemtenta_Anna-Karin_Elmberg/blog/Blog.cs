using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace HemtentaTdd2017
{
    public class Blog : IBlog
    {
        public IAuthenticator Authenticator { get; set; }
        public bool UserIsLoggedIn { get; set; } = false;

        public void LoginUser(User u)
        {
            if (u == null)
                throw new NullReferenceException();
            var userFound = Authenticator.GetUserFromDatabase(u.Name);
            if (u.Name == userFound.Name && u.Password == userFound.Password)
                UserIsLoggedIn = true;
            else
                UserIsLoggedIn = false;
        }

        public void LogoutUser(User u)
        {
            UserIsLoggedIn = false;
        }

        public bool PublishPage(Page p)
        {
            if (string.IsNullOrEmpty(p.Content) || string.IsNullOrEmpty(p.Title))
                throw new NullReferenceException();
            if (UserIsLoggedIn && p != null)
                return true;
            return false;
        }

        public int SendEmail(string address, string caption, string body)
        {
            if (UserIsLoggedIn && !string.IsNullOrEmpty(address) && !string.IsNullOrEmpty(caption) && !string.IsNullOrEmpty(body))
                return 1;
            return 0;
        }
    }
}


