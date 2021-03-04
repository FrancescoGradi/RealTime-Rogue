import matplotlib.pyplot as plt
import json
import numpy as np


def visualize_history(directory):

    with open(directory + '/History/history.json') as h:
        history = json.load(h)

    print("Reward totale media per episodio: " + str(np.mean(history['rewards'])))

    fig, ax = plt.subplots()
    ax.plot(range(1, len(history['rewards']) + 1), history['rewards'])

    ax.set(xlabel='Episodio', ylabel='Reward',
           title='Rewards per episodio')
    ax.grid()

    plt.show()


if __name__ == '__main__':

    directory = "Model_Checkpoints"
    model_name = "net_baseline_25UR_5LR_net_128"
    total_directory = directory + "/" + model_name

    visualize_history(directory=total_directory)
