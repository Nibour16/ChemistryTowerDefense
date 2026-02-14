using System;
using System.Collections.Generic;
using UnityEngine;

public class MapServiceRegistry : MonoBehaviour
{

    private Dictionary<Type, object> _services = new();

    public void RegisterAllServices()
    {
        var behaviours = FindObjectsByType<MonoBehaviour>(FindObjectsSortMode.None);

        foreach (var mb in behaviours)
        {
            var interfaces = mb.GetType().GetInterfaces();

            foreach (var type in interfaces)
            {
                if (typeof(IMapService).IsAssignableFrom(type) &&
                    type != typeof(IMapService))
                {
                    RegisterService(type, mb);
                }
            }
        }
    }

    private void RegisterService(Type type, MonoBehaviour mb)
    {
        if (!_services.ContainsKey(type))
            _services[type] = mb;
        else
            Debug.LogWarning($"Service {type.Name} already registered.");
    }

    public T GetService<T>() where T : class, IMapService
    {
        if (_services.TryGetValue(typeof(T), out var service))
            return service as T;

        Debug.LogError($"Service {typeof(T).Name} not found.");
        return null;
    }
}
