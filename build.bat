.\tools\nuget.exe pack src\RouteDebug\RouteDebugger.csproj -Build -Symbols -Properties Configuration=Release -o .\out
.\tools\nuget.exe pack src\RouteMagic\RouteMagic.csproj -Build -Symbols -Properties Configuration=Release -o .\out
.\tools\nuget.exe pack src\RouteMagic.Mvc\RouteMagic.Mvc.csproj -Build -Symbols -Properties Configuration=Release -o .\out