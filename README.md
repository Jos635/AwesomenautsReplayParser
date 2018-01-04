# Awesomenauts Replay Parser
This is an incomplete parser for the Awesomenauts replay format. It can parse both continuous (.c) and blockData (.b) files.

# Compatibility
The parser is only compatible with version 0x1E (30) of the replay format. Newer versions most likely have some additional data stored for new character abilities. The current replay version (as of Jan 4, 2018) is version 0x20 (32). Based on my (limited) observations, no additional data structures were added to the save file, and new 0x20 saves can be loaded without any changes.

# How to use
Add a replay folder to the bin/Debug or bin/Release folder, and edit the folder name in the Program class to the name of the folder you added, then run the program.

# Visualising a replay
The ReplayViewer project contains some very bare-bones code to visualise the replays. All characters are rendered onto the screen as ellipses, and their names are drawn above them.

# Contributing
All contributions are welcome! I'd be especially interested in pull requests in the following two areas:

* Adding support for version 0x1F and 0x20
* Labelling fields and adding classes to represent those fields properly 

# License
This project is licensed under the GPL. Please see the LICENSE file for more info.
