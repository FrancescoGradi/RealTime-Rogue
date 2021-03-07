from tensorforce.environments import Environment
from mlagents.envs import UnityEnvironment

import numpy as np

import signal
import time
import json
import os


class UnityEnvWrapper(Environment):

    def __init__(self, game_name=None, no_graphics=True, seed=None, worker_id=0, config=None,
                 directory="Model_Checkpoints", is_training=True):

        super(UnityEnvWrapper, self).__init__()

        self.game_name = game_name
        self.no_graphics = no_graphics
        self.seed = seed
        self.worker_id = worker_id
        self.unity_env = self.open_unity_environment(game_name, no_graphics, seed, worker_id)
        self.default_brain = self.unity_env.brain_names[0]

        self.directory = directory
        self.is_training = is_training

        self.history = dict(rewards=[])
        self.cumulative_rewards = 0

        self.set_config(config)

    def states(self):
        return dict(position=dict(type='float', shape=(2,)), target_position=dict(type='float', shape=(2,)),
                    env_objects_positions=dict(type='float', shape=(3,)))

    def actions(self):
        return dict(type='float', shape=(2,), min_value=-1.0, max_value=1.0)

    def reset(self):

        env_info = None

        while env_info == None:

            signal.signal(signal.SIGALRM, self.handler)
            signal.alarm(60)

            try:
                env_info = self.unity_env.reset(train_mode=True, config=self.config)[self.default_brain]

            except Exception as exc:
                self.close()
                self.unity_env = self.open_unity_environment(self.game_name, self.no_graphics, seed=int(time.time()),
                                                             worker_id=self.worker_id)
                env_info = self.unity_env.reset(train_mode=True, config=self.config)[self.default_brain]
                print("The environment didn't respond, it was necessary to close and reopen it")

        obs = self.get_input_observation(env_info)

        self.history['rewards'].append(self.cumulative_rewards)
        self.cumulative_rewards = 0

        return obs

    def execute(self, actions):

        env_info = None

        signal.alarm(0)

        while env_info == None:

            signal.signal(signal.SIGALRM, self.handler)
            signal.alarm(3000)

            try:
                env_info = self.unity_env.step([actions])[self.default_brain]

            except Exception as exc:
                self.close()
                self.unity_env = self.open_unity_environment(self.game_name, self.no_graphics, seed=int(time.time()),
                                                             worker_id=self.worker_id)
                env_info = self.unity_env.reset(train_mode=True, config=self.config)[self.default_brain]
                print("The environment didn't respond, it was necessary to close and reopen it")

        reward = env_info.rewards[0]
        done = env_info.local_done[0]

        observation = self.get_input_observation(env_info)

        self.cumulative_rewards += reward

        return [observation, done, reward]

    def open_unity_environment(self, game_name, no_graphics, seed, worker_id):
        return UnityEnvironment(game_name, no_graphics=no_graphics, seed=seed, worker_id=worker_id)

    def close(self):

        if self.is_training:
            os.makedirs(os.path.dirname(self.directory + '/History/history.json'), exist_ok=True)
            with open(self.directory + '/History/history.json', 'w') as history:
                json.dump(self.history, history)

        self.unity_env.close()

    def set_config(self, config):
        self.config = config

    def handler(self, signum, frame):
        print("Timeout!")
        raise Exception("end of time")

    def get_input_observation(self, env_info):

        observation = {
            'position': np.asarray(env_info.vector_observations[0][:2]),
            'target_position': np.asarray(env_info.vector_observations[0][2:4]),
            'env_objects_positions': np.asarray(env_info.vector_observations[0][4:7])
        }

        return observation