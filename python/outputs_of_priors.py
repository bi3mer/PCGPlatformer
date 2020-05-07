import matplotlib.pyplot as plt
import json
import os

def build_n_gram(n):
    ngram = {}
    
    path = os.path.join('..', 'Assets', 'Resources', 'Levels')
    for file_name in os.listdir(path):
        if 'meta' in file_name:
            continue
        
        # read level file in as json
        f = open(os.path.join(path, file_name))
        content = f.read()
        f.close()

        # convert file to json and into a list of columns
        level_matrix = json.loads(content)
        columns = [[] for i in range(len(level_matrix[0]))]

        for row in level_matrix:
            for i in range(len(row)):
                columns[i].append(row[i])

        # build n-gram
        prior = []
        for col in columns:
            json_col = json.dumps(col)
            if len(prior) == n - 1:
                json_prior = json.dumps(prior)
                
                if json_prior not in ngram:
                    ngram[json_prior] = {}
                    ngram[json_prior][json_col] = 1
                elif json_col not in ngram[json_prior]:
                    ngram[json_prior][json_col] = 1
                else:
                    ngram[json_prior][json_col] += 1
                
                prior.pop(0)

            prior.append(col)

    return ngram

def build_histogram():
    min_n = 2
    max_n = 5

    colors = ['red', 'blue', 'green']
    labels = ['n=2', 'n=3', 'n=4']

    # build n-grams
    n_grams = []
    for n in range(min_n, max_n):
        n_grams.append(build_n_gram(n))

    # build histogram array and plot results. 
    max_count = 0
    outputs = []
    for n_gram in n_grams:
        outputs.append([])
        for prior in n_gram:
            outputs[-1].append(len(n_gram[prior]))
            max_count = max(max_count, len(n_gram[prior]))

    plt.xlabel("Outputs")
    plt.ylabel("Priors")
    plt.title("Priors with the Same Number of Outputs")
    n, bins, patches = plt.hist(outputs, bins=range(1,max_count +1), alpha=0.5, edgecolor='black', linewidth=1.2, color=colors, label=labels)
    plt.xticks(range(1, max_count))
    plt.xlim(1)
    plt.legend()
    plt.show()

    print(f'2: {len(n_grams[0])}')
    print(f'3: {len(n_grams[1])}')
    print(f'4: {len(n_grams[2])}')

    # show the number of priors
    # print(f'num priors: {len(ngram)}')

if __name__ == '__main__':
    build_histogram()