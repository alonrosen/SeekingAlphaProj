using MySql.Data.MySqlClient;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Configuration;

namespace SeekingAlphaProj.Models
{
    public class UserRepo : IUserRepo
    {
        private MySqlConnection GetConnection()
        {
            string connStr = WebConfigurationManager.ConnectionStrings["MySqlConnStr"].ConnectionString;
            return new MySqlConnection(connStr);
        }

        public UserRepo()
        {
            
        }
        

        public IEnumerable<UserFollowList> GetAll(int CurrentUid)
        {
            List<UserFollowList> ufl = new List<UserFollowList>();
            MySqlConnection conn = GetConnection();
            conn.Open();
            try
            {
                if (conn.State == System.Data.ConnectionState.Closed)
                {
                    throw new Exception("Cannot connect to Database");
                }
                MySqlCommand cmd = new MySqlCommand("get_user_following(@uid)", conn);
                cmd.Parameters.AddWithValue("uid", CurrentUid);
                using (MySqlDataReader reader = cmd.ExecuteReader())
                {
                    while (reader.Read())
                    {
                        ufl.Add(new UserFollowList()
                        {
                            Id = reader.GetInt32("id"),
                            Name = reader.GetString("name"),
                            Group = reader.GetString("group_name"),
                            Followers = reader.GetInt32("followers"),
                            Following = reader.GetBoolean("following")
                        });
                    }
                }

            }
            catch (MySql.Data.MySqlClient.MySqlException mse)
            {
                Console.WriteLine("Error in GetList->MySql - " + mse.Message);
            }
            catch (Exception e)
            {
                Console.WriteLine("Error in GetList->General - " + e.Message);
            }
            finally
            {
                if (conn.State != System.Data.ConnectionState.Closed)
                {
                    conn.Close();
                }
            }
            return ufl;
        }

        //update followers - action=F => Follow, action=U => Unfollow
        public void FollowUnfollow(int following_user_id, int followed_user_id, string action)
        {
            MySqlConnection conn = GetConnection();
            conn.Open();
            try
            {
                if (conn.State == System.Data.ConnectionState.Closed)
                {
                    throw new Exception("Cannot connect to Database");
                }
                MySqlCommand cmd = new MySqlCommand("follow_unfollow(@action,@uid,@fuid)", conn);
                cmd.Parameters.AddWithValue("uid", following_user_id);
                cmd.Parameters.AddWithValue("fuid", followed_user_id);
                cmd.Parameters.AddWithValue("action", action);
                int followed = cmd.ExecuteNonQuery();


            }
            catch (MySql.Data.MySqlClient.MySqlException mse)
            {
                Console.WriteLine("Error in FollowUnfollow - " + mse.Message);
            }
            catch (Exception e)
            {
                Console.WriteLine("Error in FollowUnfollow - " + e.Message);
            }
            finally
            {
                if (conn.State != System.Data.ConnectionState.Closed)
                {
                    conn.Close();
                }
            }
        }


        public string GetUserName(int uid)
        {
            MySqlConnection conn = GetConnection();
            conn.Open();
            string uname = "";
            try
            {
                if (conn.State == System.Data.ConnectionState.Closed)
                {
                    throw new Exception("Cannot connect to Database");
                }
                MySqlCommand cmd = new MySqlCommand("select name from users where user_id = @uid", conn);
                cmd.Parameters.AddWithValue("uid", uid);
                uname = cmd.ExecuteScalar().ToString();
                
            }
            catch (MySql.Data.MySqlClient.MySqlException mse)
            {
                Console.WriteLine("Error in GetUserName - " + mse.Message);
            }
            catch (Exception e)
            {
                Console.WriteLine("Error in GetUserName - " + e.Message);
            }
            finally
            {
                if (conn.State != System.Data.ConnectionState.Closed)
                {
                    conn.Close();
                }
            }
            return uname;
        }

    }
}