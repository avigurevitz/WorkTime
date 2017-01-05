using System;
using System.Collections.Generic;
using WorkTime.Models;

namespace WorkTime.Interfaces
{
    public interface IManager
    {
        void Init(ISubscriber subscriber);
        void ChangeAction(ActionType newAction);
        IEnumerable<WorkingData> LoadHistory(DateTime selectedMonth);
    }
}