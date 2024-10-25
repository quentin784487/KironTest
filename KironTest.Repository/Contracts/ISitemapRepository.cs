using KironTest.DataModel;

namespace KironTest.Repository.Contracts
{
    public interface ISitemapRepository
    {
        Task<IEnumerable<Navigation>> GetSitemap();
    }
}
