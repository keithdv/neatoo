﻿using Microsoft.Extensions.DependencyInjection;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using Neatoo.Portal;
using Neatoo.UnitTest.ObjectPortal;
using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;

namespace Neatoo.UnitTest.Portal
{
    [TestClass]
    public class PortalEditListSaveTests
    {

        private IServiceScope scope = UnitTestServices.GetLifetimeScope(true);
        private IReadWritePortal<IEditObjectList> portal;
        private IEditObjectList list;
        private IEditObject child;

        [TestInitialize]
        public void TestInitialize()
        {
            portal = scope.GetRequiredService<IReadWritePortal<IEditObjectList>>();
            list = portal.Fetch().Result;
            child = list.CreateAdd().Result;
            child.MarkUnmodified();
            child.MarkOld();

            Assert.IsFalse(list.IsModified);

        }

        [TestCleanup]
        public void TestCleanup()
        {
            scope.Dispose();
        }

    }
}
