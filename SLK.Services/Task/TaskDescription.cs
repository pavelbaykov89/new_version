using System;

namespace SLK.Services.Task
{
    public class TaskDescription
    {
        private DateTime _created;

        private string _estimated;

        private decimal _progress;

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
            _created = new DateTime();

            _estimated = "calculating...";

            _progress = 0;

            Caption = caption;

            FileName = fileName;
        }

        public string Caption { get; protected set; }

        public string FileName { get; protected set; }

        public decimal Progress
        {
            get
            {
                return _progress;
            }

            set
            {                
                if (value != 0 && _progress != value)
                {
                    var estimated = new TimeSpan(_elapsed.Ticks * 100 / (long)value);
                    _estimated = $"{(int)estimated.TotalHours:D2}:{estimated.Minutes:D2}:{estimated.Seconds:D2}";
                }
                _progress = value;
            }
        }

        public string Elapsed
        {
            get
            {
                if (_created.Ticks == 0) return "starting...";
                return $"{(int)_elapsed.TotalHours:D2}:{_elapsed.Minutes:D2}:{_elapsed.Seconds:D2}";
            }
        }

        public string Estimated
        {
            get
            {
                return _estimated;
            }
        }

        public void Start()
        {
            _created = DateTime.Now;
            _progress = 0;
        }
    }
        
}
