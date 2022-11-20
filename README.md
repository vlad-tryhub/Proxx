### General architecture idea

The solution contains 3 prjects
* Executable console application
* 'Core' project containing only domain logic
* 'Console UI' project containing presentation logic

### Usage
Change game configuration in appsettings.json (or leave default) and start main porject

### Notes
* Solution is build on dotnet 6
* Game board signs:
    * `?` - closed cell
    * `X` - revealed black hole (lose)
    * a digit - opened cell
* There are cases with multiple types inside one file. It was done consciously for convenience.
* There are some TODOs left which I'm lazy to finish