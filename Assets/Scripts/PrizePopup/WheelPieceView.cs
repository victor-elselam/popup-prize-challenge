using UnityEngine;
using UnityEngine.UI;

public class WheelPieceView : MonoBehaviour
{
    public int Number { get; private set; }
    [SerializeField] private Text numberField;
    [SerializeField] private Image image;

    public void SetNumber(PieceViewModel pieceView)
    {
        numberField.text = pieceView.Value.ToString();
        image.sprite = pieceView.Sprite;
    }
}
