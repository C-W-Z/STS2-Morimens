Get-ChildItem . -fi "*.uid" -r | Remove-Item -Force
Get-ChildItem . -fi "*.import" -r | Remove-Item -Force
Get-ChildItem . -fi "obj" -r -Directory | Remove-Item -Recurse -Force
Get-ChildItem . -fi "bin" -r -Directory | Remove-Item -Recurse -Force
Get-ChildItem . -fi ".godot" -r -Directory | Remove-Item -Recurse -Force
# You need to reopen the project by Godot to reimport resources & packages, otherwise the next dotnet build wont success
