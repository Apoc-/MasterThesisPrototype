using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.CompilerServices;

namespace Tasklist
{
    public static class BonusTaskProvider
    {
        private static Queue<BonusTask> _bonusTaskQueue = new Queue<BonusTask>();
        
        public static BonusTask GetNextBonusTask()
        {
            return _bonusTaskQueue.Dequeue();
        }
        
        public static void EnqueueImpedimentsTask(int target)
        {
            var task = new BonusTask("Räume Impediments aus dem Weg", 
                BonusTaskType.Todo,
                target*5,
                target);
            
            _bonusTaskQueue.Enqueue(task);
        }

        public static void EnqueueTaskboardTask(int target)
        {
            var task = new BonusTask("Korrigiere Fehler auf dem Taskboard", 
                BonusTaskType.Taskboard,
                target*5,
                target);
            
            _bonusTaskQueue.Enqueue(task);
        }
        
        public static void EnqueuePlayerWorkTask(string text, int target, int bonus)
        {
            var task = new BonusTask(
                $"(Büroarbeit) {text}",
                BonusTaskType.MakeProgress,
                bonus,
                target);

            _bonusTaskQueue.Enqueue(task);
        }
        
        public static void EnqueueReadWikiTask(int target)
        {
            var task = new BonusTask(
                $"Lies {target} Seiten im Wiki",
                BonusTaskType.Wiki,
                5*target,
                target);
            
            _bonusTaskQueue.Enqueue(task);
        }

        public static void EnqueueReachProgressTask(int target, int bonus)
        {
            void SetCurrentToProgressValue(BonusTask thisTask)
            {
                var current = GameManager.Instance
                    .Company.CompanyScores.First(score => score.Name == "Fortschritt").Value;

                thisTask.CurrentCount = current;
            }
            
            var task = new BonusTask(
                $"Erreiche einen Fortschritt von {target}",
                BonusTaskType.ReachProgress,
                bonus,
                target,
                SetCurrentToProgressValue);
            
            _bonusTaskQueue.Enqueue(task);
        }

        public static bool HasTasksQueued()
        {
            return _bonusTaskQueue.Count > 0;
        }
    }
}