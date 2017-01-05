using System;
using System.Collections.Concurrent;
using System.Collections.Generic;
using System.Configuration;
using System.Diagnostics.PerformanceData;
using System.Linq;
using System.Timers;
using Caliburn.Micro;
using WorkTime.BL;
using WorkTime.Interfaces;
using WorkTime.Models;

namespace WorkTime.Main
{
    public class MainViewModel : Screen, ISubscriber
    {
        #region Members

        private readonly TimeSpan FULL_WORK_TIME = new TimeSpan(Consts.TOTAL_HOURS_IN_DAY, Consts.TOTAL_MINUTES_IN_DAY, 0);
        private readonly TimeSpan MIN_WORK_TIME = new TimeSpan(5, 30, 0);
        private readonly IManager m_LogicManager;
        private readonly Timer m_SecondsTimer;
        private ActionType m_CurrentStatus = ActionType.Working;
        private string m_Time;
        private bool m_IsStart;
        private TimeStatus m_IsOk;
        private DateTime m_StartTime;
        private string m_TotalDays;
        private string m_TotalWorkTime;
        private BindableCollection<WorkingInfo> m_WorkingInfo;
        private BindableCollection<DateTime> m_HistoryDates;
        private DateTime m_SelectedHistoryDate;
        private TimeSpan m_TodayWorkDurationTime;
        private int m_TotalWorkDays;

        #endregion

        #region Properties

        public string EndTime
        {
            get { return string.Format(Consts.END_TIME_MSG_TEMPLATE, m_StartTime.Add(FULL_WORK_TIME - m_TodayWorkDurationTime).ToString("HH:mm")); }
        }

        public string ActionName
        {
            get { return m_CurrentStatus == ActionType.Working ? "End work" : "Start work"; }
        }

        public string WorkDurationTime
        {
            get { return m_Time; }
            set
            {
                if (value == m_Time)
                    return;

                m_Time = value;
                NotifyOfPropertyChange(() => WorkDurationTime);
            }
        }

        public bool IsStart
        {
            get { return m_IsStart; }
            set
            {
                if (value == m_IsStart)
                    return;

                m_IsStart = value;
                NotifyOfPropertyChange(() => IsStart);
            }
        }

        public TimeStatus IsOk
        {
            get { return m_IsOk; }
            set
            {
                if (value == m_IsOk)
                    return;

                m_IsOk = value;
                NotifyOfPropertyChange(() => IsOk);
            }
        }

        public string TotalDays
        {
            get { return m_TotalDays; }
            set
            {
                if (value == m_TotalDays)
                    return;

                m_TotalDays = value;
                NotifyOfPropertyChange(() => TotalDays);
            }
        }

        public string TotalWorkTime
        {
            get { return m_TotalWorkTime; }
            set
            {
                if (value == m_TotalWorkTime)
                    return;

                m_TotalWorkTime = value;
                NotifyOfPropertyChange(() => TotalWorkTime);
            }
        }

        public BindableCollection<WorkingInfo> Data
        {
            get { return m_WorkingInfo; }
            set
            {
                if (value == m_WorkingInfo)
                    return;

                m_WorkingInfo = value;
                NotifyOfPropertyChange(() => Data);
            }
        }

        public BindableCollection<DateTime> HistoryDates
        {
            get { return m_HistoryDates; }
            set
            {
                if (value == m_HistoryDates)
                    return;

                m_HistoryDates = value;
                NotifyOfPropertyChange(() => HistoryDates);
            }
        }

        public DateTime SelectedHistoryDate
        {
            get { return m_SelectedHistoryDate; }
            set
            {
                if (value == m_SelectedHistoryDate)
                    return;

                m_SelectedHistoryDate = value;
                LoadHistory();
            }
        }

        #endregion

        #region Ctor

        public MainViewModel()
        {
            var mockConfig = (MockConfig)ConfigurationManager.GetSection(MockConfig.Name);
            Data = new BindableCollection<WorkingInfo>();
            HistoryDates = new BindableCollection<DateTime>();
            m_LogicManager = ManagerFactory.CreateManager(mockConfig);
            m_SecondsTimer = new Timer(1000);
            m_SecondsTimer.Elapsed += OnSecondsTimerElapsed;
        }

