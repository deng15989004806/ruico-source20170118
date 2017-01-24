using System;

namespace Ruico.Infrastructure.Utility.Caching
{
    public interface ICacheManager
    {
        T Get<T>(string key);

        void Set(string key, object value, int minutes);

        void Set(string key, object value, int minutes, bool isAbsoluteExpiration,
            Action<string, object, string> onRemove);

        bool IsSet(string key);

        void Remove(string key);

        void RemoveByPattern(string pattern);

        void ClearAll();
    }
}
