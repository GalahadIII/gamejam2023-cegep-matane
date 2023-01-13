using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class InteractionGUI : MonoBehaviour
{
    public static InteractionGUI Inst { get; private set; }
    private InteractionGUI() { Inst = this; }
    private void OnEnable()
    {
        Inst = this;
    }

    public IPosition Active = null;
    
    public RectTransform t;
    public Image Background;
    public TextMeshProUGUI Text;
    
    public int Timer = 0;
    public int HideAfter = 5;

    public static void Display()
    {
        Inst.DisplayHint();
    }

    protected void DisplayHint()
    {
        transform.position = (Vector2)Camera.main!.WorldToScreenPoint(Active.WorldPosition);
        gameObject.SetActive(true);
        Timer = 0;
    }

    private void FixedUpdate()
    {
        gameObject.SetActive(++Timer < HideAfter);
    }
}
