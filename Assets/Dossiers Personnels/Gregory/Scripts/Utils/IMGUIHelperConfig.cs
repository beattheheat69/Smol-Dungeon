using UnityEngine;

[CreateAssetMenu(fileName = "IMGUIHelperConfig", menuName = "Scriptable Objects/IMGUIHelperConfig")]
public class IMGUIHelperConfig : ScriptableObject
{
    [field: SerializeField, Header("Margins"), Range(0f, 50f)]
    public float MarginLeft { get; private set; } = 10f;

    [field: SerializeField, Range(0f, 50f)]
    public float MarginRight { get; private set; } = 10f;

    [field: SerializeField, Range(0f, 50f)]
    public float MarginTop { get; private set; } = 10f;

    [field: SerializeField, Range(0f, 50f)]
    public float MarginBottom { get; private set; } = 10f;


    [field: SerializeField, Space, Header("Text"), Range(10f, 30f)]
    public float TextHeight { get; private set; } = 20f;

    [field: SerializeField, Range(0f, 50f)]
    public float InterlineSpacing { get; private set; } = 20f;


    [field: SerializeField, Space, Header("Buttons"), Range(50f, 200f)]
    public float ButtonWidth { get; private set; } = 100f;

    [field: SerializeField, Range(50f, 200f)]
    public float ButtonHeight { get; private set; } = 30f;
}
