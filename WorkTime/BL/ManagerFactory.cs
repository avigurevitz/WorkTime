using WorkTime.Interfaces;
using WorkTime.Models;

namespace WorkTime.BL
{
    public class ManagerFactory
    {
        public static IManager CreateManager(MockConfig config)
        {
            return (config != null && config.IsTest == false) ? new LogicManager() : null;
        }
    }
}
