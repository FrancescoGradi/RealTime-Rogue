from tensorforce.execution.runner import Runner
import numpy as np
import time


class UnityRunner(Runner):

    def __init__(self, agent, environment, max_episode_timesteps=None, curriculum=None, num_completed_episodes=0):

        self.curriculum = curriculum
        self.num_completed_episodes = num_completed_episodes
        self.unity_env = environment

        super(UnityRunner, self).__init__(agent, environment=environment,
                                          max_episode_timesteps=None)

        self.unity_env.set_config(self.set_curriculum(self.curriculum, self.num_completed_episodes))

    def set_curriculum(self, curriculum, num_episodes):

        if curriculum is None:
            return {}

        thresholds = curriculum['thresholds']

        curriculum_step = 0

        for (index, value) in enumerate(thresholds):
            if num_episodes >= value:
                curriculum_step = index + 1

        parameters = curriculum['parameters']
        config = {}

        for (par, value) in parameters.items():
            config[par] = value[curriculum_step]

        return config

    def handle_terminal(self, parallel):
        # Update experiment statistics
        self.episode_rewards.append(self.episode_reward[parallel])
        self.episode_timesteps.append(self.episode_timestep[parallel])
        self.episode_seconds.append(time.time() - self.episode_start[parallel])
        self.episode_agent_seconds.append(self.episode_agent_second[parallel])
        if self.is_environment_remote:
            self.episode_env_seconds.append(self.environments[parallel].episode_seconds)

        # Maximum number of episodes or episode callback (after counter increment!)
        self.episodes += 1
        if self.terminate == 0 and ((
            self.episodes % self.callback_episode_frequency == 0 and
            not self.callback(self, parallel)
        ) or self.episodes >= self.num_episodes):
            self.terminate = 1

        # Reset episode statistics
        self.episode_reward[parallel] = 0.0
        self.episode_timestep[parallel] = 0
        self.episode_agent_second[parallel] = 0.0
        self.episode_start[parallel] = time.time()

        # Reset environment
        if self.terminate == 0 and not self.sync_episodes:
            self.terminals[parallel] = -1
            self.environments[parallel].start_reset()

            # Modifiche
            self.num_completed_episodes += 1
            self.unity_env.set_config(self.set_curriculum(self.curriculum, self.num_completed_episodes))
