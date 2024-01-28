using UnityEngine;

public class Hearths : MonoBehaviour
{
    [SerializeField] private GameObject[] hearths;
    
    public int Value
    {
        set
        {
            for (var i = 0; i < hearths.Length; i++)
            {
                hearths[i].SetActive(i < value);
            }
        }
    }
}