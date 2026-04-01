# Ayah 
A Gorilla Tag BepInEx mod that replaces the Message of the Day with a random 
Quranic verse or authentic Hadith every time you load in.

## What it does
Every time you enter a room, the MOTD board displays a randomly selected verse 
from the Quran or an authentic Hadith from Prophet Muhammad SAW.

## Installation
1. Make sure you have [BepInEx](https://github.com/BepInEx/BepInEx) installed
2. Download `Ayah.dll` from the Releases page
3. Drop it into your `BepInEx/plugins` folder
4. Launch the game

## Want to contribute?
You can add more Quran verses or Hadiths by editing the `Ayahs` array in `Plugin.cs`.

**Rules:**
- Hadiths must be Sahih — sourced from Bukhari, Muslim, or Tirmidhi
- No weak (da'if) or fabricated hadiths
- Keep entries short enough to fit on the board
- Check for duplicates before adding

## License
This project is licensed under **GPL v3** — if you build on top of this, 
keep it open source.

## Author
Made by [itsreallyhex](https://github.com/itsreallyhex)
