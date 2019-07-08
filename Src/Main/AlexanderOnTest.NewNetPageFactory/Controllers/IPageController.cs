using System;
using System.Collections.Generic;
using System.Text;

namespace AlexanderOnTest.NewNetPageFactory.Controllers
{
    interface IPageController
    {
        string GetExpectedPageTitle();

        Uri GetExpectedUriPath();
    }
}
