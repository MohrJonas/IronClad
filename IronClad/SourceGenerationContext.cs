using System.Text.Json;
using System.Text.Json.Nodes;
using System.Text.Json.Serialization;
using Mohr.Jonas.IronClad.Features.Impls;

namespace Mohr.Jonas.IronClad;

[JsonSourceGenerationOptions(WriteIndented = true, DefaultIgnoreCondition = JsonIgnoreCondition.WhenWritingNull)]
[JsonSerializable(typeof(DevContainer))]
[JsonSerializable(typeof(DockerPassthroughFeatureSettings))]
[JsonSerializable(typeof(PipewirePassthroughFeatureSettings))]
[JsonSerializable(typeof(PulsePassthroughFeatureSettings))]
[JsonSerializable(typeof(UserPassthroughFeatureSettings))]
[JsonSerializable(typeof(WaylandPassthroughFeatureSettings))]
[JsonSerializable(typeof(X11PassthroughFeatureSettings))]
[JsonSerializable(typeof(GitPassthroughFeatureSettings))]
[JsonSerializable(typeof(GpuPassthroughFeatureSettings))]
[JsonSerializable(typeof(JsonObject))]
[JsonSerializable(typeof(JsonElement))]
internal partial class SourceGenerationContext : JsonSerializerContext { }