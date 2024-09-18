#if UNITY_EDITOR
using UnityEditor;

[CustomPropertyDrawer(typeof(ChoiceSerializedValueWithType<,,>), true)]
public class TwoChoiceSerializeValueWithTypeDrawer : ChoiceSerializeValueWithTypeDrawer
{
    public override int ValueCount => 2;
}
#endif