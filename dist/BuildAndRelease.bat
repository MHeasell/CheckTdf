MSBuild.exe ../CheckTdf.sln /p:Configuration=Release /t:Clean,Build && python MakeReleaseZip.py --release
@pause
