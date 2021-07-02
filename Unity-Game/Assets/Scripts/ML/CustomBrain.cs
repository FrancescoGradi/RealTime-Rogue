using System;
using System.Collections;
using System.Collections.Generic;
using TensorFlow;
using UnityEngine;

// This class works only for Beta distribution for now
public class CustomBrain : MonoBehaviour
{

    public TextAsset modelName;

    public float[] actionMinValues;
    public float[] actionMaxValues;
    
    public int actionSize;
    
    private TFSession session;
    private TFGraph graph;
    
    // List of inputs
    private TFTensor target_transformer_input;
    private TFTensor items_transformer_input;
    private TFTensor cell_view_input;
    private TFTensor position_input;
    private TFTensor forward_direction_input;
    private TFTensor in_range_input;
    private TFTensor actual_health_potion_input;
    private TFTensor actual_bonus_potion_input;
    private TFTensor active_bonus_potion_input;
    private TFTensor agent_actual_HP_input;
    private TFTensor target_actual_HP_input;
    private TFTensor agent_actual_damage_input;
    private TFTensor target_actual_damage_input;
    private TFTensor agent_actual_def_input;
    private TFTensor target_actual_def_input;

    private long target_transformer_size;
    private long items_transformer_size;
    private long num_items;

    private long width_cell;
    private long height_cell;

    private long position_size;
    private long forward_direction_size;
    private long in_range_size;
    
    private long actual_health_potion_size;
    private long actual_bonus_potion_size;
    private long active_bonus_potion_size;
    private long agent_actual_HP_size;
    private long target_actual_HP_size;
    private long agent_actual_damage_size;
    private long target_actual_damage_size;
    private long agent_actual_def_size;
    private long target_actual_def_size;

    // Start is called before the first frame update
    void Start() {

        graph = new TFGraph();

        graph.Import(modelName.bytes);

        session = new TFSession(graph);

        // Empty input and output tensors
        
        // Transformer inputs
        target_transformer_size = 8;
        var target_transformer_shape = new long[] { 1, 1, target_transformer_size };
        target_transformer_input = new TFTensor(TFDataType.Float, target_transformer_shape, (int)target_transformer_size * sizeof(float));
        
        items_transformer_size = 8;
        num_items = 9;
        var items_transformer_shape = new long[] { 1, num_items, items_transformer_size };
        items_transformer_input = new TFTensor(TFDataType.Float, items_transformer_shape, (int)items_transformer_size * (int)num_items * sizeof(float));
        
        // Cell view input
        width_cell = 7;
        height_cell = 7;
        var cell_shape = new long[] { 1, width_cell, height_cell };
        cell_view_input = new TFTensor(TFDataType.Int32, cell_shape, (int)width_cell * (int)height_cell * sizeof(int));
        
        // Agent Position inputs
        position_size = 2;
        var position_shape = new long[] { 1, position_size };
        position_input = new TFTensor(TFDataType.Float, position_shape, (int)position_size*  sizeof(float));
        
        forward_direction_size = 1;
        var forward_direction_shape = new long[] { 1, forward_direction_size };
        forward_direction_input = new TFTensor(TFDataType.Float, forward_direction_shape, (int)forward_direction_size*  sizeof(float));
        
        in_range_size = 1;
        var in_range_shape = new long[] { 1, in_range_size };
        in_range_input = new TFTensor(TFDataType.Int32, in_range_shape, (int)in_range_size*  sizeof(int));
        
        // Potions inputs
        actual_health_potion_size = 1;
        var actual_health_potion_shape = new long[] { 1, actual_health_potion_size };
        actual_health_potion_input = new TFTensor(TFDataType.Int32, actual_health_potion_shape, (int)actual_health_potion_size*  sizeof(int));
        
        actual_bonus_potion_size = 1;
        var actual_bonus_potion_shape = new long[] { 1, actual_bonus_potion_size };
        actual_bonus_potion_input = new TFTensor(TFDataType.Int32, actual_bonus_potion_shape, (int)actual_bonus_potion_size*  sizeof(int));
        
        active_bonus_potion_size = 1;
        var active_bonus_potion_shape = new long[] { 1, active_bonus_potion_size };
        active_bonus_potion_input = new TFTensor(TFDataType.Int32, active_bonus_potion_shape, (int)active_bonus_potion_size*  sizeof(int));
        
        // Stats
        agent_actual_HP_size = 1;
        var agent_actual_HP_shape = new long[] { 1, agent_actual_HP_size };
        agent_actual_HP_input = new TFTensor(TFDataType.Int32, agent_actual_HP_shape, (int)agent_actual_HP_size*  sizeof(int));

        target_actual_HP_size = 1;
        var target_actual_HP_shape = new long[] { 1, target_actual_HP_size };
        target_actual_HP_input = new TFTensor(TFDataType.Int32, target_actual_HP_shape, (int)target_actual_HP_size*  sizeof(int));
        
        agent_actual_damage_size = 1;
        var agent_actual_damage_shape = new long[] { 1, agent_actual_damage_size };
        agent_actual_damage_input = new TFTensor(TFDataType.Int32, agent_actual_damage_shape, (int)agent_actual_damage_size*  sizeof(int));
        
        target_actual_damage_size = 1;
        var target_actual_damage_shape = new long[] { 1, target_actual_damage_size };
        target_actual_damage_input = new TFTensor(TFDataType.Int32, target_actual_damage_shape, (int)target_actual_damage_size*  sizeof(int));
        
        agent_actual_def_size = 1;
        var agent_actual_def_shape = new long[] { 1, agent_actual_def_size };
        agent_actual_def_input = new TFTensor(TFDataType.Int32, agent_actual_def_shape, (int)agent_actual_def_size*  sizeof(int));
        
        target_actual_def_size = 1;
        var target_actual_def_shape = new long[] { 1, target_actual_def_size };
        target_actual_def_input = new TFTensor(TFDataType.Int32, target_actual_def_shape, (int)target_actual_def_size*  sizeof(int));
        
    }

