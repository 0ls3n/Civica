using Civica.Models;
using Civica.Interfaces;

namespace UnitTest
{
    [TestClass]
    public class UnitTest1
    {
        IRepository<Project> projectRepo;
        IRepository<User> userRepo;

        Project p1;

        User u1;

        [TestInitialize]
        public void Initialize() // Arrange
        {
            projectRepo = new Repository<Project>(id => { return projectRepo.GetAll().FindAll(x => x.Id == id); });
            userRepo = new Repository<User>(id => { return userRepo.GetAll().FindAll(x => x.Id == id); });

            p1 = new Project(1, "test1", string.Empty, string.Empty, string.Empty, DateTime.Now);

            u1 = new User("Rasmus", "Olsen", 1709);
        }

        [TestMethod]
        public void TestCreateAndDeleteProjectFromDatabase()
        {
            projectRepo.Add(p1); // Act

            Assert.IsNotNull(projectRepo.GetAll().Find(x=> (x as Project).Name == "test1")); // Assert

            projectRepo.Remove(p1); // Act

            Assert.IsNull(projectRepo.GetAll().Find(x => (x as Project).Name == "test1")); // Assert
        }

        [TestMethod]
        public void TestUpdateProjectFromDatabase()
        {
            projectRepo.Add(p1); //Act

            Assert.IsNotNull(projectRepo.GetAll().Find(x => (x as Project).Name == "test1")); // Assert

            p1.Name = "Hejsa!"; // Arrange
            projectRepo.Update(p1); // Act

            Assert.IsNull(projectRepo.GetAll().Find(x => (x as Project).Name == "test1")); // Assert
            Assert.IsNotNull(projectRepo.GetAll().Find(x => (x as Project).Name == "Hejsa!")); // Assert

            projectRepo.Remove(p1);
        }

        [TestMethod]
        public void TestUpdateProjectFromDatabase2()
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
        public void TestCreateAndDeleteUserFromDatabase()
        {
            userRepo.Add(u1); // Act

            Assert.IsNotNull(userRepo.GetAll().Find(x => (x as User).FirstName == "Rasmus")); // Assert

            userRepo.Remove(u1); // Act

            Assert.IsNull(userRepo.GetAll().Find(x => (x as User).FirstName == "Rasmus")); // Assert
        }

        [TestMethod]
        public void TestUpdateUserFromDatabase()
        {
            userRepo.Add(u1); //Act

            Assert.IsNotNull(userRepo.GetAll().Find(x => (x as User).FirstName == "Rasmus")); // Assert

            u1.FirstName = "Hejsa!"; // Arrange
            userRepo.Update(u1); // Act

            Assert.IsNull(userRepo.GetAll().Find(x => (x as User).FirstName == "Rasmus")); // Assert
            Assert.IsNotNull(userRepo.GetAll().Find(x => (x as User).FirstName == "Hejsa!")); // Assert

            userRepo.Remove(u1);
        }

        [TestMethod]
        public void TestUpdateUserFromDatabase2()
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
    }
}