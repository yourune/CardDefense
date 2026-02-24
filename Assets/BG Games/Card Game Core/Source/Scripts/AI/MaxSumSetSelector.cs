using System;
using System.Collections.Generic;

namespace BG_Games.Card_Game_Core.AI
{
    class MaxSumSetSelector
    {
        public static List<int> FindSet(List<int> preSet, int energyBalance)
        {
            int n = preSet.Count;
            int[] dp = new int[energyBalance + 1];

            for (int i = 0; i < n; i++)
            {
                for (int j = energyBalance; j >= preSet[i]; j--)
                {
                    dp[j] = Math.Max(dp[j], dp[j - preSet[i]] + preSet[i]);
                }
            }

            List<int> resultSet = new List<int>();
            int k = energyBalance;

            for (int i = n - 1; i >= 0; i--)
            {
                if (k - preSet[i] >= 0 && dp[k] == dp[k - preSet[i]] + preSet[i])
                {
                    resultSet.Add(i);
                    k -= preSet[i];
                }
            }

            resultSet.Reverse();
            return resultSet;
        }

        public static List<int> FindSubsetWithMaxSum(List<int> set, int targetSum)
        {
            int n = set.Count;

            // Create a 2D array to store the subset sum information
            int[,] dp = new int[n + 1, targetSum + 1];

            // Initialize the first column to true
            for (int i = 0; i <= n; i++)
            {
                dp[i, 0] = 1;
            }

            // Build the subset sum array
            for (int i = 1; i <= n; i++)
            {
                for (int j = 1; j <= targetSum; j++)
                {
                    if (set[i - 1] <= j)
                    {
                        dp[i, j] = Math.Max(dp[i - 1, j], set[i - 1] + dp[i - 1, j - set[i - 1]]);
                    }
                    else
                    {
                        dp[i, j] = dp[i - 1, j];
                    }
                }
            }

            // Backtrack to find the elements in the subset
            List<int> subset = new List<int>();
            int iBacktrack = n;
            int jBacktrack = targetSum;

            while (iBacktrack > 0 && jBacktrack > 0)
            {
                if (dp[iBacktrack, jBacktrack] != dp[iBacktrack - 1, jBacktrack])
                {
                    subset.Add(iBacktrack - 1);
                    jBacktrack -= set[iBacktrack - 1];
                }
                iBacktrack--;
            }

            return subset;
        }
    }
}
