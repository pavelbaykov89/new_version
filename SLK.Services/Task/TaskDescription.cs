using System;

namespace SLK.Services.Task
{
    public class TaskDescription
    {
        private DateTime _created;

        public TimeSpan _elapsed
        {
            get
            {
                return DateTime.Now.Subtract(_created);
            }
        }

        protected TaskDescription() { }

        public TaskDescription(string caption, string fileName)
        {
            _created = DateTime.Now;

            Caption = caption;

            FileName = fileName;

            Progress = 0;
        }

        public string Caption { get; protected set; }

        public string FileName { get; protected set; }

        public decimal Progress { get; set; }

        public string Elapsed
        {
            get
            {
                return $"{(int)_elapsed.TotalHours:D2}:{_elapsed.Minutes:D2}:{_elapsed.Seconds:D2}";
            }
        }

        public string Estimated
        {
            get
            {
                if (Progress == 0) return "calculating...";
                var estimated = new TimeSpan(_elapsed.Ticks * 100 / (long)Progress);
                return $"{(int)estimated.TotalHours:D2}:{estimated.Minutes:D2}:{estimated.Seconds:D2}";
            }
        }
    }
        
}
