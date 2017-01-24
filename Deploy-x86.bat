@echo off
cd /d %~dp0build

cmd /c "build-x86.bat /Target:Build" & cd .. & pause & exit /b
