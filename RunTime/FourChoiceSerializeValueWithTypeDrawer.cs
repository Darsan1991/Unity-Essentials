#if UNITY_EDITOR
using UnityEditor;

[CustomPropertyDrawer(typeof(ChoiceSerializedValueWithType<,,,,>), true)]
public class FourChoiceSerializeValueWithTypeDrawer : ChoiceSerializeValueWithTypeDrawer
{
    public override int ValueCount => 4;
}
#endif