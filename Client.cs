using CSharpNotion.Api.General;
using CSharpNotion.Api.Request;
using CSharpNotion.Api.Response;
using CSharpNotion.Entities;
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
        private User? _user;

        public User User
        {
            get
            {
                CheckSetupState();
                return _user!;
            }
            set => _user = value;
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
            cookieContainer = new CookieContainer();
            cookieContainer.Add(new Cookie("token_v2", _tokenV2, "", "www.notion.so"));
            httpClientHandler = new() { CookieContainer = cookieContainer };
            HttpClient = new HttpClient(httpClientHandler);
        }

        public async Task<Client> Setup()
        {
            if (_settedUp) return this;

            RecordMap recordMap = (await QuickRequestSetup.GetSpaces().Send(HttpClient).DeserializeJson<Dictionary<string, RecordMap>>()).First().Value;
            User = new User(recordMap.UserSettings!.First().Value.Value!);

            _settedUp = true;
            return this;
        }

        private void CheckSetupState()
        {
            if (!_settedUp) throw new InvalidOperationException("Client is not setted up. Use 'await Client.Setup()' before using this method");
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
        public async Task<T> GetBlockAsync<T>(string pageId) where T : Entities.BaseBlock
        {
            CheckSetupState();
            pageId = Utils.ExtractId(pageId);
            RecordMapResopnse recordValues = await QuickRequestSetup.SyncRecordValues(pageId, "block").Send(HttpClient).DeserializeJson<RecordMapResopnse>();
            if (recordValues?.RecordMap?.Block is null || recordValues.RecordMap.Block.Count == 0) throw new InvalidDataException("Invalid response");
            if (Constants.BlockTypeToTypeName.GetValueOrDefault(typeof(T)) != recordValues.RecordMap.Block.First().Value.Value!.Type) throw new InvalidDataException("Invalid type of block excepted");
            return Utils.ActivatorCreateNewBlock<T>(this, recordValues.RecordMap.Block.First().Value.Value!);
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
        public async Task<BaseBlock> GetBlockAsync(string pageId)
        {
            CheckSetupState();
            pageId = Utils.ExtractId(pageId);
            RecordMapResopnse recordValues = await QuickRequestSetup.SyncRecordValues(pageId, "block").Send(HttpClient).DeserializeJson<RecordMapResopnse>();
            if (recordValues?.RecordMap?.Block is null || recordValues.RecordMap.Block.Count == 0) throw new InvalidDataException("Invalid response");
            return Utils.ConvertBlockFromResponse(this, recordValues.RecordMap.Block.First().Value.Value!);
        }

        internal void AddOperation(Operation operation) => _operations.Add(operation);

        internal void AddOperation(Operation operation, Action action)
        {
            _operations.Add(operation);
            _actions.Add(action);
        }

        internal void OperationsToTransaction()
        {
            if (_operations.Count == 0)
            {
                return;
            }
            else
            {
                _transactions.Add(new Transaction() { Operations = _operations.ToArray() });
                _operations.Clear();
            }
        }

        public async Task Commit()
        {
            OperationsToTransaction();
            if (_transactions.Count == 0) return;
            (await QuickRequestSetup.SaveTransactions(_transactions.ToArray()).Send(HttpClient)).EnsureSuccessStatusCode();
            foreach (Action action in _actions) action();
            _transactions.Clear();
        }
    }
}