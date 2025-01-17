﻿using Neatoo.Portal;
using Neatoo.Rules;
using System;
using System.Collections.Generic;
using System.Runtime.Serialization;
using System.Text;

namespace Neatoo.Netwonsoft.Json.Test.ValidateTests
{
    public interface IValidateObject : IValidateBase
    {
        Guid ID { get; set; }
        string Name { get; set; }
        int RuleRunCount { get; }

        IValidateObject Child { get; set; }
        IEnumerable<IRule> Rules { get; }
        void MarkInvalid(string message);
        IRuleResult OverrideResult { get; }

    }

    public class ValidateObject : ValidateBase<ValidateObject>, IValidateObject
    {
        public ValidateObject(IValidateBaseServices<ValidateObject> services) : base(services)
        {
            RuleManager.AddRule(t =>
            {
                t.RuleRunCount++;
                if (t.Name == "Error") { return RuleResult.PropertyError(nameof(Name), "Error"); }
                return RuleResult.Empty();
            }, nameof(Name));
        }

        public int RuleRunCount { get => Getter<int>(); set => Setter(value); }
        public Guid ID { get => Getter<Guid>(); set => Setter(value); }
        public string Name { get => Getter<string>(); set => Setter(value); }
        public IValidateObject Child { get => Getter<IValidateObject>(); set => Setter(value); }

        public IEnumerable<IRule> Rules => RuleManager.Rules;
        void IValidateObject.MarkInvalid(string message)
        {
            base.MarkInvalid(message);
        }

        IRuleResult IValidateObject.OverrideResult => RuleManager.OverrideResult;

        [Create]
        public void Create(Guid ID, string Name)
        {
            this.ID = ID;
            this.Name = Name;
        }

        [Update]
        public void Update()
        {
            this.Name = "Updated";
        }
    }

    public interface IValidateObjectList : IValidateListBase<IValidateObject>
    {
        Guid ID { get; set; }
        string Name { get; set; }
        int RuleRunCount { get; }
        IEnumerable<IRule> Rules { get; }
        void MarkInvalid(string message);
        IRuleResult OverrideResult { get; }
    }

    public class ValidateObjectList : ValidateListBase<ValidateObjectList, IValidateObject>, IValidateObjectList
    {
        public ValidateObjectList(IValidateListBaseServices<ValidateObjectList, IValidateObject> services) : base(services)
        {
            RuleManager.AddRule(t =>
            {
                t.RuleRunCount++;
                if (t.Name == "Error") { return RuleResult.PropertyError(nameof(Name), "Error"); }
                return RuleResult.Empty();
            }, nameof(Name));

        }

        public int RuleRunCount { get => Getter<int>(); set => Setter(value); }
        public Guid ID { get => Getter<Guid>(); set => Setter(value); }
        public string Name { get => Getter<string>(); set => Setter(value); }
        public IEnumerable<IRule> Rules => RuleManager.Rules;

        void IValidateObjectList.MarkInvalid(string message)
        {
            base.MarkInvalid(message);
        }

        IRuleResult IValidateObjectList.OverrideResult => RuleManager.OverrideResult;

    }
}
