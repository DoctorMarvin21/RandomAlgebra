using Microsoft.VisualStudio.TestTools.UnitTesting;
using System;
using System.Globalization;
using System.Linq;
using System.Threading;

namespace RandomAlgebra.Test
{
    [TestClass]
    public class UsResourcesTests
    {
        [TestInitialize]
        public void SetLocale()
        {
            Thread.CurrentThread.CurrentCulture = new CultureInfo("en-US");
            Thread.CurrentThread.CurrentUICulture = new CultureInfo("en-US");
        }


        [TestMethod]
        public void TestArgumentExceptions()
        {
            var values = Enum.GetValues(typeof(DistributionsArgumentExceptionType))
                .Cast<DistributionsArgumentExceptionType>().ToArray();

            Assert.IsTrue(values.Length > 0);

            foreach (DistributionsArgumentExceptionType value in values)
            {
                string message = Resources.GetMessage(value.ToString());
                Assert.IsNotNull(message);
            }
        }

        [TestMethod]
        public void TestInvalidOperationExceptions()
        {
            var values = Enum.GetValues(typeof(DistributionsInvalidOperationExceptionType))
                .Cast<DistributionsInvalidOperationExceptionType>().ToArray();

            Assert.IsTrue(values.Length > 0);

            Thread.CurrentThread.CurrentCulture = new CultureInfo("en-US");

            foreach (DistributionsInvalidOperationExceptionType value in values)
            {
                string message = Resources.GetMessage(value.ToString());
                Assert.IsNotNull(message);
            }
        }
    }

        [TestClass]
    public class RuResourcesTests
    {
        [TestInitialize]
        public void SetLocale()
        {
            Thread.CurrentThread.CurrentCulture = new CultureInfo("ru-RU");
            Thread.CurrentThread.CurrentUICulture = new CultureInfo("ru-RU");
        }


        [TestMethod]
        public void TestArgumentExceptions()
        {
            var values = Enum.GetValues(typeof(DistributionsArgumentExceptionType))
                .Cast<DistributionsArgumentExceptionType>().ToArray();

            Assert.IsTrue(values.Length > 0);

            foreach (DistributionsArgumentExceptionType value in values)
            {
                string message = Resources.GetMessage(value.ToString());
                Assert.IsNotNull(message);
            }
        }

        [TestMethod]
        public void TestInvalidOperationExceptions()
        {
            var values = Enum.GetValues(typeof(DistributionsInvalidOperationExceptionType))
                .Cast<DistributionsInvalidOperationExceptionType>().ToArray();

            Assert.IsTrue(values.Length > 0);

            foreach (DistributionsInvalidOperationExceptionType value in values)
            {
                string message = Resources.GetMessage(value.ToString());
                Assert.IsNotNull(message);
            }
        }
    }
}
