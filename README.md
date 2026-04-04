# Ayah 
A Gorilla Tag BepInEx mod that replaces the Message of the Day with a random 
Quranic verse or authentic Hadith every time you load in. 
<img width="1091" height="455" alt="image" src="https://github.com/user-attachments/assets/6e73fda4-0288-492c-8114-1f86642a862d" />


## What it does
Every time you enter the game, the MOTD board displays a randomly selected verse 
from the Quran or an authentic Hadith from Prophet Muhammad SAW. Currently includes 40+ Quranic verses and authentic Hadiths

## Installation
1. Make sure you have [BepInEx](https://github.com/BepInEx/BepInEx) installed
2. Download `Ayah.dll` from the [Releases](https://github.com/itsreallyhex/Ayah/releases) page
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
