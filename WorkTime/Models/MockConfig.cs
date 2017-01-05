using System.Configuration;

namespace WorkTime.Models
{
    public class MockConfig : ConfigurationSection
    {
        public static string Name
        {
            get { return "MockConfig"; }
        }

        [ConfigurationProperty("IsTest")]
        public bool IsTest
        {
            get { return (bool) this["IsTest"]; }
            set { this["IsTest"] = value; }
        }
    }
}
