using System;

namespace Code
{
    public class TimeStamp
    {
        internal int Hours;
        internal int Minutes;
        internal int Seconds;
        internal float RealSeconds = 0f;

        public TimeStamp(int hours, int minutes, int totalSeconds)
        {
            Hours = hours;
            Minutes = minutes;
            Seconds = totalSeconds;
        }

        public TimeStamp(int totalSeconds)
        {
            Hours = totalSeconds / 3600;
            totalSeconds -= Hours * 3600;
            
            Minutes = totalSeconds / 60;
            totalSeconds -= Minutes * 60;
            
            Seconds = totalSeconds;
        }

        public static TimeStamp operator -(TimeStamp a, TimeStamp b)
        {
            var seconds = a.GetTimeAsSeconds() - b.GetTimeAsSeconds();
            if(seconds < 0) throw new InvalidOperationException("TimeStamps cannot be negative");
            
            return new TimeStamp(seconds);
        }

        public bool LaterThanOrEqualTo(TimeStamp other)
        {
            return this.GetTimeAsSeconds() >= other.GetTimeAsSeconds();
        }

        public int GetTimeAsSeconds()
        {
            return Hours * 60 * 60 + Minutes * 60 + Seconds;
        }

        public override string ToString()
        {
            return $"{Hours:00}:{Minutes:00}";
        }

        protected bool Equals(TimeStamp other)
        {
            return Hours == other.Hours && Minutes == other.Minutes && Seconds == other.Seconds;
        }

        public override bool Equals(object obj)
        {
            if (ReferenceEquals(null, obj)) return false;
            if (ReferenceEquals(this, obj)) return true;
            if (obj.GetType() != this.GetType()) return false;
            return Equals((TimeStamp) obj);
        }

        public override int GetHashCode()
        {
            unchecked
            {
                var hashCode = Hours;
                hashCode = (hashCode * 397) ^ Minutes;
                hashCode = (hashCode * 397) ^ Seconds;
                return hashCode;
            }
        }
    }
}