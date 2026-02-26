namespace Mohr.Jonas.IronClad.Exceptions;

public sealed class FeatureConfigurationException<TFeature>(string message)
    : Exception($"Error while configuring feature '{typeof(TFeature).Name}': {message}");