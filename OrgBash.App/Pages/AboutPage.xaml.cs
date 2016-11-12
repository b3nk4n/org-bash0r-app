using OrgBash.App.Helpers;

namespace OrgBash.App.Pages
{
    public partial class AboutPage : ChangeableBackgroundPhoneApplicationPage
    {
        public AboutPage()
        {
            InitializeComponent();
        }

        private void EasterEggDoubleTapped(object sender, System.Windows.Input.GestureEventArgs e)
        {
            LicenceEasterEggHelper.TriggerEasterEggRequested();
        }
    }
}