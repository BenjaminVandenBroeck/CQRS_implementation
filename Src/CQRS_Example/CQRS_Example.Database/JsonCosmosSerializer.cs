using Microsoft.Azure.Cosmos;
using System.Text;
using Newtonsoft.Json;

namespace CQRS_Example.Database
{
    internal class JsonCosmosSerializer : CosmosSerializer
    {
        private static readonly Encoding DefaultEncoding = new UTF8Encoding(false, true);
        private readonly JsonSerializer _serializer;
        
        public JsonCosmosSerializer(JsonSerializerSettings settings)
        {
            _serializer= JsonSerializer.Create(settings);
        }

        public override T FromStream<T>(Stream stream)
        {
            if(typeof(Stream).IsAssignableFrom(typeof(T)))
            {
                return (T)(object)stream;
            }

            using var sr= new StreamReader(stream);
            using var jsonTextReader= new JsonTextReader(sr);
            return _serializer.Deserialize<T>(jsonTextReader);
        }

        public override Stream ToStream<T>(T input)
        {
            var streamPayLoad = new MemoryStream();
            using var streamWriter = new StreamWriter(streamPayLoad, encoding: DefaultEncoding, bufferSize:1024, leaveOpen:true);
            using JsonWriter writer= new JsonTextWriter(streamWriter);

            writer.Formatting= _serializer.Formatting;
            _serializer.Serialize(writer, input);
            writer.Flush();
            streamWriter.Flush();

            streamPayLoad.Position = 0;
            return streamPayLoad;
        }
    }
}
