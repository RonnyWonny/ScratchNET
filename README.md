# scratchNET
Run sb3 (Scratch 3.0 Project File) in .net.

The [``scratch``](https://github.com/RonnyWonny/ScratchNET/tree/main/scratch) folder is the scratch virtual machine and handles sprites and scratch blocks.
You can extend [``Scratch``](https://github.com/RonnyWonny/ScratchNET/blob/main/scratch/Scratch.cs), [``Sprite``](https://github.com/RonnyWonny/ScratchNET/blob/main/scratch/Sprite.cs), [``Stage``](https://github.com/RonnyWonny/ScratchNET/blob/main/scratch/Stage.cs), [``BlocksGroup``](http://github.com/RonnyWonny/ScratchNET/blob/main/scratch/BlocksGroup.cs), and etc depending on what you plan on doing with them. 

## MANUAL BUILDING

### Requirements

#### !! WINDOWS ONLY AT THE MOEMENT !!
---
#### [.NET 10](https://dotnet.microsoft.com/en-us/download/dotnet/10.0)
---

to install the library dependencies for ScratchNET. Go to the location of the source code after you installed it, open terminal to source code path and run ``dotnet restore`` and everything should be installed.

```bash
cd "location/to/sourcecode/path
dotnet restore
```

## Running
Running the project is simple. After following the [Requirements](#requirements). In your terminal that is still located to your source code path. Run ``dotnet build``, and once build, it'll automactially run the project for you.

```bash
dotnet build
```

## contribute
#### to do
