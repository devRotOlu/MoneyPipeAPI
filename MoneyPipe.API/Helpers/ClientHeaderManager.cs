namespace MoneyPipe.API.Helpers
{
    public static class ClientHeaderManager
    {
        public static string? GetClientURL(this HttpRequest request, string path)
        {
            var clientType = request.Headers["X-Client-Type"].FirstOrDefault()?.ToLower();

            string? clientURL = null;

            if (clientType == "web")
            {
                var baseURL = request.Headers["X-Origin"].ToString();
                clientURL = $"{baseURL}{path}";
            }

            return clientURL;
        }
    }
}
