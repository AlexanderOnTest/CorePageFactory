namespace AlexanderOnTest.NewNetPageFactory
{
    /// <summary>
    /// Interface for returning the expected url and title of a web page.
    /// </summary>
    public interface IPageData
    {
        /// <summary>
        /// Return the expected page title.
        /// </summary>
        /// <returns>The expected page title.</returns>
        string GetExpectedPageTitle();

        /// <summary>
        /// Return the expected url.
        /// </summary>
        /// <returns>The expected url.</returns>
        string GetExpectedUri();
    }
}