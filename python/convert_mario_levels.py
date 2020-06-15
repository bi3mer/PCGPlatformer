from pathlib import Path
import json
import os

def convert_character(character):
    if character == '-': return ''
    elif character == 'X': return 'b'
    elif character == 'x': return 'b'
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
    elif character == '#': return 'b'
    elif character == 'p': return 'b'
    elif character == 'P': return 'b'
    elif character == 'e': return 'A'
    elif character == 'c': return 'M'
    elif character == 'g': return 'A'
    else:
        print(f'{character} not handled by conversion')
        return character

def convert(path, name, start_row, start_col, vertical=False):
    pcg_platformer_path = os.path.join('..', 'Assets', 'Resources', 'Levels', name)

    if not os.path.exists(pcg_platformer_path):
        os.mkdir(pcg_platformer_path)

    for file_name in os.listdir(path):
        f = open(os.path.join(path, file_name))
        content = f.readlines()
        f.close()

        converted = []
        for line in content:
            row = list(line.strip())
            row.append('f')
            converted.append([convert_character(c) for c in row])

        # where the player starts
        converted[start_row][start_col] = 's' 

        f = open(os.path.join(pcg_platformer_path, file_name), 'w')
        f.write(json.dumps(converted))
        f.close()
        
def get_vglc_path(game_name):
    return os.path.join(Path.home(), 'data', 'TheVGLC', game_name, 'Processed')

if __name__ == '__main__':
    convert(get_vglc_path("Super Mario Bros"), "SuperMarioBros", -2, 3)
    convert(get_vglc_path("Super Mario Bros 2 (Japan)"), "SuperMarioBros2Japan", -2, 3)
    convert(get_vglc_path("Super Mario Land"), "SuperMarioLand", -2, 3)
    convert(os.path.join(get_vglc_path("Super Mario Bros 2"), 'WithEnemies'), "SuperMarioBros2", -2, 3)