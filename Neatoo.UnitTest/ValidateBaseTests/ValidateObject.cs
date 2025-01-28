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

        public ValidateObject(ValidateBaseServices<ValidateObject> services,
            IShortNameRule<ValidateObject> shortNameRule,
            IFullNameRule<ValidateObject> fullNameRule,
            IRecursiveRule recursiveRule,
            IRuleThrowsException ruleThrowsException
            ) : base(services)
        {
            RuleManager.AddRules(shortNameRule, fullNameRule, recursiveRule, ruleThrowsException);
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


    public interface IValidateObjectList : IValidateListBase<IValidateObject>
    {
        void Add(IValidateObject obj);

    }

    public class ValidateObjectList : ValidateListBase<IValidateObject>, IValidateObjectList
    {

        public ValidateObjectList(ValidateListBaseServices<IValidateObject> services
            ) : base(services)
        {
        }

    }

}
