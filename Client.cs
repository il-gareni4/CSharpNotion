using CSharpNotion.Api.General;
using CSharpNotion.Api.Request;
using CSharpNotion.Api.Response;
using CSharpNotion.Entities;
using CSharpNotion.Entities.Blocks;
using System.Net;
using System.Text.Json;

namespace CSharpNotion
{
    public class Client
    {
        public readonly HttpClient HttpClient;
        private readonly CookieContainer cookieContainer;
        private readonly HttpClientHandler httpClientHandler;
        private readonly string _tokenV2;
        private readonly List<Transaction> _transactions = new();
        private readonly List<Operation> _operations = new();
        private readonly List<Action> _actions = new();
        private bool _settedUp = false;
        private UserSettings? _user;
        private readonly CacheManager _cache;

        public UserSettings User
        {
            get
            {
                CheckSetupState();
                return _user!;
            }
        }

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
            _cache = new CacheManager();
            cookieContainer = new CookieContainer();
            cookieContainer.Add(new Cookie("token_v2", _tokenV2, "", "www.notion.so"));
            httpClientHandler = new() { CookieContainer = cookieContainer };
            HttpClient = new HttpClient(httpClientHandler);
        }

        /// <summary>
        /// Setup a <see cref="Client"/> for use (get a user settings)
        /// </summary>
        /// <returns></returns>
        public async Task<Client> Setup()
        {
            if (_settedUp) return this;

            RecordMap recordMap = (await ReqSetup.GetSpaces().Send(HttpClient).DeserializeJson<Dictionary<string, RecordMap>>()).First().Value;
            _user = new UserSettings((recordMap.UserSettings!).First().Value.Value!);
            if (recordMap.Block is not null)
                foreach (var blockRole in recordMap.Block.Values)
                    _cache.CacheBlock(Utils.ConvertBlockFromResponse(this, blockRole.Value!));

            _settedUp = true;
            return this;
        }

        private void CheckSetupState()
        {
            if (!_settedUp) throw new InvalidOperationException("Client is not setted up. Use 'await Client.Setup()' before using this method");
        }

        /// <summary>
        /// Get a <typeparamref name="T"/> by <paramref name="blockId"/>
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="blockId">Id of block you need</param>
        /// <returns></returns>
        /// <exception cref="InvalidDataException">Inva</exception>
        public async Task<T> GetBlockAsync<T>(string blockId) where T : BaseBlock
        {
            CheckSetupState();
            blockId = Utils.ExtractId(blockId);
            BaseBlock? cachedBlock = _cache.GetCachedBlock(blockId);
            if (cachedBlock is not null) return (T)cachedBlock;

            RecordMapResopnse recordValues = await ReqSetup.SyncRecordValues(blockId, "block").Send(HttpClient).DeserializeJson<RecordMapResopnse>();
            if (recordValues?.RecordMap?.Block is null || recordValues.RecordMap.Block.Count == 0) throw new InvalidDataException("Invalid response");
            string? needType = Constants.BlockTypeToTypeName.GetValueOrDefault(typeof(T));
            string blockType = recordValues.RecordMap.Block.First().Value.Value!.Type!;
            if (needType != blockType) throw new InvalidDataException($"Excepted type of block '{needType}', got '{blockType}'");
            T blockInstance = Utils.ActivatorCreateNewBlock<T>(this, recordValues.RecordMap.Block.First().Value.Value!);
            _cache.CacheBlock(blockInstance);
            return blockInstance;
        }

        /// <summary>
        /// Get a <see cref="BaseBlock"/> by <paramref name="blockId"/>
        /// </summary>
        /// <param name="blockId">Id of block you need</param>
        /// <returns></returns>
        /// <exception cref="InvalidDataException">Response doesn't contain a block</exception>
        public async Task<BaseBlock> GetBlockAsync(string blockId)
        {
            CheckSetupState();
            blockId = Utils.ExtractId(blockId);
            BaseBlock? cachedBlock = _cache.GetCachedBlock(blockId);
            if (cachedBlock is not null) return cachedBlock;

            RecordMapResopnse recordValues = await ReqSetup.SyncRecordValues(blockId, "block").Send(HttpClient).DeserializeJson<RecordMapResopnse>();
            if (recordValues?.RecordMap?.Block is null || recordValues.RecordMap.Block.Count == 0) throw new InvalidDataException("Invalid response");
            BaseBlock blockInstance = Utils.ConvertBlockFromResponse(this, recordValues.RecordMap.Block.First().Value.Value!);
            _cache.CacheBlock(blockInstance);
            return blockInstance;
        }

        /// <summary>
        /// Get a <see cref="List{T}"/> of <see cref="BaseBlock"/> with one request
        /// </summary>
        /// <param name="blockIds">List of block ids</param>
        /// <returns></returns>
        /// <exception cref="InvalidDataException">Response doesn't contain blocks</exception>
        public async Task<List<BaseBlock>> GetBlocksAsync(IEnumerable<string> blockIds)
        {
            CheckSetupState();
            List<BaseBlock> resultBlocks = new();
            blockIds = blockIds.Select((id) => Utils.ExtractId(id));
            List<string> idsBlocksNeedsToFetch = new();

            foreach (string blockId in blockIds)
            {
                BaseBlock? cachedBlock = _cache.GetCachedBlock(blockId);
                if (cachedBlock is not null) resultBlocks.Add(cachedBlock);
                else idsBlocksNeedsToFetch.Add(blockId);
            }

            IEnumerable<Pointer> blockPointers = idsBlocksNeedsToFetch.Select((id) => new Pointer(id, "block"));
            RecordMapResopnse recordValues = await ReqSetup.SyncRecordValues(blockPointers).Send(HttpClient).DeserializeJson<RecordMapResopnse>();
            if (recordValues?.RecordMap?.Block is null || recordValues.RecordMap.Block.Count == 0) throw new InvalidDataException("Invalid response");
            foreach (var blockRole in recordValues.RecordMap.Block.Values)
            {
                BaseBlock blockInstance = Utils.ConvertBlockFromResponse(this, blockRole.Value!);
                _cache.CacheBlock(blockInstance);
                resultBlocks.Add(blockInstance);
            }
            return resultBlocks;
        }

        internal void CacheBlock(BaseBlock block) => _cache.CacheBlock(block);

        internal void AddOperation(Operation operation) => _operations.Add(operation);

        internal void AddOperation(Operation operation, Action action)
        {
            _operations.Add(operation);
            _actions.Add(action);
        }

        internal void OperationsToTransaction()
        {
            if (_operations.Count == 0) return;
            else
            {
                _transactions.Add(new Transaction() { Operations = _operations.ToArray() });
                _operations.Clear();
            }
        }

        /// <summary>
        /// Commit all changes (operations) by sending a request to the Notion server
        /// </summary>
        public async Task Commit()
        {
            OperationsToTransaction();
            if (_transactions.Count == 0) return;
            (await ReqSetup.SaveTransactions(_transactions).Send(HttpClient)).EnsureSuccessStatusCode();
            foreach (Action action in _actions) action();
            _transactions.Clear();
        }
    }
}