import tensorflow as tf

from tensorforce import Environment
from unity_env_wrapper import UnityEnvWrapper

from stats_visualization import visualize_history
from train import train, evaluate


if __name__ == '__main__':

    curriculum = {
        'thresholds': [4000, 6000, 8000, 10000, 12000],
        'parameters': {
            'range': [5, 6, 7, 8, 9, 10],
            'agent_fixed': [0, 0, 0, 0, 0, 0],
            'target_fixed': [0, 0, 0, 0, 0, 0]
        }
    }

    curriculum = {
        'thresholds': [5000, 8000, 11000],
        'parameters': {
            'range': [5, 6, 7, 8, 9, 10],
            'agent_fixed': [0, 0, 0, 0, 0, 0],
            'target_fixed': [0, 0, 0, 0, 0, 0]
        }
    }

    physical_devices = tf.config.experimental.list_physical_devices('GPU')
    if len(physical_devices) > 0:
        tf.config.experimental.set_memory_growth(physical_devices[0], True)

    directory = "Model_Checkpoints/CurriculumObstacles"
    model_name = "BIGOB_CURRICULUM_RANDOM5_5RAY_10_20angle"
    total_directory = directory + "/" + model_name

    num_episodes = 10000
    max_episode_timesteps = 250
    game_name = 'Compilati/14_03_10_20angle_ray_bigOb'
    # game_name = None

    with tf.device('/device:GPU:0'):
        env = UnityEnvWrapper(game_name=game_name)
        env = Environment.create(environment=env, max_episode_timesteps=max_episode_timesteps)

        train(env=env, directory=total_directory, num_episodes=num_episodes, curriculum=curriculum)
        # evaluate(env=env, directory=total_directory, num_episodes=num_episodes, curriculum=curriculum)

    visualize_history(directory=total_directory, num_mean=100, save_fig=True)
