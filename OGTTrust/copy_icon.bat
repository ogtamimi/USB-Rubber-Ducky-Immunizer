@echo off
echo Copying icon file...
echo.

REM Check if 3.ico exists in Downloads
if exist "%USERPROFILE%\Downloads\3.ico" (
    echo Found 3.ico in Downloads folder
    echo Copying to project directory...
    
    REM Copy and rename the icon file
    copy "%USERPROFILE%\Downloads\3.ico" "app.ico"
    
    if errorlevel 0 (
        echo Icon file copied successfully!
        echo File: app.ico
        echo.
        echo Now you can build the application:
        echo 1. dotnet clean
        echo 2. dotnet restore  
        echo 3. dotnet build --configuration Release
        echo 4. bin\Release\net10.0-windows\USBSec.exe
    ) else (
        echo Failed to copy icon file. Please check permissions.
    )
) else (
    echo ERROR: 3.ico not found in Downloads folder
    echo Please make sure the file exists at: %USERPROFILE%\Downloads\3.ico
)

echo.
pause