        #endregion

        #region Public methods

        #region ChangeAction

        public void ChangeAction()
        {
            m_CurrentStatus = m_CurrentStatus == ActionType.NotWorking ? ActionType.Working : ActionType.NotWorking;
            m_LogicManager.ChangeAction(m_CurrentStatus);
        }

        #endregion

        #region UpdateWorkingData

        public void UpdateWorkingData(IEnumerable<WorkingData> workingData)
        {
            SetData(workingData);
            UpdateProperties();
        }

        #endregion

        #region UppdateHistoryList

        public void UppdateHistoryList(IEnumerable<DateTime> dates)
        {
            HistoryDates.Clear();
            HistoryDates = new BindableCollection<DateTime>(dates);
            SelectedHistoryDate = HistoryDates[0];
        }

        #endregion

        #region SaveToFile

        public void SaveToFile()
        {
        }

        #endregion

        #endregion

        #region Private methods

        #region OnActivate

        protected override void OnActivate()
        {
            m_LogicManager.Init(this);
        }

        #endregion

        #region OnSecondsTimerElapsed

        private void OnSecondsTimerElapsed(object sender, ElapsedEventArgs e)
        {
            var timePass = DateTime.Now - m_StartTime + m_TodayWorkDurationTime;
            if (timePass > FULL_WORK_TIME)
                IsOk = TimeStatus.Over;
            else if (timePass > MIN_WORK_TIME)
                IsOk = TimeStatus.Ok;
            else
                IsOk = TimeStatus.Err;

            WorkDurationTime = string.Format(Consts.WORK_DURATION_TIME_MSG_TEMPLATE, timePass.ToString(Consts.TIME_SPAN_TEMPLATE));
        }

        #endregion

        #region SetData

        private void SetData(IEnumerable<WorkingData> workingData)
        {
            var dict = FillDictionary(workingData);
            m_TotalWorkDays = dict.Keys.Count;
            UpdateOverTime(dict);
            FillData(dict);
        }

        #endregion

        #region FillDictionary

        private Dictionary<int, List<WorkingInfo>> FillDictionary(IEnumerable<WorkingData> workingData)
        {
            var dict = new Dictionary<int, List<WorkingInfo>>();
            foreach (var data in workingData)
            {
                if (dict.ContainsKey(data.StartDate.Day) == false)
                    dict[data.StartDate.Day] = new List<WorkingInfo>();

                var totalDuration = TimeSpan.Zero;
                if (data.EndDate != Consts.DEFAULT_DATE_TIME)
                    totalDuration = data.EndDate - data.StartDate;

                dict[data.StartDate.Day].Add(new WorkingInfo()
                {
                    StartTime = data.StartDate,
                    Date = data.StartDate.Day,
                    Day = data.StartDate.DayOfWeek,
                    EndTime = data.EndDate,
                    Total = totalDuration
                });
            }
            return dict;
        }

        #endregion

        #region UpdateOverTime

        private void UpdateOverTime(Dictionary<int, List<WorkingInfo>> dict)
        {
            foreach (var item in dict)
            {
                var total = TimeSpan.Zero;
                foreach (var workingInfo in item.Value)
                {
                    var workDuration = workingInfo.EndTime != Consts.DEFAULT_DATE_TIME ? workingInfo.EndTime - workingInfo.StartTime : TimeSpan.Zero;
                    total = total.Add(workDuration);
                }
                var lastWorkingInfo = item.Value.OrderBy(w => w.StartTime).Last();
                if (lastWorkingInfo.EndTime == Consts.DEFAULT_DATE_TIME)
                    continue;

                lastWorkingInfo.OverTime = total - FULL_WORK_TIME;
            }
        }

        #endregion

        #region FillData

        private void FillData(Dictionary<int, List<WorkingInfo>> dict)
        {
            Data.Clear();
            var orderedKeys = dict.Keys.OrderBy(d => d);
            foreach (var key in orderedKeys)
            {
                var workingInfos = dict[key].OrderBy(w => w.StartTime).ToList();
                foreach (var workingInfo in workingInfos)
                    Data.Add(workingInfo);
            }
        }

        #endregion

