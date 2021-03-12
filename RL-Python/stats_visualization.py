import matplotlib.pyplot as plt
import numpy as np

import json


def visualize_history(directory, num_mean=100, save_fig=False):

    with open(directory + '/History/history.json') as h:
        history = json.load(h)

    history['rewards'] = np.asarray(history['rewards'])

    print('Reward totale media per episodio: ' + str(np.mean(history['rewards'])))

    waste = np.alen(history['rewards']) % num_mean

    plt.figure('Reward Media')

    if waste != 0:
        num_episodes = np.asarray(range(1, np.size(np.mean(history['rewards'][:-waste].reshape(-1, num_mean), axis=1))
                                        + 1)) * num_mean
        plt.plot(num_episodes, np.mean(history['rewards'][:-waste].reshape(-1, num_mean), axis=1))
    else:
        num_episodes = np.asarray(
            range(1, np.size(np.mean(history['rewards'].reshape(-1, num_mean), axis=1)) + 1)) * num_mean
        plt.plot(num_episodes, np.mean(history['rewards'].reshape(-1, num_mean), axis=1))

    plt.title('Reward Media')
    plt.xlabel('Episodi')
    plt.ylabel('Reward Media ogni ' + str(num_mean) + ' episodi')
    plt.grid()

    if save_fig:
        plt.savefig(directory + '/History/' + 'reward.png', dpi=300)

    plt.show()


if __name__ == '__main__':

    directory = "Model_Checkpoints/CurriculumObstacles"
    model_name = "CURRICULUM_FIXEDTAR_FIXEDAG_RANDOM8_RANDOM10_3RAY"
    total_directory = directory + "/" + model_name

    visualize_history(directory=total_directory, num_mean=100, save_fig=True)
