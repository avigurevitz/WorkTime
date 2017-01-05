using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using WorkTime.Models;
using WorkTime.Interfaces;

namespace WorkTime.BL
{
    public class LogicManager : IManager
    {
        #region Members

        private const char DATA_SEPERATOR = ',';
        private const string FOLDER_PATH = "Log";
        private const string FILE_EXTENSION = "txt";
        private const string FILE_NAME_TEMPLATE = "{0}_{1}.{2}";
        private string m_FilePath = string.Empty;
        private ISubscriber m_Subscriber;
        private DateTime m_LastDate;

        #endregion

        #region Public methods

        #region Init

        public void Init(ISubscriber subscriber)
        {
            m_Subscriber = subscriber;
            CreateFile();
            GetWorkingData();
            GetHistoryDates();
        }

        #endregion

        #region ChangeAction

        public void ChangeAction(ActionType newAction)
        {
            WriteToFile(newAction);
            GetWorkingData();
        }

        #endregion

        #region LoadHistory

        public IEnumerable<WorkingData> LoadHistory(DateTime selectedMonth)
        {
            var fileName = string.Format(FILE_NAME_TEMPLATE, selectedMonth.Year, selectedMonth.Month.ToString().PadLeft(2, '0'), FILE_EXTENSION);
            fileName = Path.Combine(FOLDER_PATH, fileName);
            var workingData = new List<WorkingData>();
            var lines = File.ReadLines(fileName).ToList();
            foreach (var line in lines)
            {
                var lineData = line.Split(DATA_SEPERATOR);
                workingData.Add(lineData.Length == 2
                    ? new WorkingData() {StartDate = Convert.ToDateTime(lineData[0]), EndDate = Convert.ToDateTime(lineData[1])}
                    : new WorkingData() {StartDate = Convert.ToDateTime(lineData[0]), EndDate = Consts.DEFAULT_DATE_TIME});
            }
            return workingData;
        }

        #endregion

        #endregion

        #region Private methods

        #region CreateFile

        private void CreateFile()
        {
            if (Directory.Exists(FOLDER_PATH) == false)
                Directory.CreateDirectory(FOLDER_PATH);

            var fileName = string.Format(FILE_NAME_TEMPLATE, DateTime.Now.Year, DateTime.Now.Month.ToString().PadLeft(2, '0'), FILE_EXTENSION);
            m_FilePath = Path.Combine(FOLDER_PATH, fileName);
            if (File.Exists(m_FilePath) == true)
                return;

            var fileStream = File.Create(m_FilePath);
            fileStream.Close();
        }

        #endregion

        #region WriteToFile

        private void WriteToFile(ActionType action)
        {
            var now = DateTime.Now;
            if (now.Date.Month != m_LastDate.Date.Month)
                CreateFile();
            
            using (var stream = File.Open(m_FilePath, FileMode.Append))
            {

                var bytesToWrite =
                    Encoding.UTF8.GetBytes(action == ActionType.Working
                        ? string.Format("{0}", now)
                        : string.Format("{0}{1}{2}", DATA_SEPERATOR, now, Environment.NewLine));
                stream.Write(bytesToWrite, 0, bytesToWrite.Length);
            }
        }

        #endregion

        #region GetWorkingData

        private void GetWorkingData()
        {
            var workingData = new List<WorkingData>();
            var lines = File.ReadLines(m_FilePath).ToList();
            foreach (var line in lines)
            {
                var lineData = line.Split(DATA_SEPERATOR);
                workingData.Add(lineData.Length == 2
                    ? new WorkingData() {StartDate = Convert.ToDateTime(lineData[0]), EndDate = Convert.ToDateTime(lineData[1])}
                    : new WorkingData() {StartDate = Convert.ToDateTime(lineData[0]), EndDate = Consts.DEFAULT_DATE_TIME});
            }

            m_LastDate = workingData.Count != 0 ?  workingData.Last().StartDate : DateTime.Now;
            m_Subscriber.UpdateWorkingData(workingData);
        }

        #endregion

        #region GetHistoryDates

        private void GetHistoryDates()
        {
            var files = Directory.GetFiles(FOLDER_PATH);
            var historyDates = new List<DateTime>();
            foreach (var file in files)
            {
                var fileName = Path.GetFileNameWithoutExtension(file);
                var data = fileName.Split('_');
                historyDates.Add(new DateTime(int.Parse(data[0]), int.Parse(data[1]), 1));
            }
            m_Subscriber.UppdateHistoryList(historyDates.OrderByDescending(a => a.Date));
        }

        #endregion

        #endregion
    }
}
