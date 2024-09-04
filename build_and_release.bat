@echo off
rem Script to build the project, move the executable to the release folder, and rename it with the current date

rem Execute the publish command
dotnet publish -c Release -r win-x86

rem Define the source path and destination folder
set "source_path=.\bin\Release\net6.0-windows\win-x86\publish\PrintApp.exe"
set "destination_folder=..\printAppv2\release"

rem Create the release directory if it doesn't exist
if not exist "%destination_folder%" (
    mkdir "%destination_folder%"
)

rem Move the executable to the release folder
move "%source_path%" "%destination_folder%\PrintApp.exe"

rem Get the current date in dd-MM-yy format

rem Remove any leading zeroes from the day and month (optional, depending on your preference)
SET YY=%DATE:~-4%
SET MM=%DATE:~-7,2%
SET DD=%DATE:~-10,2%
set date=%YY%-%MM%-%DD%

rem Define the new filename with the date
set "new_filename=PrintApp_%date%.exe"

rem If the file already exists, delete it
if exist "%destination_folder%\%new_filename%" (
    del "%destination_folder%\%new_filename%"
)

rem Rename the executable
ren "%destination_folder%\PrintApp.exe" "%new_filename%"

set "version_Name=PrintApp_%date%"
set "name_Scrip=Run_%version_Name%.bat"
if exist "%destination_folder%\%name_Scrip%" (
    del "%destination_folder%\%name_Scrip%"
)

rem Generate the run.bat script
(
    echo @echo off
    echo REM Este script ejecuta %new_filename% con el archivo test.txt como argumento
    echo.
    echo REM Cambia al directorio donde est%E1 %new_filename% si es necesario
    echo REM cd /d "C:\ruta\a\tu\aplicacion"
    echo.
    echo REM Ejecuta el comando con el archivo de prueba
    echo %new_filename% test.txt
) > "%destination_folder%\%name_Scrip%"


rem Confirmation message
echo PrintApp.exe has been successfully moved and renamed to %new_filename%.
pause
