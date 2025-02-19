using System;
using System.Threading.Tasks;

namespace Neatoo.UnitTest.ObjectPortal;

public interface IBaseObject : IBase
{
    int IntCriteria { get; }
    Guid GuidCriteria { get; }
    object[] MultipleCriteria { get; }
    bool CreateCalled { get; set; }
    bool FetchCalled { get; set; }
}