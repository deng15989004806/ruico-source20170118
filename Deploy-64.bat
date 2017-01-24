@echo off
cd /d %~dp0build

cmd /c "build-64.bat /Target:Build" & cd .. & pause & exit /b
