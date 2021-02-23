from tensorforce import Agent, Environment, Runner
from tensorforce.agents import PPOAgent
from unity_env_wrapper import UnityEnvWrapper

net = [
    dict(type='dense', size=64, activation='relu'),
    dict(type='dense', size=64, activation='relu'),
]

baseline = [
    dict(type='dense', size=400, activation='relu')
]


def train(env, directory, num_episodes=200, max_episode_timesteps=500):

    steps_per_episode = 50

    '''
    agent = PPOAgent(
        states=dict(position=dict(type=float, shape=(2,))),
        actions=dict(type=float, shape=(2,)),
        # Automatically configured network
        network=net,
        # PPO optimization parameters
        batch_size=10, update_frequency=2, learning_rate=1e-4,
        subsampling_fraction=0.33,
        # Reward estimation
        likelihood_ratio_clipping=0.2, discount=0.99,
        # Regularization
        l2_regularization=0.0, entropy_regularization=0.0,
        max_episode_timesteps=max_episode_timesteps
    )
    '''
    agent = Agent.create(agent='ppo', environment=env, batch_size=10, learning_rate=1e-3)

    runner = Runner(
        agent=agent,
        environment=env,
    )

    runner.run(num_episodes=num_episodes)

    agent.save(directory='{directory}_checkpoints/{env}/'.format(directory=directory, env="Unity"),
               filename='model-{ep}-{env}'.format(ep=num_episodes, env="Unity"))

    runner.close()
    agent.close()
    env.close()


def evaluate(env, directory, num_episodes=200, max_episode_timesteps=500):

    environment = Environment.create(
        environment=env, max_episode_timesteps=max_episode_timesteps, visualize=True
    )

    agent = Agent.load(directory='{directory}_checkpoints/{env}/'.format(directory=directory, env="Unity"),
                       filename='model-{ep}-{env}-1.data-00000-of-00001'.format(ep=num_episodes, env="Unity"),
                       environment=environment
                       )

    runner = Runner(
        agent=agent,
        environment=environment
    )

    runner.run(num_episodes=num_episodes/10, evaluation=True)

    runner.close()
    agent.close()
    environment.close()


if __name__ == '__main__':

    env = UnityEnvWrapper(game_name=None, no_graphics=True, seed=None, worker_id=0, config=None)

    directory = "unity"
    num_episodes = 200
    max_episode_timesteps = 200

    train(env=env, directory=directory, num_episodes=num_episodes, max_episode_timesteps=max_episode_timesteps)
    # evaluate(env=env, directory=directory, num_episodes=num_episodes, max_episode_timesteps=max_episode_timesteps)
