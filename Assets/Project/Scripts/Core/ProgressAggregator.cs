using System;
using System.Collections.Generic;
using System.Linq;

public class ProgressAggregator
{
    private readonly Dictionary<string, float> _progressTrackers = new();
    private readonly Action<float> _onProgressChanged;

    public ProgressAggregator(Action<float> onProgressChanged)
    {
        _onProgressChanged = onProgressChanged;
    }

    public void RegisterTracker(string key)
    {
        _progressTrackers[key] = 0f;
    }

    public void UpdateProgress(string key, float progress)
    {
        if (_progressTrackers.ContainsKey(key))
        {
            _progressTrackers[key] = progress;
            var averageProgress = _progressTrackers.Values.Average();
            _onProgressChanged?.Invoke(averageProgress);
        }
    }
}