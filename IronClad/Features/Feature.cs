
using System.Text.Json;
using System.Text.Json.Nodes;
using System.Text.Json.Serialization.Metadata;

namespace Mohr.Jonas.IronClad.Features;

public abstract class Feature<TData>(JsonObject @object, JsonTypeInfo<TData> jsonTypeInfo) : IFeature
{
    protected TData FeatureConfiguration = JsonSerializer.Deserialize(@object, jsonTypeInfo)!;

    public abstract void Apply(DevContainerBuilder devContainerBuilder);
}
