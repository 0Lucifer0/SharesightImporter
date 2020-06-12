using System.Collections.Generic;
using System.Threading.Tasks;

namespace SharesiesToSharesight.SharesiesClient
{
    public interface ISharesiesClient
    {
        public Task LoginAsync();
    }
}