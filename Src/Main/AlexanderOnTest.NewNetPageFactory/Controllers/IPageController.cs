using System;

namespace AlexanderOnTest.NewNetPageFactory.Controllers
{
    interface IPageController
    {
        string GetExpectedPageTitle();

        Uri GetExpectedUriPath();
    }
}
