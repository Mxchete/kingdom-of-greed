//using System.Collections;
//using System.Collections.Generic;
//using UnityEngine;

//public class BuildingGoal : Quest.QuestGoal
//{
//    public string Building;//name of building

//    // Start is called before the first frame update
//    public override string GetDescriptino()
//    {
//        return $"Build a {Building}";
//    }

//    public override void Initialize()
//    {
//        base.Initialize();
//        //figure out what evenmanager i need to use/same as the video?
//        EventManager.Instance.AddListener<BuildingGameEvent>(OnBuilding);
//    }

//    // Update is called once per frame
//    private void OnBuilding(BuildingGameEvent eventInfo)
//    {
//        if(eventInfo.BuildingName == Building)
//        {
//            CurrentAmount++;
//            Evaluate();
//        }
//    }
//}
