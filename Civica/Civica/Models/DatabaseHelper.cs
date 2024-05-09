using System;
using System.Collections.Generic;
using System.Data;
using System.Diagnostics.Eventing.Reader;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using Civica.Models.Enums;
using Microsoft.Data.SqlClient;
using Microsoft.Extensions.Configuration;
using Microsoft.VisualBasic.ApplicationServices;
using Microsoft.VisualBasic.Devices;


namespace Civica.Models
{
    public static class DatabaseHelper<T>
    {
        #region DB Connection
        private static IConfigurationRoot config = new ConfigurationBuilder().AddJsonFile("appsettings.json").Build();

        private static string? connectionString = config.GetConnectionString("MyDBConnection");
        #endregion
        #region Initialize
        public static List<DomainModel> Initialize(Type type)
        {
            List<DomainModel> list = new List<DomainModel>();

            using (SqlConnection con = new SqlConnection(connectionString))
            {
                con.Open();

                if (type == typeof(Project))
                {
                    SqlCommand projectCmd = new SqlCommand("SELECT UserId, ProjectId, ProjectName, OwnerName, ManagerName, Description, CreatedDate FROM PROJECTS", con);
                    using (SqlDataReader reader = projectCmd.ExecuteReader())
                    {
                        while (reader.Read())
                        {
                            int userId = reader["UserId"] != DBNull.Value ? Convert.ToInt32(reader["UserId"]) : 0;
                            DateTime createdDate = reader["CreatedDate"] != DBNull.Value ? Convert.ToDateTime(reader["CreatedDate"]) : new DateTime(default);
                            int id = Convert.ToInt32(reader["ProjectId"]);
                            string name = Convert.ToString(reader["ProjectName"]);
                            string owner = Convert.ToString(reader["OwnerName"]);
                            string manager = Convert.ToString(reader["ManagerName"]);
                            string desc = Convert.ToString(reader["Description"]);

                            Project p = new Project(userId, name, owner, manager, desc, createdDate);

                            p.Id = id;

                            list.Add(p);
                        }
                    }
                }
                else if (type == typeof(Progress))
                {
                    SqlCommand progressCmd = new SqlCommand("SELECT UserId, ProgressId, Phase, Status, CreatedDate, Description, ProjectId FROM PROGRESSES", con);
                    using (SqlDataReader reader = progressCmd.ExecuteReader())
                    {
                        while (reader.Read())
                        {
                            int userId = reader["UserId"] != DBNull.Value ? Convert.ToInt32(reader["UserId"]) : 0;
                            DateTime createdDate = reader["CreatedDate"] != DBNull.Value ? Convert.ToDateTime(reader["CreatedDate"]) : new DateTime(0, 0, 0);
                            int id = Convert.ToInt32(reader["ProgressId"]);
                            Phase phase = Enum.Parse<Phase>(Convert.ToString(reader["Phase"]));
                            Status status = Enum.Parse<Status>(Convert.ToString(reader["Status"]));
                            string desc = Convert.ToString(reader["Description"]);
                            int projectId = Convert.ToInt32(reader["ProjectId"]);

                            Progress prog = new Progress(userId, projectId, phase, status, desc, createdDate);

                            prog.Id = id;

                            list.Add(prog);
                        }
                    }
                }
                else if (type == typeof(Resource))
                {
                    SqlCommand resourceCmd = new SqlCommand("SELECT UserId, ResourceId, StartAmount, ExpectedYearlyCost, ProjectId, CreatedDate FROM RESOURCES", con);
                    using (SqlDataReader reader = resourceCmd.ExecuteReader())
                    {
                        while (reader.Read())
                        {
                            int userId = reader["UserId"] != DBNull.Value ? Convert.ToInt32(reader["UserId"]) : 0;
                            DateTime createdDate = reader["CreatedDate"] != DBNull.Value ? Convert.ToDateTime(reader["CreatedDate"]) : new DateTime();
                            int id = Convert.ToInt32(reader["ResourceId"]);
                            decimal startAmount = Convert.ToDecimal(reader["StartAmount"]);
                            decimal expectedYearlyCost = Convert.ToDecimal(reader["ExpectedYearlyCost"]);
                            int projectId = Convert.ToInt32(reader["ProjectId"]);

                            Resource r = new Resource(userId, projectId, startAmount, expectedYearlyCost, createdDate);

                            r.Id = id;

                            list.Add(r);
                        }
                    }
                }
                else if (type == typeof(Audit))
                {
                    SqlCommand auditCmd = new SqlCommand("SELECT UserId, AuditId, Amount, Year, ResourceId, ADescription, CreatedDate FROM AUDITS", con);
                    using (SqlDataReader reader = auditCmd.ExecuteReader())
                    {
                        while (reader.Read())
                        {
                            int userId = reader["UserId"] != DBNull.Value ? Convert.ToInt32(reader["UserId"]) : 0;
                            DateTime createdDate = reader["CreatedDate"] != DBNull.Value ? Convert.ToDateTime(reader["CreatedDate"]) : DateTime.MinValue;
                            int id = Convert.ToInt32(reader["AuditId"]);
                            decimal Amount = Convert.ToDecimal(reader["Amount"]);
                            int Year = Convert.ToInt32(reader["Year"]);
                            string desc = Convert.ToString(reader["ADescription"]);
                            int ResourceId = Convert.ToInt32(reader["ResourceId"]);

                            Audit a = new Audit(userId, ResourceId, Amount, Year, createdDate);

                            a.Id = id;
                            list.Add(a);
                        }
                    }
                }
                else if (type == typeof(WorkTime))
                {
                    SqlCommand workTimeCmd = new SqlCommand("SELECT UserId, WorkTimeId, Time, InvolvedName, ResourceId, CreatedDate FROM WORKTIMES", con);
                    using (SqlDataReader reader = workTimeCmd.ExecuteReader())
                    {
                        while (reader.Read())
                        {
                            int userId = reader["UserId"] != DBNull.Value ? Convert.ToInt32(reader["UserId"]) : 0;
                            DateTime createdDate = reader["CreatedDate"] != DBNull.Value ? Convert.ToDateTime(reader["CreatedDate"]) : new DateTime(0, 0, 0);
                            int id = Convert.ToInt32(reader["WorkTimeId"]);
                            double Time = Convert.ToDouble(reader["Time"]);
                            string InvolvedName = Convert.ToString(reader["InvolvedName"]);
                            int resourceId = Convert.ToInt32(reader["ResourceId"]);

                            WorkTime wt = new WorkTime(userId, resourceId, Time, InvolvedName, createdDate);

                            wt.Id = id;
                            list.Add(wt);
                        }
                    }
                }
                else if (type == typeof(User))
                {
                    SqlCommand userCmd = new SqlCommand("SELECT UserId, FirstName, LastName, Password FROM USERS", con);
                    using (SqlDataReader reader = userCmd.ExecuteReader())
                    {
                        while(reader.Read())
                        {
                            int id = Convert.ToInt32(reader["UserId"]);
                            string firstName = Convert.ToString(reader["FirstName"]);
                            string lastName = Convert.ToString(reader["LastName"]);
                            int password = Convert.ToInt32(reader["Password"]);
                            User u = new User(firstName, lastName, password);

                            u.Id = id;
                            list.Add(u);
                        }
                    }
                }
                else
                {
                    throw new ArgumentNullException($"{type} er ikke implementeret i DatabaseHelper!");
                }
            }
            return list;
        }
        #endregion
        #region Add

