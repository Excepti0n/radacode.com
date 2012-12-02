namespace RadaCode.Web.Core.Setttings
{
    class LocalRadaCodeWebSettings : IRadaCodeWebSettings
    {
        public string CurrentHost
        {
            get { return "localhost"; }
        }

        public int PortfolioProjectsCount { get { return 10; } }
    }
}