cd HMI
start /min start-HMI
@echo off
start /min D:\Projects\GreenEye\Debug\FleetController 50
start D:\Projects\GreenEye\Logic\bin\Debug\Logic "Full"
exit
