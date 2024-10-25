namespace KironTest.Shared.ViewModel
{
    public class SitemapDTO
    {
        public string text { get; set; }
        public List<SitemapDTO> children { get; set; } = [];
    }
}
