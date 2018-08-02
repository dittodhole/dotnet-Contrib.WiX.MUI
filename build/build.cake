#tool "nuget:?package=GitVersion.CommandLine"

var configuration = "Release";
var artifactsDirectory = Directory("../artifacts");
var sourceDirectory = Directory("../src");
var solutionFile = sourceDirectory + File("Contrib.WiX.MUI.sln");

Task("Build")
  .IsDependentOn("Clean")
  .IsDependentOn("Version")
  .IsDependentOn("Restore")
  .Does(() =>
{
  Information($"Building {MakeAbsolute(solutionFile)}");

  MSBuild(solutionFile,
          settings => settings.SetConfiguration(configuration)
                              .SetPlatformTarget(PlatformTarget.MSIL)
                              .SetMaxCpuCount(0)
                              .SetMSBuildPlatform(MSBuildPlatform.x86));

  var projectFile = sourceDirectory + Directory("Contrib.WiX.MUI") + File("Contrib.WiX.MUI.csproj");
  NuGetPack(projectFile,
            new NuGetPackSettings
            {
              OutputToToolFolder = true,
              Properties = new Dictionary<string, string>
              {
                { "Configuration", configuration },
                { "branch", EnvironmentVariable("APPVEYOR_REPO_BRANCH") ?? "develop" }
              },
              IncludeReferencedProjects = true,
              Symbols = true,
              OutputDirectory = artifactsDirectory
            });
});

Task("Clean")
  .Does(() =>
{
  Information($"Cleaning {MakeAbsolute(artifactsDirectory)}");

  if (DirectoryExists(artifactsDirectory))
  {
    DeleteDirectory(artifactsDirectory,
                    new DeleteDirectorySettings
                    {
                      Recursive = true
                    });
  }

  CreateDirectory(artifactsDirectory);
});

Task("Version")
  .Does(() =>
{
  var assemblyInfoFile = sourceDirectory + Directory("Contrib.WiX.MUI") +  Directory("Properties") + File("AssemblyInfo.cs");
  Information($"Versioning {MakeAbsolute(assemblyInfoFile)}");

  GitVersion(new GitVersionSettings
             {
               UpdateAssemblyInfo = true,
               UpdateAssemblyInfoFilePath = assemblyInfoFile,
               OutputType = GitVersionOutput.BuildServer
             });
});

Task("Restore")
  .Does(() =>
{
  Information($"Restoring packages for {MakeAbsolute(solutionFile)}");

  NuGetRestore(solutionFile);
});

var targetArgument = Argument("target", "Build");
RunTarget(targetArgument);
