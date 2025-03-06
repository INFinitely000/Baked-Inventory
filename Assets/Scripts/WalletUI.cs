using TMPro;
using UnityEngine;


public class WalletUI : MonoBehaviour
{
    [SerializeField] private TextMeshProUGUI m_coinsBar;


    private void OnEnable()
    {
        Wallet.Changed += OnChanged;
    }

    private void OnDisable()
    {
        Wallet.Changed -= OnChanged;
    }



    private void OnChanged(int difference)
    {
        m_coinsBar.text = Wallet.Coins.ToString();
    }
}
