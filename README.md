# IronClad - Devcontainer on steroids

## About

I love devcontainers for development. They keep my system clean and avoid the classic "it works on my machine" predicament.  
However, I often find myself missing certain features like the ability to easily develop gui applications over games.  
IronClad attempts to close this gap while remaining as close to the original experience as possible.  

## Usage

```shell
Description:
  Wrapper around devcontainer to allow for more high-level features

Usage:
  clad [command] [options]

Options:
  --cwd <cwd>                                        The working directory to operate in
  --config-path <config-path>                        The path of the config file to use, relative to the working directory
  --log-level <Debug|Error|Information|Off|Warning>  Log-level of the application [default: Off]
  -?, -h, --help                                     Show help and usage information
  --version                                          Show version information

Commands:
  up       Create a new container
  down     Stop and remove a container
  exec     Run a command in the container interactively
  run      Run a command in a container
  upgrade  Upgrade the lockfile
  build    Generate a devcontainer config with features applied
  watch    Watch config and rebuild on change
```

IronClad reads its configuration from a file located either at `.clad.json` or `.ironclad/clad.json`, relative to the current working directory.  
This working directory can be overwritten via the `--cwd` option.  
The relative file path can further be overwritten via the `--config-path` option.

The usual workflow is the following:
- Write a .clad.json config file. The syntax is explained further down below
- Generate a devcontainer-compatible file. This can be accomplished with:
    - `build`: This will build the devcontainer file and exit
    - `watch`: This will watch the .clad.json file and rebuild on change
    - `up`: This will invoke a build and also create a container from the generated config
- Generate a container from the generated config. This can either be done via the `up` command, via vscode, the devcontainer cli or any other tool

## Syntax

The .clad.json file uses the same syntax as a devcontainer config. The only change is the added `ironClad` block under `customizations`:

```json
{
  "image": "debian:latest",
  "features": {
    "ghcr.io/devcontainers/features/dotnet:2": {}
  },
  "customizations": {
    "ironClad": {
      "user": {},
      "git": {},
      "docker": {}
    }
  }
}
```

A json schema file is available [here](./schema.json).

The sample .clad.json file from above would produce this devcontainer config. There might be small differences on your system, since it depends on its configuration:

```json
{
  "build": {
    "dockerfile": ".ironClad.dockerfile"
  },
  "features": {
    "ghcr.io/devcontainers/features/dotnet:2": {}
  },
  "mounts": [
    "type=bind,src=/home/jonas/.gitconfig,dst=/home/clad/.gitconfig",
    "type=bind,src=/var/run/docker.sock,dst=/var/run/docker.sock"
  ],
  "containerUser": "clad",
  "customizations": {
    "ironClad": {
      "user": {},
      "git": {},
      "docker": {}
    }
  }
}
```

Further, it creates this dockerfile, courtesy of the user passthrough feature:

```docker
FROM debian:latest

RUN if getent passwd 1000; then userdel -f $(getent passwd 1000 | cut -d ":" -f 1); fi
RUN if getent group 100; then groupdel -f $(getent group 100 | cut -d ":" -f 1); fi

RUN groupadd -g 100 clad
RUN useradd -m -s /bin/bash -g 100 -u 1000 clad
```

## Features

Currently, the following features are supported:
- `x11`: Passthrough a host-provided x11 socket
- `wayland`: Passthrough a host-provided wayland socket
- `docker`: Passthrough a host-provided docker socket
- `pulseaudio`: Passthrough a host-provided pulseaudio socket
- `pipewire`: Passthrough a host-provided pipewire socket"
- `user`: Create a user named clad on the container with the same uid and gid as on the host
- `git`: Passthrough a host-provided .gitconfig
- `gpu`: Passthrough the host-provided dri folder