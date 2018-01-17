using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace SeekingAlphaProj.Models
{
    public class UserFollowList
    {
        public int Id { get; set; }
        public string Name { get; set; }
        public string Group { get; set; }
        public int Followers { get; set; }
        public bool Following { get; set; }
    }
}