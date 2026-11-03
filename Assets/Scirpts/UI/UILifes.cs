using System;
using System.Collections.Generic;
using UnityEngine;

namespace UI
{
    public class UILifes : MonoBehaviour
    {
        private List<GameObject> lifes = new();

        public void Setup()
        {
            LifesManager.LifesChanged += OnLifesChanged;

            foreach(Transform t in transform)
            {
                lifes.Add(t.gameObject);
            }
        }

        private void OnDisable()
        {
            LifesManager.LifesChanged -= OnLifesChanged;
        }

        private void OnLifesChanged(int lifes, int currentLifes)
        {
            for(int i = 0; i < this.lifes.Count; i++)
            {
                this.lifes[i].SetActive(i < currentLifes);
            }
        }
    }
}