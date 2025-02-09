using Microsoft.VisualStudio.TestTools.UnitTesting;
using Neatoo.Rules;
using System.Linq.Expressions;

namespace Neatoo.UnitTest
{

    public class TriggerPropertiesTestSubject
    {
        public TriggerPropertiesTestSubjectChild Child { get; set; }

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

            var triggerProperty = new TriggerProperty<TriggerPropertiesTestSubject, string>((TriggerPropertiesTestSubject t) => t.Child.ChildProperty);
            // Act
            var result = triggerProperty.IsMatch(testSubject, "Child.ChildProperty");
            // Assert
            Assert.IsTrue(result);
        }

    }
}
