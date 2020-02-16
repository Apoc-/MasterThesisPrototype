using System;
using System.Collections.Generic;
using System.Linq;
using System.Timers;
using TMPro;
using UnityEngine;
using UnityEngine.Events;

namespace Code
{
    public class Clock : MonoBehaviour
    {
        class Alarm
        {
            internal readonly TimeStamp time;
            internal readonly Action callback;
            internal readonly bool recurring;
            internal bool triggeredToday;

            public Alarm(TimeStamp time, Action callback, bool recurring)
            {
                this.time = time;
                this.callback = callback;
                this.recurring = recurring;
                this.triggeredToday = false;
            }
        }

        public float SpeedFactor = 60f;
        public bool Running = false;
        private TimeStamp CurrentTime = new TimeStamp(0,0,0);
        private TextMeshProUGUI _clockText;

        public delegate void ClockTickAction();

        public event ClockTickAction OnSecondTick;

        private List<Alarm> _alarms = new List<Alarm>();
        private List<Alarm> _newAlarms = new List<Alarm>();

        public Color BaseColor;
        public Color WarningColor;
        
        private void Start()
        {
            _clockText = GetComponent<TextMeshProUGUI>();
        }

        private void Update()
        {
            if (Running) IncrementClock();

            _clockText.text = CurrentTime.ToString();
        }

        private void IncrementClock()
        {
            CurrentTime.RealSeconds += Time.deltaTime;
            CurrentTime.Seconds = (int) (CurrentTime.RealSeconds * SpeedFactor);
            if (CurrentTime.Seconds >= 60)
            {
                OnSecondTick?.Invoke();
                
                CurrentTime.RealSeconds = 0;
                CurrentTime.Minutes += 1;
            }

            if (CurrentTime.Minutes >= 60)
            {
                CurrentTime.Minutes = 0;
                CurrentTime.Hours += 1;
            }

            if (CurrentTime.Hours >= 24)
            {
                CurrentTime.Hours = 0;
            }

            CheckAlarms();
        }

        public void SetTime(int hours, int minutes, int seconds)
        {
            CurrentTime.Hours = hours % 24;
            CurrentTime.Minutes = minutes % 60;
            CurrentTime.Seconds = seconds % 60;
            CurrentTime.RealSeconds = seconds % 60;
        }

        public TimeStamp GetTime()
        {
            return CurrentTime;
        }

        private void CheckAlarms()
        {
            _alarms.ForEach(alarm =>
            {
                if (alarm.triggeredToday) return;
                if (alarm.time.LaterThanOrEqualTo(CurrentTime)) return;
                
                alarm.callback.Invoke();
                alarm.triggeredToday = true;
            });
            
            _alarms.AddRange(_newAlarms);
            _newAlarms.Clear();
        }

        public void SetAlarm(TimeStamp time, Action callback, bool recurring = false)
        {
            var alarm = new Alarm(time, callback, recurring);
            _newAlarms.Add(alarm);
        }

        public void ResetAlarms()
        {
            var recurringAlarms = _alarms.Where(alarm => alarm.recurring || alarm.triggeredToday == false).ToList();
            recurringAlarms.ForEach(alarm => alarm.triggeredToday = false);
            _alarms = recurringAlarms;
        }

        public void SetToBaseColor()
        {
            _clockText.color = BaseColor;
        }

        public void SetToWarningColor()
        {
            _clockText.color = WarningColor;
        }
    }
}