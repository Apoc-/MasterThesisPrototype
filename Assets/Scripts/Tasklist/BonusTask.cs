using System;

namespace Tasklist
{
    public class BonusTask
    {
        public string Text;
        public int MaxCount;
        public BonusTaskType Type;
        public int Bonus;
        public int CurrentCount = 0;

        private Action<BonusTask> _onStartedCallback;

        public BonusTask(string text, BonusTaskType type, int bonus, int maxCount, Action<BonusTask> onStartedCallback = null) {
            Text = text;
            MaxCount = maxCount;
            Type = type;
            Bonus = bonus;
            _onStartedCallback = onStartedCallback;
        }

        public void Start()
        {
            _onStartedCallback?.Invoke(this);
        }
    }
}