import tensorflow as tf

from tensorforce import Environment
from unity_env_wrapper import UnityEnvWrapper

from stats_visualization import visualize_history
from train import train, evaluate


if __name__ == '__main__':

    curriculum = {
        'thresholds': [5000, 10000],
        'parameters': {
            'range': [10, 10, 10],
            'agent_fixed': [0, 1, 0],
            'target_fixed': [1, 0, 0]
        }
    }

    physical_devices = tf.config.experimental.list_physical_devices('GPU')
    if len(physical_devices) > 0:
        tf.config.experimental.set_memory_growth(physical_devices[0], True)

    directory = "Model_Checkpoints/CurriculumObstacles"
    model_name = "CURRICULUM_FIXEDTAR_FIXEDAG_RANDOM10_3RAY"
    total_directory = directory + "/" + model_name

    num_episodes = 15000
    max_episode_timesteps = 250
    game_name = 'Compilati/12_03'
    # game_name = None

    with tf.device('/device:GPU:0'):
        env = UnityEnvWrapper(game_name=game_name)
        env = Environment.create(environment=env, max_episode_timesteps=max_episode_timesteps)

        train(env=env, directory=total_directory, num_episodes=num_episodes, curriculum=curriculum)
        # evaluate(env=env, directory=total_directory, num_episodes=num_episodes, curriculum=curriculum)

    visualize_history(directory=total_directory, num_mean=100, save_fig=True)
