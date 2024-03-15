using Avro.Generic;
using Confluent.Kafka;
using Confluent.SchemaRegistry;
using Confluent.SchemaRegistry.Serdes;
using QCSMobile2024.Shared.Models.CustomModels;
using System.Net.Sockets;

namespace QCSMobile2024.Server.Kafka
{
    public class KafkaClient
    {
        public static string KafkaHost { get; set; } = "192.168.29.82";
        public static string KafkaHost2 { get; set; } = "";
        public static string InboxTopic { get; set; } = "EAS_Updates";

        static KafkaClient()
        {
            Task.Run(CheckConnection);
        }


        public static DateTime? KafkaHostLastCheck = null;
        public static DateTime? KafkaHost2LastCheck = null;
        static async void CheckConnection()
        {
            while (true)
            {
                using (TcpClient tcpClient = new TcpClient())
                {
                    var producerMessage = "";
                    try
                    {
                        tcpClient.Connect(KafkaHost, 9092);
                    }
                    catch (Exception ex)
                    {
                        producerMessage = "Cannot Connect to Kakfa Producer.";
                    }
                    ProducerErrorMessage = producerMessage;
                }
                using (TcpClient tcpClient = new TcpClient())
                {
                    var schemaMessage = "";
                    try
                    {
                        tcpClient.Connect(KafkaHost, 8081);
                        schemaMessage = "";
                    }
                    catch (Exception ex)
                    {
                        schemaMessage = "Cannot Connect to Kakfa Schema Registry.";
                    }
                    SchemaRegistryErrorMessage = schemaMessage;
                };
                KafkaStarted = true;
                KafkaHostLastCheck = DateTime.Now;
                KafkaConnectionUpdate?.Invoke(new object(), EventArgs.Empty);

                using (TcpClient tcpClient = new TcpClient())
                {
                    var producerMessage = "";
                    try
                    {
                        tcpClient.Connect(KafkaHost2, 9092);
                    }
                    catch (Exception ex)
                    {
                        producerMessage = "Cannot Connect to Kakfa Producer.";
                    }
                    Producer2ErrorMessage = producerMessage;
                }
                using (TcpClient tcpClient = new TcpClient())
                {
                    var schemaMessage = "";
                    try
                    {
                        tcpClient.Connect(KafkaHost2, 8081);
                        schemaMessage = "";
                    }
                    catch (Exception ex)
                    {
                        schemaMessage = "Cannot Connect to Kakfa Schema Registry.";
                    }
                    SchemaRegistry2ErrorMessage = schemaMessage;
                };
                Kafka2Started = true;
                KafkaHost2LastCheck = DateTime.Now;
                KafkaConnectionUpdate?.Invoke(new object(), EventArgs.Empty);

                /// not sure this will work in ASP.NET - has a good way of killing threads depending on start point
                await Task.Delay(TimeSpan.FromMinutes(1));
            }
        }
        public static string ProducerErrorMessage { get; private set; }
        public static string Producer2ErrorMessage { get; private set; }
        public static bool KafkaStarted { get; private set; }
        public static bool Kafka2Started { get; private set; }
        public static string SchemaRegistryErrorMessage { get; private set; }
        public static string SchemaRegistry2ErrorMessage { get; private set; }
        public static event EventHandler KafkaConnectionUpdate;


        static IProducer<string, GenericRecord> cachedProducer;
        static CachedSchemaRegistryClient schemaRegistry;

        static IProducer<string, GenericRecord> GetProducer1()
        {
            try
            {
                if (cachedProducer != null)
                    //    cachedProducer.Dispose();
                    //cachedProducer = null;
                    return cachedProducer;

                var config = new ProducerConfig
                {
                    BootstrapServers = KafkaHost + ":9092",
                    MessageTimeoutMs = 2000
                };

                schemaRegistry = new CachedSchemaRegistryClient(new SchemaRegistryConfig { Url = KafkaHost + ":8081", RequestTimeoutMs = 2000 });
                cachedProducer = new ProducerBuilder<string, GenericRecord>(config)
                    //.SetKeySerializer(new AvroSerializer<string>(schemaRegistry))
                    .SetValueSerializer(new AvroSerializer<GenericRecord>(schemaRegistry))
                    .Build();
                return cachedProducer;
            }
            catch (Exception ex)
            {
                PrimaryErrorMessage = ex.ToString();
                //ex.LogError();
            }
            return null;
        }

        static IProducer<string, GenericRecord> cachedProducer2;
        static CachedSchemaRegistryClient schemaRegistry2;

        static IProducer<string, GenericRecord> GetProducer2()
        {
            try
            {
                if (cachedProducer2 != null)
                    return cachedProducer2;

                var config = new ProducerConfig
                {
                    BootstrapServers = KafkaHost2 + ":9092",
                };
                schemaRegistry2 = new CachedSchemaRegistryClient(new SchemaRegistryConfig { Url = KafkaHost2 + ":8081" });
                cachedProducer2 = new ProducerBuilder<string, GenericRecord>(config)
                    .SetKeySerializer(new AvroSerializer<string>(schemaRegistry2))
                    .SetValueSerializer(new AvroSerializer<GenericRecord>(schemaRegistry2))
                    .Build();
                return cachedProducer2;
            }
            catch (Exception ex)
            {
                BackupErrorMessage = ex.ToString();
                //ex.LogError();
            }
            return null;
        }

        public static DateTime? PrimaryLastSent = null;
        public static DateTime? BackupLastSent = null;

        public static async Task SendUpdate(KafkaUpdate update)
        {
            var body = KafkaUpdate.GetGenericRecord(update);
            try
            {
                var producer = GetProducer1();
                var start = DateTime.Now;
                var dr = await producer.ProduceAsync(InboxTopic, new Message<string, GenericRecord>
                {
                    Key = Guid.NewGuid().ToString(),// null,// update.ClaimNumber,
                    Value = body
                });
                var sendTime = DateTime.Now - start;
                PrimaryLastSent = DateTime.Now;
                Console.WriteLine("Sending Update took " + sendTime.ToString());
            }
            catch (ProduceException<string, GenericRecord> e)
            {
                //e.LogError();
                PrimaryErrorMessage = $"Error publishing update for claim: {update.Message} - {update.ClaimNumber} - {update.CarrierName}" +
                    $"\r\n\r\n {e}".Replace("\r\n", "<br />");
                cachedProducer = null; // flags the producer for a rebuild...
                                        // todo: send failure back to user?
                throw;
            }
            if (!string.IsNullOrEmpty(KafkaHost2))
            {
                try
                {
                    var producer2 = GetProducer2();
                    var start = DateTime.Now;
                    var dr = await producer2.ProduceAsync(InboxTopic, new Message<string, GenericRecord>
                    {
                        Key = update.ClaimNumber,
                        Value = body
                    });
                    var sendTime = DateTime.Now - start;
                    BackupLastSent = DateTime.Now;
                    Console.WriteLine("Sending Update took " + sendTime.ToString());
                }
                catch (ProduceException<string, GenericRecord> e)
                {
                    //e.LogError();
                    BackupErrorMessage = $"Error publishing update for claim: {update.Message} - {update.ClaimNumber} - {update.CarrierName} " +
                    $"\r\n\r\n {e}".Replace("\r\n", "<br />");
                    cachedProducer2 = null; // flags the producer for a rebuild...
                                            // todo: send failure back to user?
                    throw;
                }
            }
        }

        public static string PrimaryErrorMessage { get; set; }
        public static string BackupErrorMessage { get; set; }
    }
}
