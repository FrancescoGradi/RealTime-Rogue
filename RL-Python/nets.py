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
