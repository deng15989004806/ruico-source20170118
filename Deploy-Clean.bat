@echo off
cd /d %~dp0build

rem cmd /c "build.bat /Target:Clean" & cd .. & pause & exit /b

rem win8 use
cmd /c "build-win8.bat /Target:Clean" & cd .. & pause & exit /b
