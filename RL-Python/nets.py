net1 = [
    dict(type='retrieve', tensors=['forward_direction', 'env_objects_distances'], aggregation='concat'),
    dict(type='dense', size=256, activation='relu'),
    dict(type='dense', size=64, activation='relu')
]

baseline1 = [
    dict(type='retrieve', tensors=['forward_direction', 'env_objects_distances'], aggregation='concat'),
    dict(type='dense', size=128, activation='relu'),
    dict(type='dense', size=64, activation='relu')
]

net = [
    dict(type='retrieve', tensors=['cell_view']),
    dict(type='dense', size=256, activation='relu'),
    dict(type='dense', size=64, activation='relu')
]

baseline = [
    dict(type='retrieve', tensors=['cell_view']),
    dict(type='dense', size=128, activation='relu'),
    dict(type='dense', size=64, activation='relu')
]
