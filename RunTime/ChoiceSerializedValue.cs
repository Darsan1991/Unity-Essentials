using System;

[Serializable]
public class ChoiceSerializedValue<TValueType1, TValueType2, TValueType3, TValueType4, TValueType5> : ChoiceSerializedValueWithType<object, TValueType1, TValueType2, TValueType3, TValueType4, TValueType5>
{
}

[Serializable]
public class ChoiceSerializedValue<TValueType1, TValueType2, TValueType3, TValueType4> : ChoiceSerializedValueWithType<
    object, TValueType1, TValueType2, TValueType3, TValueType4>
{
}

[Serializable]
public class
    ChoiceSerializedValue<TValueType1, TValueType2, TValueType3> : ChoiceSerializedValueWithType<object, TValueType1,
    TValueType2, TValueType3>
{
}

[Serializable]
public class
    ChoiceSerializedValue<TValueType1, TValueType2> : ChoiceSerializedValueWithType<object, TValueType1, TValueType2>
{
}