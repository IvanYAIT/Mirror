using TMPro;
using UnityEngine;

public class PlayerUI : MonoBehaviour
{
    public TextMeshProUGUI PlayerText;

    public void SetPlayer(string name)
    {
        PlayerText.text = name;
    }
}