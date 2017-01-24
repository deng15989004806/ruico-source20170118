@echo off
cd /d %~dp0build

cmd /c "build.bat /Target:Build" & cd .. & pause & exit /b
