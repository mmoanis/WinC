# WinC
Run c/c++ "scripts" on windows on the go!!

> "There isn't much that's special about C. That's one of the reasons why it's fast."

[Based on](https://github.com/ryanmjacobs/c) Ryan's c project that let c files runs the shell way.
Here comes its windows version.
With this dll you can run any c/c++ "script" just by a double click!
You need no IDE or terminals, just do it the windows and shell way and double click the script to run it!



## How to install it?
Just make sure you have MinGW and it ```..\MinGW\bin``` is in the ```%PATH%```. Then run install.bat file to setup WinC.

## How to use it?
Right click any c/c++ file and click the Run! menu item. WinC will asume your input are in input.txt file near
the source code and it will make output.txt file in same directory.

## What is missing?
The project is still at the very beginning, for now those are the things planned to:
- running multiple files
- support Visual c++ cl compiler
- run with arguments
- ~~add custom compiling flags~~ Done!

## Notes
WinC is **not** a compiler, its just a front end for MinGW (for now) to quickly running c/c++ files as shell scripts.

## Contributing?
WinC is under GNU GPL2. You can fork and add what ever you want, if you want to make pull requests you are welcomed.
WinC is made with SharpShell library that makes the context menu entry and necessary registry entries for all c files.
