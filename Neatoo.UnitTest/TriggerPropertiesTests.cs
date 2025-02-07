using Microsoft.VisualStudio.TestTools.UnitTesting;
using Neatoo.Rules;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Text;
using System.Threading.Tasks;

namespace Neatoo.UnitTest
{

    public class TriggerPropertiesTestSubject
    {
        public TriggerPropertiesTestSubjectChild Child { get; set; }

        public Expression Expression => () => Child.ChildProperty;
    }


    public class TriggerPropertiesTestSubjectChild
    {
        public string ChildProperty { get; set; }
    }

    [TestClass]
    public class TriggerPropertiesTests
    {

        [TestMethod]
        public void TriggerProperty_WithExpression()
        {
            var testSubject = new TriggerPropertiesTestSubject();

            var triggerProperty = new TriggerProperty(testSubject.Expression);
            // Act
            var result = triggerProperty.IsMatch(testSubject, "Child.ChildProperty");
            // Assert
            Assert.IsTrue(result);
        }

    }
}
