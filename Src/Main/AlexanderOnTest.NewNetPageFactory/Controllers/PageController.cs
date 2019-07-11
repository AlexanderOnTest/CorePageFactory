using System;

namespace AlexanderOnTest.NewNetPageFactory.Controllers
{
    public abstract class AbstractPageController : IPageController
    {
        public abstract string GetExpectedPageTitle();

        public abstract Uri GetExpectedUriPath();
    }
}
