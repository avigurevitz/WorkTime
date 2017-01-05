using System;
using System.Collections.Generic;
using WorkTime.Models;

namespace WorkTime.Interfaces
{
    public interface ISubscriber
    {
        void UpdateWorkingData(IEnumerable<WorkingData> workingData);
        void UppdateHistoryList(IEnumerable<DateTime> dates);
    }
}