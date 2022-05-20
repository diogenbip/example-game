using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class manager : MonoBehaviour
{
    public wallGroup nearestWall
    {
        get { return walls.Count > 0 ? walls[0] : null; }
    }

    public Vector3 wallSpawnPosition;
    private float lastSpawn;
    private float lastDelete;
    public List<wallGroup> wallsPrefab;

    private float maxFitness;
    private int generation;
    private int deadCount;

    public int mutationChance;
    public float mutationVal;
    public int populationSize;

    public GameObject playerPrefab;

    private int[] layers;

    private List<wallGroup> walls = new List<wallGroup>();
    private List<player> lastPopulation = new List<player>();
    private List<player> players = new List<player>();
    // Start is called before the first frame update

    void Start()
    {
        wallGroup wall_ = Instantiate(wallsPrefab[UnityEngine.Random.Range(0, wallsPrefab.Count)],
       wallSpawnPosition, Quaternion.identity);
        walls.Add(wall_);
        lastSpawn = Time.time;
        layers = new int[] { 4, 8, 8, 1 };
        InstantiatePopulation();
    }

    private void Update()
    {
        GenerateWall();
    }

    private void StartGeneration()
    {
        wallGroup wall_ = Instantiate(wallsPrefab[UnityEngine.Random.Range(0, wallsPrefab.Count)],wallSpawnPosition, Quaternion.identity);
        walls.Add(wall_);
        lastSpawn = Time.time;

        generation++;
        deadCount = 0;
        lastPopulation = players;
        players = new List<player>();
        InstantiatePopulation();
    }

    private void EndGeneration()
    {
        for (int i = 0; i < players.Count; i++)
            Destroy(players[i].gameObject);
        for (int i = 0; i < walls.Count; i++)
            Destroy(walls[i].gameObject);
        walls.Clear();
        StartGeneration();
    }

    private void InstantiateOne()
    {
        player player = Instantiate(playerPrefab, new Vector2(0, 0),Quaternion.identity).GetComponent<player>();
        players.Add(player);
    }

    private int SortByFitness(player a, player b)
    {
        return -(a.fitness.CompareTo(b.fitness));
    }
    private void FixedUpdate()
    {
        players.Sort(SortByFitness);
        if (players.Count > 0)
        {
            maxFitness = Math.Max(players[0].fitness, maxFitness);
        }
    }

    private void InstantiatePopulation()
    {
        players = new List<player>();

        for (int i=0; i< populationSize; i++)
        {
            InstantiateOne();
            if(generation == 0)
            {
                players[i].SetBrain(new NN(layers));
            }
            else
            {
                Mutation(i);
            }
        }
    }

    private void Mutation(int i)
    {
        int top = 3;

        if (i < top)
        {
            NN backBrain = new NN(layers).copy(lastPopulation[i].nN);
            players[i].SetBrain(backBrain);
            Debug.Log("A");
        }
        else if(i<populationSize*0.25f)
        {
            NN backBrain = new NN(layers).copy(lastPopulation[i].nN);
            backBrain.Mutate(mutationChance,mutationVal);
            players[i].SetBrain(backBrain);
            Debug.Log("B");
        }
        else if (i < populationSize * 0.5f)
        {
            NN backBrain = new NN(layers).copy(lastPopulation[i%top].nN);
            backBrain.Mutate(mutationChance, mutationVal);
            players[i].SetBrain(backBrain);
            Debug.Log("C");
        }
        else if (i < populationSize * 0.75f)
        {
            NN backBrain = new NN(layers).copy(lastPopulation[i % top].nN);
            backBrain.Mutate(mutationChance/2, mutationVal);
            players[i].SetBrain(backBrain);
            Debug.Log("D");
        }
        else if (i < populationSize *1f)
        {
            NN backBrain = new NN(layers).copy(lastPopulation[i % top].nN);
            backBrain.Mutate(mutationChance*2, mutationVal);
            players[i].SetBrain(backBrain);
            Debug.Log("E");
        }
    }
    
    public void PlayerLose()
    {
        deadCount++;
        if(deadCount == populationSize)
        {
            EndGeneration();
        }
    }
    private void GenerateWall()
    {
       if(Time.time - lastSpawn >1.5f)
        {
            wallGroup wall_ = Instantiate(wallsPrefab[UnityEngine.Random.Range(0,wallsPrefab.Count)],
               wallSpawnPosition, Quaternion.identity);
            walls.Add(wall_);
            lastSpawn = Time.time;
            
        }
    }
    
    public void RemoveUpWall()
    {
        Destroy(walls[0].gameObject);
        walls.RemoveAt(0);
            
    }

}
