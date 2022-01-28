using Assets.Scripts.PrizePopup;
using UnityEngine;
using UnityEngine.UI;

public class WheelPieceView : MonoBehaviour
{
    public int Number { get; private set; }
    public float Angle;

    [SerializeField] private Text numberField;
    [SerializeField] private Image image;
    [SerializeField] private Canvas overrideCanvas;

    public void SetNumber(PieceViewModel pieceView)
    {
        Number = pieceView.Value;

        numberField.text = $"{pieceView.Value}X";
        image.sprite = pieceView.Sprite;

        if (pieceView.Value >= 10)
        {
            if (overrideCanvas)
            {
                overrideCanvas.overrideSorting = true;
                overrideCanvas.sortingOrder = 5;
            }
        }
        else
        {
            if (overrideCanvas)
            {
                overrideCanvas.overrideSorting = false;
            }
        }
    }

    public void SetAngle(float angle)
    {
        Angle = Mathf.Abs(angle);
        transform.localEulerAngles = new Vector3(0, 0, angle);
    }
}
