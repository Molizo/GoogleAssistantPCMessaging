using MQTTnet;
using MQTTnet.Client;
using System;
using System.Text;
using System.Threading.Tasks;

namespace GoogleAssistantPCMessaging
{
    internal class Program
    {
        private static string nickname;
        private static string username;
        private static string password;
        private static string key;

        private static void Main(string[] args)
        {
            RunInit();
            var factory = new MqttFactory();
            var mqttClient = factory.CreateMqttClient();
            var options = new MqttClientOptionsBuilder()
                                .WithClientId("PC Client")
                                .WithTcpServer("io.adafruit.com", 8883)
                                .WithCredentials(username, password)
                                .WithTls()
                                .WithCleanSession()
                                .Build();

            mqttClient.Disconnected += async (s, e) =>
            {
                Console.WriteLine("[System] Disconnected from Adafruit IO");
                await Task.Delay(TimeSpan.FromSeconds(5));

                try
                {
                    await mqttClient.ConnectAsync(options);
                }
                catch
                {
                    Console.WriteLine("[System] Connection failed");
                    Console.WriteLine("[System] Please check your connection and credentials");
                    Console.WriteLine("[System] The credentials may be reentered using /init");
                }
            };
            mqttClient.Connected += async (s, e) =>
            {
                Console.WriteLine("[System] Connected to Adafruit IO");
                await mqttClient.SubscribeAsync(new TopicFilterBuilder().WithTopic(username + "/feeds/" + key).Build());
                Console.WriteLine("[System] Waiting for messages");
            };
            mqttClient.ApplicationMessageReceived += (s, e) =>
            {
                Console.WriteLine(Encoding.UTF8.GetString(e.ApplicationMessage.Payload));
            };
            MqttConnect(mqttClient, options).Wait();
            while (true)
            {
                string message;
                message = Console.ReadLine();
                if (message.ToCharArray()[0] == '/')
                {
                    switch (message)
                    {
                        case "/help":
                            Console.WriteLine("[System] Available commands:");
                            Console.WriteLine("[System] /help - Displays this menu");
                            Console.WriteLine("[System] /clear - Clears the screen");
                            Console.WriteLine("[System] /init - Runs the first time use dialog");
                            continue;

                        case "/clear":
                            Console.Clear();
                            continue;

                        case "/init":
                            System.IO.File.Delete("config.ini");
                            RunInit();
                            continue;

                        default:
                            Console.WriteLine("[System] The command you have entered is not valid");
                            Console.WriteLine("[System] For a list of commands type /help");
                            continue;
                    }
                }
                else
                    SendMessage(mqttClient, message).Wait();
            }
        }

        private static async Task MqttConnect(IMqttClient mqttClient, IMqttClientOptions options)
        {
            Console.WriteLine("[System] Attempting to connect to Adafruit IO...");
            await mqttClient.ConnectAsync(options);
        }

        private static async Task SendMessage(IMqttClient mqttClient, string message)
        {
            var mqttMessage = new MqttApplicationMessageBuilder()
                                  .WithTopic(username + "/feeds/" + key)
                                  .WithPayload(message)
                                  .Build();

            await mqttClient.PublishAsync(mqttMessage);
        }

        private static void RunInit()
        {
            if (System.IO.File.Exists("config.ini"))
            {
                Console.WriteLine("[System] Reading configuration file");
                string[] data = System.IO.File.ReadAllLines("config.ini");
                nickname = data[0];
                username = data[1];
                password = data[2];
                key = data[3];
                return;
            }
            Console.WriteLine("Copyright 2018 Visoiu Mihnea Theodor");
            Console.WriteLine();
            Console.WriteLine("This application communicates with");
            Console.WriteLine("Google Assistant on a device using");
            Console.WriteLine("Adafruit IO and IFTTT");
            Console.WriteLine();
            Console.WriteLine("You must add the IFTTT applet linked at");
            Console.WriteLine("https://ifttt.com/applets/fL8CTDpz-google-assistant-to-pc-messages");
            Console.WriteLine("in order to use this software");
            Console.WriteLine();
            Console.WriteLine("All the user data is stored in your");
            Console.WriteLine("Adafruit IO feed");
            Console.WriteLine();
            Console.WriteLine("To list the available commands please type /help");
            Console.WriteLine();
            Console.WriteLine();
            Console.Write("Please enter a name for this PC: ");
            nickname = Console.ReadLine();
            Console.Write("Please enter your Adafruit IO username: ");
            username = Console.ReadLine();
            Console.Write("Please enter your AIO key: ");
            password = Console.ReadLine();
            Console.Write("Please enter your Adafruit IO feed key: ");
            key = Console.ReadLine();
            Console.Clear();
            Console.WriteLine("[System] Writing configuration file");
            string configData = nickname + System.Environment.NewLine +
                                username + System.Environment.NewLine +
                                password + System.Environment.NewLine +
                                key;
            System.IO.File.WriteAllText("config.ini", configData);
        }
    }
}