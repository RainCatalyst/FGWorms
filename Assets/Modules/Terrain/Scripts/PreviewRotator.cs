using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace FGWorms.Terrain
{
    public class PreviewRotator : MonoBehaviour
    {
        [SerializeField]
        private float _speed = 1;

        private void Update()
        {
            transform.Rotate(Vector3.up, _speed * Time.deltaTime);
        }
    }
}