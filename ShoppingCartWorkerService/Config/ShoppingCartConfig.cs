namespace ShoppingCartWorkerService.Config
{
    public  class ShoppingCartConfig
    {
        public const string NAME = "WorkerService";
        public int TaskInterval { get; set; }
        public string UserName { get; set; }
        public string ProductServerUrl { get; set; }
        public string ShoppingCartServerUrl { get; set; }
    }
}
