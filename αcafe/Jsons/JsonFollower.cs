using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace αcafe.Jsons
{
    class JsonFollower
    {
        public class FollowersList
        {
            public string followerDate { get; set; }
            public string follower_id { get; set; }
            public string userIconPath { get; set; }
        }

        public class Followering
        {
            public string following_id { get; set; }
            public string followingDate { get; set; }
            public string userIconPath { get; set; }
        }

        public class RootObject
        {
            public string code { get; set; }
            public string count { get; set; }
            public List<FollowersList> followersList { get; set; }
            public List<Followering> following { get; set; }
            public string programId { get; set; }
            public string userId { get; set; }
        }
    }
}
