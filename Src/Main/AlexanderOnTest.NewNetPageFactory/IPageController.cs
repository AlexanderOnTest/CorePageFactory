using System;

namespace AlexanderOnTest.NewNetPageFactory
{
    /// <summary>
    /// Required interface for any PageController.
    /// </summary>
    public interface IPageController
    {
        /// <summary>
        /// Return the current Page Title
        /// </summary>
        /// <returns>The Page title as displayed in the browser tab.</returns>
        string GetActualPageTitle();

        /// <summary>
        /// Return the current url.
        /// </summary>
        /// <returns>The url as given in the address bar.</returns>
        string GetActualUri();
    }
}