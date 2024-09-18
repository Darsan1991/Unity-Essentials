using System;
using UnityEngine;

[Serializable]
public class ChoiceSerializedValueWithType<TType, TValueType1, TValueType2,TValueType3>
    where TValueType1 : TType where TValueType2 : TType where TValueType3 : TType
{
    [SerializeField] private int _choice;
    [SerializeField] private TValueType1 _value1;
    [SerializeField] private TValueType2 _value2;
    [SerializeField] private TValueType3 _value3;

    public TType Value => _choice switch
    {
        0 => _value1,
        1 => _value2,
        2 => _value3,
        _ => throw new ArgumentOutOfRangeException()
    };
}

[Serializable]
public class ChoiceSerializedValueWithType<TType, TValueType1, TValueType2, TValueType3, TValueType4>
    where TValueType1 : TType where TValueType2 : TType where TValueType3 : TType where TValueType4 : TType
{
    [SerializeField] private int _choice;
    [SerializeField] private TValueType1 _value1;
    [SerializeField] private TValueType2 _value2;
    [SerializeField] private TValueType3 _value3;
    [SerializeField] private TValueType4 _value4;

    public TType Value => _choice switch
    {
        0 => _value1,
        1 => _value2,
        2 => _value3,
        3 => _value4,
        _ => throw new ArgumentOutOfRangeException()
    };
}

[Serializable]
public class ChoiceSerializedValueWithType<TType, TValueType1, TValueType2, TValueType3, TValueType4, TValueType5>
    where TValueType1 : TType
    where TValueType2 : TType
    where TValueType3 : TType
    where TValueType4 : TType
    where TValueType5 : TType
{
    [SerializeField] private int _choice;
    [SerializeField] private TValueType1 _value1;
    [SerializeField] private TValueType2 _value2;
    [SerializeField] private TValueType3 _value3;
    [SerializeField] private TValueType4 _value4;
    [SerializeField] private TValueType5 _value5;

    public TType Value => _choice switch
    {
        0 => _value1,
        1 => _value2,
        2 => _value3,
        3 => _value4,
        4 => _value5,
        _ => throw new ArgumentOutOfRangeException()
    };
}

[Serializable]
public class ChoiceSerializedValueWithType<TType, TValueType1, TValueType2>
    where TValueType1 : TType where TValueType2 : TType
{
    [SerializeField] private int _choice;
    [SerializeField] private TValueType1 _value1;
    [SerializeField] private TValueType2 _value2;

    public TType Value => _choice switch
    {
        0 => _value1,
        1 => _value2,
        _ => throw new ArgumentOutOfRangeException()
    };
}