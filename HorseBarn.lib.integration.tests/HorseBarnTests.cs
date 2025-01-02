using Autofac;
using Neatoo.Portal;

namespace HorseBarn.lib.integration.tests
{
    [TestClass]
    public sealed class HorseBarnTests
    {
        private ILifetimeScope scope;
        private ISendReceivePortal<IHorseBarn> portal;

        [TestInitialize]
        public void TestInitialize()
        {
            scope = AutofacContainer.GetLifetimeScope();
            portal = scope.Resolve<ISendReceivePortal<IHorseBarn>>();
        }

        [TestCleanup]
        public void TestCleanup()
        {
            scope.Dispose();
        }

        [TestMethod]
        public async Task HorseBarn_Create()
        {
            var horseBarn = await portal.Create();

            var horse = await horseBarn.AddNewHorse(Breed.Clydesdale);

            Assert.IsFalse(horseBarn.IsValid);

            horse.Name = "Heavy Horse A";

            Assert.IsInstanceOfType<IHeavyHorse>(horse);

            horse = await horseBarn.AddNewHorse(Breed.Thoroughbred);

            horse.Name = "Light Horse B";

            Assert.IsInstanceOfType<ILightHorse>(horse);


            Assert.IsTrue(horseBarn.IsValid);

        }
    }
}
