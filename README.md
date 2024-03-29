﻿[marketplace]: https://marketplace.visualstudio.com/items?itemName=MadsKristensen.DocumentMargin
[vsixgallery]: http://vsixgallery.com/extension/DocumentMargin.a5a7bd52-f250-4930-83b4-2085f8b9c7de4/
[repo]:https://github.com/madskristensen/DocumentMargin

# Editor Info extension for Visual Studio

<!--[![Build](https://github.com/madskristensen/DocumentMargin/actions/workflows/build.yaml/badge.svg)](https://github.com/madskristensen/DocumentMargin/actions/workflows/build.yaml)-->

Download this extension from the [Visual Studio Marketplace][marketplace]
or get the [CI build][vsixgallery].

----------------------------------------

> Inspired by [a popular Visual Studio feature request](https://developercommunity.visualstudio.com/t/Code-Editor-Horizontal-Scroll-Bar-Displa/10514754) on Developer Community.

This extension adds information about the document to the bottom editor margin. It shows the selection, length, and language details, as well as the encoding of the of the file and let you easily change it.

![Basic](art/basic.png)

## Document info
Hovering the mouse over the info icon in the corner, shows a tootip with more details about the document.

![Info](art/info.png)

## Selection
When you select text, the margin will display the number of selected characters.

![Selection](art/selection.png)

You can also make multiple selections like shown below using **CTRL+ALT+Selection**.

![Selected Text](art/selected-text.png)

That will display the total number of characters selected and the number of selection shown in parenthesis. Hovering over the margin will show a tooltip that breaks down the number of characters in each selection.

![Multi Selection](art/multi-selection.png)

## Encoding
The file encoding is shown in the margin as well and hovering it will show a tooltip with more details.

![Encoding](art/encoding-tooltip.png)

Clicking the encoding will open a drop down menu that lets you change the encoding of the file.

![Encoding](art/encoding.png)

The file is saved automatically with the newly selected encoding. 

> Please note that Visual Studio guesses the encoding of the file based on the content and that might not always be correct. When the file contains a BOM (byte order mark) it will always be detected correctly.

## How can I help?
If you enjoy using the extension, please give it a ★★★★★ rating on the [Visual Studio Marketplace][marketplace].

Should you encounter bugs or if you have feature requests, head on over to the [GitHub repo][repo] to open an issue if one doesn't already exist.

Pull requests are also very welcome, since I can't always get around to fixing all bugs myself. This is a personal passion project, so my time is limited.

Another way to help out is to [sponsor me on GitHub](https://github.com/sponsors/madskristensen).