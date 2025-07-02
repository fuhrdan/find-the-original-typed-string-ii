//*****************************************************************************
//** 3333. Find the Original Typed String II                        leetcode **
//*****************************************************************************

#define MOD 1000000007
#define MAX_K 1001
#define min(a, b) ((a) < (b) ? (a) : (b))

int possibleStringCount(char* word, int k)
{
    int n = strlen(word);
    int* lens = (int*)malloc(sizeof(int) * n);
    int len = 0;

    for (int i = 0; i < n;)
    {
        char c = word[i];
        int left = i;
        while (i < n && word[i] == c) i++;
        lens[len++] = i - left;
    }

    long long res = 1;
    for (int i = 0; i < len; i++)
    {
        res = (res * lens[i]) % MOD;
    }

    if (len >= k)
    {
        free(lens);
        return (int)res;
    }

    int** dp = (int**)malloc(sizeof(int*) * 2);
    for (int i = 0; i < 2; i++)
    {
        dp[i] = (int*)calloc(k, sizeof(int));
    }

    dp[0][0] = 1;

    for (int i = 0; i < len; i++)
    {
        int* prev = dp[i % 2];
        int* curr = dp[(i + 1) % 2];
        memset(curr, 0, sizeof(int) * k);

        int* prefix = (int*)calloc(k + 1, sizeof(int));
        for (int j = 0; j < k; j++)
        {
            prefix[j + 1] = (prefix[j] + prev[j]) % MOD;
        }

        for (int used = 0; used < k; used++)
        {
            int min_required = len - i - 1;
            int max_pick = min(lens[i], k - used - min_required);
            if (max_pick <= 0) continue;

            int lo = used + 1;
            int hi = used + max_pick;
            if (hi >= k) hi = k - 1;

            if (lo < k)
            {
                curr[lo] = (curr[lo] + prev[used]) % MOD;
            }
            if (hi + 1 < k)
            {
                curr[hi + 1] = (curr[hi + 1] - prev[used] + MOD) % MOD;
            }
        }

        for (int j = 1; j < k; j++)
        {
            curr[j] = (curr[j] + curr[j - 1]) % MOD;
        }

        free(prefix);
    }

    int* final = dp[len % 2];
    int less = 0;
    for (int i = 0; i < k; i++)
    {
        less = (less + final[i]) % MOD;
    }

    for (int i = 0; i < 2; i++) free(dp[i]);
    free(dp);
    free(lens);

    return (int)((res - less + MOD) % MOD);
}