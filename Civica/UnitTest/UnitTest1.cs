using Civica.Models;

namespace UnitTest
{
    [TestClass]
    public class UnitTest1
    {
        [TestMethod]
        public void TestAddProject()
        {
            //ARRANGE
            List<Project> projects = DatabaseHelper.InitializeProjects();
            Project p = new Project("Test", "Jens", "Rasmus", "Dette er en test");

            //ACT


            //ASSERT

        }
    }
}