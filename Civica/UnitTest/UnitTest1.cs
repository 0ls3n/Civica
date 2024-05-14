using Civica.Models;
using Civica.Interfaces;
using Microsoft.Identity.Client;

namespace UnitTest
{
    [TestClass]
    public class UnitTest1
    {
        IRepository<Project> projectRepo;
        IRepository<User> userRepo;
        IRepository<Progress> progressRepo;
        IRepository<Resource> resourceRepo;
        IRepository<Audit> auditRepo;
        IRepository<Worktime> worktimeRepo;

        Project p1;
        Progress prog;
        Resource resource;
        Audit audit;
        User u1;

        [TestInitialize]
        public void Initialize() // Arrange
        {
            projectRepo = new Repository<Project>
            (
                id =>
                {
                    return projectRepo.GetAll().FindAll(x => x.Id == id);
                },
                id =>
                {
                    return projectRepo.GetAll().FindAll(x => x.UserId == id);
                }
            );

            userRepo = new Repository<User>
            (
                id =>
                {
                    return userRepo.GetAll().FindAll(x => x.Id == id);
                },
                id =>
                {
                    return userRepo.GetAll().FindAll(x => x.UserId == id);
                },
                id =>
                {
                    return userRepo.GetAll().FindAll(x => x.RefId == id);
                }
            );

            progressRepo = new Repository<Progress>
            (
                id =>
                {
                    return progressRepo.GetAll().FindAll(x => x.Id == id);
                },
                id =>
                {
                    return progressRepo.GetAll().FindAll(x => x.UserId == id);
                },
                id =>
                {
                    return progressRepo.GetAll().FindAll(x => x.RefId == id);
                }
            );

            resourceRepo = new Repository<Resource>
            (
                id =>
                {
                    return resourceRepo.GetAll().FindAll(x => x.Id == id);
                },
                id =>
                {
                    return resourceRepo.GetAll().FindAll(x => x.UserId == id);
                },
                id =>
                {
                    return resourceRepo.GetAll().FindAll(x => x.RefId == id);
                }
            );

            auditRepo = new Repository<Audit>
            (
                id =>
                {
                    return auditRepo.GetAll().FindAll(x => x.Id == id);
                },
                id =>
                {
                    return auditRepo.GetAll().FindAll(x => x.UserId == id);
                },
                id =>
                {
                    return auditRepo.GetAll().FindAll(x => x.RefId == id);
                }
            );

            worktimeRepo = new Repository<Worktime>
            (
                id =>
                {
                    return worktimeRepo.GetAll().FindAll(x => x.Id == id);
                },
                id =>
                {
                    return worktimeRepo.GetAll().FindAll(x => x.UserId == id);
                },
                id =>
                {
                    return worktimeRepo.GetAll().FindAll(x => x.RefId == id);
                }
            );

            p1 = new Project(1, "test1", string.Empty, string.Empty, string.Empty, DateTime.Now);

            u1 = new User("TestBruger", "Olsen", 1709);
        }

        [TestMethod]
        public void Add_SingleProject_AddedToDatabase()
        {
            projectRepo.Add(p1); // Act

            Assert.IsNotNull(projectRepo.GetAll().Find(x => (x as Project).Name == "test1")); // Assert

            projectRepo.Remove(p1);
        }

        [TestMethod]
        public void Remove_SingleProject_RemoveFromDatabase()
        {
            projectRepo.Add(p1); // Act
            projectRepo.Remove(p1); // Act

            Assert.IsNull(projectRepo.GetAll().Find(x => (x as Project).Name == "test1")); // Assert
        }

        [TestMethod]
        public void Update_SingleProject_UpdatedToDatabase()
        {
            projectRepo.Add(p1); //Act

            p1.Name = "Hejsa!"; // Arrange
            projectRepo.Update(p1); // Act

            Assert.IsNull(projectRepo.GetAll().Find(x => (x as Project).Name == "test1")); // Assert
            Assert.IsNotNull(projectRepo.GetAll().Find(x => (x as Project).Name == "Hejsa!")); // Assert

            projectRepo.Remove(p1);
        }

