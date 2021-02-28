using System;
using UnityEngine;

namespace GeneralScripts.CustomEditors
{
    [AttributeUsage(AttributeTargets.Field, AllowMultiple = false, Inherited = true)]
    public class ShowIfAttribute : PropertyAttribute
    {
        public ShowConditions.ActionOnConditionFail Action { get; private set; }
        public ShowConditions.ConditionOperator Operator { get; private set; }
        public string[] Conditions { get; private set; }

        public ShowIfAttribute(ShowConditions.ActionOnConditionFail action, ShowConditions.ConditionOperator conditionOperator, params string[] conditions)
        {
            Action = action;
            Operator = conditionOperator;
            Conditions = conditions;
        }
    }
}