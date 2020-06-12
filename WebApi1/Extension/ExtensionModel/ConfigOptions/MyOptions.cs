namespace WebApi1.Extension.ExtensionModel.ConfigOptions
{
    /// <summary>
    /// Config的映射模型
    /// </summary>
    public class MyOptions
    {
        public string UrlBase { get; set; }
        public ConnModel ConnectionStrings { get; set; }
    }

    public class ConnModel
    {
        public string DefaultConnection { get; set; }
    }
}
