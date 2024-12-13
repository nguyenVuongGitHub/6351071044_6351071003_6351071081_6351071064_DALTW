using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace Dookki_Web.Models.Map
{
    public class mapAccount
    {
        DOOKKIEntities db = new DOOKKIEntities();

        //1. Tim kiem
        public ACCOUNT find(string username, string password)
        {
            var user = db.ACCOUNTs.Where(m=>m.UserName == username & m.Password == password).ToList();
            if(user.Count > 0)
            {
                return user[0];
            }
            else
            {
                return null;
            }
        }
        public ACCOUNT find(string username, string password, string role)
        {
            var user = db.ACCOUNTs.Where(m => m.UserName == username & m.Password == password && m.Role == role).ToList();
            if (user.Count > 0)
            {
                return user[0];
            }
            else
            {
                return null;
            }
        }
        //2. Lay danh sach
        public List<ACCOUNT> ListAccount()
        {
            var users = db.ACCOUNTs.ToList();
            return users;
        }

        //3. Them moi
        public void AddNew()
        {
            ACCOUNT account = new ACCOUNT();

        }
    }
}