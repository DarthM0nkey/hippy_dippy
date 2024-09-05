using Unity.VisualScripting.ReorderableList.Element_Adder_Menu;
using UnityEditor.UI;
using UnityEngine;

public enum LungElements{
    Air,
    Water
}

public class Lungs
{
    private int capacity = 3;
    private LungElements[] lungChunks;

    public Lungs(int capacity=3, LungElements element = LungElements.Air){
        this.capacity = capacity;
        this.lungChunks = new LungElements[capacity];
        FillLungs(element);
    }

    void FillLungs(LungElements elementType){
        for (int i = 0; i < lungChunks.Length; i++)
        {
            lungChunks[i] = elementType;
        }
    }

    public LungElements TakeBreath(LungElements element){
        //Add new element to 3. move 3 to 2, move 2 to 1, remove 1
        LungElements expelledElement = lungChunks[0];
        for(int i = capacity - 1; i > 0; i--)
        {
            lungChunks[i - 1] = lungChunks[i];
        }
        lungChunks[capacity - 1] = element;
        return expelledElement;
    }

}