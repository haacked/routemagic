
.\src\.nuget\nuget.exe pack src\RouteDebug\RouteDebugger.csproj -Build -Symbols -Properties Configuration=Release -o .\out
.\src\.nuget\nuget.exe pack src\RouteMagic\RouteMagic.csproj -Build -Symbols -Properties Configuration=Release -o .\out
.\src\.nuget\nuget.exe pack src\RouteMagic.Mvc\RouteMagic.Mvc.csproj -Build -Symbols -Properties Configuration=Release -o .\out
.\src\.nuget\nuget.exe pack src\RouteMagic.WebApi\RouteMagic.WebApi.csproj -Build -Symbols -Properties Configuration=Release -o .\out
