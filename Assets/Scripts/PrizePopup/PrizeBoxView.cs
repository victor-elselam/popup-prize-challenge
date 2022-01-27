using System;
using UnityEngine;
using UnityEngine.UI;

public class PrizeBoxView : MonoBehaviour
{
    public Action OnSpin;

    [SerializeField] private Text initialPrize;
    [SerializeField] private Text multiplier;
    [SerializeField] private Text totalResult;

    [SerializeField] private Button spinButton;

    private void Start()
    {
        spinButton.onClick.AddListener(() => OnSpin?.Invoke());
    }

    public void SetValues(int initialValue, int multiplierValue, int totalResultValue)
    {
        initialPrize.text = initialValue.ToString(); //todo: format to money method
        multiplier.text = multiplierValue.ToString(); //todo: format to money method
        totalResult.text = totalResultValue.ToString(); //todo: format to money method
    }
}
