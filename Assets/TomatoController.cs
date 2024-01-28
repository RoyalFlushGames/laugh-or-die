using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TomatoController : MonoBehaviour
{
    [SerializeField] private List<Tomato> _tomatoes;
    
    public List<Tomato> Tomatoes => _tomatoes;
}
