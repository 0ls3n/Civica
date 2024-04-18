using System;
using System.Collections.Generic;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Civica.Models.Enums;
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
                SqlCommand projectCmd = new SqlCommand("SELECT ProjectId, ProjectName, OwnerName, ManagerName, Description FROM PROJECTS", con);
                using (SqlDataReader reader = projectCmd.ExecuteReader())
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

        public static List<Progress> InitializeProgress()
        {
            List<Progress> progresses = new List<Progress>();

            using (SqlConnection con = new SqlConnection(connectionString))
            {
                con.Open();
                SqlCommand progressCmd = new SqlCommand("SELECT ProgressId, Phase, Status, Description, ProjectId FROM PROGRESSES", con);
                using (SqlDataReader reader = progressCmd.ExecuteReader())
                {
                    while (reader.Read())
                    {
                        int id = Convert.ToInt32(reader["ProgressId"]);
                        Phase phase = Enum.Parse<Phase>(Convert.ToString(reader["Phase"]));
                        Status status = Enum.Parse<Status>(Convert.ToString(reader["Status"]));
                        string desc = Convert.ToString(reader["Description"]);
                        int projectId = Convert.ToInt32(reader["ProjectId"]);

                        Progress prog = new Progress(projectId, phase, status, DateTime.Now, desc);

                        prog.Id = id;

                        progresses.Add(prog);
                    }
                }
            }
            return progresses;
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
                cmd.Parameters.Add("@DESC", SqlDbType.Text).Value = p.Description;

                id = Convert.ToInt32(cmd.ExecuteScalar());
            }
            return id;
        }

        public static int Add(Progress prog)
        {
            int id = -1;
            using (SqlConnection con = new SqlConnection(connectionString))
            {
                con.Open();
                SqlCommand cmd = new SqlCommand("INSERT INTO PROGRESSES (Phase, Status, Date, Description, ProjectId)" +
                                                                     "VALUES (@PH, @ST, @DA, @DESC, @PID) SELECT @@IDENTITY ", con);

                cmd.Parameters.Add("@PH", SqlDbType.NVarChar).Value = prog.Phase;
                cmd.Parameters.Add("@ST", SqlDbType.NVarChar).Value = prog.Status;
                cmd.Parameters.Add("@DA", SqlDbType.NVarChar).Value = prog.Date;
                cmd.Parameters.Add("@DESC", SqlDbType.Text).Value = prog.Description;
                cmd.Parameters.Add("@PID", SqlDbType.Int).Value = prog.ProjectId;

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

        public static void Update(Project p)
        {
            using (SqlConnection con = new SqlConnection(connectionString))
            {
                con.Open();
                SqlCommand cmd = new SqlCommand("UPDATE PROJECTS SET ProjectName = @PN, OwnerName = @ON, ManagerName = @MN, Description = @DESC " +
                                                "WHERE ProjectId = @ID", con); // Opsætter parameterne der skal opdateres.

                // Indsætter opdaterede værdier i parameterne 
                cmd.Parameters.Add("@ID", SqlDbType.Int).Value = p.Id;
                cmd.Parameters.Add("@PN", SqlDbType.NVarChar).Value = p.Name;
                cmd.Parameters.Add("@ON", SqlDbType.NVarChar).Value = p.Owner;
                cmd.Parameters.Add("@MN", SqlDbType.NVarChar).Value = p.Manager;
                cmd.Parameters.Add("@DESC", SqlDbType.Text).Value = p.Description;

                // Exe
                cmd.ExecuteNonQuery();
            }

        }
    }
}
