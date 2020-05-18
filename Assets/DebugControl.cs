using UnityEngine;
using UnityEngine.UI;

public class DebugControl : MonoBehaviour
{
    [SerializeField] Slider[] _Sliders;
    [SerializeField] TMPro.TextMeshProUGUI[] _Values;

    private void Awake()
    {
        _Sliders[0].onValueChanged.AddListener((value) => { _Values[0].text = value.ToString("F2"); });
        _Sliders[1].onValueChanged.AddListener((value) => { _Values[1].text = value.ToString("F2"); });
        _Sliders[2].onValueChanged.AddListener((value) => { _Values[2].text = value.ToString("F2"); });
        _Sliders[3].onValueChanged.AddListener((value) => { _Values[3].text = value.ToString("F2"); });
        _Sliders[4].onValueChanged.AddListener((value) => { _Values[4].text = value.ToString("F2"); });
        _Sliders[5].onValueChanged.AddListener((value) => { _Values[5].text = value.ToString("F2"); });

        _Sliders[0].value = FindObjectOfType<PlayerGamepad>()._XForce;
        _Sliders[1].value = FindObjectOfType<PlayerGamepad>()._DownForce;
        _Sliders[2].value = FindObjectOfType<PlayerGamepad>()._UpForceWhenGoingUp;
        _Sliders[3].value = FindObjectOfType<PlayerGamepad>()._UpForceWhenGoingDown;
        _Sliders[4].value = FindObjectOfType<PlayerGamepad>()._TimeOnGroundBeforeBigJump;
        _Sliders[5].value = FindObjectOfType<PlayerGamepad>()._BigJumpForce;
    }

    public void Close()
    {
        FindObjectOfType<PlayerGamepad>()._XForce = _Sliders[0].value;
        FindObjectOfType<PlayerGamepad>()._DownForce = _Sliders[1].value;
        FindObjectOfType<PlayerGamepad>()._UpForceWhenGoingUp = _Sliders[2].value;
        FindObjectOfType<PlayerGamepad>()._UpForceWhenGoingDown = _Sliders[3].value;
        FindObjectOfType<PlayerGamepad>()._TimeOnGroundBeforeBigJump = _Sliders[4].value;
        FindObjectOfType<PlayerGamepad>()._BigJumpForce = _Sliders[5].value;
    }

    public void Restart()
    {
        Application.LoadLevel(0);
    }
}