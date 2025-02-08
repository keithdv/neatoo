using Microsoft.Extensions.DependencyInjection;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using Neatoo.Rules;
using Neatoo.UnitTest.PersonObjects;
using System;
using System.Linq.Expressions;
using System.Threading;
using System.Threading.Tasks;

namespace Neatoo.UnitTest.ValidateBaseTests
{

    // Contravariance on IRule< in T > is required for this to work
    // That way ActualType that inherits from IValidateBase can be cast to IRule < IValidateBase >

    public class SharedShortNameRule<T> : Rules.AsyncRuleBase<T> where T : IValidateBase
    {
        private readonly Expression<Func<T, object?>> shortName;
        private readonly Expression<Func<T, object?>> firstName;
        private readonly Expression<Func<T, object?>> lastName;

        public SharedShortNameRule(Expression<Func<T, object?>> shortName, Expression<Func<T, object?>> firstName, Expression<Func<T, object?>> lastName) : base(shortName, firstName, lastName)
        {
            this.shortName = shortName;
            this.firstName = firstName;
            this.lastName = lastName;
        }

        public override async Task<PropertyErrors> Execute(T target, CancellationToken token)
        {
            await Task.Delay(10);

            var sn = $"{ReadProperty(firstName)?.ToString()} {ReadProperty(lastName)?.ToString()}";

            LoadProperty(shortName, sn);

            return PropertyErrors.None;

        }
    }

    public interface ISharedAsyncRuleObject : IPersonBase { }

    public class SharedAsyncRuleObject : PersonValidateBase<SharedAsyncRuleObject>, ISharedAsyncRuleObject
    {

        public SharedAsyncRuleObject(IValidateBaseServices<SharedAsyncRuleObject> services) : base(services)
        {
            RuleManager.AddRule(new SharedShortNameRule<SharedAsyncRuleObject>(_ => _.ShortName, _ => _.FirstName, _ => _.LastName));
        }

    }

    [TestClass]
    public class SharedAsyncRuleTests
    {

        private IServiceScope scope;
        private ISharedAsyncRuleObject target;

        [TestInitialize]
        public void TestInitailize()
        {
            scope = UnitTestServices.GetLifetimeScope();
            target = scope.GetRequiredService<ISharedAsyncRuleObject>();

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

            await target.WaitForTasks();

            Assert.AreEqual("John Smith", target.ShortName);

        }
    }
}
