import tensorflow as tf

import os
import argparse
import json

from tensorforce import Environment
from unity_env_wrapper import UnityEnvWrapper, UnityDiscreteEnvWrapper

from stats_visualization import visualize_history
from train import train, evaluate


if __name__ == '__main__':

    physical_devices = tf.config.experimental.list_physical_devices('GPU')
    if len(physical_devices) > 0:
        tf.config.experimental.set_memory_growth(physical_devices[0], True)

    parser = argparse.ArgumentParser()
    parser.add_argument('--config', help="Path to json config file", required=True)

    group = parser.add_mutually_exclusive_group(required=True)
    group.add_argument('--train', action='store_true', help="If training")
    group.add_argument('--eval', action='store_true', help="If evaluation")

    args = parser.parse_args()

    try:
        with open(args.config) as c:
            config = json.load(c)
    except Exception as ex:
        print(ex)

    directory = config['directory']
    model_name = config['model_name']
    total_directory = directory + '/' + model_name

    num_episodes = config['num_episodes']
    max_episode_timesteps = config['max_episode_timesteps']
    game_name = config['game_name']

    worker_id = config['worker_id']

    curriculum = config['curriculum']

    with tf.device('/device:GPU:0'):
        if args.train:
            env = UnityDiscreteEnvWrapper(game_name=game_name, worker_id=worker_id)
            env = Environment.create(environment=env, max_episode_timesteps=max_episode_timesteps)
            train(env=env, directory=total_directory, num_episodes=num_episodes, curriculum=curriculum)
        elif args.eval:
            env = UnityDiscreteEnvWrapper(game_name=None, worker_id=0)
            env = Environment.create(environment=env, max_episode_timesteps=max_episode_timesteps)
            evaluate(env=env, directory=total_directory, num_episodes=num_episodes, curriculum=curriculum)
        else:
            raise Exception('Argument not parsed correctly')

    os.makedirs(os.path.dirname(total_directory + '/History/config.json'), exist_ok=True)
    with open(total_directory + '/History/config.json', 'w') as c:
        json.dump(config, c)

    visualize_history(directory=total_directory, num_mean=100, save_fig=True)
