using System.Text.Json.Nodes;
using Mohr.Jonas.IronClad.Features.Impls;

namespace Mohr.Jonas.IronClad.Features;

public static class FeatureFactory
{
    public static IFeature GetFeatureByName(string name, JsonObject @object, string cwd) => name switch
    {
        "x11" => new X11PassthroughFeature(@object),
        "wayland" => new WaylandPassthroughFeature(@object),
        "docker" => new DockerPassthroughFeature(@object),
        "pulseaudio" => new PulsePassthroughFeature(@object),
        "pipewire" => new PipewirePassthroughFeature(@object),
        "user" => new UserPassthroughFeature(@object, cwd),
        "git" => new GitPassthroughFeature(@object),
        "gpu" => new GpuPassthroughFeature(@object),
        _ => throw new NotSupportedException($"Unknown feature '{name}'")
    };
}