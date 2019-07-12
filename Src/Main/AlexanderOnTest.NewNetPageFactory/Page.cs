using System;

namespace AlexanderOnTest.NewNetPageFactory
{
    public abstract class Page
    {
        public abstract string GetExpectedPageTitle();

        public abstract Uri GetExpectedUriPath();
    }
}
