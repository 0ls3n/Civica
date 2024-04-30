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
        #region DB Connection
        private static IConfigurationRoot config = new ConfigurationBuilder().AddJsonFile("appsettings.json").Build();

        private static string? connectionString = config.GetConnectionString("MyDBConnection");
        #endregion
        #region Initialize
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
                SqlCommand progressCmd = new SqlCommand("SELECT ProgressId, Phase, Status, Date, Description, ProjectId FROM PROGRESSES", con);
                using (SqlDataReader reader = progressCmd.ExecuteReader())
                {
                    while (reader.Read())
                    {
                        int id = Convert.ToInt32(reader["ProgressId"]);
                        Phase phase = Enum.Parse<Phase>(Convert.ToString(reader["Phase"]));
                        Status status = Enum.Parse<Status>(Convert.ToString(reader["Status"]));
                        string desc = Convert.ToString(reader["Description"]);
                        int projectId = Convert.ToInt32(reader["ProjectId"]);
                        DateTime date = Convert.ToDateTime(reader["Date"]);

                        Progress prog = new Progress(projectId, phase, status, DateTime.Now, desc);

                        prog.Date = date;
                        prog.Id = id;

                        progresses.Add(prog);
                    }
                }
            }
            return progresses;
        }
        public static List<Resource> InitializeEconomy()
        {
            List<Resource> economies = new List<Resource>();

            using (SqlConnection con = new SqlConnection(connectionString))
            {
                con.Open();
                SqlCommand economyCmd = new SqlCommand("SELECT EconomyId, StartAmount, ExpectedYearlyCost, ProjectId FROM ECONOMIES", con);
                using (SqlDataReader reader = economyCmd.ExecuteReader())
                {
                    while (reader.Read())
                    {
                        int id = Convert.ToInt32(reader["EconomyId"]);
                        decimal startAmount = Convert.ToDecimal(reader["StartAmount"]);
                        decimal expectedYearlyCost = Convert.ToDecimal(reader["ExpectedYearlyCost"]);
                        int projectId = Convert.ToInt32(reader["ProjectId"]);

                        Resource econ = new Resource(projectId, startAmount, expectedYearlyCost);

                        econ.Id = id;

                        economies.Add(econ);
                    }
                }
            }
            return economies;
        }
        public static List<Audit> InitializeAudit()
        {
            List<Audit> audits = new List<Audit>();

            using (SqlConnection con = new SqlConnection(connectionString))
            {
                con.Open();
                SqlCommand auditCmd = new SqlCommand("SELECT AuditId, Amount, Year, EconomyId FROM AUDITS", con);
                using (SqlDataReader reader = auditCmd.ExecuteReader())
                {
                    while (reader.Read())
                    {
                        int id = Convert.ToInt32(reader["AuditId"]);
                        decimal Amount = Convert.ToDecimal(reader["Amount"]);
                        DateTime Year = Convert.ToDateTime(reader["Year"]);
                        int economyId = Convert.ToInt32(reader["EconomyId"]);

                        Audit aud = new Audit(Amount, Year);

                        aud.Id = id;
                        aud.ResourceId = economyId;
                        audits.Add(aud);
                    }
                }
            }
            return audits;
        }
        public static List<WorkTime> InitializeWorkTime()
        {
            List<WorkTime> workTimes = new List<WorkTime>();

            using (SqlConnection con = new SqlConnection(connectionString))
            {
                con.Open();
                SqlCommand workTimeCmd = new SqlCommand("SELECT WorkTimeId, Time, InvolvedName, ResourceId FROM WORKTIMES", con);
                using (SqlDataReader reader = workTimeCmd.ExecuteReader())
                {
                    while (reader.Read())
                    {
                        int id = Convert.ToInt32(reader["WorkTimeId"]);
                        double Time = Convert.ToDouble(reader["Time"]);
                        string InvolvedName = Convert.ToString(reader["InvolvedName"]);
                        int resourceId = Convert.ToInt32(reader["ResourceId"]);

                        WorkTime wt = new WorkTime(Time, InvolvedName);

                        wt.Id = id;
                        wt.ResourceId = resourceId;
                        workTimes.Add(wt);
                    }
                }
            }
            return workTimes;
        }
        #endregion
        #region Add
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
        public static int Add(Resource econ)
        {
            int id = -1;
            using (SqlConnection con = new SqlConnection(connectionString))
            {
                con.Open();
                SqlCommand cmd = new SqlCommand("INSERT INTO ECONOMIES (StartAmount, ExpectedYearlyCost, ProjectId)" +
                                                                     "VALUES (@SA, @EYC, @PID) SELECT @@IDENTITY ", con);

                cmd.Parameters.Add("@SA", SqlDbType.Decimal).Value = econ.StartAmount;
                cmd.Parameters.Add("@EYC", SqlDbType.Decimal).Value = econ.ExpectedYearlyCost;
                cmd.Parameters.Add("@PID", SqlDbType.Int).Value = econ.ProjectId;

                id = Convert.ToInt32(cmd.ExecuteScalar());
            }
            return id;
        }
        public static int Add(Audit aud)
        {
            int id = -1;
            using (SqlConnection con = new SqlConnection(connectionString))
            {
                con.Open();
                SqlCommand cmd = new SqlCommand("INSERT INTO AUDITS (Amount, Year, EconomyId)" +
                                                                     "VALUES (@Am, @Ye, @EID) SELECT @@IDENTITY ", con);

                cmd.Parameters.Add("@Am", SqlDbType.Decimal).Value = aud.Amount;
                cmd.Parameters.Add("@Ye", SqlDbType.DateTime2).Value = aud.Year;
                cmd.Parameters.Add("@Eid", SqlDbType.Int).Value = aud.ResourceId;

                id = Convert.ToInt32(cmd.ExecuteScalar());
            }
            return id;
        }
        #endregion
        #region Update
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
        public static void Update(Progress p)
        {
            using(SqlConnection con = new SqlConnection(connectionString))
            {
                con.Open();
                SqlCommand UpdateProgress = new SqlCommand("UPDATE PROGRESSES SET Phase = @PH, Status = @ST, Date = @DA, Description = @DESC" +
                                                           "WHERE ProgressId = @ID");
                UpdateProgress.Parameters.Add("@PH", SqlDbType.NVarChar).Value = p.Phase;
                UpdateProgress.Parameters.Add("@ST", SqlDbType.NVarChar).Value = p.Status;
                UpdateProgress.Parameters.Add("@DA", SqlDbType.DateTime2).Value = p.Date;
                UpdateProgress.Parameters.Add("@DESC", SqlDbType.Text).Value = p.Description;
                UpdateProgress.Parameters.Add("@ID", SqlDbType.Int).Value = p.Id;

                UpdateProgress.ExecuteNonQuery();
            }
        }
        public static void Update(Resource e)
        {
            using (SqlConnection con = new SqlConnection(connectionString))
            {
                con.Open();
                SqlCommand cmd = new SqlCommand("UPDATE ECONOMIES SET StartAmount = @SA, ExpectedYearlyCost = @EYC" +
                                                "WHERE EconomyId = @ID", con); // Opsætter parameterne der skal opdateres.

                // Indsætter opdaterede værdier i parameterne 
                cmd.Parameters.Add("@ID", SqlDbType.Int).Value = e.Id;
                cmd.Parameters.Add("@SA", SqlDbType.Decimal).Value = e.StartAmount;
                cmd.Parameters.Add("@EYC", SqlDbType.Decimal).Value = e.ExpectedYearlyCost;


                // Exe
                cmd.ExecuteNonQuery();
            }

        }
        public static void Update(Audit a)
        {
            using (SqlConnection con = new SqlConnection(connectionString))
            {
                con.Open();
                SqlCommand cmd = new SqlCommand("UPDATE AUDITS SET Amount = @Am, Year = @Ye" +
                                                "WHERE AuditId = @ID", con); // Opsætter parameterne der skal opdateres.

                // Indsætter opdaterede værdier i parameterne 
                cmd.Parameters.Add("@ID", SqlDbType.Int).Value = a.Id;
                cmd.Parameters.Add("@Am", SqlDbType.Decimal).Value = a.Amount;
                cmd.Parameters.Add("@Ye", SqlDbType.DateTime2).Value = a.Year;


                // Exe
                cmd.ExecuteNonQuery();
            }

        }
        #endregion
        #region Remove
        public static void Remove(Project p)
        {
            using (SqlConnection con = new SqlConnection(connectionString))
            {
                con.Open();
                SqlCommand RemoveProgress = new SqlCommand("DELETE FROM PROGRESSES WHERE ProjectId=@ID", con);
                SqlCommand RemoveEconomy = new SqlCommand("DELETE FROM ECONOMIES WHERE ProjectId=@ID", con);
                SqlCommand RemoveProject = new SqlCommand("DELETE FROM PROJECTS WHERE ProjectId=@ID", con);

                RemoveProgress.Parameters.Add("@ID", SqlDbType.Int).Value = p.Id;
                RemoveEconomy.Parameters.Add("@ID", SqlDbType.Int).Value = p.Id;
                RemoveProject.Parameters.Add("@ID", SqlDbType.Int).Value = p.Id;

                RemoveProgress.ExecuteNonQuery();
                RemoveEconomy.ExecuteNonQuery();
                RemoveProject.ExecuteNonQuery();
            }
        }
        public static void Remove(Progress p)
        {
            using (SqlConnection con = new SqlConnection(connectionString))
            {
                con.Open();
                SqlCommand RemoveProgress = new SqlCommand("DELETE FROM PROGRESSES WHERE ProgressId = @ID", con);
                RemoveProgress.Parameters.Add("@ID", SqlDbType.Int).Value = p.Id;
                RemoveProgress.ExecuteNonQuery();
            }
        }
        public static void Remove(Resource e)
        {

            using (SqlConnection con = new SqlConnection(connectionString))

            {
                con.Open();
                SqlCommand RemoveAudits = new SqlCommand("DELETE FROM AUDITS WHERE EconomyId=@ID", con);
                SqlCommand RemoveEconomy = new SqlCommand("DELETE FROM ECONOMIES WHERE EconomyId=@ID", con);
                RemoveAudits.Parameters.Add("@ID", SqlDbType.Int).Value = e.Id;
                RemoveEconomy.Parameters.Add("@ID", SqlDbType.Int).Value = e.Id;
                RemoveAudits.ExecuteNonQuery();
                RemoveEconomy.ExecuteNonQuery();

            }
        }
        public static void Remove(Audit a)
        {
            using (SqlConnection con = new SqlConnection(connectionString))

            {
                con.Open();
                SqlCommand cmd = new SqlCommand("DELETE FROM AUDITS WHERE AuditId=@ID", con);

                cmd.Parameters.Add("@ID", SqlDbType.Int).Value = a.Id;

                cmd.ExecuteNonQuery();
            }
        }
        #endregion
    }
}
