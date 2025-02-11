using Neatoo.Core;
using Neatoo.Portal;
using Neatoo.UnitTest.PersonObjects;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Neatoo.UnitTest.ValidateBaseTests
{
    public interface IValidateAsyncObject : IPersonBase
    {
        string aLabel { get; set; }
        IValidateAsyncObject Child { get; set; }
        int RuleRunCount { get; }
        string ThrowException { get; set; }

        List<IValidateProperty> Properties => PropertyManager.GetProperties.Cast<IValidateProperty>().ToList();
    }

    internal class ValidateAsyncObject : PersonValidateBase<ValidateAsyncObject>, IValidateAsyncObject
    {
        public IShortNameAsyncRule<ValidateAsyncObject> ShortNameRule { get; }
        public IFullNameAsyncRule<ValidateAsyncObject> FullNameRule { get; }

        public ValidateAsyncObject(IValidateBaseServices<ValidateAsyncObject> services,
            IShortNameAsyncRule<ValidateAsyncObject> shortNameRule,
            IFullNameAsyncRule<ValidateAsyncObject> fullNameRule,
            IAsyncRuleThrowsException asyncRuleThrowsException,
            IRecursiveAsyncRule recursiveAsyncRule
            ) : base(services)
        {
            RuleManager.AddRules(shortNameRule, fullNameRule, recursiveAsyncRule);
            ShortNameRule = shortNameRule;
            FullNameRule = fullNameRule;
            // TODO : Can add a rule that's not the correct type, Handle?
            RuleManager.AddRule(asyncRuleThrowsException);

            RuleManager.AddActionAsync((v) => Task.Delay(10), _ => _.Child);
        }

        public string aLabel { get => Getter<string>(); set => Setter(value); }
        public IValidateAsyncObject Child { get { return Getter<IValidateAsyncObject>(); } set { Setter(value); } }

        public string ThrowException { get => Getter<string>(); set => Setter(value); }

        [Fetch]
        [FetchChild]
        public async Task Fetch(PersonDto person, IReadPortalChild<IValidateAsyncObject> portal, IReadOnlyList<PersonDto> personTable)
        {
            base.FillFromDto(person);

            var childDto = personTable.FirstOrDefault(p => p.FatherId == Id);

            if (childDto != null)
            {
                Child = await portal.FetchChild(childDto);
            }
        }

        public int RuleRunCount => ShortNameRule.RunCount + FullNameRule.RunCount;

    }


    public interface IValidateAsyncObjectList : IValidateListBase<IValidateAsyncObject>
    {
        void Add(IValidateAsyncObject o);
    }

    public class ValidateAsyncObjectList : ValidateListBase<ValidateAsyncObjectList, IValidateAsyncObject>, IValidateAsyncObjectList
    {

        public ValidateAsyncObjectList(IValidateListBaseServices<ValidateAsyncObjectList, IValidateAsyncObject> services) : base(services)
        {

        }

    }

}