        #region UpdateProperties

        private void UpdateProperties()
        {
            if (Data.Count == 0)
                m_CurrentStatus = ActionType.NotWorking;
            else
                m_CurrentStatus = Data.Last().EndTime == Consts.DEFAULT_DATE_TIME ? ActionType.Working : ActionType.NotWorking;

            var workingDays = GetWorkingDaysInMonth();
            TotalDays = string.Format("Days of work: {0}\\{1}", m_TotalWorkDays, workingDays);

            var totalWorkDuration = GetTotalWorkDuration();
            var totalWorkingDays = Data.Count == 0 || Data.Last().EndTime != Consts.DEFAULT_DATE_TIME ? m_TotalWorkDays : m_TotalWorkDays - 1;
            var expectedWorkTime = new TimeSpan((totalWorkingDays * Consts.TOTAL_HOURS_IN_DAY), 0, 0);
            TotalWorkTime = string.Format(@"Total work time: {0}:{1}\{2}:{3}", (int)totalWorkDuration.TotalHours, totalWorkDuration.Minutes.ToString().PadLeft(2, '0'),
                expectedWorkTime.TotalHours, expectedWorkTime.Minutes.ToString().PadLeft(2, '0'));

            m_TodayWorkDurationTime = GetTodayWorkDurationTime();
            if (m_CurrentStatus == ActionType.NotWorking)
            {
                m_SecondsTimer.Enabled = false;
                WorkDurationTime = string.Format(Consts.WORK_DURATION_TIME_MSG_TEMPLATE, m_TodayWorkDurationTime.ToString(Consts.TIME_SPAN_TEMPLATE));
                IsStart = true;
            }
            else
            {
                m_SecondsTimer.Enabled = true;
                m_StartTime = Data.Last().StartTime;
                IsStart = false;
                NotifyOfPropertyChange(() => EndTime);
            }
            NotifyOfPropertyChange(() => ActionName);
        }

        #endregion

        #region GetTotalWorkDuration

        private TimeSpan GetTotalWorkDuration()
        {
            return Data.Aggregate(TimeSpan.Zero, (current, item) => current.Add(item.Total));
        }

        #endregion

        #region GetTodayWorkDurationTime

        private TimeSpan GetTodayWorkDurationTime()
        {
            var todayDate = DateTime.Now.Date;
            var todayWorkingInfo = Data.Where(workingInfo => workingInfo.StartTime.Date == todayDate).ToList();
            if (todayWorkingInfo.Count == 0)
                return TimeSpan.Zero;

            var todayWorkDurationTime = TimeSpan.Zero;
            foreach (var workingInfo in todayWorkingInfo)
                todayWorkDurationTime += (workingInfo.EndTime == Consts.DEFAULT_DATE_TIME) ? TimeSpan.Zero : workingInfo.EndTime - workingInfo.StartTime;

            return todayWorkDurationTime;
        }

        #endregion

        #region GetWorkingDaysInMonth

        private int GetWorkingDaysInMonth()
        {
            var canContinue = true;
            var counter = 0;
            var date = new DateTime(DateTime.Now.Year, DateTime.Now.Month, 1);
            var month = date.Month;
            while (canContinue)
            {
                if (IsWorkingDay(date))
                    counter++;

                date = date.AddDays(1);
                if (date.Month != month)
                    canContinue = false;
            }
            return counter;
        }

        #endregion

        #region IsWorkingDay

        private bool IsWorkingDay(DateTime date)
        {
            return ((date.DayOfWeek != DayOfWeek.Friday) && (date.DayOfWeek != DayOfWeek.Saturday));
        }

        #endregion

        #region LoadHistory

        private void LoadHistory()
        {
             var historyData = m_LogicManager.LoadHistory(SelectedHistoryDate);
            SetData(historyData);
        }

        #endregion

        #endregion

        #region class WorkingInfo

        public class WorkingInfo
        {
            public int Date { get; set; }
            public DayOfWeek Day { get; set; }
            public DateTime StartTime { get; set; }
            public DateTime EndTime { get; set; }
            public TimeSpan Total { get; set; }
            public TimeSpan OverTime { get; set; }
        }

        #endregion
    }
}
