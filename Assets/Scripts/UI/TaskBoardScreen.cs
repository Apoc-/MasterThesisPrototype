using System.Collections.Generic;
using Core;
using UnityEditor.VersionControl;
using UnityEngine;

namespace UI
{
    public class TaskBoardScreen : ScreenBehaviour
    {
        public Jun_TweenRuntime BackgroundTween;

        public TaskBehaviour YellowPrefab;
        public TaskBehaviour GreenPrefab;
        public TaskBehaviour RedPrefab;

        public TaskboardLane TodoLane;
        public TaskboardLane DoingLane;
        public TaskboardLane DoneLane;

        public List<TaskBehaviour> Tasks = new List<TaskBehaviour>();
        
        public void DoErrorShake()
        {
            BackgroundTween.Play();
        }
        
        public void CloseTaskBoardScreen()
        {
            GameManager.Instance.player.CanGiveMoveCommand = true;
            gameObject.SetActive(false);
        }

        public void CreateNewTask(NPC npc)
        {
            var pref = GetRandomPrefab();
            var task = Instantiate(pref, TodoLane.transform, true);

            task.Owner = npc.Name;
            
            Tasks.Add(task);
        }

        public void MoveTaskToLane(TaskBehaviour task, TaskboardLane lane)
        {
            task.transform.SetParent(lane.transform);
        }

        public TaskBehaviour GetRandomPrefab()
        {
            var r = Random.Range(0f, 100f);

            if (r > 90.0f) return RedPrefab;
            if (r > 60.0f) return GreenPrefab;
            
            return YellowPrefab;
        }
    }
}