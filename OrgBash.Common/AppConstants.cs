using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Media;

namespace OrgBash.Common
{
    /// <summary>
    /// Gloabal application constants
    /// </summary>
    public static class AppConstants
    {
        public const string PARAM_ACTION = "action";
        public const string PARAM_TERM = "term";
        public const string PARAM_TYPE = "vote_type";
        public const string PARAM_ID = "id";
        public const string PARAM_ORDER = "order";
        public const string PARAM_NUMBER = "number";
        public const string PARAM_PAGE = "page";
        public const string PARAM_FAVORITES = "favorites";

        public const string ORDER_VALUE_LATEST = "latest";
        public const string ORDER_VALUE_TOP = "top";
        public const string ORDER_VALUE_FLOP = "flop";
        public const string ORDER_VALUE_RANDOM = "random";

        public const string ACTION_VOTE = "vote";
        public const string METHOD_SEARCH = "search";

        public const string TYPE_VALUE_POS = "pos";
        public const string TYPE_VALUE_NEG = "neg";

        public static readonly Color[] COLORS = new Color[8];

        public static readonly Color SERVER_COLOR = Color.FromArgb(255, 108, 108, 108); // #6c6c6c

        public const string IAP_AWESOME_EDITION = "awesome_edition";

        public const string BACKGROUND_TASK_NAME = "OrgBash.BackgroundTask";
        public const string BACKGROUND_TASK_DESC = "Aktualisiert deinen Sperrbildschirm in regelmäßigen Zeitabständen.";

        static AppConstants()
        {
            var accentColor = (Color)Application.Current.Resources["PhoneAccentColor"];
            COLORS[0] = accentColor;
            COLORS[1] = GetSubColor(accentColor, 0.42);
            COLORS[2] = GetSubColor(accentColor, 0.18);
            COLORS[3] = GetSubColor(accentColor, 0.3);
            COLORS[4] = GetSubColor(accentColor, 0.06);
            COLORS[5] = GetSubColor(accentColor, 0.36);
            COLORS[6] = GetSubColor(accentColor, 0.12);
            COLORS[7] = GetSubColor(accentColor, 0.24);
        }

        /// <summary>
        /// Creates the darker child colors.
        /// </summary>
        /// <param name="color">The color to make darker</param>
        /// <param name="factor">The color factor [0 ... 1]</param>
        /// <returns>The darker child color.</returns>
        private static Color GetSubColor(Color color, double factor)
        {
            double r = color.R * (1.0 - factor);
            double g = color.G * (1.0 - factor);
            double b = color.B * (1.0 - factor);

            return Color.FromArgb(color.A, (byte)r, (byte)g, (byte)b);
        }
    }
}
