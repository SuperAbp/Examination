using System;
using System.Collections.Generic;
using System.Text;

namespace SuperAbp.Exam;

public class RankingHelper
{
    /// <summary>
    /// 标准排名（并列、跳号）
    /// </summary>
    public static void AssignRank<T, TScore>(
        IList<T> items,
        Func<T, TScore> scoreSelector,
        Action<T, int> assignRank)
        where TScore : notnull
    {
        int rank = 0;
        int index = 0;

        TScore? lastScore = default;
        bool hasLast = false;

        foreach (var item in items)
        {
            index++;
            var score = scoreSelector(item);

            if (!hasLast || !EqualityComparer<TScore>.Default.Equals(lastScore!, score))
            {
                rank = index;
                lastScore = score;
                hasLast = true;
            }

            assignRank(item, rank);
        }
    }
}