using Civica.Models;

namespace UnitTest
{
    [TestClass]
    public class UnitTest1
    {
        

        [TestMethod]
        public void TestAddProjectToDatabase()
        {
            ProjectRepository projectRepo = new ProjectRepository();

            Project p = new Project("test", "123", "321", "Blah Blah Blah"); // ARRANGE

            projectRepo.Add(p); // ACT

            Assert.IsNotNull(projectRepo.Get(p.Id)); // ASSERT
        }
    }
}