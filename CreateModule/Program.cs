using System.CommandLine;
using System.Diagnostics;
using Console = System.Console;

namespace CreateModule;

internal sealed class Program
{
    private static async Task<int> Main(string[] args)
    {
        var moduleNameArgument = new Argument<string>(
            name: "--module",
            description: "The name of the module");
        var noTestsOption = new Option<bool>(
            name: "--noTests",
            description: "Don't generate tests");
        var mergeDomainAndApplicationOption = new Option<bool>(
            name: "--mdc",
            description: "Merge Domain and Core Application");
        
        var rootCommand = new RootCommand("Create a module in the modular monolithic architecture");
        
        rootCommand.AddArgument(moduleNameArgument);
        rootCommand.AddOption(noTestsOption);
        rootCommand.AddOption(mergeDomainAndApplicationOption);
        
        rootCommand.SetHandler(CreateModule, moduleNameArgument, noTestsOption, mergeDomainAndApplicationOption);
        
        return await rootCommand.InvokeAsync(args);
        
    }

    private static void CreateModule(string? moduleName, bool noTestsOption, bool mergeDomainAndApplicationOption)
    {
        Console.WriteLine($"Creating {moduleName} module...");
        // Create a new folder
        Directory.CreateDirectory($"{moduleName}");

        string[] mainProjects = mergeDomainAndApplicationOption switch
        {
            true =>
            [
                $"{moduleName}.ApplicationCore",
                $"{moduleName}.Infrastructure",
                $"{moduleName}.Presentation"
            ],
            false =>
            [
                $"{moduleName}.Domain",
                $"{moduleName}.Application",
                $"{moduleName}.Infrastructure",
                $"{moduleName}.Presentation"
            ]
        };

        string[] testProjects = [
            $"{moduleName}.UnitTests",
            $"{moduleName}.IntegrationTests",
            $"{moduleName}.ArchitectureTests"
        ];

        foreach (string project in mainProjects)
        {
            string projectDir = $"{moduleName}/{project}";

            RunCommand("dotnet", $"new classlib -n {project} -o {projectDir}");
            
            DeleteClass1File(projectDir);
        }

        if (!noTestsOption)
        {
            foreach (string project in testProjects)
            {
                string projectDir = $"{moduleName}/{project}";

                RunCommand("dotnet", $"new xunit -n {project} -o {projectDir}");
            
                DeleteUnitTest1File(projectDir);
            }
        }
        
        Console.WriteLine("Module and projects created successfully.");
    }

    private static void DeleteClass1File(string projectDir)
    {
        string classFilePath = $"{projectDir}/Class1.cs";
        if (!File.Exists(classFilePath))
        {
            return;
        }

        File.Delete(classFilePath);
    }
    
    private static void DeleteUnitTest1File(string projectDir)
    {
        string unitTestFilePath = $"{projectDir}/UnitTest1.cs";
        if (!File.Exists(unitTestFilePath))
        {
            return;
        }

        File.Delete(unitTestFilePath);
    }

    private static void RunCommand(string command, string args)
    {
        var processInfo = new ProcessStartInfo(command, args)
        {
            RedirectStandardOutput = true,
            RedirectStandardError = true,
            UseShellExecute = false,
            CreateNoWindow = true
        };

        using var process = Process.Start(processInfo);
        process?.WaitForExit();

        if (process != null && process.ExitCode != 0)
        {
            Console.WriteLine($"Error creating project '{args}':");
            Console.WriteLine(process.StandardError.ReadToEnd());
            return;
        }

        Console.WriteLine($"Project '{args}' created successfully.");
    }
    
}
