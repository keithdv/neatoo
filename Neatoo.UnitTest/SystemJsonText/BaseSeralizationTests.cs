using Microsoft.Extensions.DependencyInjection;
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
        private NeatooJsonSerializer serializer;




        [TestInitialize]
        public void TestInitialize()
        {

            scope = UnitTestServices.GetLifetimeScope();

            // Note: Can't be an interface type
            single = scope.GetRequiredService<IBaseObject>();

            single.Id = Guid.NewGuid();
            single.FirstName = Guid.NewGuid().ToString();
            single.LastName = Guid.NewGuid().ToString();

            child = scope.GetRequiredService<IBaseObject>();

            child.Id = Guid.NewGuid();
            child.FirstName = Guid.NewGuid().ToString();
            child.LastName = Guid.NewGuid().ToString();

            single.Child = child;

            third = scope.GetRequiredService<IBaseObject>();
            third.Id = Guid.NewGuid();
            third.FirstName = Guid.NewGuid().ToString();
            third.LastName = Guid.NewGuid().ToString();

            third.Child = child;

            serializer = scope.GetRequiredService<NeatooJsonSerializer>();

        }

        [TestCleanup]
        public void TestCleanup()
        {
            scope.Dispose();
        }


        [TestMethod]
        public void Serialize_BaseObject()
        {
            List<IBase> list = new List<IBase>() { single, third, child };
            var json = serializer.Serialize(list);

            var deserialized = (List<IBase>) serializer.Deserialize(json, typeof(List<IBase>));


        }

        [TestMethod]
        public void Serialize_IBase_Deserialize()
        {
            List<IBase> list = new List<IBase>() { single, third, child };
            var json = serializer.Serialize(list);

            var deserialized = (List<IBase>)serializer.Deserialize(json, typeof(List<IBase>));

            var result = deserialized.Cast<IBaseObject>().ToList();

            Assert.AreSame(result[0].Child, result[1].Child);
            Assert.AreSame(result[2], result[0].Child);
        }
    }



}
