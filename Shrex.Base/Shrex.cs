using Microsoft.Graph;

namespace Shrex
{
    public class Shrex : IDisposable
    {
        public GraphServiceClient Client { get; private set; }

        public Shrex(GraphServiceClient client)
        {
            Client = client;
        }

        public void Dispose()
        {
            GC.SuppressFinalize(this);
        }
    }
}
