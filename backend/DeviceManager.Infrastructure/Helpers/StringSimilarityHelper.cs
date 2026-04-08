using System;

namespace DeviceManager.Infrastructure.Helpers;

/// <summary>
/// Utility to calculate similarity between strings using modern algorithms.
/// </summary>
public static class StringSimilarityHelper
{
    /// <summary>
    /// Calculates the Levenshtein distance between two strings.
    /// The number of edits required to change one word into another.
    /// </summary>
    public static int GetLevenshteinDistance(string s, string t)
    {
        if (string.IsNullOrEmpty(s)) return string.IsNullOrEmpty(t) ? 0 : t.Length;
        if (string.IsNullOrEmpty(t)) return s.Length;

        int n = s.Length;
        int m = t.Length;
        int[,] d = new int[n + 1, m + 1];

        for (int i = 0; i <= n; d[i, 0] = i++) ;
        for (int j = 0; j <= m; d[0, j] = j++) ;

        for (int i = 1; i <= n; i++)
        {
            for (int j = 1; j <= m; j++)
            {
                int cost = (t[j - 1] == s[i - 1]) ? 0 : 1;
                d[i, j] = Math.Min(
                    Math.Min(d[i - 1, j] + 1, d[i, j - 1] + 1),
                    d[i - 1, j - 1] + cost);
            }
        }
        return d[n, m];
    }

    /// <summary>
    /// Returns a similarity score between 0 and 1.
    /// 1.0 = Exact match.
    /// 0.0 = Completely different.
    /// </summary>
    public static double GetSimilarity(string s, string t)
    {
        if (string.IsNullOrEmpty(s) || string.IsNullOrEmpty(t)) return 0;
        
        s = s.ToLowerInvariant().Trim();
        t = t.ToLowerInvariant().Trim();

        if (s == t) return 1.0;

        int distance = GetLevenshteinDistance(s, t);
        int maxLength = Math.Max(s.Length, t.Length);

        // Normalize to a 0-1 range
        return 1.0 - ((double)distance / maxLength);
    }
}
