using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;
public class Animal : MonoBehaviour
{
    [Header("Species Variables")]
    public float _lifeTimeInHours;
    public float _defaultSpeed;
    public float _defaultFoodLossPerHour;
    public int _maxBabiesInLife;
    public int _maxFood;
    public int _foodOnBirth;
    public int _minFoodForHorny;

    [Header("Personal Genetics")]
    public float _sizeFactor;
    public float _speedFactor;
    public float _hungerResistance;

    [Header("Life Variables")]
    public int _generation;
    public float _currentFood;
    public float _foodLostPerHour;
    public int _numberOfDescendants;
    public GameObject _parent;
    void Start()
    {
        Destroy(gameObject,_lifeTimeInHours * 3600);
        InvokeRepeating("Breed", 3600, 3600);
        _numberOfDescendants = 0;
        _currentFood = _foodOnBirth;
        //_foodLostPerHour = _defaultFoodLossPerHour;
        PopulationManager.instance.AddAnimal(gameObject);
        gameObject.name = "wolfGen" + _generation;
    }

    public void Born(Animal parent)
    {
        _sizeFactor = Genetics.genetics.MutateSize(parent._sizeFactor);
        _speedFactor = Genetics.genetics.MutateSpeed(parent._speedFactor);
        _hungerResistance = Genetics.genetics.MutateHungerResistance(parent._hungerResistance,_sizeFactor, _speedFactor);

        _parent = parent.gameObject;

        ApplyChanges();
        //Destroy(parent.gameObject); // Tests ONLY
    }

    private void ApplyChanges()
    {
        gameObject.transform.localScale = Vector3.one * _sizeFactor;
        gameObject.GetComponent<NavMeshAgent>().speed = _defaultSpeed * _speedFactor;
        _foodLostPerHour *= (2 - _hungerResistance);
    }

    private void LoseFood(float forcedLoss = 0)
    {
        _currentFood -= _foodLostPerHour /3600 * Time.deltaTime + forcedLoss;
    }

    private void GainFood(float amount)
    {
        _currentFood += amount;
        if (_currentFood >= _maxFood) _currentFood = _maxFood;
    }
    
    public void Breed()
    {
        if (_numberOfDescendants >= _maxBabiesInLife) return;
        else if (_currentFood <= _minFoodForHorny) return;

        GameObject descendant = Instantiate(gameObject, transform.position - transform.forward, Quaternion.identity);
        var descendantScript = descendant.GetComponent<Animal>();
        descendantScript.Born(this);
        descendantScript._generation = _generation + 1;
        descendant.transform.parent = transform.parent;
        _numberOfDescendants++;
    }

    // Update is called once per frame
    void Update()
    {
        if (_currentFood <= 0) Destroy(gameObject);
        else LoseFood();

        //Test Poop Baby
        //if (Input.GetKeyDown(KeyCode.B))
        //{
        //    Breed();
        //}

    }

    private void OnDestroy()
    {
        PopulationManager.instance.RemoveAnimal(gameObject);
    }
}