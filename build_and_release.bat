@echo off
rem Script to build the project, move the executable to the release folder, and rename it with the current date

rem Execute the publish command
dotnet publish -c Release -r win-x86

rem Define the source path and destination folder
set "source_path=.\bin\Release\net6.0\win-x86\publish\PrintApp.exe"
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

rem Rename the executable
ren "%destination_folder%\PrintApp.exe" "%new_filename%"

rem Confirmation message
echo PrintApp.exe has been successfully moved and renamed to %new_filename%.
pause
