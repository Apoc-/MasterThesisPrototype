using System.Collections.Generic;
using System.Runtime.CompilerServices;
using Core;
using TMPro;
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

        public Tooltip TaskTooltip;
        
        public List<TaskBehaviour> Tasks = new List<TaskBehaviour>();
        
        public void DoErrorShake()
        {
            BackgroundTween.Play();
        }
        
        public void CloseTaskBoardScreen()
        {
            var player = GameManager.Instance.player;
            player.CurrentInteractTarget.FinishInteraction(player);
            player.CanGiveCommand = true;
            gameObject.SetActive(false);
        }

        public TaskBehaviour CreateNewTask(NPC npc)
        {
            var pref = GetRandomPrefab();
            var task = Instantiate(pref, TodoLane.transform, false);

            task.Owner = npc;
            Tasks.Add(task);
            
            MoveTaskToLane(task, TodoLane);
            return task;
        }

        public void MoveTaskToLane(TaskBehaviour task, TaskboardLane lane)
        {
            task.transform.SetParent(lane.transform);
            task.CurrentLane = lane;
            if (lane.transform.childCount > lane.MaxTasks)
            {
                var child = lane.transform.GetChild(0).gameObject;
                child.SetActive(false);
                Destroy(child);
            }
        }

        public TaskBehaviour GetRandomPrefab()
        {
            var r = Random.Range(0f, 100f);

            if (r > 90.0f) return RedPrefab;
            if (r > 60.0f) return GreenPrefab;
            
            return YellowPrefab;
        }

        public void ProgressTask(TaskBehaviour task)
        {
            if (task.CurrentLane == TodoLane)
            {
                MoveTaskToLane(task, DoingLane);
                
                task.IsStarted = true;
                task.StartTime = GameManager.Instance.Clock.GetTime().ToString();
            } 
            else if (task.CurrentLane == DoingLane)
            {
                MoveTaskToLane(task, DoneLane);

                task.EndTime = GameManager.Instance.Clock.GetTime().ToString();
                task.IsDocumented = true;
                task.IsTested = true;
                task.IsProgrammed = true;
            }
        }
    }
}