using Microsoft.AspNetCore.SignalR.Client;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.Logging;

IConfigurationRoot configuration;
IConfigurationBuilder builder = new ConfigurationBuilder().AddJsonFile("appsettings.json", optional: false);

configuration = builder.Build();

string runnerIp = Environment.GetEnvironmentVariable("RUNNER_IPV4") ?? configuration.GetSection("RunnerIP").Value!;

if (!runnerIp.StartsWith("http://"))
{
	runnerIp = $"http://{runnerIp}";
}

string botNickname = Environment.GetEnvironmentVariable("BOT_NICKNAME") ?? configuration.GetSection("BotNickname")?.Value ?? "WhatWhere";
string token = Environment.GetEnvironmentVariable("Token") ?? new Guid("no-token").ToString();
string port = configuration.GetSection("RunnerPort").Value ?? "5000";
string runnerHubUrl = $"{runnerIp}:{port}/runnerhub";

HubConnection connection = new HubConnectionBuilder()
	.WithUrl(runnerHubUrl)
	.ConfigureLogging(logging =>
	{
		logging.SetMinimumLevel(LogLevel.Debug);
	})
	.WithAutomaticReconnect()
	.Build();

Console.WriteLine("Started connection!");

connection.StartAsync().Wait();

Console.WriteLine("Connected to Runner!");
Console.WriteLine("Press any key to continue");
Console.ReadLine();