namespace Mohr.Jonas.IronClad;

public sealed class DevContainerBuilder(DevContainer? container = null)
{
    public readonly DevContainer Container = container ?? new();

    public DevContainerBuilder WithName(string name)
    {
        Container.Name = name;
        return this;
    }

    public DevContainerBuilder WithImage(string image)
    {
        Container.Image = image;
        return this;
    }

    public DevContainerBuilder WithDockerFile(string dockerFile)
    {
        Container.DockerFile = dockerFile;
        return this;
    }

    public DevContainerBuilder WithContext(string context)
    {
        Container.Context = context;
        return this;
    }

    public DevContainerBuilder WithBuild(DevContainerBuild build)
    {
        Container.Build = build;
        return this;
    }

    public DevContainerBuilder WithFeatures(Dictionary<string, object> features)
    {
        Container.Features = features;
        return this;
    }

    public DevContainerBuilder AddFeature(string name, object value)
    {
        Container.Features ??= new();
        Container.Features[name] = value;
        return this;
    }

    public DevContainerBuilder WithRunArgs(params string[] args)
    {
        Container.RunArgs = args.ToList();
        return this;
    }

    public DevContainerBuilder WithContainerEnv(string key, string value)
    {
        Container.ContainerEnv ??= new();
        Container.ContainerEnv[key] = value;
        return this;
    }

    public DevContainerBuilder WithRemoteEnv(string key, string value)
    {
        Container.RemoteEnv ??= new();
        Container.RemoteEnv[key] = value;
        return this;
    }

    public DevContainerBuilder AddMount(string mount)
    {
        Container.Mounts ??= new();
        Container.Mounts.Add(mount);
        return this;
    }

    public DevContainerBuilder WithInit(bool value = true)
    {
        Container.Init = value;
        return this;
    }

    public DevContainerBuilder WithPrivileged(bool value = true)
    {
        Container.Privileged = value;
        return this;
    }

    public DevContainerBuilder AddCapability(string capability)
    {
        Container.CapAdd ??= new();
        Container.CapAdd.Add(capability);
        return this;
    }

    public DevContainerBuilder AddSecurityOpt(string option)
    {
        Container.SecurityOpt ??= new();
        Container.SecurityOpt.Add(option);
        return this;
    }

    public DevContainerBuilder WithRemoteUser(string user)
    {
        Container.RemoteUser = user;
        return this;
    }

    public DevContainerBuilder WithContainerUser(string user)
    {
        Container.ContainerUser = user;
        return this;
    }

    public DevContainerBuilder WithPostCreateCommand(object command)
    {
        Container.PostCreateCommand = command;
        return this;
    }

    public DevContainerBuilder WithPostStartCommand(object command)
    {
        Container.PostStartCommand = command;
        return this;
    }

    public DevContainerBuilder WithPostAttachCommand(object command)
    {
        Container.PostAttachCommand = command;
        return this;
    }

    public DevContainerBuilder WithOnCreateCommand(object command)
    {
        Container.OnCreateCommand = command;
        return this;
    }

    public DevContainerBuilder AddForwardPort(int port)
    {
        Container.ForwardPorts ??= new();
        Container.ForwardPorts.Add(port);
        return this;
    }

    public DevContainerBuilder WithWorkspaceFolder(string folder)
    {
        Container.WorkspaceFolder = folder;
        return this;
    }

    public DevContainerBuilder WithWorkspaceMount(string mount)
    {
        Container.WorkspaceMount = mount;
        return this;
    }

    public DevContainerBuilder WithCustomizations(DevContainerCustomizations customizations)
    {
        Container.Customizations = customizations;
        return this;
    }

    public DevContainer Build() => Container;
}

public sealed class DevContainerBuildBuilder
{
    private readonly DevContainerBuild _build = new();

    public DevContainerBuildBuilder WithDockerfile(string dockerfile)
    {
        _build.Dockerfile = dockerfile;
        return this;
    }

    public DevContainerBuildBuilder WithContext(string context)
    {
        _build.Context = context;
        return this;
    }

    public DevContainerBuildBuilder AddArg(string key, string value)
    {
        _build.Args ??= new();
        _build.Args[key] = value;
        return this;
    }

    public DevContainerBuildBuilder WithTarget(string target)
    {
        _build.Target = target;
        return this;
    }

    public DevContainerBuild Build() => _build;
}

public sealed class DevContainerVSCodeBuilder
{
    private readonly DevContainerVSCode _vscode = new();

    public DevContainerVSCodeBuilder AddSetting(string key, object value)
    {
        _vscode.Settings ??= new();
        _vscode.Settings[key] = value;
        return this;
    }

    public DevContainerVSCodeBuilder AddExtension(string extension)
    {
        _vscode.Extensions ??= new();
        _vscode.Extensions.Add(extension);
        return this;
    }

    public DevContainerVSCode Build() => _vscode;
}

public sealed class DevContainerCustomizationsBuilder
{
    private readonly DevContainerCustomizations _customizations = new();

    public DevContainerCustomizationsBuilder WithVSCode(DevContainerVSCode vscode)
    {
        _customizations.VSCode = vscode;
        return this;
    }

    public DevContainerCustomizationsBuilder AddExtension(string extension)
    {
        _customizations.Extensions ??= new();
        _customizations.Extensions.Add(extension);
        return this;
    }

    public DevContainerCustomizationsBuilder AddSetting(string key, object value)
    {
        _customizations.Settings ??= new();
        _customizations.Settings[key] = value;
        return this;
    }

    public DevContainerCustomizations Build() => _customizations;
}