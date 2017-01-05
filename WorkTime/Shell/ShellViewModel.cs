using System.Reflection;
using Caliburn.Micro;

namespace WorkTime.Shell
{
    public class ShellViewModel : Conductor<object>
    {
        public string Title { get; private set; }

        protected override void OnActivate()
        {
            ActivateItem(new Main.MainViewModel());
            base.OnActivate();
            Title = string.Format("Work time {0}", Assembly.GetExecutingAssembly().GetName().Version);
        }
    }
}
