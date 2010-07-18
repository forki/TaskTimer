// include Fake libs
#I "tools\FAKE"
#r "FakeLib.dll"

open Fake

// Directories
let buildDir  = @".\build\"
let testDir   = @".\test\"
let deployDir = @".\deploy\"

// tools
let nunitPath = @".\Tools\NUnit"

// Filesets
let appReferences  = 
    !+ @"src\app\**\*.csproj" 
      ++ @"src\app\**\*.fsproj" 
        |> Scan

let testReferences = 
    !+ @"src\test\**\*.csproj" 
      |> Scan

// version info
let productName = "TaskTimer"

// Targets
Target? Clean <-
    fun _ -> CleanDirs [buildDir; testDir; deployDir]

Target? BuildApp <-
    fun _ -> 
        if buildServer <> LocalBuild then
            AssemblyInfo 
              (fun p -> 
                {p with
                  CodeLanguage = CSharp;
                  AssemblyVersion = buildVersion;
                  AssemblyTitle = "Task Timer";
                  AssemblyDescription = "C# Time tracker for Windows";
                  Guid = "8264251e-ecb4-4221-b995-5f4cab30c24d";
                  OutputFileName = @".\src\app\TaskTimer\Properties\AssemblyInfo.cs"})
                 
      
        // compile all projects below src\app\
        MSBuildRelease buildDir "Build" appReferences
          |> Log "AppBuild-Output: "

Target? BuildTest <-
    fun _ -> 
        MSBuildDebug testDir "Build" testReferences
          |> Log "TestBuild-Output: "

Target? Test <-
    fun _ ->  
        !+ (testDir + @"\*.Specs.dll") 
          |> Scan
          |> NUnit (fun p -> 
                {p with 
                    ToolPath = nunitPath; 
                    DisableShadowCopy = true; 
                    OutputFile = testDir + @"TestResults.xml"}) 

Target? Deploy <-
    fun _ ->
        !+ (buildDir + "\**\*.*") 
          -- "*.zip" 
          |> Scan
          |> Zip buildDir (deployDir + productName + "." + buildVersion + ".zip")

Target? Default <- DoNothing

// Dependencies
For? BuildApp <- Dependency? Clean    
For? BuildTest <- Dependency? Clean
For? Test <- Dependency? BuildApp |> And? BuildTest  
For? Deploy <- Dependency? Test      
For? Default <- Dependency? Deploy
 
// start build
Run? Default