using Cysharp.Threading.Tasks;
using R3;
using UnityEngine;

namespace _Project.Logic.Helpers
{
  public class Timer
  {
    private float _tickTime;
    private float _time;
    private bool _isEnd;

    public readonly Subject<Unit> TimerTick = new();
  
    public Timer(float tickTime)
    {
      _tickTime = tickTime;
      _isEnd = true;
    }

    public void StartTimer()
    {
      if (!_isEnd)
      {
        Debug.Log("Таймер уже запушен");
      }
      else
      {
        _time = 0;
        _isEnd = false;
        AsyncTimer();
      }
    }
  
    private async void AsyncTimer()
    {
      while (_time < _tickTime)
      {
        _time += Time.deltaTime;
        await UniTask.Yield(PlayerLoopTiming.Update);
      }
        
      _isEnd = true;
      TimerTick.OnNext(Unit.Default);
    }
  }
}