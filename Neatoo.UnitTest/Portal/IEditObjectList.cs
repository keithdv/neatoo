namespace Neatoo.UnitTest.ObjectPortal
{
    public interface IEditObjectList : IEditListBase<IEditObject>
    {
        bool CreateCalled { get; set; }
        bool CreateChildCalled { get; set; }
        bool FetchCalled { get; set; }
        bool FetchChildCalled { get; set; }
        bool DeleteCalled { get; set; }
        bool DeleteChildCalled { get; set; }
        bool UpdateCalled { get; set; }
        bool UpdateChildCalled { get; set; }
        bool InsertCalled { get; set; }
        bool InsertChildCalled { get; set; }

    }
}