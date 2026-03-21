using System.IO.Compression;
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
        "kvm" => new KvmPassthroughFeature(@object),
        _ => throw new NotSupportedException($"Unknown feature '{name}'")
    };

    public static string GetFeatureNameByType(Type featureType)
    {
        if (featureType == typeof(X11PassthroughFeature))
            return "x11";
        else if (featureType == typeof(WaylandPassthroughFeature))
            return "wayland";
        else if (featureType == typeof(DockerPassthroughFeature))
            return "docker";
        else if (featureType == typeof(PulsePassthroughFeature))
            return "pulseaudio";
        else if (featureType == typeof(PipewirePassthroughFeature))
            return "pipewire";
        else if (featureType == typeof(UserPassthroughFeature))
            return "user";
        else if (featureType == typeof(GitPassthroughFeature))
            return "git";
        else if (featureType == typeof(GpuPassthroughFeature))
            return "gpu";
        else if (featureType == typeof(KvmPassthroughFeature))
            return "kvm";
        else throw new NotSupportedException($"Unknown feature '{featureType}'");
    }
}