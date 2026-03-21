namespace Mohr.Jonas.IronClad.Features.Dependence;

[AttributeUsage(AttributeTargets.Class, AllowMultiple = false, Inherited = false)]
public sealed class RequiresFeatureAttribute(params Type[] featureTypes) : Attribute
{
    public Type[] FeatureTypes { get; } = featureTypes;
}