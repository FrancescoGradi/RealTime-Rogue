net = [
    dict(type='retrieve', tensors=['position', 'forward_direction', 'env_objects_distances'], aggregation='concat'),
    dict(type='dense', size=256, activation='relu'),
    dict(type='dense', size=64, activation='relu')
]

baseline = [
    dict(type='retrieve', tensors=['position', 'forward_direction', 'env_objects_distances'], aggregation='concat'),
    dict(type='dense', size=128, activation='relu'),
    dict(type='dense', size=64, activation='relu')
]
