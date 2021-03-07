import tensorflow as tf

from tensorforce import Agent, Environment, Runner
from tensorforce.agents import PPOAgent
from unity_env_wrapper import UnityEnvWrapper

from stats_visualization import visualize_history

net = [
    dict(type='retrieve', tensors=['position', 'target_position', 'env_objects_positions'], aggregation='concat'),
    dict(type='dense', size=256, activation='relu'),
    dict(type='dense', size=64, activation='relu')
]

baseline = [
    dict(type='retrieve', tensors=['position', 'target_position', 'env_objects_positions'], aggregation='concat'),
    dict(type='dense', size=128, activation='relu'),
    dict(type='dense', size=64, activation='relu')
]


def train(env, directory, num_episodes=200):

    agent = Agent.create(agent='ppo',
                         environment=env,
                         batch_size=10,
                         network=net,
                         use_beta_distribution=True,
                         update_frequency=10,
                         learning_rate=5e-4,
                         subsampling_fraction=0.33,
                         optimization_steps=10,
                         likelihood_ratio_clipping=0.2,
                         discount=0.99,
                         estimate_terminal=False,
                         critic_network=baseline,
                         critic_optimizer=dict(
                              optimizer=dict(type='adam', learning_rate=5e-3),
                         ),
                         exploration=0.0,
                         variable_noise=0.0,
                         l2_regularization=0.0,
                         entropy_regularization=0.01,
                         saver=dict(
                             directory=directory,
                             frequency=2000
                         )
                         )

    runner = Runner(
        agent=agent,
        environment=env,
    )

    runner.run(num_episodes=num_episodes)

    runner.agent.save(directory=directory)

    runner.close()
    agent.close()
    env.close()


def evaluate(env, directory, num_episodes=200):

    agent = Agent.load(directory=directory)

    runner = Runner(
        agent=agent,
        environment=env
    )

    runner.run(num_episodes=num_episodes/10, evaluation=True)

    runner.close()
    agent.close()
    env.close()


if __name__ == '__main__':

    use_cuda = True

    physical_devices = tf.config.experimental.list_physical_devices('GPU')
    print(physical_devices)
    # if len(physical_devices) > 0:
    #    tf.config.experimental.set_memory_growth(physical_devices[0], True)

    directory = "Model_Checkpoints"
    model_name = "RANDOM_AGENT10_RANDOM_TARGET10_10000EP_30UR_4LR_net_256_300TS_WITH_OBSTACLES_WITH_RAYCASTS"
    total_directory = directory + "/" + model_name

    num_episodes = 10000
    max_episode_timesteps = 300
    # game_name = 'Compilati/7_03'
    game_name = None

    with tf.device('/device:GPU:0'):
        env = UnityEnvWrapper(game_name=game_name, no_graphics=True, seed=None, worker_id=0, config=None,
                              directory=total_directory, is_training=True)

        env = Environment.create(environment=env, max_episode_timesteps=max_episode_timesteps)

        # train(env=env, directory=total_directory, num_episodes=num_episodes)
        evaluate(env=env, directory=total_directory, num_episodes=num_episodes)

    visualize_history(directory=total_directory)
