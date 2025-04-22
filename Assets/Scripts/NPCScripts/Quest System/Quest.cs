//using System.Collections.Generic;
//using System.Linq;
//using System.Xml.Serialization;
//using Unity.VisualScripting;
//using UnityEditor;
//using UnityEngine;
//using UnityEngine.Events;

//public class Quest : ScriptableObject
//{
//    [System.Serializable]
//    public struct Info
//    {
//        public string Name;
//        public Sprite Icon;
//        public string Description;
//    }
//    [Header("Info")] public Info Information;
//    [System.Serializable]
//    public struct Stat
//    {
//        public int Currency;
//        public int XP;
//    }

//    [Header("Reward")] public Stat Reward = new Stat { Currency = 10, XP = 10 };

//    public bool Completed { get; private set; }
//    public QuestCompletedEvent QuestCompleted;

//    public abstract class QuestGoal : ScriptableObject
//    {
//        protected string Description;
//        public int CurrentAmount { get; protected set; }
//        public int RequiredAmount = 1;

//        public bool Completed { get; protected set; }
//        [HideInInspector] public UnityEvent GoalCompleted;
//        public virtual string GetDescription()
//        {
//            return Description;
//        }

//        public virtual void Initialize()
//        {
//            Completed = false;
//            GoalCompleted = new UnityEvent();
//        }
        
//        protected void Evaluate()
//        {
//            if(CurrentAmount >= RequiredAmount)
//            {
//                Complete();
//            }
//        }

//        private void Complete()
//        {
//            Completed = true;
//            GoalCompleted.Invoke();
//            GoalCompleted.RemoveAllListeners();
//        }

//        public void Skip()
//        {
//            //charge player currency
//        }
//    }

//    public List<QuestGoal> Goals;
    
//    public void Initialize()
//    {
//        Completed = false;
//        QuestCompleted = new QuestCompletedEvent();

//        foreach (var goal in Goals)
//        {
//            goal.Initialize();
//            goal.GoalCompleted.AddListener(call:delegate { CheckGoals(); });
//        }
//    }

//    private void CheckGoals()
//    {
//        //Might have an issue here
//        Completed = Goals.All(g => g.Completed);
//        if (Completed)
//        {
//            //give reward
//            QuestCompleted.Invoke(arg0:this);
//            QuestCompleted.RemoveAllListeners();
//        }
//    }
//}

//public class QuestCompletedEvent : UnityEvent<Quest> { }

//#if UNITY_EDITOR
//[CustomEditor(inspectedType: typeof(Quest))]
//public class QuestEditor: Editor
//{
//    SerializedProperty m_QuestInfoProperty;
//    SerializedProperty m_QuestStatProperty;

//    List<string> m_QuestGoalType;
//    SerializedProperty m_QuestGoalListProperty;

//    [MenuItem("Assets/Quest", priority = 0)]
//    public static void CreateQuest()
//    {
//        var newQuest = CreateInstance<Quest>();

//        ProjectWindowUtil.CreateAsset(newQuest, "quest.asset");
//    }

//    private void OnEnable()
//    {
//        m_QuestInfoProperty = serializedObject.FindProperty(nameof(Quest.Information));
//        m_QuestStatProperty = serializedObject.FindProperty(nameof(Quest.Reward));

//        m_QuestGoalListProperty = serializedObject.FindProperty(nameof(Quest.Goals));

//        var lookup:Type = typeof(Quest.QuestGoal);
//        m_QuestGoalType = System.AppDomain.CurrentDomain.GetAssemblies()
//            .SelectMany(assembly => assembly.GetTypes())
//            .Where(x: Type => x.IsAbstract && x.IsSubclassOf(lookup))
//            .Select(type => type.name)
//            .ToList();
//    }

//    public override void OnInspectorGUI()
//    {
//        var child:SerializedProperty = m_QuestGoalListProperty.Copy();
//        var depth:int = child.depth;
//        child.NextVisible(enterChildren: true);

//        EditorGUILayout.LabelField("Quest info", EditorStyles.boldLabel);
//        while(child.depth > depth)
//        {
//            EditorGUILayout.PropertyField(child, includeChildren: true);
//            child.NextVisible(enterChildren: false);
//        }
//        child = m_QuestStatProperty.Copy();
//        depth = child.depth;
//        child.NextVisible(enterChildren: true);

//        EditorGUILayout.LabelField("Quest reward", EditorStyles.boldLabel);
//        while (child.depth > depth)
//        {
//            EditorGUILayout.PropertyField(child, includeChildren: true);
//            child.NextVisible(enterChildren: false);
//        }

//     int choice = EditorGUILayout.Popup(label: "Add new Quest Goal", selectedIndex: -1, displayedOptions: m_QuestGoalType.ToArray());
//    if(choice != -1)
//    {
//        var newInstance = ScriptableObject.CreateInstance(m_QuestGoalType[choice]);

//        AssetDatabase.AddObjectToAsset(objectToAdd: newInstance, assetObject: target);

//        m_QuestGoalListProperty.InsertArrayElementAtIndex(m_QuestGoalListProperty.arraySize);
//        m_QuestGoalListProperty.GetArrayElementAtIndex(m_QuestGoalListProperty.arraySize - 1)
//            .objectReferenceValue = newInstance;
//    }
    
//        Editor ed = null;
//        int toDelete = -1;
//        for(int i = 0; int< m_questGoalListProperty.arrayaSize; ++i)
//        {
//            EditorGUILayout.BeginHorizontal();
//            EditorGUILayout.BeginVertical();
//            var item:SerializedProperty = m_QuestGoalListProperty.GetArrayElementAtIndex(i);
//            SerializedObject obj = new SerializedObject(item.objectReferenceValue);

//            Editor.CreateCachedEditor(item.objectReferenceValue, editorType: null, ref ed);

//            ed.OnInspectorGUI();
//            EditorGUILayout.EndVertical();

//            if (GUILayout.Button(text: "-", params options: GUILayout.Width(32)))
//            {
//                toDelete = i;
//            }
//            EditorGUILayout.EndHorizontal();
//        }

//        if(toDelete != -1)
//        {
//            var item:Object = m_QuestListProperty.GetArrayElementAtIndex(toDelete).objectReferenceValue;
//            DestroyImmediate(item, alloqDestoryingAssets: true);

//            m_QuestGoalListProperty.DeleteArrayElementAtIndex(toDelete);
//            m_QuestGoalListProperty.DeleteArrayElementAtIndex(toDelete);
//        }

//        serailziedObject.ApplyModifiedProperties();
//    }

    


   
//}
//#endif