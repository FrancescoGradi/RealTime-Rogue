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
            'target_fixed': [0, 0, 0, 0, 0, 0],
            'agent_update_rate': [25, 25, 25, 25],
            'speed': [2, 2, 2, 2, 2, 2],
            'update_movement': [100, 100, 100, 100, 100, 100]
        }
    }

    curriculum = {
        'thresholds': [5000, 10000, 15000],
        'parameters': {
            'range': [5, 6, 7, 8],
            'agent_fixed': [0, 0, 0, 0],
            'target_fixed': [0, 0, 0, 0],
            'agent_update_rate': [30, 30, 30, 30],
            'speed': [3, 3, 3, 3],
            'update_movement': [100, 100, 100, 100]
        }
    }

    physical_devices = tf.config.experimental.list_physical_devices('GPU')
    if len(physical_devices) > 0:
        tf.config.experimental.set_memory_growth(physical_devices[0], True)

    directory = "Model_Checkpoints/SmoothMovingTarget"
    model_name = "RAPID100_3_CURRICULUM_RANDOM5-7_3RAY_15000EP_25TS_UR30"
    # model_name = "PROVA"
    total_directory = directory + "/" + model_name

    num_episodes = 15000
    max_episode_timesteps = 25
    # game_name = 'Compilati/19_03_nolights_fixed0_02'
    game_name = None

    with tf.device('/device:GPU:0'):
        env = UnityEnvWrapper(game_name=game_name)
        env = Environment.create(environment=env, max_episode_timesteps=max_episode_timesteps)

        # train(env=env, directory=total_directory, num_episodes=num_episodes, curriculum=curriculum)
        evaluate(env=env, directory=total_directory, num_episodes=num_episodes, curriculum=None)

    visualize_history(directory=total_directory, num_mean=100, save_fig=True)
