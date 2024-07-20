using System.Linq;
using System.Reflection;
using UnityEditor;
using UnityEngine;

namespace DGames.Essentials.Unity.Editor
{
    [CustomEditor(typeof(MonoBehaviour),true)]
    public class Editor : DGames.Essentials.Editor.Editor
    {
        private FieldInfo[] _serializeInterfaces;

        protected override void OnEnable()
        {
            base.OnEnable();
            
            var type = target.GetType();
            _serializeInterfaces = SerializeInterfaceField.GetAllSerializedInterfaceFields(type).ToArray();
        }
        
               protected override void OnDrawEndOfContent()
              
                {
                    base.OnDrawEndOfContent();
                    if (_serializeInterfaces != null)
                    {
                        EditorGUI.BeginChangeCheck();
                        foreach (var fieldInfo in _serializeInterfaces)
                        {
                            var fName = fieldInfo.Name.Replace("_","");
                            var value = EditorGUILayout.ObjectField(char.ToUpper(fName.First())+ fName[1..],fieldInfo.GetValue(target) as Object, fieldInfo.FieldType, true);
                    
                            if ((Object)fieldInfo.GetValue(target) != value)
                            {
                                fieldInfo.SetValue(target, value);
                            }
                        }
                        // if (EditorGUI.EndChangeCheck())
                        // {
                        //     var property = serializedObject.FindProperty("fieldVsObjects");
                        //     property.arraySize +=1 ;
                        // }
                    }
                }
    }
}