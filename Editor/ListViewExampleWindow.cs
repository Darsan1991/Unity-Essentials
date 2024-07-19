using System;
using System.Collections.Generic;
using System.Linq;
using DGames.Presets;
using UnityEditor;
using UnityEditor.UIElements;
using UnityEngine;
using UnityEngine.UIElements;

namespace DGames.Essentials.Unity.Editor
{
    public class ScriptableEditor : MyCustomEditor<ScriptableObject,ScriptableEditor>
    {
        [MenuItem("Tools/Editor/Scriptables")]
        public static void Open()
        {
            ShowMyEditor(typeof(Presets.Presets));
        }
    }
    
    // public class SampleEditor : MyCustomEditor<ScriptableObject,SampleEditor>
    // {
    //     [MenuItem("Tools/Editor/Sample")]
    //     public static void Open()
    //     {
    //         ShowMyEditor(typeof(Presets.Presets));
    //     }
    // }

    public class ColorPresetsEditor : MyCustomEditor<ColorPresets,ColorPresetsEditor>
    {
        [MenuItem("Tools/Editor/ColorPresets")]
        public static void Open()
        {
            ShowMyEditor();
        }
    }
    
    public class TextPresetsEditor : MyCustomEditor<TextPresets,TextPresetsEditor>
    {
        [MenuItem("Tools/Editor/TextPresets")]
        public static void Open()
        {
            ShowMyEditor();
        }
    }

public class MyCustomEditor<T,TJ> : EditorWindow where TJ : MyCustomEditor<T,TJ>
{
  [SerializeField] private int m_SelectedIndex = -1;
  private VisualElement _rightPane;
  [SerializeField]private List<Type> _excludeTypes = new();
  private ListView _leftPane;

  public static void ShowMyEditor(params Type[] excludeTypes)
  {
      
      // This method is called when the user selects the menu item in the Editor
    MyCustomEditor<T,TJ> wnd = GetWindow<TJ>();
    wnd.minSize = new Vector2(450, 200);
    wnd.titleContent = new GUIContent(typeof(T).Name + " Window");
    
    wnd._excludeTypes.Clear();
    wnd._excludeTypes.AddRange(excludeTypes?.ToList() ?? new List<Type>());
    wnd.RefreshItems();
    // wnd.titleContent = new GUIContent("My Custom Editor");
    // Limit size of the window
  }
  

  public void CreateGUI()
  {
    // Get a list of all sprites in the project


    // Create a two-pane view with the left pane being fixed with
    var splitView = new TwoPaneSplitView(0, 250, TwoPaneSplitViewOrientation.Horizontal);

    // Add the panel to the visual tree by adding it as a child to the root element
    rootVisualElement.Add(splitView);

    // A TwoPaneSplitView always needs exactly two child elements
    _leftPane = new ListView
    {
        selectionType = SelectionType.Single
    };
    

    var element = new VisualElement();
    // element.Add(new PopupField<string>
    // {
    //     choices = new List<string> {"Hello","World","How"}
    // });
    // element.Add(new Box()
    // {
    //     style =
    //     {
    //         height = 40
    //     }
    // });
    element.Add(_leftPane);

    splitView.Add(element);
    _rightPane = new ScrollView(ScrollViewMode.VerticalAndHorizontal);
    _rightPane.style.paddingRight = 20;
    _rightPane.style.paddingTop = 20;
    _rightPane.style.paddingLeft = 20;
    splitView.Add(_rightPane);

    // Initialize the list view with all sprites' names
    _leftPane.makeItem = () =>
    {
        var box = new VisualElement
        {
            style =
            {
                flexDirection = FlexDirection.Row,
                flexGrow = 1f,
                flexShrink = 0f,
                flexBasis = 0f,
                paddingBottom = 5,
                borderBottomWidth = 1,
                borderBottomColor =  new StyleColor(new Color(0.15f,0.15f,0.15f))

            }
        };
        box.Add(new Label
        {
            style =
            {
                unityTextAlign = new StyleEnum<TextAnchor>(TextAnchor.MiddleLeft),
                paddingLeft = 20,
               
            }
        });
        
        return box;
        var label = new Label
        {
            style =
            {
                unityTextAlign = new StyleEnum<TextAnchor>(TextAnchor.MiddleLeft),
                paddingLeft = 20
            }
        };

        return label;
    };
    
    RefreshItems();


    // React to the user's selection
    _leftPane.onSelectionChange += OnSpriteSelectionChange;

    // Restore the selection index from before the hot reload
    _leftPane.selectedIndex = m_SelectedIndex;

    // Store the selection index when the selection changes
    _leftPane.onSelectionChange += (items) => { m_SelectedIndex = _leftPane.selectedIndex; };
  }

  private void RefreshItems()
  {
      var allObjectGuids = AssetDatabase.FindAssets($"t:{typeof(T).Name}", new[] { "Assets/Resources" });
      Debug.Log(_excludeTypes.Count);
      var allObjects = allObjectGuids
          .Select(guid => AssetDatabase.LoadAllAssetsAtPath((AssetDatabase.GUIDToAssetPath(guid)))).SelectMany(os=>os)
          .Where(item => _excludeTypes.All(t => !t.IsInstanceOfType(item)))
          .ToList();
      _leftPane.itemsSource = allObjects;
      _leftPane.bindItem = (item, index) => { ((Label)item.Children().First()).text = allObjects[index].name; };
  }

  private void OnSpriteSelectionChange(IEnumerable<object> selectedItems)
  {
    // Clear all previous content from the pane
    _rightPane.Clear();

    // Get the selected sprite
    var selectedSprite = selectedItems.First() as ScriptableObject;
    if (selectedSprite == null)
      return;

   
    var editor = UnityEditor.Editor.CreateEditor(selectedSprite);
    // Add the Image control to the right-hand pane
    _rightPane.Add(new IMGUIContainer(() => editor.OnInspectorGUI()));
    InspectorElement.FillDefaultInspector(_rightPane,editor.serializedObject, editor);
  }
}


}