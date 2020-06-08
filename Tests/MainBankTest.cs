using projekt;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using System.Collections.Generic;

namespace Tests
{
    
    
    /// <summary>
    ///This is a test class for MainBankTest and is intended
    ///to contain all MainBankTest Unit Tests
    ///</summary>
    [TestClass()]
    public class MainBankTest
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
        ///A test for sendMoney
        ///</summary>
        [TestMethod()]
        public void sendMoneyTest()
        {
            double amountOfMoney = 20; 
            string SenderID = "0123456789"; 
            string ReciverID = "1111111111"; 
            bool expected = true; 
            bool actual;
            actual = MainBank.sendMoney(amountOfMoney, SenderID, ReciverID);
            Assert.AreEqual(expected, actual);
        }

        /// <summary>
        ///A test for RandomID
        ///</summary>
        [TestMethod()]
        public void RandomIDTest()
        {
            string expected = "0123456789"; 
            string actual;
            actual = MainBank.RandomID();
            Assert.AreNotEqual(expected, actual);
        }


        /// <summary>
        ///A test for getOtherDepartment
        ///</summary>
        [TestMethod()]
        public void getOtherDepartmentTest()
        {
            string department = "OddzialKrakow"; 
            string expected = "OddzialWarszawa"; 
            string actual;
            actual = MainBank.getOtherDepartment(department);
            Assert.AreEqual(expected, actual);
        }

        /// <summary>
        ///A test for getDepartmentID
        ///</summary>
        [TestMethod()]
        public void getDepartmentIDTest()
        {
            string department = "OddzialKrakow"; 
            string expected = "KR1234"; 
            string actual;
            actual = MainBank.getDepartmentID(department);
            Assert.AreEqual(expected, actual);
        }

        /// <summary>
        ///A test for getDepartment
        ///</summary>
        [TestMethod()]
        public void getDepartmentTest()
        {
            string ID = "0123456789"; 
            string expected = "OddzialKrakow"; 
            string actual;
            actual = MainBank.getDepartment(ID);
            Assert.AreEqual(expected, actual);
        }

        /// <summary>
        ///A test for departmentID
        ///</summary>
        [TestMethod()]
        public void departmentIDTest()
        {
            string ID = "0123456789";
            Dictionary<string, string> expected = new Dictionary<string,string> {{"KR1234", "OddzialKrakow"}, { "WA1234", "OddzialWarszawa" }}; 
            Dictionary<string, string> actual;
            actual = MainBank.departmentID(ID);
            CollectionAssert.AreEqual(expected, actual);
        }

        /// <summary>
        ///A test for checkPesel
        ///</summary>
        [TestMethod()]
        public void checkPeselTest()
        {
            string pesel = "21545545454544"; 
            bool expected = true; 
            bool actual;
            actual = MainBank.checkPesel(pesel);
            Assert.AreEqual(expected, actual);
        }

        /// <summary>
        ///A test for checkDepartments
        ///</summary>
        [TestMethod()]
        public void checkDepartmentsTest()
        {
            string ID = "0123456789";
            bool expected = true; 
            bool actual;
            actual = MainBank.checkDepartments(ID);
            Assert.AreEqual(expected, actual);
        }

        /// <summary>
        ///A test for checkClientID
        ///</summary>
        [TestMethod()]
        public void checkClientIDTest()
        {
            string ID = "9845685478"; 
            bool expected = false;
            bool actual;
            actual = MainBank.checkClientID(ID);
            Assert.AreEqual(expected, actual);
        }
    }
}
