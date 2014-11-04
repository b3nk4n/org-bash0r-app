using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Navigation;
using Microsoft.Phone.Controls;
using Microsoft.Phone.Shell;
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