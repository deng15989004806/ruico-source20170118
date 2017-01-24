@IF NOT EXIST "C:\Program Files\MSBuild\12.0\Bin\MSBuild.exe" @ECHO COULDN'T FIND MSBUILD: "C:\Program Files\MSBuild\12.0\Bin\MSBuild.exe" (Is .NET 4 installed?)

set target="%1"
"C:\Program Files\MSBuild\12.0\Bin\MSBuild.exe" Soultion.msbuild %target%
