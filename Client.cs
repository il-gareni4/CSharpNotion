using CSharpNotion.Api.Response;
using System.Net;
using System.Text.Json;

namespace CSharpNotion
{
    public class Client
    {
        private static readonly CookieContainer cookieContainer = new();
        private static readonly HttpClientHandler httpClientHandler = new() { CookieContainer = cookieContainer };
        public static readonly HttpClient HttpClient = new(httpClientHandler);
        private readonly string _tokenV2;

        public static readonly JsonSerializerOptions SerializeOptions = new()
        {
            PropertyNamingPolicy = JsonNamingPolicy.CamelCase
        };

        /// <summary>
        /// Initializes a new instance of Notion Client
        /// </summary>
        /// <param name="tokenV2">The token used to access the API</param>
        public Client(string tokenV2)
        {
            _tokenV2 = tokenV2;
            cookieContainer.Add(new Cookie("token_v2", _tokenV2, "", "www.notion.so"));
        }

        /// <summary>
        /// Get a block by ID with type T. Type checking included
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="pageId">Id of block you need</param>
        /// <returns></returns>
        /// <exception cref="InvalidDataException"></exception>
        /// <exception cref="InvalidOperationException"></exception>
        /// <exception cref="HttpRequestException"></exception>
        /// <exception cref="TaskCanceledException"></exception>
        public async Task<T> GetBlockAsync<T>(string pageId) where T : Entities.BaseBlock => await GetBlockAsync<T>(HttpClient, pageId);

        public static async Task<T> GetBlockAsync<T>(HttpClient httpClient, string pageId) where T : Entities.BaseBlock
        {
            pageId = Utils.ExtractId(pageId);
            SyncRecordValuesResponse recordValues = await QuickRequestSetup.SyncRecordValues(pageId, "block").Send(httpClient).DeserializeJson<SyncRecordValuesResponse>();
            if (recordValues?.RecordMap?.Block is null || recordValues.RecordMap.Block.Count == 0) throw new InvalidDataException("Invalid response");
            if (Constants.BlockToType.GetValueOrDefault(typeof(T)) != recordValues.RecordMap.Block.First().Value.Value!.Type) throw new InvalidDataException("Invalid type of block excepted");
            return (T)Activator.CreateInstance(typeof(T), recordValues.RecordMap.Block.First().Value.Value)!; ;
        }

        /// <summary>
        /// Get a block by ID with <see cref="Entities.BaseBlock"/> type
        /// </summary>
        /// <param name="pageId">Id of block you need</param>
        /// <returns></returns>
        /// <exception cref="InvalidDataException"></exception>
        /// <exception cref="InvalidOperationException"></exception>
        /// <exception cref="HttpRequestException"></exception>
        /// <exception cref="TaskCanceledException"></exception>
        public async Task<Entities.BaseBlock> GetBlockAsync(string pageId)
        {
            pageId = Utils.ExtractId(pageId);
            SyncRecordValuesResponse recordValues = await QuickRequestSetup.SyncRecordValues(pageId, "block").Send(HttpClient).DeserializeJson<SyncRecordValuesResponse>();
            if (recordValues?.RecordMap?.Block is null || recordValues.RecordMap.Block.Count == 0) throw new InvalidDataException("Invalid response");
            Type blockType = Constants.TypeToBlock[recordValues.RecordMap.Block.First().Value.Value!.Type!];
            return (Entities.BaseBlock)Activator.CreateInstance(blockType, recordValues.RecordMap.Block.First().Value.Value)!;
        }
    }
}