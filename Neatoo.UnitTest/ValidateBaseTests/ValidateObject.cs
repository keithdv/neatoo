using Neatoo.Portal;
using Neatoo.Rules;
using Neatoo.UnitTest.PersonObjects;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Neatoo.UnitTest.ValidateBaseTests
{
    public interface IValidateObject : IPersonBase
    {
        IValidateObject Child { get; set; }
        int RuleRunCount { get; }

        void TestMarkInvalid(string message);
    }

    internal class ValidateObject : PersonValidateBase<ValidateObject>, IValidateObject
    {
        public IShortNameRule<ValidateObject> ShortNameRule { get; }
        public IFullNameRule<ValidateObject> FullNameRule { get; }

        public ValidateObject(IValidateBaseServices<ValidateObject> services,
            IShortNameRule<ValidateObject> shortNameRule,
            IFullNameRule<ValidateObject> fullNameRule,
            IRecursiveRule recursiveRule
            ) : base(services)
        {
            RuleManager.AddRules(shortNameRule, fullNameRule, recursiveRule);
            ShortNameRule = shortNameRule;
            FullNameRule = fullNameRule;
        }

        public IValidateObject Child { get { return Getter<IValidateObject>(); } set { Setter(value); } }

        [Fetch]
        [FetchChild]
        public async Task Fetch(PersonDto person, IReadPortalChild<IValidateObject> portal, IReadOnlyList<PersonDto> personTable)
        {
            base.FillFromDto(person);

            var childDto = personTable.FirstOrDefault(p => p.FatherId == Id);

            if (childDto != null)
            {
                Child = await portal.FetchChild(childDto);
            }
        }

        public int RuleRunCount => ShortNameRule.RunCount + FullNameRule.RunCount;

        public void TestMarkInvalid(string message)
        {
            MarkInvalid(message);
        }
    }


    public interface IValidateObjectList : IValidateListBase<IValidateObject>, IPersonBase
    {
        int RuleRunCount { get; }
        void Add(IValidateObject obj);
        void TestMarkInvalid(string message);

    }

    public class ValidateObjectList : PersonValidateListBase<ValidateObjectList, IValidateObject>, IValidateObjectList
    {

        public ValidateObjectList(IValidateListBaseServices<ValidateObjectList, IValidateObject> services,
            IShortNameRule<ValidateObjectList> shortNameRule,
            IFullNameRule<ValidateObjectList> fullNameRule
            ) : base(services)
        {
            RuleManager.AddRules(shortNameRule, fullNameRule);
            ShortNameRule = shortNameRule;
            FullNameRule = fullNameRule;
        }

        public int RuleRunCount => ShortNameRule.RunCount + FullNameRule.RunCount + this.Select(v => v.RuleRunCount).Sum();
        public IShortNameRule<ValidateObjectList> ShortNameRule { get; }
        public IFullNameRule<ValidateObjectList> FullNameRule { get; }
        public void TestMarkInvalid(string message)
        {
            MarkInvalid(message);
        }
    }

}
