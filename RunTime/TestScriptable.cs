using System;
using System.Collections.Generic;
using DGames.Essentials.Attributes;
using DGames.Essentials.Unity;
using UnityEngine;

namespace DGames.Essentials
{
    [HideScriptField]
    [DashboardMessage("This Info will used in Test purposes adfasdfasd fsdafasfasdf adfasfasfdsaf asfasfasfas asdfasfsafsaf asfasfasfa asdafafa afsafasfas asfasfafsa asdfafasfasd fasfafas  fafadsfasf sfasfa as afasfasf sfa fs fs faf")]
    [DashboardResourceList(nameof(_items), tabPath: "Games/Test")]
    public class TestScriptable : ScriptableObject
    {
        [UseTab] [ForceExpand()] [SerializeField]
        private List<Item> _items = new();

        [Serializable]
        public struct Item
        {
            [Tab("Basic", order: 0)] public string name;
            [Tab("Basic")] public string age;
            [Tab("General", order: 1)] public Sprite image;
            [Tab("General")] [Range(0, 1f)] public float range;

            [Tab("Details")]
            [ScriptableSymbolsToggle(nameof(DetailNew.enable), "TEST", BuildTargetGroup.Android)]
            [ToggleGroup(nameof(DetailNew.enable))]
            public DetailNew detailNew;

            [Tab("Details")]
            [ScriptableSymbolsEnum(nameof(DetailNew2.sample), typeof(Sample), "DETAILS_",BuildTargetGroup.Android,"enable")]
            [ToggleGroup(nameof(DetailNew.enable))]
            public DetailNew2 detailNew2;

            // [Tab("Details",order:2)]public Detail detail;
            [HideInInspector] [SerializeField] private int _selectedTab;
        }

        [Serializable]
        public struct Detail
        {
            public string name;
            public int value;
        }


        [Serializable]
        public struct DetailNew
        {
            public bool enable;
            public string name;
            public int value;
        }
        
        [Serializable]
        public struct DetailNew2
        {
            public bool enable;
            public string name;
            public Sample sample;
        }
        
        public enum Sample
        {
            TEST_1,TEST_2,TEST_3
        }
        
    }
}