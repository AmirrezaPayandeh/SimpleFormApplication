using Microsoft.Extensions.Configuration;
using System.Collections.Generic;
using System.Data.SqlClient;

using SimpleApplication.ViewModels;

namespace SimpleApplication.Helpers
{
    public class CommonHelper
    {
        private IConfiguration m_Config;

        public CommonHelper(IConfiguration config)
        {
            m_Config = config;
        }

        public int ExecuteQuery(string query)
        {
            int Result = 0;
            string ConnectionString = m_Config["ConnectionStrings:DefaultConnection"];

            using (SqlConnection Connection = new SqlConnection(ConnectionString))
            {
                Connection.Open();

                SqlCommand Command = new SqlCommand(query, Connection);
                Result = Command.ExecuteNonQuery();

                Connection.Close();
            }

            return Result;
        }

        public List<UserInfoViewModel> SelectQuery(string query)
        {
            List<UserInfoViewModel> Data = new List<UserInfoViewModel>();
            string ConnectionString = m_Config["ConnectionStrings:DefaultConnection"];

            using (SqlConnection Connection = new SqlConnection(ConnectionString))
            {
                Connection.Open();

                SqlCommand Command = new SqlCommand(query, Connection);
                SqlDataReader Reader = Command.ExecuteReader();
                
                while (Reader.Read())
                {
                    var details = new UserInfoViewModel();
                    details.Id = Reader["Id"].ToString();
                    details.FirstName = Reader["FirstName"].ToString();
                    details.FamilyName = Reader["FamilyName"].ToString();
                    details.DateOfBirth = Reader["DateOfBirth"].ToString();
                    details.TelephoneNumber = Reader["TelephoneNumber"].ToString();
                    details.Address = Reader["Address"].ToString();
                    details.IsMale = Reader["IsMale"].ToString();
                    Data.Add(details);
                }

                Connection.Close();
            }

            return Data;
        }

        public UserInfoViewModel SelectUser(string query)
        {
            var User = new UserInfoViewModel();
            string ConnectionString = m_Config["ConnectionStrings:DefaultConnection"];

            using (SqlConnection Connection = new SqlConnection(ConnectionString))
            {
                Connection.Open();

                SqlCommand Command = new SqlCommand(query, Connection);
                SqlDataReader Reader = Command.ExecuteReader();

                Reader.Read();
                User.Id = Reader["Id"].ToString();
                User.FirstName = Reader["FirstName"].ToString();
                User.FamilyName = Reader["FamilyName"].ToString();
                User.DateOfBirth = Reader["DateOfBirth"].ToString();
                User.TelephoneNumber = Reader["TelephoneNumber"].ToString();
                User.Address = Reader["Address"].ToString();
                User.IsMale = Reader["IsMale"].ToString();

                Connection.Close();
            }

            return User;
        }
    }
}
