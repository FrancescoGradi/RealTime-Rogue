from tensorforce import Agent

from unity_runner import UnityRunner
from nets import net, baseline


def train(env, directory, curriculum=None, num_episodes=200):

    agent = Agent.create(agent='ppo',
                         environment=env,
                         batch_size=10,
                         network=net,
                         use_beta_distribution=True,
                         update_frequency=10,
                         learning_rate=1e-5,
                         subsampling_fraction=0.33,
                         optimization_steps=10,
                         likelihood_ratio_clipping=0.2,
                         discount=0.99,
                         estimate_terminal=False,
                         critic_network=baseline,
                         critic_optimizer=dict(
                              optimizer=dict(type='adam', learning_rate=1e-4),
                         ),
                         exploration=0.0,
                         variable_noise=0.0,
                         l2_regularization=0.0,
                         entropy_regularization=0.01,
                         saver=dict(
                             directory=directory,
                             frequency=10000
                         )
    )

    runner = UnityRunner(
        agent=agent,
        environment=env,
        directory=directory,
        curriculum=curriculum
    )

    runner.run(num_episodes=num_episodes)

    runner.agent.save(directory=directory)
    runner.save_history()

    runner.close()
    agent.close()
    env.close()


def evaluate(env, directory, curriculum=None, num_episodes=200):

    agent = Agent.load(directory=directory)

    runner = UnityRunner(
        agent=agent,
        environment=env,
        directory=directory,
        curriculum=curriculum
    )

    runner.run(num_episodes=num_episodes/10, evaluation=True)

    runner.close()
    agent.close()
    env.close()
