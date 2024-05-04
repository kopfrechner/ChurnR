﻿using ChurnR;
using ChurnR.Extensions;
using ChurnR.Options;
using CommandLine;
using Microsoft.Extensions.DependencyInjection;
using Serilog;

var parserResult = Parser.Default.ParseArguments<GitOptions, SvnOptions>(args);

parserResult.WithNotParsed(_ =>
{
    Exit(ExitCode.Parameters);
});

var services = new ServiceCollection(); 

parserResult.WithParsed<GitOptions>(gitOptions => services.AddChurnR(gitOptions));
parserResult.WithParsed<SvnOptions>(svnOptions => services.AddChurnR(svnOptions));

await using var provider = services.BuildServiceProvider();
using var serviceScope = provider.CreateScope();
var engine = serviceScope.ServiceProvider.GetRequiredService<Engine>();

Exit(engine.Run());


void Exit(ExitCode exitCode, string? message = null)
{
    if (!string.IsNullOrWhiteSpace(message))
    {
        Log.Logger.Fatal(message);
    }

    Environment.Exit((int)exitCode);
}