﻿using Autofac;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using Neatoo.Core;
using Neatoo.Portal;
using Neatoo.UnitTest.BaseTests;
using Newtonsoft.Json.Linq;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using System.Security.Cryptography.X509Certificates;
using System.Text;
using System.Text.Json;
using System.Text.Json.Serialization;
using System.Text.Json.Serialization.Metadata;
using System.Threading.Tasks;
using System.Xml.Linq;
using static Moq.It;
using static System.Formats.Asn1.AsnWriter;

namespace Neatoo.UnitTest.SystemJsonText
{

    [TestClass]
    public class BaseSeralizationTests
    {
        private IServiceScope scope;

        private IBaseObject single;
        private IBaseObject child;
        private IBaseObject third;
        private MyReferenceHandler referenceHandler;
        private JsonSerializerOptions options;




        [TestInitialize]
        public void TestInitialize()
        {

            scope = AutofacContainer.GetLifetimeScope().Resolve<IServiceScope>();

            // Note: Can't be an interface type
            single = scope.Resolve<IBaseObject>();

            single.Id = Guid.NewGuid();
            single.FirstName = Guid.NewGuid().ToString();
            single.LastName = Guid.NewGuid().ToString();

            child = scope.Resolve<IBaseObject>();

            child.Id = Guid.NewGuid();
            child.FirstName = Guid.NewGuid().ToString();
            child.LastName = Guid.NewGuid().ToString();

            single.Child = child;

            third = scope.Resolve<IBaseObject>();
            third.Id = Guid.NewGuid();
            third.FirstName = Guid.NewGuid().ToString();
            third.LastName = Guid.NewGuid().ToString();

            third.Child = child;

            referenceHandler = new MyReferenceHandler();

            options = new()
            {
                Converters = { new NeatooJsonConverterFactory(scope) },
                ReferenceHandler = referenceHandler,
                WriteIndented = true
            };
        }

        [TestCleanup]
        public void TestCleanup()
        {
            referenceHandler.Reset();
            scope.Dispose();
        }


        [TestMethod]
        public void Serialize_BaseObject()
        {
            List<IBase> list = new List<IBase>() { single, third, child };
            var json = System.Text.Json.JsonSerializer.Serialize(list, options);

            var deserialized = (List<IBase>) System.Text.Json.JsonSerializer.Deserialize(json, typeof(List<IBaseObject>), options);


        }

        [TestMethod]
        public void Serialize_IBase_Deserialize()
        {
            List<IBase> list = new List<IBase>() { single, third, child };
            var json = System.Text.Json.JsonSerializer.Serialize(list, options);

            var deserialized = (List<IBase>)System.Text.Json.JsonSerializer.Deserialize(json, typeof(List<IBase>), options);

            var result = deserialized.Cast<IBaseObject>().ToList();

            Assert.AreSame(result[0].Child, result[1].Child);
            Assert.AreSame(result[2], result[0].Child);
        }
    }



}