        [TestMethod]
        public void Update_SingleProject_UpdatedToDatabase_2()
        {
            projectRepo.Add(p1); // Act

            p1.Name = "What!"; // Arrange
            p1.Owner = "Simon Hansen"; // Arrange
            p1.CreatedDate = new DateTime(2003, 05, 20); // Arrange
            projectRepo.Update(p1); // Act

            Project project = (Project)projectRepo.GetAll().Find(x => (x as Project).Name == "What!"); // Arrange

            Assert.AreEqual("What!", project.Name); // Assert
            Assert.AreEqual("Simon Hansen", project.Owner); // Assert

            projectRepo.Remove(p1);
        }

        [TestMethod]
        public void Add_SingleUser_AddedToDatabase()
        {
            userRepo.Add(u1); // Act

            Assert.IsNotNull(userRepo.GetAll().Find(x => (x as User).FirstName == "TestBruger")); // Assert

            userRepo.Remove(u1);
        }

        [TestMethod]
        public void Remove_SingleUser_RemovedFromDatabase()
        {
            userRepo.Add(u1);

            userRepo.Remove(u1); // Act

            Assert.IsNull(userRepo.GetAll().Find(x => (x as User).FirstName == "TestBruger")); // Assert
        }

        [TestMethod]
        public void Update_SingleUser_UpdatedToDatabase()
        {
            userRepo.Add(u1); //Act

            Assert.IsNotNull(userRepo.GetAll().Find(x => (x as User).FirstName == "TestBruger")); // Assert

            u1.FirstName = "Hejsa!"; // Arrange
            userRepo.Update(u1); // Act

            Assert.IsNull(userRepo.GetAll().Find(x => (x as User).FirstName == "TestBruger")); // Assert
            Assert.IsNotNull(userRepo.GetAll().Find(x => (x as User).FirstName == "Hejsa!")); // Assert

            userRepo.Remove(u1);
        }

        [TestMethod]
        public void Update_SingleUser_UpdatedToDatabase_2()
        {
            userRepo.Add(u1); // Act

            u1.FirstName = "What!"; // Arrange
            u1.LastName = "Is"; // Arrange
            u1.Password = 0000; // Arrange
            userRepo.Update(u1); // Act

            User user = (User)userRepo.GetAll().Find(x => (x as User).FirstName == "What!"); // Arrange

            Assert.AreEqual("What!", user.FirstName); // Assert
            Assert.AreEqual("Is", user.LastName); // Assert
            Assert.AreEqual(0000, user.Password); // Assert

            userRepo.Remove(u1);
        }

        [TestMethod]
        public void GetByRefId_SingleProgress_ProgressFoundThroughProjectReference()
        {
            projectRepo.Add(p1); // Act

            prog = new Progress(1, p1.Id, Civica.Models.Enums.Phase.IDENTIFIED, Civica.Models.Enums.Status.ON_TRACK, "jeg er en progress", DateTime.Now);

            progressRepo.Add(prog); // Act

            Assert.IsNotNull(progressRepo.GetByRefId(p1.Id).Find(x => x.Id == prog.Id)); // This finds a progress thru the project id

            projectRepo.Remove(p1);
            progressRepo.Remove(prog);

        }

        [TestMethod]
        public void Remove_ProjectAndAllDomainsWithReferenceToProject_RemovedFromDatabase()
        {
            projectRepo.Add(p1);

            prog = new Progress(1, p1.Id, Civica.Models.Enums.Phase.IDENTIFIED, Civica.Models.Enums.Status.ON_TRACK, "jeg er en progress", DateTime.Now);
            progressRepo.Add(prog);

            resource = new Resource(1, p1.Id, DateTime.Now);
            resourceRepo.Add(resource);

            audit = new Audit(1, resource.Id, 0, 2003, "hej", DateTime.Now);
            auditRepo.Add(audit);

            projectRepo.Remove(p1);
            progressRepo.Remove(prog);
            resourceRepo.Remove(resource);
            auditRepo.Remove(audit);

            Assert.IsNull(projectRepo.GetAll().Find(x => (x as Project).Name == "test1"));
            Assert.IsNull(progressRepo.GetByRefId(p1.Id).Find(x => x.Id == prog.Id));
            Assert.IsNull(resourceRepo.GetByRefId(p1.Id).Find(x => x.Id == resource.Id));
            Assert.IsNull(auditRepo.GetByRefId(resource.Id).Find(x => x.Id == audit.Id));

        }
    }
}