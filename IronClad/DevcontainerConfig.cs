namespace Mohr.Jonas.IronClad;

using System.Text.Json.Nodes;
using System.Text.Json.Serialization;

public sealed record DevContainer
{
    [JsonPropertyName("name")]
    public string? Name { get; set; }

    [JsonPropertyName("image")]
    public string? Image { get; set; }

    [JsonPropertyName("dockerFile")]
    public string? DockerFile { get; set; }

    [JsonPropertyName("context")]
    public string? Context { get; set; }

    [JsonPropertyName("build")]
    public DevContainerBuild? Build { get; set; }

    [JsonPropertyName("features")]
    public Dictionary<string, object>? Features { get; set; }

    [JsonPropertyName("overrideFeatureInstallOrder")]
    public List<string>? OverrideFeatureInstallOrder { get; set; }

    [JsonPropertyName("runArgs")]
    public List<string>? RunArgs { get; set; }

    [JsonPropertyName("containerEnv")]
    public Dictionary<string, string>? ContainerEnv { get; set; }

    [JsonPropertyName("remoteEnv")]
    public Dictionary<string, string>? RemoteEnv { get; set; }

    [JsonPropertyName("mounts")]
    public List<string>? Mounts { get; set; }

    [JsonPropertyName("init")]
    public bool? Init { get; set; }

    [JsonPropertyName("privileged")]
    public bool? Privileged { get; set; }

    [JsonPropertyName("capAdd")]
    public List<string>? CapAdd { get; set; }

    [JsonPropertyName("securityOpt")]
    public List<string>? SecurityOpt { get; set; }

    [JsonPropertyName("remoteUser")]
    public string? RemoteUser { get; set; }

    [JsonPropertyName("containerUser")]
    public string? ContainerUser { get; set; }

    [JsonPropertyName("userEnvProbe")]
    public string? UserEnvProbe { get; set; }

    [JsonPropertyName("updateRemoteUserUID")]
    public bool? UpdateRemoteUserUID { get; set; }

    [JsonPropertyName("overrideCommand")]
    public bool? OverrideCommand { get; set; }

    [JsonPropertyName("postCreateCommand")]
    public object? PostCreateCommand { get; set; }

    [JsonPropertyName("postStartCommand")]
    public object? PostStartCommand { get; set; }

    [JsonPropertyName("postAttachCommand")]
    public object? PostAttachCommand { get; set; }

    [JsonPropertyName("onCreateCommand")]
    public object? OnCreateCommand { get; set; }

    [JsonPropertyName("shutdownAction")]
    public string? ShutdownAction { get; set; }

    [JsonPropertyName("forwardPorts")]
    public List<int>? ForwardPorts { get; set; }

    [JsonPropertyName("portsAttributes")]
    public Dictionary<string, DevContainerPortAttributes>? PortsAttributes { get; set; }

    [JsonPropertyName("otherPortsAttributes")]
    public DevContainerPortAttributes? OtherPortsAttributes { get; set; }

    [JsonPropertyName("hostRequirements")]
    public DevContainerHostRequirements? HostRequirements { get; set; }

    [JsonPropertyName("customizations")]
    public DevContainerCustomizations? Customizations { get; set; }

    [JsonPropertyName("workspaceFolder")]
    public string? WorkspaceFolder { get; set; }

    [JsonPropertyName("workspaceMount")]
    public string? WorkspaceMount { get; set; }

    [JsonPropertyName("initializeCommand")]
    public object? InitializeCommand { get; set; }

    [JsonPropertyName("waitFor")]
    public string? WaitFor { get; set; }

    [JsonPropertyName("containerSession")]
    public DevContainerSession? ContainerSession { get; set; }
}

public sealed record DevContainerBuild
{
    [JsonPropertyName("dockerfile")]
    public string? Dockerfile { get; set; }

    [JsonPropertyName("context")]
    public string? Context { get; set; }

    [JsonPropertyName("args")]
    public Dictionary<string, string>? Args { get; set; }

    [JsonPropertyName("target")]
    public string? Target { get; set; }
}

public sealed record DevContainerPortAttributes
{
    [JsonPropertyName("label")]
    public string? Label { get; set; }

    [JsonPropertyName("onAutoForward")]
    public string? OnAutoForward { get; set; }

    [JsonPropertyName("elevateIfNeeded")]
    public bool? ElevateIfNeeded { get; set; }

    [JsonPropertyName("requireLocalPort")]
    public bool? RequireLocalPort { get; set; }
}

public sealed record DevContainerHostRequirements
{
    [JsonPropertyName("cpus")]
    public int? Cpus { get; set; }

    [JsonPropertyName("memory")]
    public string? Memory { get; set; }

    [JsonPropertyName("storage")]
    public string? Storage { get; set; }
}

public sealed record DevContainerCustomizations
{
    [JsonPropertyName("vscode")]
    public DevContainerVSCode? VSCode { get; set; }

    [JsonPropertyName("codespaces")]
    public DevContainerCodespaces? Codespaces { get; set; }

    [JsonPropertyName("settings")]
    public Dictionary<string, object>? Settings { get; set; }

    [JsonPropertyName("extensions")]
    public List<string>? Extensions { get; set; }

    [JsonPropertyName("ironClad")]
    public JsonObject? IronClad { get; set; }
}

public sealed record DevContainerVSCode
{
    [JsonPropertyName("settings")]
    public Dictionary<string, object>? Settings { get; set; }

    [JsonPropertyName("extensions")]
    public List<string>? Extensions { get; set; }
}

public sealed record DevContainerCodespaces
{
    [JsonPropertyName("openFiles")]
    public List<string>? OpenFiles { get; set; }
}

public sealed record DevContainerSession
{
    [JsonPropertyName("initCommand")]
    public object? InitCommand { get; set; }

    [JsonPropertyName("startCommand")]
    public object? StartCommand { get; set; }
}
