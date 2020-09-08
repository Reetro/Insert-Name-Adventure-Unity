public class ShowConditions
{
    public enum ActionOnConditionFail
    {
        // If condition(s) are false, don't draw the field at all.
        DontDraw,
        // If condition(s) are false, just set the field as disabled.
        JustDisable,
    }

    public enum ConditionOperator
    {
        // A field is visible/enabled only if all conditions are true.
        And,
        // A field is visible/enabled if at least ONE condition is true.
        Or,
    }
}
