using System;
using System.Collections.Generic;

namespace _Project.Logic.DTO
{
  public class PlayerDTO
  {
    private readonly Dictionary<String, object> _playerStats = new();
  
    public T GetData<T>(String dataName)
    {
      if (_playerStats.TryGetValue(dataName, out var value) && value is T typedValue)
      {
        return typedValue;
      }
      throw new InvalidCastException($"Unable to cast value of '{dataName}' to type {typeof(T)}.");
    }

    public void SetStat(String dataName, object data) => _playerStats[dataName] = data;
  }
}