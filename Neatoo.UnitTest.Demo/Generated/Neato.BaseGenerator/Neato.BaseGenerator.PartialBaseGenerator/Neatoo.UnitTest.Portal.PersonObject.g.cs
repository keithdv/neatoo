#nullable enable
using Neatoo;
using Microsoft.Extensions.DependencyInjection;

/*
                    Debugging Messages:
                    Yay!
                    
                    */
namespace Neatoo.UnitTest.Portal
{
    public partial interface IPersonObject
    {
        string FirstName { get; set; }

        string LastName { get; set; }
    }

    internal partial class PersonObject
    {
        public partial string FirstName { get => Getter<string>(); set => Setter(value); }
        public partial string LastName { get => Getter<string>(); set => Setter(value); }
    }
}