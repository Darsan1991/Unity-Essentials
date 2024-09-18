#if UNITY_EDITOR
using UnityEditor;

[CustomPropertyDrawer(typeof(ChoiceSerializedValueWithType<,,,,,>), true)]
public class FiveChoiceSerializeValueWithTypeDrawer : ChoiceSerializeValueWithTypeDrawer
{
    public override int ValueCount => 5;
}
#endif