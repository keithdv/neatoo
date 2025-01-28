using Autofac;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using Neatoo.Portal;
using Neatoo.Rules;
using Neatoo.UnitTest.PersonObjects;
using System;
using System.Collections.Generic;
using System.Text;
using System.Threading;
using System.Threading.Tasks;

namespace Neatoo.UnitTest.ValidateBaseTests
{

    // Contravariance on IRule< in T > is required for this to work
    // That way ActualType that inherits from IValidateBase can be cast to IRule < IValidateBase >

    public class ShortNameRule : Rules.AsyncRuleBase<IValidateBase>
    {
        private readonly IRegisteredProperty shortName;
        private readonly IRegisteredProperty firstName;
        private readonly IRegisteredProperty lastName;

        public ShortNameRule(IRegisteredProperty shortName, IRegisteredProperty firstName, IRegisteredProperty lastName) : base(shortName, firstName, lastName)
        {
            this.shortName = shortName;
            this.firstName = firstName;
            this.lastName = lastName;
        }

        public override async Task<PropertyErrors> Execute(IValidateBase target, CancellationToken token)
        {
            await Task.Delay(10);

            var sn = $"{ReadProperty<string>(firstName)} {ReadProperty<string>(lastName)}";

            LoadProperty(shortName, sn);

            return PropertyErrors.None;

        }
    }

    public interface ISharedAsyncRuleObject : IPersonBase { }

    public class SharedAsyncRuleObject : PersonValidateBase<SharedAsyncRuleObject>, ISharedAsyncRuleObject
    {

        public SharedAsyncRuleObject(IValidateBaseServices<SharedAsyncRuleObject> services) : base(services)
        {

            var fn = services.RegisteredPropertyManager.GetRegisteredProperty(nameof(FirstName));
            var ln = services.RegisteredPropertyManager.GetRegisteredProperty(nameof(LastName));
            var sn = services.RegisteredPropertyManager.GetRegisteredProperty(nameof(ShortName));

            RuleManager.AddRule(new ShortNameRule(sn, fn, ln));

        }

    }

    [TestClass]
    public class SharedAsyncRuleTests
    {

        private ILifetimeScope scope;
        private ISharedAsyncRuleObject target;

        [TestInitialize]
        public void TestInitailize()
        {
            scope = AutofacContainer.GetLifetimeScope();
            target = scope.Resolve<ISharedAsyncRuleObject>();

        }


        [TestCleanup]
        public void TestInitalize()
        {
            scope.Dispose();
        }

        [TestMethod]
        public async Task SharedAsyncRuleTests_ShortName()
        {
            target.FirstName = "John";
            target.LastName = "Smith";

            await target.WaitForRules();

            Assert.AreEqual("John Smith", target.ShortName);

        }
    }
}