        public static void Add(T o)
        {
            using (SqlConnection con = new SqlConnection(connectionString))
            {
                con.Open();
                DomainModel d = null;
                SqlCommand cmd = null;
                if (o is Project p)
                {
                    cmd = new SqlCommand("INSERT INTO PROJECTS (UserId, ProjectName, OwnerName, ManagerName, Description, CreatedDate)" +
                                                                         "VALUES (@UID, @PN, @ON, @MN, @DESC, @CD) SELECT @@IDENTITY ", con);
                    cmd.Parameters.Add("@UID", SqlDbType.Int).Value = p.UserId;
                    cmd.Parameters.Add("@CD", SqlDbType.DateTime2).Value = p.CreatedDate;
                    cmd.Parameters.Add("@PN", SqlDbType.NVarChar).Value = p.Name;
                    cmd.Parameters.Add("@ON", SqlDbType.NVarChar).Value = p.Owner;
                    cmd.Parameters.Add("@MN", SqlDbType.NVarChar).Value = p.Manager;
                    cmd.Parameters.Add("@DESC", SqlDbType.Text).Value = p.Description;
                    

                    d = p;
                }
                else if (o is Progress prog)
                {
                    cmd = new SqlCommand("INSERT INTO PROGRESSES (UserId, Phase, Status, Description, ProjectId, CreatedDate)" +
                                                                         "VALUES (@UID, @PH, @ST, @DESC, @PID, @CD) SELECT @@IDENTITY ", con);
                    cmd.Parameters.Add("@UID", SqlDbType.Int).Value = prog.UserId;
                    cmd.Parameters.Add("@CD", SqlDbType.DateTime2).Value = prog.CreatedDate;
                    cmd.Parameters.Add("@PH", SqlDbType.NVarChar).Value = prog.Phase;
                    cmd.Parameters.Add("@ST", SqlDbType.NVarChar).Value = prog.Status;
                    cmd.Parameters.Add("@DESC", SqlDbType.Text).Value = prog.Description;
                    cmd.Parameters.Add("@PID", SqlDbType.Int).Value = prog.RefId;

                    d = prog;
                }
                else if (o is Resource r)
                {
                    cmd = new SqlCommand("INSERT INTO RESOURCES (UserId, StartAmount, ExpectedYearlyCost, ProjectId, CreatedDate)" +
                                                                         "VALUES (@UID, @SA, @EYC, @PID, @CD) SELECT @@IDENTITY ", con);
                    cmd.Parameters.Add("@UID", SqlDbType.Int).Value = r.UserId;
                    cmd.Parameters.Add("@CD", SqlDbType.DateTime2).Value = r.CreatedDate;
                    cmd.Parameters.Add("@SA", SqlDbType.Decimal).Value = r.StartAmount;
                    cmd.Parameters.Add("@EYC", SqlDbType.Decimal).Value = r.ExpectedYearlyCost;
                    cmd.Parameters.Add("@PID", SqlDbType.Int).Value = r.RefId;

                    d = r;
                }
                else if (o is Audit a)
                {
                    cmd = new SqlCommand("INSERT INTO AUDITS (UserId, Amount, Year, ResourceId, ADescription, CreatedDate)" +
                                                                         "VALUES (@UID, @AM, @YE, @RID, @AD, @CD) SELECT @@IDENTITY ", con);
                    cmd.Parameters.Add("@UID", SqlDbType.Int).Value = a.UserId;
                    cmd.Parameters.Add("@CD", SqlDbType.DateTime2).Value = a.CreatedDate;
                    cmd.Parameters.Add("@AM", SqlDbType.Decimal).Value = a.Amount;
                    cmd.Parameters.Add("@AD", SqlDbType.NVarChar).Value = a.Description;
                    cmd.Parameters.Add("@YE", SqlDbType.Int).Value = a.Year;
                    cmd.Parameters.Add("@RID", SqlDbType.Int).Value = a.RefId;

                    d = a;
                }
                else if(o is WorkTime w)
                    {
                    cmd = new SqlCommand("INSERT INTO WORKTIMES (UserId, Time, InvolvedName, ResourceId, CreatedDate)" +
                                                                       "VALUES (@UID, @TI, @IN, @RID, @CD) SELECT @@IDENTITY ", con);
                    cmd.Parameters.Add("@UID", SqlDbType.Int).Value = w.UserId;
                    cmd.Parameters.Add("@CD", SqlDbType.DateTime2).Value = w.CreatedDate;
                    cmd.Parameters.Add("@TI", SqlDbType.Decimal).Value = w.Time;
                    cmd.Parameters.Add("@IN", SqlDbType.NVarChar).Value = w.InvolvedName;
                    cmd.Parameters.Add("@RID", SqlDbType.Int).Value = w.RefId;

                    d = w;
                }
                else if (o is User u)
                {
                    cmd = new SqlCommand("INSERT INTO USERS (FirstName, LastName, Password)" + 
                                         "VALUES (@FN, @LN, @PW) SELECT @@IDENTITY", con);
                    cmd.Parameters.Add("@FN", SqlDbType.NVarChar).Value = u.FirstName;
                    cmd.Parameters.Add("@LN", SqlDbType.NVarChar).Value = u.LastName;
                    cmd.Parameters.Add("@PW", SqlDbType.Int).Value = u.Password;
                    d = u;

                }
                else
                {
                    throw new ArgumentNullException($"{o} er ikke implementeret i DatabaseHelper!");
                }

                d.Id = Convert.ToInt32(cmd.ExecuteScalar());
            }
        }
        #endregion
        #region Update
        public static void Update(T o)
        {
            using (SqlConnection con = new SqlConnection(connectionString))
            {
                con.Open();
                SqlCommand cmd = null;
                if (o is Project p)
                {
                    cmd = new SqlCommand("UPDATE PROJECTS SET ProjectName = @PN, OwnerName = @ON, ManagerName = @MN, Description = @DESC " +
                                          "WHERE ProjectId = @ID", con); 

                    
                    cmd.Parameters.Add("@ID", SqlDbType.Int).Value = p.Id;
                    cmd.Parameters.Add("@PN", SqlDbType.NVarChar).Value = p.Name;
                    cmd.Parameters.Add("@ON", SqlDbType.NVarChar).Value = p.Owner;
                    cmd.Parameters.Add("@MN", SqlDbType.NVarChar).Value = p.Manager;
                    cmd.Parameters.Add("@DESC", SqlDbType.Text).Value = p.Description;
                }
                else if (o is Progress prog)
                {
                    cmd = new SqlCommand("UPDATE PROGRESSES SET Phase = @PH, Status = @ST, CreatedDate = @DA, Description = @DESC" +
                                                       "WHERE ProgressId = @ID");
                    cmd.Parameters.Add("@PH", SqlDbType.NVarChar).Value = prog.Phase;
                    cmd.Parameters.Add("@ST", SqlDbType.NVarChar).Value = prog.Status;
                    cmd.Parameters.Add("@DA", SqlDbType.DateTime2).Value = prog.CreatedDate;
                    cmd.Parameters.Add("@DESC", SqlDbType.Text).Value = prog.Description;
                    cmd.Parameters.Add("@ID", SqlDbType.Int).Value = prog.Id;
                }
                else if (o is Resource r)
                {
                    cmd = new SqlCommand("UPDATE RESOURCES SET StartAmount = @SA, ExpectedYearlyCost = @EYC " +
                                            "WHERE ResourceId = @ID", con); // Opsætter parameterne der skal opdateres.

                    // Indsætter opdaterede værdier i parameterne 
                    cmd.Parameters.Add("@ID", SqlDbType.Int).Value = r.Id;
                    cmd.Parameters.Add("@SA", SqlDbType.Decimal).Value = r.StartAmount;
                    cmd.Parameters.Add("@EYC", SqlDbType.Decimal).Value = r.ExpectedYearlyCost;
                }
                else if (o is Audit a)
                {
                    cmd = new SqlCommand("UPDATE AUDITS SET Amount = @Am, Year = @Ye, ADescription = @AD " +
                                          "WHERE AuditId = @ID", con); // Opsætter parameterne der skal opdateres.

                    // Indsætter opdaterede værdier i parameterne 
                    cmd.Parameters.Add("@ID", SqlDbType.Int).Value = a.Id;
                    cmd.Parameters.Add("@Am", SqlDbType.Decimal).Value = a.Amount;
                    cmd.Parameters.Add("@AD", SqlDbType.NVarChar).Value = a.Description;
                    cmd.Parameters.Add("@Ye", SqlDbType.Int).Value = a.Year;
                }
                else if (o is WorkTime w)
                {
                    cmd = new SqlCommand("UPDATE WORKTIMES SET Time = @Ti, InvolvedName = @IN" +
                                        "WHERE WorkTimeId = @ID", con); // Opsætter parameterne der skal opdateres.

                    // Indsætter opdaterede værdier i parameterne 
                    cmd.Parameters.Add("@ID", SqlDbType.Int).Value = w.Id;
                    cmd.Parameters.Add("@Ti", SqlDbType.Decimal).Value = w.Time;
                    cmd.Parameters.Add("@In", SqlDbType.NVarChar).Value = w.InvolvedName;
                }
                else if (o is User u) 
                {
                    cmd = new SqlCommand("UPDATE USERS SET FirstName = @FN, LastName = @LN, Password = @PW WHERE UserId = @ID", con);
                    cmd.Parameters.Add("@ID", SqlDbType.Int).Value= u.Id;
                    cmd.Parameters.Add("@FN", SqlDbType.NVarChar).Value = u.FirstName;
                    cmd.Parameters.Add("@LN", SqlDbType.NVarChar).Value = u.LastName;
                    cmd.Parameters.Add("@PW", SqlDbType.Int).Value = u.Password;
                }

                else
                {
                    throw new ArgumentNullException($"{o} er ikke implementeret i DatabaseHelper!");
                }

                cmd.ExecuteNonQuery();
            }
        }
        #endregion
        #region Remove
        public static void Remove(T o)
        {
            using (SqlConnection con = new SqlConnection(connectionString))
            {
                con.Open();
                if (o is Project p)
                {
                    SqlCommand GetResourceId = new SqlCommand("SELECT ResourceId FROM RESOURCES WHERE ProjectId=@ID", con);

                    SqlCommand RemoveProgress = new SqlCommand("DELETE FROM PROGRESSES WHERE ProjectId=@ID", con);
                    SqlCommand RemoveAudit = new SqlCommand("DELETE FROM AUDITS WHERE ResourceId=@ID", con);
                    SqlCommand RemoveWorktime = new SqlCommand("DELETE FROM WORKTIMES WHERE ResourceId=@ID", con);
                    SqlCommand RemoveResource = new SqlCommand("DELETE FROM RESOURCES WHERE ProjectId=@ID", con);
                    SqlCommand RemoveProject = new SqlCommand("DELETE FROM PROJECTS WHERE ProjectId=@ID", con);

                    GetResourceId.Parameters.Add("@ID", SqlDbType.Int).Value = p.Id;

                    RemoveProgress.Parameters.Add("@ID", SqlDbType.Int).Value = p.Id;
                    RemoveAudit.Parameters.Add("@ID", SqlDbType.Int).Value = Convert.ToInt32(GetResourceId.ExecuteScalar());
                    RemoveWorktime.Parameters.Add("@ID", SqlDbType.Int).Value = p.RefId;
                    RemoveResource.Parameters.Add("@ID", SqlDbType.Int).Value = p.Id;
                    RemoveProject.Parameters.Add("@ID", SqlDbType.Int).Value = p.Id;

                    RemoveProgress.ExecuteNonQuery();
                    RemoveAudit.ExecuteNonQuery();
                    RemoveWorktime.ExecuteNonQuery();
                    RemoveResource.ExecuteNonQuery();
                    RemoveProject.ExecuteNonQuery();
                }
                else if (o is Progress prog)
                {
                    SqlCommand RemoveProgress = new SqlCommand("DELETE FROM PROGRESSES WHERE ProgressId = @ID", con);
                    RemoveProgress.Parameters.Add("@ID", SqlDbType.Int).Value = prog.Id;
                    RemoveProgress.ExecuteNonQuery();
                }
                else if (o is Resource r)
                {
                    SqlCommand RemoveAudits = new SqlCommand("DELETE FROM AUDITS WHERE ResourceId=@ID", con);
                    SqlCommand RemoveWorktimes = new SqlCommand("DELETE FROM WORKTIMES WHERE ResourceId=@ID", con);
                    SqlCommand RemoveResource = new SqlCommand("DELETE FROM RESOURCES WHERE ResourceId=@ID", con);
                    RemoveAudits.Parameters.Add("@ID", SqlDbType.Int).Value = r.Id;
                    RemoveWorktimes.Parameters.Add("@ID", SqlDbType.Int).Value = r.Id;
                    RemoveResource.Parameters.Add("@ID", SqlDbType.Int).Value = r.Id;
                    RemoveAudits.ExecuteNonQuery();
                    RemoveWorktimes.ExecuteNonQuery();
                    RemoveResource.ExecuteNonQuery();
                }
                else if (o is Audit a)
                {
                    SqlCommand cmd = new SqlCommand("DELETE FROM AUDITS WHERE AuditId=@ID", con);
                    cmd.Parameters.Add("@ID", SqlDbType.Int).Value = a.Id;
                    cmd.ExecuteNonQuery();
                }
                else if (o is WorkTime w)
                {
                    SqlCommand cmd = new SqlCommand("DELETE FROM WORKTIMES WHERE WorkTimeId=@ID", con);
                    cmd.Parameters.Add("@ID", SqlDbType.Int).Value = w.Id;
                    cmd.ExecuteNonQuery();
                }
                else if (o is User u) 
                {
                    SqlCommand cmd = new SqlCommand("DELETE FROM USERS WHERE UserId=@ID", con);
                    cmd.Parameters.Add("@ID", SqlDbType.Int).Value = u.Id;
                    cmd.ExecuteNonQuery();
                }
                else
                {
                    throw new ArgumentNullException($"{o} er ikke implementeret i DatabaseHelper!");
                }
            }
        }
        #endregion
    }
}
