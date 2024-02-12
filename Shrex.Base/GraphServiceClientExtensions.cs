using Microsoft.Graph;

namespace Shrex
{
    public static class GraphServiceClientExtensions
    {
        public static Shrex SP(this GraphServiceClient client)
        {
            return new Shrex(client);
        }
    }
}
