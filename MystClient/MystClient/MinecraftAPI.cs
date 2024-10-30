using System;
using System.IO.MemoryMappedFiles;
using System.Net.Sockets;
using System.Text;
using System.Threading.Tasks;
using MystClient.Commands;
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

        class ServerCommand
        {
            public string CommandType { get; set; }
            public object[] Arguments { get; set; }
        }

        public async Task<Response<T>> SendCommand<T>(Command command)
        {
            return await SendCommandToServer<T>(command);
        }

        private async Task<Response<T>> SendCommandToServer<T>(Command command)
        {
            ServerCommand serverCommand = new ServerCommand()
            {
                CommandType = command.Type,
                Arguments = command.Arguments
            };

            string commandJson = JsonConvert.SerializeObject(serverCommand);

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
                    MissingMemberHandling = MissingMemberHandling.Ignore,
                    NullValueHandling = NullValueHandling.Ignore,
                    Error = (sender, args) =>
                    {
                        args.ErrorContext.Handled = true;
                    }
                };

                try
                {
                    return JsonConvert.DeserializeObject<Response<T>>(responseJson, settings);
                }
                catch (JsonSerializationException ex)
                {
                    Console.WriteLine("Error during deserialization: " + ex.Message);
                    return new Response<T> { Success = false, Data = default(T) };
                }
            }
        }

        public void Dispose() { }

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
