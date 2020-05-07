from pathlib import Path
import json
import os

def convert_character(character):
    if character == '-': return ''
    elif character == 'X': return 'b'
    elif character == 'S': return 'b'
    elif character == 'Q': return 'b'
    elif character == 'E': return 'A'
    elif character == '<': return 'b'
    elif character == '>': return 'b'
    elif character == '[': return 'b'
    elif character == ']': return 'b'
    elif character == 'o': return '$'
    elif character == 'B': return 'M'
    elif character == 'b': return 'b'
    elif character == '?': return 'b'
    elif character == 'f': return 'f'
    else:
        print(f'{character} not handled by conversion')
        return character

def main():
    mario_levels_path = os.path.join(Path.home(), 'data', 'vglc', 'Super Mario Bros', 'Processed')
    pcg_platformer_path = os.path.join('..', 'Assets', 'Resources', 'Levels', 'SuperMarioBros')
    local = os.path.join('.', 'regular_vglc')

    for file_name in os.listdir(mario_levels_path):
        f = open(os.path.join(mario_levels_path, file_name))
        content = f.readlines()
        f.close()

        unconverted = []
        converted = []
        for line in content:
            row = list(line.strip())
            row.append('f')
            unconverted.append(row)
            converted.append([convert_character(c) for c in row])

        # where the player starts
        unconverted[-2][3] = 's' 
        converted[-2][3] = 's'

        f = open(os.path.join(pcg_platformer_path, file_name), 'w')
        f.write(json.dumps(converted))
        f.close()

        f = open(os.path.join(local, file_name), 'w')
        f.write(json.dumps(unconverted))
        f.close()

if __name__ == '__main__':
    main()