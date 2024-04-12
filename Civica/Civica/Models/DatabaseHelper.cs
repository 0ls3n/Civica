using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Microsoft.Data.SqlClient;
using Microsoft.Extensions.Configuration;
using Microsoft.VisualBasic.ApplicationServices;


namespace Civica.Models
{
    public static class DatabaseHelper
    {
        private static IConfigurationRoot config = new ConfigurationBuilder().AddJsonFile("appsettings.json").Build();

        private static string? connectionString = config.GetConnectionString("MyDBConnection");

        public static List<Project> InitializeProjects()
        {
            List<Project> projects = new List<Project>();

            using (SqlConnection con = new SqlConnection(connectionString))
            {
                con.Open();
                SqlCommand cmd = new SqlCommand("SELECT ProjectId, ProjectName, OwnerName, ManagerName, Description FROM PROJECTS", con);
                using (SqlDataReader reader = cmd.ExecuteReader())
                {
                    while (reader.Read())
                    {
                        int id = Convert.ToInt32(reader["ProjectId"]);
                        string name = Convert.ToString(reader["ProjectName"]);
                        string owner = Convert.ToString(reader["OwnerName"]);
                        string manager = Convert.ToString(reader["ManagerName"]);
                        string desc = Convert.ToString(reader["Description"]);

                        Project p = new Project(name, owner, manager, desc);

                        p.Id = id;

                        projects.Add(p);

                    }
                }
            }
            return projects;
        }

        public static int Add(Project p)
        {
            int id = -1;
            using (SqlConnection con = new SqlConnection(connectionString))

            {
                con.Open();
                SqlCommand cmd = new SqlCommand("INSERT INTO PROJECTS (ProjectName, OwnerName, ManagerName, Description)" +
                                                                     "VALUES (@PN, @ON, @MN, @DESC) SELECT @@IDENTITY ", con);

                cmd.Parameters.Add("@PN", SqlDbType.NVarChar).Value = p.Name;
                cmd.Parameters.Add("@ON", SqlDbType.NVarChar).Value = p.Owner;
                cmd.Parameters.Add("@MN", SqlDbType.NVarChar).Value = p.Manager;
                cmd.Parameters.Add("@DESC", SqlDbType.NVarChar).Value = p.Description;

                id = Convert.ToInt32(cmd.ExecuteScalar());

            }
            return id;
        }

        public static void Remove(Project p)
        {
            using (SqlConnection con = new SqlConnection(connectionString))

            {
                con.Open();
                SqlCommand cmd = new SqlCommand("DELETE FROM PROJECTS WHERE ProjectId=@ID", con);

                cmd.Parameters.Add("@ID", SqlDbType.NVarChar).Value = p.Id;

                cmd.ExecuteNonQuery();
            }
        }
    }
}
