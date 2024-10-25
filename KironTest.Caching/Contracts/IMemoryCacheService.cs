﻿namespace KironTest.Caching.Contracts
{
    public interface IMemoryCacheService
    {
        T Get<T>(string key);
        void Set<T>(string key, T value, TimeSpan expiration);
        void Remove(string key);
    }
}
