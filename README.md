# cutewittlevirus
Virus-themed game played in Windows file explorer

# How to Play
cutewittlevirus is played entirely through Windows file explorer. When the game starts, a procedurally-generated folder tree is created in the same directory as the game's executable. Through the parameters on the game's start screen, you can control how wide and how deep the folder tree goes. 

To start the game, a number of text files starting with "cutewittlevirus" are randomly placed inside folders in the folder tree. These text files self-replicate every so often, first in the same folder until it reaches a certain number of "virus" files considered to be "infected". Once the folder is infected, all future spawns will happen first in any subfolders, if there are any. If there are no subfolders the viruses will spawn in the next uninfected parent folder.

The object of the game is to delete all the virus files before the root folder becomes "infected". A number of parameters are available to tweak on the game's start screen, including virus spawn rate, infection threshold, folder tree size, and more.

In addition to manually deleting files, the player can also create an "antivirus" by creating a new text file titled "antivirus.txt" in any folder. While an antivirus is present, all viruses will be auto-deleted out of that folder and new viruses cannot spawn into it. You are only allowed a certain number of antiviruses per game, and they "decay" after a set number of spawn cycles.

# Installation
To run the game, download the CuteWittleVirus.exe executable and place it in a folder in which you have full write permissions.
