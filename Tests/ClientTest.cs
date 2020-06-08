using projekt;
using Microsoft.VisualStudio.TestTools.UnitTesting;
namespace Tests
{
    
    
    /// <summary>
    ///This is a test class for ClientTest and is intended
    ///to contain all ClientTest Unit Tests
    ///</summary>
    [TestClass()]
    public class ClientTest
    {


        private TestContext testContextInstance;

        /// <summary>
        ///Gets or sets the test context which provides
        ///information about and functionality for the current test run.
        ///</summary>
        public TestContext TestContext
        {
            get
            {
                return testContextInstance;
            }
            set
            {
                testContextInstance = value;
            }
        }


        /// <summary>
        ///A test for Surname
        ///</summary>
        [TestMethod()]
        public void SurnameTest()
        {
            Client target = new Client(); 
            string expected = "Kowalski"; 
            string actual;
            target.Surname = expected;
            actual = target.Surname;
            Assert.AreEqual(expected, actual);
        }

        /// <summary>
        ///A test for PESEL
        ///</summary>
        [TestMethod()]
        public void PESELTest()
        {
            Client target = new Client(); 
            string expected = "14555555555"; 
            string actual;
            target.PESEL = expected;
            actual = target.PESEL;
            Assert.AreEqual(expected, actual);
        }

        /// <summary>
        ///A test for Name
        ///</summary>
        [TestMethod()]
        public void NameTest()
        {
            Client target = new Client(); 
            string expected = "Monika"; 
            string actual;
            target.Name = expected;
            actual = target.Name;
            Assert.AreEqual(expected, actual);
        }

        /// <summary>
        ///A test for ID
        ///</summary>
        [TestMethod()]
        public void IDTest()
        {
            Client target = new Client();
            string expected = "11111111111"; 
            string actual;
            target.ID = expected;
            actual = target.ID;
            Assert.AreEqual(expected, actual);
        }

        /// <summary>
        ///A test for City
        ///</summary>
        [TestMethod()]
        public void CityTest()
        {
            Client target = new Client(); 
            string expected = "Krakow"; 
            string actual;
            target.City = expected;
            actual = target.City;
            Assert.AreEqual(expected, actual);
        }

        /// <summary>
        ///A test for Balance
        ///</summary>
        [TestMethod()]
        public void BalanceTest()
        {
            Client target = new Client(); 
            double expected = 568.4F; 
            double actual;
            target.Balance = expected;
            actual = target.Balance;
            Assert.AreEqual(expected, actual);
        }

    }
}
