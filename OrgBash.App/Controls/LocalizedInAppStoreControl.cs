using OrgBash.App.Resources;
using OrgBash.Common;

namespace OrgBash.App.Controls
{
    public class LocalizedInAppStoreControl : MyInAppStoreControlBase
    {
        /// <summary>
        /// Localizes the user control content and texts.
        /// </summary>
        protected override void LocalizeContent()
        {
            InAppStoreLoadingText = AppResources.InAppStoreLoading;
            InAppStoreNoProductsText = AppResources.InAppStoreNoProducts;
            InAppStorePurchasedText = AppResources.InAppStorePurchased;
            SupportedProductIds = AppConstants.IAP_AWESOME_EDITION;
        }
    }
}
