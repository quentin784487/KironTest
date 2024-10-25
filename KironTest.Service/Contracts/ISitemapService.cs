using KironTest.Shared.ViewModel;

namespace KironTest.Service.Contracts
{
    public interface ISitemapService
    {
        Task<List<SitemapDTO>> GetSitemap();
    }
}
