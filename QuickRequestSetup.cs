using CSharpNotion.Api.General;
using CSharpNotion.Api.Request;
using MimeTypes;

namespace CSharpNotion
{
    internal static class QuickRequestSetup
    {
        public static HttpRequestMessage LoadPageChunk(string pageId, int limit = 30)
        {
            HttpRequestMessage request = new(HttpMethod.Post, Constants.ApiUrl + "/loadCachedPageChunk");
            LoadCachedPageChunk body = new()
            {
                Page = new LoadCachedPageChunkPage()
                {
                    Id = pageId
                },
                Limit = limit,
                ChunkNumber = 1
            };
            Utils.SetHttpContent(ref request, body);
            return request;
        }

        public static HttpRequestMessage LoadPageChunk(LoadCachedPageChunk requestBody)
        {
            HttpRequestMessage request = new(HttpMethod.Post, Constants.ApiUrl + "/loadCachedPageChunk");
            Utils.SetHttpContent(ref request, requestBody);
            return request;
        }

        public static HttpRequestMessage SyncRecordValues(IEnumerable<Pointer> pointers)
        {
            HttpRequestMessage request = new(HttpMethod.Post, Constants.ApiUrl + "/syncRecordValues");
            SyncRecordElement[] elements = pointers.Select((pointer) => new SyncRecordElement() { Pointer = pointer }).ToArray();
            SyncRecordValues requestBody = new() { Requests = elements };
            Utils.SetHttpContent(ref request, requestBody);
            return request;
        }

        public static HttpRequestMessage SyncRecordValues(string guid, string table)
        {
            HttpRequestMessage request = new(HttpMethod.Post, Constants.ApiUrl + "/syncRecordValues");
            SyncRecordValues requestBody = new()
            {
                Requests = new SyncRecordElement[1] {
                    new SyncRecordElement
                    {
                        Pointer = new Pointer(guid, table)
                    }
                }
            };
            Utils.SetHttpContent(ref request, requestBody);
            return request;
        }

        public static HttpRequestMessage SaveTransactions(SaveTransactions requestBody)
        {
            HttpRequestMessage request = new(HttpMethod.Post, Constants.ApiUrl + "/saveTransactions");
            Utils.SetHttpContent(ref request, requestBody);
            return request;
        }

        public static HttpRequestMessage SaveTransactions(IEnumerable<Transaction> transactions) => SaveTransactions(new SaveTransactions()
        {
            Transactions = transactions.ToArray()
        });

        public static HttpRequestMessage SaveTransactions(IEnumerable<Operation> operations) => SaveTransactions(new Transaction[]
        {
            new Transaction()
            {
                Operations = operations.ToArray()
            }
        });

        public static HttpRequestMessage SaveTransactions(Operation operation) => SaveTransactions(new Operation[] { operation });

        public static HttpRequestMessage GetUploadFileUrl(Pointer pointer, FileInfo fileInfo)
        {
            if (!fileInfo.Exists) throw new ArgumentException("filePath");

            HttpRequestMessage request = new(HttpMethod.Post, Constants.ApiUrl + "/getUploadFileUrl");
            GetUploadFileUrl requestBody = new()
            {
                ContentType = MimeTypeMap.GetMimeType(fileInfo.Extension),
                Name = fileInfo.Name,
                Record = pointer
            };
            Utils.SetHttpContent(ref request, requestBody);
            return request;
        }
    }
}