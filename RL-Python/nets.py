net1 = [
    dict(type='retrieve', tensors=['position', 'forward_direction', 'target_position', 'env_objects_distances',
                                   'in_range', 'actual_potion'], aggregation='concat'),
    dict(type='dense', size=256, activation='relu'),
    dict(type='dense', size=64, activation='relu')
]

baseline1 = [
    dict(type='retrieve', tensors=['position', 'forward_direction', 'target_position', 'env_objects_distances',
                                   'in_range', 'actual_potion'], aggregation='concat'),
    dict(type='dense', size=128, activation='relu'),
    dict(type='dense', size=64, activation='relu')
]

net = [
    dict(type='retrieve', tensors=['position', 'target_position', 'cell_view', 'in_range', 'actual_potion'],
         aggregation='concat'),
    dict(type='dense', size=256, activation='relu'),
    dict(type='dense', size=64, activation='relu')
]

baseline = [
    dict(type='retrieve', tensors=['position', 'target_position', 'cell_view', 'in_range', 'actual_potion'],
         aggregation='concat'),
    dict(type='dense', size=128, activation='relu'),
    dict(type='dense', size=64, activation='relu')
]
