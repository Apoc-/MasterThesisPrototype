using System;
using System.Collections.Generic;
using Core;
using UnityEngine;
using Random = UnityEngine.Random;

namespace UI
{
    public class TaskBoardScreen : ScreenBehaviour
    {
        public GameObject TaskboardBackground;
        private Jun_TweenRuntime _errorTween;
        private Jun_TweenRuntime _successTween;

        public TaskBehaviour YellowPrefab;
        public TaskBehaviour GreenPrefab;
        public TaskBehaviour RedPrefab;

        public TaskboardLane TodoLane;
        public TaskboardLane DoingLane;
        public TaskboardLane DoneLane;

        public TaskboardInteractible TaskboardInteractible;
        
        public Tooltip TaskTooltip;
        
        public List<TaskBehaviour> Tasks = new List<TaskBehaviour>();

        private void OnEnable()
        {
            GameManager.Instance.GameSpeedController.Pause();
            var tweens = TaskboardBackground.GetComponentsInChildren<Jun_TweenRuntime>();
            _errorTween = tweens[0];
            _successTween = tweens[1];
        }

        private void OnDisable()
        {
            if(_isQuitting) return;
            
            var gsc = GameManager.Instance.GameSpeedController;
            gsc.UnPause();
        }

        public void DoErrorShake()
        {
            _errorTween.Play();
        }
        
        public void DoSuccessShake()
        {
            _successTween.Play();
        }
        
        public void CloseTaskBoardScreen()
        {
            var player = GameManager.Instance.player;
            player.CurrentInteractTarget.FinishInteraction(player);
            player.EnableCommands();
            gameObject.SetActive(false);
        }

        public TaskBehaviour CreateNewTask()
        {
            var pref = YellowPrefab;
            var task = Instantiate(pref, TodoLane.transform, false);

            task.Description = GenerateTaskDescription();
            
            Tasks.Add(task);
            
            MoveTaskToLane(task, TodoLane);
            return task;
        }

        private string GenerateTaskDescription()
        {
            var prefixes = Resources.Load<TextAsset>("Tasks/Prefixes").text.Split('\n');
            var suffixes = Resources.Load<TextAsset>("Tasks/Suffixes").text.Split('\n');

            var pre = prefixes[Random.Range(0, prefixes.Length)];
            var suf = suffixes[Random.Range(0, suffixes.Length)];
            pre = pre.Replace("\r", "").Replace("\n", "");
            suf = suf.Replace("\r", "").Replace("\n", "");

            return pre + " " + suf;
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

        public void ProgressTask(TaskBehaviour task, NPC npc)
        {
            if (task.CurrentLane == TodoLane)
            {
                MoveTaskToLane(task, DoingLane);
                
                task.IsStarted = true;
                task.StartTime = GameManager.Instance.Clock.GetTime().ToString();
                if(task.Owner == null) task.Owner = npc;
            } 
            else if (task.CurrentLane == DoingLane)
            {
                MoveTaskToLane(task, DoneLane);

                if(task.StartTime == "") task.StartTime = GameManager.Instance.Clock.GetTime().ToString();
                task.EndTime = GameManager.Instance.Clock.GetTime().ToString();
                task.IsStarted = true;
                task.IsDocumented = true;
                task.IsTested = true;
                task.IsProgrammed = true;
            }
        }
    }
}