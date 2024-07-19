using System;
using System.Collections.Generic;
using DGames.Essentials.Attributes;
using UnityEngine;

namespace DGames.Essentials
{
    [ScriptLogo,FooterLogo]
    public class TestScript:MonoBehaviour
    {
        // [SerializeField]private
        [HelpBox("Hello==8")]
        [ShortLabel(2)]
        [SerializeField] private List<string> _list = new();

        [Box,Required,HelpBox("Hello World"),SectionTitle("BEHAVIOR")]
        [SerializeField] private UnityEngine.MonoBehaviour _behaviour;

        [Box(true),Inline] [SerializeField] private TestStruct1 _test;
        [Box] [SerializeField] private Vector3 _vec;
        
        
        [Required]
        [SerializeField] private string _str;
        
        
        [SectionTitle("DESIGN")]
        [MinMax(0,1,true)]
        [SerializeField] [ShortLabel(1)]private float _f;

        [Tooltip("Change Probability"),Box]
        [SerializeField][NoLabel] [MinMax(0,1,true)]
        private Vector2 _minAndMax;
        
        [Serializable]
        public struct TestStruct1
        {
            public string test;
            [Condition(nameof(test),"show",ConditionType.Show)]public int testInt;
        }
        
        
        [Button]
        public void Hello()
        {
            Debug.Log(nameof(Hello));
        }

        private void Update()
        {
            if(Input.GetKeyDown(KeyCode.H))
                gameObject.hideFlags = HideFlags.HideInHierarchy | HideFlags.HideInInspector;
            
            if(Input.GetKeyDown(KeyCode.S))
                gameObject.hideFlags = HideFlags.None;
        }
    }
}