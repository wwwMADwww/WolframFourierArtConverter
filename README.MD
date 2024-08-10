# Wolfram Alpha Fourier Art Converter

Extracting harmonic coefficients from Wolfram Alpha Fourier series art formulas for further processing.

Implementation is very crude and only works with formulas in a specific format.

Example input files in `Formulas` dir.

## Usage

`convert` command for converting formula to harmonics and saving to JSON file.

`generate` command for generating coordinates using formula from txt file or harmonic coeffs from json file and saving to text file.

Or use `WolframFourierArtParser` project in your code.

### Arguments

`WolframFourierArtConverter convert formulaInputFilename [outputFilename]`

`WolframFourierArtConverter generate formulaInputFilename argumentStep [outputFilename]`
        
### Usage examples:

`WolframFourierArtConverter convert formula.txt`

`WolframFourierArtConverter convert formula.txt harmonics.json`

`WolframFourierArtConverter generate formula.txt 0.0001`

`WolframFourierArtConverter generate harmonics.json 0.0001 coordinates.csv`
