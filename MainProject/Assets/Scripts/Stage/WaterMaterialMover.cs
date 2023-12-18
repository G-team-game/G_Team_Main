using System.Collections;
using System.Collections.Generic;
using DG.Tweening;
using UnityEngine;

public class WaterMaterialMover : MonoBehaviour
{
    [SerializeField] private float moveSpeed = 500f;
    private Material material;
    // Start is called before the first frame update
    void Start()
    {
        material = GetComponent<MeshRenderer>().material;
        material.DOOffset(Vector2.one, moveSpeed).SetLoops(-1, LoopType.Restart);
    }
}
