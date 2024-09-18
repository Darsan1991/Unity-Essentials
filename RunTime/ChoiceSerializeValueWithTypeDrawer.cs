#if UNITY_EDITOR

// [CustomPropertyDrawer(typeof(ChoiceSerializedValueWithType<,,>), true)]
using System.Collections.Generic;
using System.Linq;
using DGames.Essentials.EditorHelpers;
using UnityEditor;
using UnityEngine;

public abstract class ChoiceSerializeValueWithTypeDrawer : PropertyDrawer
{
    public abstract int ValueCount { get; } 

    public override void OnGUI(Rect position, SerializedProperty property, GUIContent label)
    {
        var choice = property.FindPropertyRelative("_choice");


        var selected = property.FindPropertyRelative("_value" + (choice.intValue + 1));

        EditorGUI.BeginProperty(position, label, property);
        property.isExpanded = EditorGUI.Foldout(new Rect(position.x, position.y, 10, EditorGUIUtility.singleLineHeight),
            property.isExpanded, GUIContent.none);
        var choiceRect = new Rect(position.x, position.y, position.width, EditorGUIUtility.singleLineHeight);

        var type = property.GetFieldInfo().FieldType;

        Debug.Log("type:" + type);


        choice.intValue = EditorGUI.Popup(choiceRect, property.displayName, choice.intValue,
            type.GetGenericArguments().TakeLast(ValueCount).Select(t => t.Name).ToArray());
        if (property.isExpanded)
        {
            EditorGUI.indentLevel++;
            var valueRect =new Rect(position.x,
                position.y + EditorGUIUtility.singleLineHeight + EditorGUIUtility.standardVerticalSpacing,
                position.width,
                EditorGUI.GetPropertyHeight(selected, true));

            if (selected.hasChildren && selected.propertyType == SerializedPropertyType.Generic && !selected.isArray )
            {
                // EditorGUI.PropertyField(valueRect, selected, GUIContent.none,true);
                DrawProperties(selected.GetPropertyWithDefaultChildren(), valueRect);
            }
            else if (selected.isArray && selected.propertyType != SerializedPropertyType.String)
            {
                EditorGUI.PropertyField(valueRect, selected,new GUIContent("Values"), true);
            }
            else
            {
                valueRect.width -= EditorGUIUtility.labelWidth - EditorGUI.indentLevel * 15;
                valueRect.x += EditorGUIUtility.labelWidth - EditorGUI.indentLevel * 15;
                EditorGUI.PropertyField(valueRect, selected, GUIContent.none, true);
            }

            EditorGUI.indentLevel--;
        }

        EditorGUI.EndProperty();
    }

    public static void DrawProperties(IEnumerable<SerializedProperty> properties, Rect position)
    {
        foreach (var property in properties)
        {
            EditorGUI.PropertyField(position, property, true);
            position.y += EditorGUI.GetPropertyHeight(property, true) + EditorGUIUtility.standardVerticalSpacing;
        }
    }


    public override float GetPropertyHeight(SerializedProperty property, GUIContent label)
    {
        var choice = property.FindPropertyRelative("_choice");


        var selected = property.FindPropertyRelative("_value" + (choice.intValue + 1));
        selected.isExpanded = true;
        return EditorGUIUtility.singleLineHeight + (property.isExpanded
            ? EditorGUIUtility.standardVerticalSpacing + EditorGUI.GetPropertyHeight(selected, true)
            : 0);
    }

}


#endif