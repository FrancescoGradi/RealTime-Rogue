﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public static class Utility {

    public static List<T> Shuffle<T>(List<T> list)  {  

        System.Random rng = new System.Random();

        int n = list.Count;  
        while (n > 1) {  
            n--;  
            int k = rng.Next(n + 1);  
            T value = list[k];  
            list[k] = list[n];  
            list[n] = value;  
        }
        return list;
    }

    public static GameObject GetRandomObject(List<GameObject> objects) {

        return objects[(int)(UnityEngine.Random.Range(0, objects.Count))];
    }

    public static int GetRandomInt(int min, int max) {

		System.Random random = new System.Random();
		return random.Next(min, max + 1);
	}

    public static float GetRandomFloat(float min, float max) {

        return Random.Range(min, max);
    }
    
}
