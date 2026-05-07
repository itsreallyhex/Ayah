# Ayah 
A Gorilla Tag BepInEx mod that replaces the Message of the Day with a random 
Quranic verse or authentic Hadith every time you load in. 

<img width="1091" height="455" alt="image" src="https://github.com/user-attachments/assets/6e73fda4-0288-492c-8114-1f86642a862d" />


## What it does
Every time you enter the game, the MOTD board shows a randomly selected verse from the Quran or an authentic Hadith from Prophet Muhammad SAW.
Currently includes 32 Quranic verses and 34 authentic Hadiths — and the list grows with contributions.


## Installation
1. Make sure you have [BepInEx](https://github.com/BepInEx/BepInEx) installed
2. Download `Ayah.dll` and `ayahs.json` from the [Releases](../../releases) page
3. Drop **both files** into your `BepInEx/plugins` folder — they must stay in the same folder or the mod won't read the data
4. Launch the game

> When a new version adds more verses or Hadiths, you only need to replace `ayahs.json` — no need to reinstall the DLL.


## Want to contribute?
You can add Quran verses or Hadiths by editing `ayahs.json` directly. The file has two arrays: `"quran"` and `"hadith"`. Add your entry to whichever fits, following this format:

```json
"\"Your text here.\" - Source reference"
```

For example:
```json
"\"Whoever removes a worldly hardship from a believer, Allah will remove from him a hardship on the Day of Resurrection.\" - Prophet Muhammad SAW (Muslim)"
```



**Rules:**
- Hadiths must be Sahih — sourced from Bukhari, Muslim, or Tirmidhi
- No weak (da'if) or fabricated hadiths
- Keep entries short enough to fit on the board
- Check for duplicates before adding

## License
This project is licensed under [GPL v3](https://github.com/itsreallyhex/Ayah/blob/master/LICENSE) — if you build on top of this, 
keep it open source.

## Author
Made by [itsreallyhex](https://github.com/itsreallyhex)
