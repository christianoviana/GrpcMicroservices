namespace ProductWorkerService.Config
{
    public class ProductWorkerConfig
    {
        public const string Name = "ProductWorker";
        public string ProductName { get; set; }
        public int Interval { get; set; }
        public string UrlProductGrpc { get; set; }
    }
}
