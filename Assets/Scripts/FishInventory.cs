using UnityEngine;
using System.Collections.Generic;

public class FishInventory : MonoBehaviour
{
    /** Private Variables **/

    private Dictionary<FishDefinition, int> fishInventory = new();

    /** Public Methods **/
    public void AddFish(FishDefinition fish)
    {
        if (fishInventory.ContainsKey(fish))
        {
            fishInventory[fish] += 1;
        }
        else
        {
            fishInventory.Add(fish, 1);
        }
    }

    public int GetQuantity(FishDefinition fish)
    {
        int quantity = 0;
        fishInventory.TryGetValue(fish, out quantity);
        return quantity;
    }
}
