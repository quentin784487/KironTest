using KironTest.Caching.Contracts;
using KironTest.DataModel;
using KironTest.Repository.Contracts;
using KironTest.Service.Contracts;
using KironTest.Shared.Exceptions;
using KironTest.Shared.ViewModel;

namespace KironTest.Service
{
    public class SitemapService : ISitemapService
    {
        private readonly ISitemapRepository _sitemapRepository;
        private readonly IMemoryCacheService _memoryCacheService;

        public SitemapService(ISitemapRepository _sitemapRepository, IMemoryCacheService _memoryCacheService)
        {
            this._sitemapRepository = _sitemapRepository;
            this._memoryCacheService = _memoryCacheService;
        }

        public async Task<List<SitemapDTO>> GetSitemap()
        {
            try
            {   
                var cachedData = _memoryCacheService.Get<List<SitemapDTO>>("sitemap_cache");

                if (cachedData != null)
                    return cachedData;

                IEnumerable<Navigation> navigationItems = await _sitemapRepository.GetSitemap();

                var navigationDictionary = navigationItems.ToDictionary(n => n.ID);

                var topLevelItems = navigationItems.Where(x => x.ParentID == -1);

                var sitemap = new List<SitemapDTO>();

                foreach (var topLevelItem in topLevelItems)
                {
                    var sitemapDTO = new SitemapDTO { text = topLevelItem.Text };
                    BuildHierarchy(sitemapDTO, topLevelItem.ID, navigationDictionary);
                    sitemap.Add(sitemapDTO);
                }

                _memoryCacheService.Set<List<SitemapDTO>>("sitemap_cache", sitemap, TimeSpan.FromHours(1));

                return sitemap;
            }
            catch (Exception ex)
            {
                throw new ServiceException("Could not authorize user.", ex);
            }            
        }

        private static void BuildHierarchy(SitemapDTO parent, int parentId, Dictionary<int, Navigation> navigationDictionary)
        {
            var children = navigationDictionary.Values.Where(n => n.ParentID == parentId).ToList();

            foreach (var child in children)
            {
                var childDto = new SitemapDTO { text = child.Text };
                BuildHierarchy(childDto, child.ID, navigationDictionary);
                parent.children.Add(childDto);
            }
        }
    }
}
