from tensorforce.execution.runner import Runner
import numpy as np
import time

curriculum = {
    'current_step': 0,
    'thresholds': [5000, 10000, 150000],
    'parameters':
        {
            'range': [5, 10, 10, 14]
        }
}


class UnityRunner(Runner):

    def __init__(self, agent, environment, max_episode_timesteps=None, curriculum=None, num_completed_episodes=0):

        self.curriculum = curriculum
        self.i = 0
        self.unity_env = environment

        super(UnityRunner, self).__init__(agent, environment=environment,
                                              max_episode_timesteps=max_episode_timesteps)

        self.unity_env.set_config(self.set_curriculum(self.curriculum, num_completed_episodes))

    def set_curriculum(self, curriculum, num_episodes):
        return {}