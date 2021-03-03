from tensorforce import Agent, Environment, Runner
from tensorforce.agents import PPOAgent
from unity_env_wrapper import UnityEnvWrapper

net = [
    dict(type='retrieve', tensors=['position', 'target_position'], aggregation='concat'),
    dict(type='dense', size=256, activation='relu'),
    dict(type='dense', size=128, activation='relu')
]

baseline = [
    dict(type='retrieve', tensors=['position', 'target_position'], aggregation='concat'),
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
                         learning_rate=1e-3,
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
                             frequency=2000  # save checkpoint every 600 seconds (10 minutes)
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

    directory = "NetConBaseline"
    num_episodes = 300
    max_episode_timesteps = 400
    # game_name = 'Compilati/3_03'
    game_name = None

    env = UnityEnvWrapper(game_name=game_name, no_graphics=True, seed=None, worker_id=0, config=None)

    env = Environment.create(environment=env, max_episode_timesteps=max_episode_timesteps)

    # train(env=env, directory=directory, num_episodes=num_episodes)
    evaluate(env=env, directory=directory, num_episodes=num_episodes)
