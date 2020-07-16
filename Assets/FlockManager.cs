﻿using UnityEngine;

namespace testFlock
{
    public class FlockManager : MonoBehaviour {

        public GameObject fishPrefab;
        public int numFish = 20;
        public GameObject[] allFish;
        public Vector3 swimLimits = new Vector3(5.0f, 5.0f, 5.0f);

        [Header("Fish Settings")]
        [Range(0.0f, 5.0f)]
        public float minSpeed;
        [Range(0.0f, 5.0f)]
        public float maxSpeed;
        [Range(1.0f, 10.0f)]
        public float neighbourDistance;
        [Range(0.0f, 5.0f)]
        public float rotationSpeed;

        void Start() {

            allFish = new GameObject[numFish];
            for (int i = 0; i < numFish; ++i) {

                Vector3 pos = this.transform.position + new Vector3(Random.Range(-swimLimits.x, swimLimits.x),
                                                                    Random.Range(-swimLimits.x, swimLimits.x),
                                                                    Random.Range(-swimLimits.x, swimLimits.x));
                allFish[i] = (GameObject)Instantiate(fishPrefab, pos, Quaternion.identity);
                allFish[i].GetComponent<Flock>().myManager = this;
            }
        }

        // Update is called once per frame
        void Update() {

        }
    }
}
