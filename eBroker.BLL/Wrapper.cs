using eBroker.Shared;
using System.Diagnostics.CodeAnalysis;

namespace eBroker.BLL
{
    [ExcludeFromCodeCoverage]
    public class Wrapper : IWrapper
    {
        public bool IsMarketOpen()
        {
            return Utilities.IsMarketOpen();
        }
    }
}
