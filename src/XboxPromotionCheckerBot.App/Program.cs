// See https://aka.ms/new-console-template for more information

using Cocona;
using Microsoft.Extensions.Configuration;
using XboxPromotionCheckerBot.App.Core.Extensions;
using XboxPromotionCheckerBot.App.Core.UseCases;
using XboxPromotionCheckerBot.App.Infrastructure.Extensions;

var builder = CoconaApp.CreateBuilder();

builder.Configuration.AddJsonFile("appsettings.json", optional: false);
builder.Configuration.AddUserSecrets<Program>();

builder.Services.AddCore(builder.Configuration);
await builder.Services.AddInfrastructure(builder.Configuration);

var app = builder.Build();

// Add a command and set its alias.
app.AddCommand(async ([FromService] ParseGamesFilterAndSendItUseCase useCase,
        [FromService] CoconaAppContext coconaAppContext) =>
    {
        await useCase.Execute(coconaAppContext.CancellationToken);
    })
    .WithDescription("Parse games from Xbox Store and send it to Discord");

await app.RunAsync();

