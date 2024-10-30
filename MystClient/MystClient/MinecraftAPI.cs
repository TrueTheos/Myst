using System;
using System.IO.MemoryMappedFiles;
using System.Net.Sockets;
using System.Text;
using System.Threading.Tasks;
using MystClient.MinecraftObjects;
using Newtonsoft.Json;

namespace MystClient
{
    public class MinecraftAPI : IDisposable
    {
        private const string ServerAddress = "127.0.0.1";
        private const int ServerPort = 5000;

        private readonly JsonSerializer _serializer;

        public MinecraftAPI()
        {
            _serializer = new JsonSerializer();
        }

        public async Task<T> SendCommand<T>(string commandType, params object[] arguments)
        {
            var command = new Command
            {
                Type = commandType,
                Arguments = arguments
            };

            string commandJson = JsonConvert.SerializeObject(command);

            using (var client = new TcpClient(ServerAddress, ServerPort))
            using (var networkStream = client.GetStream())
            using (var writer = new StreamWriter(networkStream, Encoding.UTF8, leaveOpen: true))
            using (var reader = new StreamReader(networkStream, Encoding.UTF8, leaveOpen: true))
            {
                // Send command
                await writer.WriteLineAsync(commandJson);
                await writer.FlushAsync();

                // Receive response
                string responseJson = await reader.ReadLineAsync();
                var settings = new JsonSerializerSettings
                {
                    MissingMemberHandling = MissingMemberHandling.Ignore, // Ignore properties not in the class
                    NullValueHandling = NullValueHandling.Ignore, // Ignore null values
                    Error = (sender, args) => // Suppress errors during deserialization
                    {
                        args.ErrorContext.Handled = true;
                    }
                };

                try
                {
                    var response = JsonConvert.DeserializeObject<Response<T>>(responseJson, settings);
                    return response.Data;
                }
                catch (JsonSerializationException ex)
                {
                    Console.WriteLine("Error during deserialization: " + ex.Message);
                    return default(T); // Return default value for the type, like null or 0
                }
            }
        }

        public async Task<BlockData[]> GetBlocksInRange(int x, int y, int z, int range)
        {
            return await SendCommand<BlockData[]>("GetBlocksInRange", x, y, z, range);
        }

        public async Task<BlockData> GetBlockAtPos(int x, int y, int z)
        {
            return await SendCommand<BlockData>("GetBlockAtPos", x, y, z);
        }

        public async Task<EntityData[]> GetEntitiesInRange(int x, int y, int z, int range)
        {
            return await SendCommand<EntityData[]>("GetEntitiesInRange", x, y, z, range);
        }

        public void Dispose()
        {
            // No need to manually dispose sockets, handled in using statements
        }

        public class Command
        {
            public string Type { get; set; }
            public object[] Arguments { get; set; }
        }

        public class Response<T>
        {
            public bool Success { get; set; }
            public T Data { get; set; }
        }

        public class MinecraftAPIException : Exception
        {
            public MinecraftAPIException(string message) : base(message) { }
        }
    }
}