    public float[] getDecision(List<float> obs)
    {
        TFTensor[] networkOutput;

        // Setup the runner
        var runner = session.GetRunner();
        
        // Reshape the obs
        float[,,] target_transformer = new float[1,1,target_transformer_size];
        float[,,] items_transformer = new float[1,num_items,items_transformer_size];
        
        int[,,] cell_view = new int[1, width_cell, height_cell];
        float[,] position = new float[1,position_size];
        float[,] forward_direction = new float[1, forward_direction_size];
        int[,] in_range = new int[1, in_range_size];
        
        int[,] actual_health_potion = new int[1, actual_health_potion_size];
        int[,] actual_bonus_potion = new int[1, actual_bonus_potion_size];
        int[,] active_bonus_potion = new int[1, active_bonus_potion_size];
        
        int[,] agent_actual_HP = new int[1, agent_actual_HP_size];
        int[,] target_actual_HP = new int[1, target_actual_HP_size];
        int[,] agent_actual_damage = new int[1, agent_actual_damage_size];
        int[,] target_actual_damage = new int[1, target_actual_damage_size];
        int[,] agent_actual_def = new int[1, agent_actual_def_size];
        int[,] target_actual_def = new int[1, target_actual_def_size];

        int current_size = 0;
        // Transformer input
        for (int i = 0; i < items_transformer_size; i++)
        {
            target_transformer[0, 0, i] = obs[i];
        }
        runner.AddInput(graph["ppo/target_transformer_input"][0], target_transformer);
        
        for (int i = 0; i < num_items; i++)
        {
            for (int j = 0; j < items_transformer_size; j++)
            {
                items_transformer[0, i, j] = obs[(int)target_transformer_size + (i * (int)items_transformer_size) + j];
            }
        }
        current_size += (int) target_transformer_size + (int)items_transformer_size * (int)num_items;
        runner.AddInput(graph["ppo/items_transformer_input"][0], items_transformer);
        
        // Cell input
        for (int i = 0; i < width_cell; i++)
        {
            for (int j = 0; j < height_cell; j++)
            {
                cell_view[0, i, j] = (int)obs[current_size + (i * (int) height_cell) + j];
            }
        }
        current_size += (int) width_cell * (int) height_cell;
        runner.AddInput(graph["ppo/cell_view"][0], cell_view);
        
        // Position input
        for (int i = 0; i < position_size; i++)
        {
            position[0, i] = obs[current_size + i];
        }
        current_size += (int)position_size;
        runner.AddInput(graph["ppo/position"][0], position);
        
        for (int i = 0; i < forward_direction_size; i++)
        {
            forward_direction[0, i] = obs[current_size + i];
        }
        current_size += (int)forward_direction_size;
        runner.AddInput(graph["ppo/forward_direction"][0], forward_direction);
        
        for (int i = 0; i < in_range_size; i++)
        {
            in_range[0, i] = (int)obs[current_size + i];
        }
        current_size += (int)in_range_size;
        runner.AddInput(graph["ppo/in_range"][0], in_range);
        
        // Potions
        for (int i = 0; i < actual_health_potion_size; i++)
        {
            actual_health_potion[0, i] = (int)obs[current_size + i];
        }
        current_size += (int)actual_health_potion_size;
        runner.AddInput(graph["ppo/actual_health_potion"][0], actual_health_potion);
        
        for (int i = 0; i < actual_bonus_potion_size; i++)
        {
            actual_bonus_potion[0, i] = (int)obs[current_size + i];
        }
        current_size += (int)actual_bonus_potion_size;
        runner.AddInput(graph["ppo/actual_bonus_potion"][0], actual_bonus_potion);
        
        for (int i = 0; i < active_bonus_potion_size; i++)
        {
            active_bonus_potion[0, i] = (int)obs[current_size + i];
        }
        current_size += (int)active_bonus_potion_size;
        runner.AddInput(graph["ppo/active_bonus_potion"][0], active_bonus_potion);
        
        // Stats
        for (int i = 0; i < agent_actual_HP_size; i++)
        {
            if ((int) obs[current_size + i] >= 20)
            {
                obs[current_size + i] = 19;
            }
            agent_actual_HP[0, i] = (int)obs[current_size + i];
        }
        current_size += (int)agent_actual_HP_size;
        runner.AddInput(graph["ppo/agent_actual_HP"][0], agent_actual_HP);
        
        for (int i = 0; i < target_actual_HP_size; i++)
        {
            if ((int) obs[current_size + i] >= 20)
            {
                obs[current_size + i] = 19;
            }
            target_actual_HP[0, i] = (int)obs[current_size + i];
        }
        current_size += (int)target_actual_HP_size;
        runner.AddInput(graph["ppo/target_actual_HP"][0], target_actual_HP);
 
        for (int i = 0; i < agent_actual_damage_size; i++)
        {
            agent_actual_damage[0, i] = (int)obs[current_size + i];
        }
        current_size += (int)agent_actual_damage_size;
        runner.AddInput(graph["ppo/agent_actual_damage"][0], agent_actual_damage);
        
        for (int i = 0; i < target_actual_damage_size; i++)
        {
            target_actual_damage[0, i] = (int)obs[current_size + i];
        }
        current_size += (int)target_actual_damage_size;
        runner.AddInput(graph["ppo/target_actual_damage"][0], target_actual_damage);
        
        for (int i = 0; i < agent_actual_def_size; i++)
        {
            agent_actual_def[0, i] = (int)obs[current_size + i];
        }
        current_size += (int)agent_actual_def_size;
        runner.AddInput(graph["ppo/agent_actual_def"][0], agent_actual_def);
        
        for (int i = 0; i < target_actual_def_size; i++)
        {
            target_actual_def[0, i] = (int)obs[current_size + i];
        }
        current_size += (int)target_actual_def_size;
        runner.AddInput(graph["ppo/target_actual_def"][0], target_actual_def);

        // Fetch the alpha and beta for action.
        runner.Fetch(graph["ppo/actor/add"][0]);
        runner.Fetch(graph["ppo/actor/add_1"][0]);

        // Run the model
        networkOutput = runner.Run();

        // Fetch the result from output into `result`
        float[,] alpha = networkOutput[0].GetValue() as float[,];
        float[,] beta = networkOutput[1].GetValue() as float[,];
        
        // Get the action based on alpha and beta and normalize between max and min values
        float[] normalized_action = new float[actionSize];
        for (int i = 0; i < actionSize; i++)
        {
            float action = alpha[0, i] / (alpha[0, i] + beta[0, i]);
            action = actionMinValues[i] + (
                         actionMaxValues[i] - actionMinValues[i]) * action;
            normalized_action[i] = action;
        }
        
        return normalized_action;
    }
}
