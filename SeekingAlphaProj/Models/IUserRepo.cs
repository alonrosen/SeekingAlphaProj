using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SeekingAlphaProj.Models
{
    interface IUserRepo
    {
        IEnumerable<UserFollowList> GetAll(int CurrentUid);
        void FollowUnfollow(int following_user_id, int followed_user_id, string action);
        string GetUserName(int uid);

    }
}
