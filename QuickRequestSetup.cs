namespace CSharpNotion
{
    internal static class QuickRequestSetup
    {
        public static HttpRequestMessage LoadPageChunk(string pageId, int limit = 30)
        {
            HttpRequestMessage request = new(HttpMethod.Post, Constants.ApiUrl + "/loadCachedPageChunk");
            Api.Request.LoadCachedPageChunk body = new()
            {
                Page = new Api.Request.LoadCachedPageChunkPage()
                {
                    Id = pageId
                },
                Limit = limit,
                ChunkNumber = 1
            };
            Utils.SetHttpContent(ref request, body);
            return request;
        }

        public static HttpRequestMessage LoadPageChunk(Api.Request.LoadCachedPageChunk requestBody)
        {
            HttpRequestMessage request = new(HttpMethod.Post, Constants.ApiUrl + "/loadCachedPageChunk");
            Utils.SetHttpContent(ref request, requestBody);
            return request;
        }

        public static HttpRequestMessage SyncRecordValues(IEnumerable<Api.General.Pointer> pointers)
        {
            HttpRequestMessage request = new(HttpMethod.Post, Constants.ApiUrl + "/syncRecordValues");
            Api.Request.SyncRecordElement[] elements = pointers.Select((pointer) => new Api.Request.SyncRecordElement() { Pointer = pointer }).ToArray();
            Api.Request.SyncRecordValues requestBody = new() { Requests = elements };
            Utils.SetHttpContent(ref request, requestBody);
            return request;
        }

        public static HttpRequestMessage SyncRecordValues(string guid, string table)
        {
            HttpRequestMessage request = new(HttpMethod.Post, Constants.ApiUrl + "/syncRecordValues");
            Api.Request.SyncRecordValues requestBody = new()
            {
                Requests = new Api.Request.SyncRecordElement[1] {
                    new Api.Request.SyncRecordElement
                    {
                        Pointer = new Api.General.Pointer(guid, table)
                    }
                }
            };
            Utils.SetHttpContent(ref request, requestBody);
            return request;
        }

        public static HttpRequestMessage SaveTransactions(Api.Request.SaveTransactions requestBody)
        {
            HttpRequestMessage request = new(HttpMethod.Post, Constants.ApiUrl + "/saveTransactions");
            Utils.SetHttpContent(ref request, requestBody);
            return request;
        }

        public static HttpRequestMessage SaveTransactions(Api.Request.Operation[] operations) => SaveTransactions(new Api.Request.SaveTransactions()
        {
            Transactions = new Api.Request.Transaction[]
                {
                    new Api.Request.Transaction()
                    {
                        Operations = operations
                    }
                }
        });

        public static HttpRequestMessage SaveTransactions(Api.Request.Operation operation) => SaveTransactions(new Api.Request.Operation[] { operation });
    }
}
