using System;
using UnityEngine;


public static class Wallet
{
    public static int Coins { get; private set; }

    public static event Action<int> Changed;




    public static void Add(int coins)
    {
        if (coins < 1) throw new System.ArgumentOutOfRangeException(nameof(coins));

        Coins += coins;
    
        Changed?.Invoke(coins);
    }


    public static bool TryTake(int coins)
    {
        if (coins < 1) throw new System.ArgumentOutOfRangeException(nameof(coins));

        if (Coins < coins) return false;

        Coins -= coins;

        Changed?.Invoke(-coins);

        return true;
    }


    public static void Clear()
    {
        Coins = 0;
    }
}
