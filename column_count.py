import json
import os

unique_columns = set()
column_count = 0

path = os.path.join('Assets', 'Resources', 'Levels')
for file_name in os.listdir(path):
    if 'meta' in file_name:
        continue

    f = open(os.path.join(path, file_name))
    content = f.read()
    f.close()

    level_matrix = json.loads(content)
    count = len(level_matrix[0])
    column_count += count

    columns = [[] for i in range(count)]

    for row in level_matrix:
        for i in range(len(row)):
            columns[i].append(row[i])

    for c in columns:
        unique_columns.add(','.join(c))

print(f'Unique Columns: {len(unique_columns)}')
print(f'Columns: {column_count}')