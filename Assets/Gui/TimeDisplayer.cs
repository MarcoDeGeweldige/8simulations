using TMPro;
using UnityEngine;

public class TimeDisplayer : MonoBehaviour
{
    // Start is called before the first frame update

    public TextMeshProUGUI textVak;

    public void RenderValue(float val)
    {
        textVak.SetText(val.ToString());

    }
}
