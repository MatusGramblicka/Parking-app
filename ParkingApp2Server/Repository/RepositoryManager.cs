using Repository.Contracts;
using Entities;
using System.Threading.Tasks;

namespace Repository
{
    public class RepositoryManager : IRepositoryManager
    {
        private RepositoryContext _repositoryContext;
        private IDayRepository _dayRepository;
        private ITenantRepository _tenantRepository;
        private IWebHookRepository _webHookRepository;

        public RepositoryManager(RepositoryContext repositoryContext)
        {
            _repositoryContext = repositoryContext;
        }

        public IDayRepository Day
        {
            get
            {
                if (_dayRepository == null)
                    _dayRepository = new DayRepository(_repositoryContext);

                return _dayRepository;
            }
        }

        public ITenantRepository Tenant
        {
            get
            {
                if (_tenantRepository == null)
                    _tenantRepository = new TenantRepository(_repositoryContext);

                return _tenantRepository;
            }
        }

        public IWebHookRepository WebHook
        {
            get
            {
                if (_webHookRepository == null)
                    _webHookRepository = new WebHookRepository(_repositoryContext);

                return _webHookRepository;
            }
        }

        public Task SaveAsync() => _repositoryContext.SaveChangesAsync();
    }
}