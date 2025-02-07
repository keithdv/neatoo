using Microsoft.VisualStudio.TestTools.UnitTesting;
using Neatoo.Portal;
using Neatoo.UnitTest.Objects;
using System;
using System.Threading.Tasks;

namespace Neatoo.UnitTest.ObjectPortal
{
    public class EditObjectList : EditListBase<EditObjectList, IEditObject>, IEditObjectList
    {

        public EditObjectList(IEditListBaseServices<EditObjectList, IEditObject> baseServices) : base(baseServices)
        {
        }

        public bool CreateCalled { get; set; } = false;

        [Create]
        private void Create()
        {
            CreateCalled = true;
        }

        [Create]
        private void Create(int criteria)
        {
            CreateCalled = true;
        }


        [Create]
        private void Create(Guid criteria, IDisposableDependency dependency)
        {
            Assert.IsNotNull(dependency);
            CreateCalled = true;
        }

        public bool CreateChildCalled { get; set; } = false;

        [CreateChild]
        private void CreateChild()
        {
            CreateChildCalled = true;
        }

        [CreateChild]
        private void CreateChild(int criteria)
        {
            CreateChildCalled = true;
        }

        [CreateChild]
        private void CreateChild(Guid criteria, IDisposableDependency dependency)
        {
            Assert.IsNotNull(dependency);
            CreateChildCalled = true;
        }

        public bool FetchCalled { get; set; } = false;

        [Fetch]
        private void Fetch()
        {
            FetchCalled = true;
        }

        [Fetch]
        private void Fetch(int criteria)
        {
            FetchCalled = true;
        }


        [Fetch]
        private void Fetch(Guid criteria, IDisposableDependency dependency)
        {
            Assert.IsNotNull(dependency);
            FetchCalled = true;
        }

        public bool FetchChildCalled { get; set; } = false;

        [FetchChild]
        private void FetchChild()
        {
            FetchChildCalled = true;
        }

        [FetchChild]
        private void FetchChild(int criteria)
        {
            FetchChildCalled = true;
        }

        [FetchChild]
        private void FetchChild(Guid criteria, IDisposableDependency dependency)
        {
            Assert.IsNotNull(dependency);
            FetchChildCalled = true;
        }

        public bool InsertCalled { get; set; } = false;

        [Insert]
        private void Insert()
        {
            InsertCalled = true;
        }


        [Insert]
        private void Insert(int criteria)
        {
            InsertCalled = true;
        }


        [Insert]
        private void Insert(Guid criteria, IDisposableDependency dependency)
        {
            Assert.IsNotNull(dependency);
            InsertCalled = true;
        }

        public bool InsertChildCalled { get; set; } = false;

        [InsertChild]
        private void InsertChild()
        {
            InsertChildCalled = true;
        }

        [InsertChild]
        private void InsertChild(int criteria)
        {
            InsertChildCalled = true;
        }


        [InsertChild]
        private void InsertChild(Guid criteria, IDisposableDependency dependency)
        {
            Assert.IsNotNull(dependency);
            InsertChildCalled = true;
        }

        public bool UpdateCalled { get; set; } = false;

        protected override async Task Update()
        {
            UpdateCalled = true;
            await UpdateList();
        }

        [Update]
        private void Update(int criteria)
        {
            UpdateCalled = true;
        }


        [Update]
        private void Update(Guid criteria, IDisposableDependency dependency)
        {
            Assert.IsNotNull(dependency);
            UpdateCalled = true;
        }

        public bool UpdateChildCalled { get; set; } = false;

        [UpdateChild]
        private void UpdateChild()
        {
            UpdateChildCalled = true;
        }

        [UpdateChild]
        private void UpdateChild(int criteria)
        {
            UpdateChildCalled = true;
        }


        [UpdateChild]
        private void UpdateChild(Guid criteria, IDisposableDependency dependency)
        {
            Assert.IsNotNull(dependency);
            UpdateChildCalled = true;
        }

        public bool DeleteCalled { get; set; } = false;

        [Delete]
        private void Delete_()
        {
            DeleteCalled = true;
        }

        [Delete]
        private void Delete(int criteria)
        {
            DeleteCalled = true;
        }

        [Delete]
        private void Delete(Guid criteria, IDisposableDependency dependency)
        {
            Assert.IsNotNull(dependency);
            DeleteCalled = true;
        }

        public bool DeleteChildCalled { get; set; } = false;

        [DeleteChild]
        private void DeleteChild()
        {
            DeleteChildCalled = true;
        }

        [DeleteChild]
        private void DeleteChild(int criteria)
        {
            DeleteChildCalled = true;
        }

        [DeleteChild]
        private void DeleteChild(Guid criteria, IDisposableDependency dependency)
        {
            Assert.IsNotNull(dependency);
            DeleteChildCalled = true;
        }

    }
}
