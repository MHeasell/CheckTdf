@echo off
echo Checking %1 for TDF problems...
"%~dp0CheckTdf.exe" %1 > "%~dp0CheckTdf-output.txt"
if errorlevel 6 (
    echo Errors and warnings found, results written to %~dp0CheckTdf-output.txt
    goto End
)
if errorlevel 4 (
    echo Warnings found, results written to %~dp0CheckTdf-output.txt
    goto End
)
if errorlevel 2 (
    echo Errors found, results written to %~dp0CheckTdf-output.txt
    goto End
)
if errorlevel 1 (
    echo Failed to check file.
    goto End
)
if errorlevel 0 (
    echo No errors/warnings found!
    goto End
)
:End
pause
