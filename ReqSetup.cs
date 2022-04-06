using CSharpNotion.Api.General;
using CSharpNotion.Api.Request;
using CSharpNotion.Utilities;
using MimeTypes;

namespace CSharpNotion
{
    internal static class ReqSetup
    {
        public static HttpRequestMessage SyncRecordValues(SyncRecordValues requestBody)
        {
            HttpRequestMessage request = new(HttpMethod.Post, Constants.ApiUrl + "/syncRecordValues");
            HttpUtils.SetHttpContent(ref request, requestBody);
            return request;
        }

        public static HttpRequestMessage SyncRecordValues(IEnumerable<Pointer> pointers)
        {
            SyncRecordElement[] elements = pointers.Select((pointer) => new SyncRecordElement() { Pointer = pointer }).ToArray();
            SyncRecordValues requestBody = new() { Requests = elements };
            return SyncRecordValues(requestBody);
        }

        public static HttpRequestMessage SyncRecordValues(Pointer pointer)
        {
            SyncRecordValues requestBody = new()
            {
                Requests = new SyncRecordElement[1] {
                    new SyncRecordElement
                    {
                        Pointer = pointer
                    }
                }
            };
            return SyncRecordValues(requestBody);
        }

        public static HttpRequestMessage SyncRecordValues(string guid, string table) => SyncRecordValues(new Pointer(guid, table));

        public static HttpRequestMessage SaveTransactions(SaveTransactions requestBody)
        {
            HttpRequestMessage request = new(HttpMethod.Post, Constants.ApiUrl + "/saveTransactions");
            HttpUtils.SetHttpContent(ref request, requestBody);
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
            HttpUtils.SetHttpContent(ref request, requestBody);
            return request;
        }

        public static HttpRequestMessage QueryCollection(QueryCollection requestBody)
        {
            HttpRequestMessage request = new(HttpMethod.Post, Constants.ApiUrl + "/queryCollection");
            HttpUtils.SetHttpContent(ref request, requestBody);
            return request;
        }

        public static HttpRequestMessage QueryCollection(string collectionId, string collectionViewId, int limit) => QueryCollection(new QueryCollection()
        {
            Collection = new IdPointer(collectionId),
            CollectionView = new IdPointer(collectionViewId),
            Loader = new QueryCollectionLoader()
            {
                Reducers = new LoaderReducer()
                {
                    CollectionGroupResults = new CollectionGroupResultsReducer()
                    {
                        Limit = limit
                    }
                }
            }
        });

        public static HttpRequestMessage GetSpaces() => new(HttpMethod.Post, Constants.ApiUrl + "/getSpaces");
    }
}