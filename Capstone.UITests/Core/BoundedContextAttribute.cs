namespace Capstone.UITests.Core;

[AttributeUsage(AttributeTargets.Class | AttributeTargets.Method)]
public class BoundedContextAttribute : Attribute
{
    public string Value { get; }

    public BoundedContextAttribute(string value)
    {
        Value = value;
    }
}
