using System;
using System.Collections.Generic;

public static class ServiceLocator
{
    private static readonly Dictionary<Type, object> _services = new();

    public static void Register<T>(T service) where T : class
    {
        var type = typeof(T);

        if (service == null)
            throw new ArgumentNullException(nameof(service));

        _services[type] = service;
    }

    public static T GetRequired<T>() where T : class
        => Resolve<T>() ?? throw new Exception($"{typeof(T).Name} not registered.");

    public static T Get<T>() where T : class
        => Resolve<T>();

    private static T Resolve<T>() where T : class
    {
        var type = typeof(T);

        if (_services.TryGetValue(type, out var service))
            return service as T;

        return null;
    }

    public static void Unregister<T>() where T : class
    {
        _services.Remove(typeof(T));
    }

    public static bool IsRegistered<T>() where T : class
    {
        return _services.ContainsKey(typeof(T));
    }

    public static void ClearAll()
    {
        _services.Clear();
    }
}
