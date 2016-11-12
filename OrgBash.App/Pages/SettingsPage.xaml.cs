using System;
using System.Windows.Navigation;
using Microsoft.Phone.Tasks;
using System.Windows.Media.Imaging;
using PhoneKit.Framework.Core.Storage;
using System.Diagnostics;
using System.IO;
using System.Windows.Media;

namespace OrgBash.App.Pages
{
    public partial class SettingsPage : ChangeableBackgroundPhoneApplicationPage
    {
        /// <summary>
        /// The photo chooser task.
        /// </summary>
        /// <remarks>Must be defined at class level to work properly in tombstoning.</remarks>
        private PhotoChooserTask _photoTask = new PhotoChooserTask();

        public SettingsPage()
        {
            InitializeComponent();

            // init photo chooser task
            _photoTask.ShowCamera = true;
            _photoTask.Completed += (se, pr) =>
            {
                if (pr.Error != null || pr.TaskResult != TaskResult.OK)
                    return;

                // save a copy in local storage
                FileInfo fileInfo = new FileInfo(pr.OriginalFileName);

                // store lock image
                string filePath = string.Format("lockScreenBackground{0}", fileInfo.Extension);
                if (StorageHelper.SaveFileFromStream(filePath, pr.ChosenPhoto))
                {
                    // save
                    Settings.LockScreenBackgroundImagePath.Value = filePath;

                    // update preview
                    UpdatePreviewImage();
                }
            };

            SelectBackgroundImageButton.Click += (s, e) =>
            {
                _photoTask.Show();
            };

            ClearBackgroundImageButton.Click += (s, e) =>
            {
                Settings.LockScreenBackgroundImagePath.Value = null;
                UpdatePreviewImage();
            };

            DarkerBackgroundCheckBox.Checked += (s, e) =>
            {
                Settings.DarkBackground.Value = DarkerBackgroundCheckBox.IsChecked.Value;
                UpdateBackground();
            };
            DarkerBackgroundCheckBox.Unchecked += (s, e) =>
            {
                Settings.DarkBackground.Value = DarkerBackgroundCheckBox.IsChecked.Value;
                UpdateBackground();
            };
        }

        protected override void OnNavigatedTo(NavigationEventArgs e)
        {
            base.OnNavigatedTo(e);

            // load settings
            BackgroundImageOpacitySlider.Value = Settings.LockScreenBackgroundImageOpacity.Value;
            DarkerBackgroundCheckBox.IsChecked = Settings.DarkBackground.Value;

            UpdatePreviewImage();
        }

        protected override void OnNavigatedFrom(NavigationEventArgs e)
        {
            base.OnNavigatedFrom(e);

            // save settings
            Settings.LockScreenBackgroundImageOpacity.Value = BackgroundImageOpacitySlider.Value;
            Settings.DarkBackground.Value = DarkerBackgroundCheckBox.IsChecked.Value;
        }

        /// <summary>
        /// Updates the preview image.
        /// </summary>
        private void UpdatePreviewImage()
        {
            // solidColor
            var color = Settings.LockScreenBackgroundColor.Value;
            PreviewColorBackground.Fill = new SolidColorBrush(color);

            // image
            var lockScreenPath = Settings.LockScreenBackgroundImagePath.Value;
            if (lockScreenPath != null)
            {
                BitmapImage img = new BitmapImage();
                try
                {
                    using (var imageStream = StorageHelper.GetFileStream(lockScreenPath))
                    {
                        img.SetSource(imageStream);
                        PreviewImageBackground.Source = img;
                    }
                }
                catch (Exception e)
                {
                    // BUGSENSE : Offset and length were out of bounds for the array or count is greater than the number of elements from index to the end of the source collection.
                    // Occured 2 times (05.06.2014)
                    Debug.WriteLine("Could not set the image source with error: " + e.Message);
                }
            }
            else
            {
                PreviewImageBackground.Source = null;
            }
        }
    }
}