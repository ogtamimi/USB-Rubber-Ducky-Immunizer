using System;
using System.Collections.Generic;
using System.Linq;

namespace OGTTrust
{
    public class TypingSample
    {
        public List<DateTime> Timestamps { get; } = new List<DateTime>();
        public List<int> KeyCodes { get; } = new List<int>();
    }

    public static class TypingAnalyzer
    {
        // Simple heuristic: average keys/sec > threshold or very low variance => non-human
        public static bool IsHumanLike(TypingSample sample)
        {
            if (sample == null || sample.Timestamps.Count < 5)
                return true; // not enough data to judge

            var intervals = new List<double>();
            for (int i = 1; i < sample.Timestamps.Count; i++)
            {
                intervals.Add((sample.Timestamps[i] - sample.Timestamps[i - 1]).TotalSeconds);
            }

            double avg = intervals.Average();
            double keysPerSec = 1.0 / avg;
            double stddev = Math.Sqrt(intervals.Select(x => Math.Pow(x - avg, 2)).Average());

            // Heuristics
            if (keysPerSec > 15.0)
                return false;

            if (stddev < 0.02 && keysPerSec > 6.0)
                return false;

            // If no backspace in sample -> suspicious
            bool hasBackspace = sample.KeyCodes.Any(k => k == 8);
            if (!hasBackspace && keysPerSec > 8.0)
                return false;

            return true;
        }
    }
}
