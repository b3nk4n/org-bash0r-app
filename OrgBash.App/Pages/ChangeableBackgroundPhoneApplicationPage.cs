using Microsoft.Phone.Controls;
using System.Windows;
using System.Windows.Media;

namespace OrgBash.App.Pages
{
    public class ChangeableBackgroundPhoneApplicationPage : PhoneApplicationPage
    {
        private readonly SolidColorBrush NORMAL = new SolidColorBrush((Color)Application.Current.Resources["ThemeColorGray"]);

        private readonly SolidColorBrush DARK = new SolidColorBrush(Colors.Black);

        protected override void OnNavigatedTo(System.Windows.Navigation.NavigationEventArgs e)
        {
            base.OnNavigatedTo(e);

            UpdateBackground();
        }

        protected void UpdateBackground()
        {
            if (Settings.DarkBackground.Value)
            {
                Background = DARK;
            }
            else
            {
                Background = NORMAL;
            }
        }
    }
}
