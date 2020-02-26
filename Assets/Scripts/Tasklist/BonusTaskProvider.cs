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
            if (_bonusTaskQueue.Count == 0)
            {
                var target = (int) Math.Pow(10, GameManager.Instance.Day);
                var bonus = 5;
                if (target >= 1000)
                {
                    bonus = 15;
                } else if (target >= 100)
                {
                    bonus = 10;
                }
                
                EnqueuePlayerWorkTask(target, bonus);
            }
            
            return _bonusTaskQueue.Dequeue();
        }
        
        public static void EnqueueTodoTask(int target)
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
        
        public static void EnqueuePlayerWorkTask(int target, int bonus)
        {
            var task = new BonusTask(
                $"Arbeite an deinem Schreibtisch",
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

        public static void EnqueueReachProgressTask(int target)
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
                10,
                target,
                SetCurrentToProgressValue);
            
            _bonusTaskQueue.Enqueue(task);
        }

    }
}