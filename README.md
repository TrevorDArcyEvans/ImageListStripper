# ImageListStripper
Recover those lost images from .resx files

```bash
$ .\ImageListStripper.exe .\data\imgCodeCompletion.txt
Stripping images from:  .\data\imgCodeCompletion.txt
  imgCodeCompletion-image-0.png
  imgCodeCompletion-image-1.png
  imgCodeCompletion-image-2.png
  imgCodeCompletion-image-3.png
  imgCodeCompletion-image-4.png
  imgCodeCompletion-image-5.png
```

## Background
Quite often during the development of Windows desktop applications, images are added to an image list
but the original, source images are lost.  This makes it difficult to subsequently update the image list.

.NET serialises the original images into a .resx file, so we should be able to deserialise them.

This utility extracts the original images from a .resx file and saves them to disk - you're welcome!

## Prerequisites
* Windows
* .NET Framework 4.8

## Getting started
```bash
git clone https://github.com/TrevorDArcyEvans/ImageListStripper.git
cd ImageListStripper
dotnet build
```

## Data preparation
Just copy image list data from .resx file into a plain text file.  Then pass the path to
_ImageListStripper_ on the command line.

For example, see [MainForm.resx](data/MainForm.resx) [lines 125-179] have been copied to
[imgList.txt](data/imgList.txt)

## Usage
```bash
$ ImageListStripper.exe [path-to-image-data-file]

$ ImageListStripper.exe data/imgList.txt
```

## Sample data
Image list data extracted from [MainForm.resx](data/MainForm.resx)
* [imgList.txt](data/imgList.txt)
* [imgCodeCompletion.txt](data/imgCodeCompletion.txtt)

## Further work
* support images
  * [recentDBsDropDownButton.txt](data/recentDBsDropDownButton.txt)
* support icons
  * [Icon.txt](data/Icon.txt)
* automatically process .resx file

## Acknowledgements
Largely based on code stolen from:
* https://stackoverflow.com/questions/52644854/deserialize-windowsforms-imagelist
* https://stackoverflow.com/questions/1397512/find-image-format-using-bitmap-object-in-c-sharp
