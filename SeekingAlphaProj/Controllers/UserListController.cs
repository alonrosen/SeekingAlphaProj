using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Web.Http;
using MySql.Data.MySqlClient;
using SeekingAlphaProj.Models;
using System.Web.Configuration;
using System.Net.Http.Headers;
using System.Net.Http.Formatting;
using Newtonsoft.Json.Linq;
using Newtonsoft.Json;

namespace SeekingAlphaProj.Controllers
{
    public class UserListController : ApiController
    {
        //initialize data repository
        UserRepo ur = new UserRepo();

        private int GetUserCookie()
        {
            CookieHeaderValue cookie = Request.Headers.GetCookies("userid").FirstOrDefault();
            int userid = 0;
            if (cookie != null)
            {
                bool hasCookie = int.TryParse(cookie["userid"].Value, out userid);
                if (!hasCookie)
                {
                    userid = 0;
                }
            }
            return userid;
        }

        //main Get - get all user details
        public IEnumerable<UserFollowList> Get()
        {
            int userid = GetUserCookie();
            return ur.GetAll(userid); ;
        }

        // POST: api/UserList - update followers
        public void Post([FromBody]dynamic value)
        {
            string action = JObject.Parse(value.ToString())["action"].ToString();
            int uid1 = GetUserCookie();
            int uid2 = int.Parse(JObject.Parse(value.ToString())["uid"].ToString());
            ur.FollowUnfollow(uid1,uid2,action);
        }

        // get: api/UserList/:id - get user name
        public string Get(int uid)
        {
            return ur.GetUserName(uid);
        }

    }
}
