using Employee_minimalAPI.BusinessLayer;
using Employee_minimalAPI.DataAccessLayer;
using Employee_minimalAPI.Model;
using NSubstitute;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace NUnit_Testing
{
    /*Jede NUnit hat ein SetUp wo die erforderlichen Objekte erzeugt werden.
    gegebenenfalls werden dort auch die Mocks für die Abhängigkeiten erstellt.
    
    Für jede zu testende Methode werden die entsprehcenden Methoden erstellt.
    Jeder Test läuft in 3 Schritten ab:
        -Arrange: Die zu überprüfenden Daten werden dort erstellt.
        -Act: die zu prüfende Methode wird aufgerufen und getestet.
        -Assert: dort wird das Ergebnis mit dem erwarteten Ergebnis verglichen
    */
    [TestFixture]
    internal class EmployeeServiceTests
    {
        private IDBDal mockdal;
        private EmployeeService service;

        #region TestCaseSuit
        //TestCaseSource erstellt mehrere Datensätze um mehrere Fälle abzudecken.
        //Diese werden als Parameter den Methoden übergeben
        public static IEnumerable<TestCaseData> InsertEmployeeValidateTestCases
        {
            get
            {
                //yield gibt die Werte einzelnd bzw nacheinander zurück.
                //Dies spart Ressourcen. Sinnvoll, wenn man nicht direkt alle Daten benötigt.
                yield return new TestCaseData(new Employee { FirstName = "Karl", LastName = "Oberberg", BirthDate = DateTime.Today, IsActive = true }, true).SetName("allTrue");
                yield return new TestCaseData(new Employee { FirstName = null, LastName = "Oberberg", BirthDate = DateTime.Today, IsActive = true }, false).SetName("FirstNameWrong");
                yield return new TestCaseData(new Employee { FirstName = "Karl", LastName = null, BirthDate = DateTime.Today, IsActive = true }, false).SetName("LastNameWrong");
                yield return new TestCaseData(new Employee { FirstName = "Karl", LastName = "Oberberg", BirthDate = null, IsActive = true }, false).SetName("BirthDateWrong");
                yield return new TestCaseData(new Employee { FirstName = "Karl", LastName = "Oberberg", BirthDate = DateTime.Today, IsActive = null }, false).SetName("IsActiveWrong");
                yield return new TestCaseData(new Employee { FirstName = null, LastName = null, BirthDate = null, IsActive = null }, false).SetName("allWrong");
                yield return new TestCaseData(null, false);
            }
        }



        public static IEnumerable<TestCaseData> StandardEmployeeTestCase
        {
            get
            {

                yield return new TestCaseData(
                    new Employee { FirstName = "Anna", LastName = "Schmidt", BirthDate = new DateTime(1990, 05, 12), IsActive = true },
                    true).SetName("TestData1");

                yield return new TestCaseData(
                    new Employee { FirstName = "Lars", LastName = "Meyer", BirthDate = new DateTime(1992, 11, 23), IsActive = true },
                    true).SetName("TestData2");

                yield return new TestCaseData(
                    new Employee { FirstName = "Petra", LastName = "Klein", BirthDate = new DateTime(1980, 03, 01), IsActive = true },
                    true).SetName("TestData3");

                yield return new TestCaseData(
                    new Employee { FirstName = "Jürgen", LastName = "Huber", BirthDate = new DateTime(1975, 08, 19), IsActive = true },
                    true).SetName("TestData4");

                yield return new TestCaseData(
                    new Employee { FirstName = "Sabine", LastName = "Fischer", BirthDate = new DateTime(1985, 12, 05), IsActive = true },
                    true).SetName("TestData5");
            }
        }
        #endregion

        #region Setup
        [SetUp]
        public void Setup()
        {
            mockdal = Substitute.For<IDBDal>();
            service = new EmployeeService(mockdal);
        }
        #endregion

        #region getAllEmployees
        [Test]
        public async Task GetAllEmployees_ReturnsList()
        {
            // Arrange
            var expectedList = new List<Employee>
    {
        new Employee { Id = 1, FirstName = "Max", LastName = "Mustermann", BirthDate = new DateTime(2019,01,01), IsActive = true },
        new Employee { Id = 2, FirstName = "Erika", LastName = "Musterfrau", BirthDate = new DateTime(2018,01,01), IsActive = false }
    };

            mockdal.getAllEmployees().Returns(Task.FromResult(expectedList));

            // Act
            var result = await service.GetAllEmployees();

            // Assert
            Assert.That(result, Is.EqualTo(expectedList));
        }
        #endregion

        #region GetAllEmployeesByBirthDate
        [Test]
        public async Task GetAllEmployeesByBirthDateTests()
        {
            DateTime testDate = new DateTime(1988 - 01 - 01);

            // Arrange 
            var expectedEmployees = new List<Employee>
    {
        new Employee { Id = 1, FirstName = "Sabine", LastName = "Fischer", BirthDate = new DateTime(1985, 12, 05), IsActive = true },
        new Employee { Id = 2, FirstName = "Jürgen", LastName = "Huber", BirthDate = new DateTime(1980, 08, 19), IsActive = true }
    };

            mockdal.getAllEmloyeesByBirthDate(testDate).Returns(Task.FromResult(expectedEmployees));

            // Act
            var result = await service.GetAllEmployeesByBirthDate(testDate);

            // Assert
            Assert.That(result, Is.EqualTo(expectedEmployees));
        }
        #endregion

        #region InsertEmployee

        [TestCaseSource(nameof(StandardEmployeeTestCase))]
        public async Task InsertEmployeeTests(Employee employee, bool expected)
        {

            // Arrange - schon mit TestCaseSource erledigt

            // Act
            mockdal.insertEmployee(employee).Returns(expected);
            var Result = await service.InsertEmployee(employee);
            // Assert


            Assert.That(Result, Is.True);

        }
        #endregion

        #region UpdateEmployee
        [TestCaseSource(nameof(StandardEmployeeTestCase))]
        public async Task UpdateEmployeeTests(Employee employee, bool expected)
        {
            //Arrange

            //Act
            mockdal.updateEmployee(employee).Returns(expected);
            var result = await service.UpdateEmployee(employee);

            // Assert
            Assert.That(result, Is.True);
        }

        #endregion

        #region DeleteEmployee
        
        public async Task DeleteEmployeeTests()
        {
            // Arrange
            int Id = 4;

            // Act
            mockdal.deleteEmployee(Id).Returns(true);
            var result = await service.DeleteEmployee(Id);

            // Assert
            Assert.IsTrue(result);
        }
        #endregion

        #region InsertEmployeeValidate
        [TestCaseSource(nameof(InsertEmployeeValidateTestCases))]
        public void InsertEmployeeValidate_TrueAndFalls(Employee employee, bool expected)
        {
            // Arrange - ist mit dem TestCaseSource schon erledigt

            // Act
            var result = EmployeeService.InsertEmployeeValidate(employee);

            // Assert
            Assert.That(result, Is.EqualTo(expected));
        }


        #endregion

    }
}
