using UnityEngine;
using static UnityEditor.Experimental.GraphView.GraphView;

[RequireComponent(typeof(MeshRenderer))]
public class ColorByDistance : MonoBehaviour
{
    [SerializeField] private Transform _target;

    [SerializeField] private Color _targetColor = Color.white;
    [SerializeField] private float _maxDistance = 100f;
    private MeshRenderer rend;
    private Material _material;

    void Start()
    {
        _target = FindFirstObjectByType<Player>().transform;
        rend = GetComponent<MeshRenderer>();
        _material = rend.material;
        _material.color = Color.black;
        _material.SetColor("_EmissionColor", Color.black);
    }

    void Update()
    {
        if (_target == null)
            return;

        float dist = Vector3.Distance(transform.position, _target.position);

        float t = Mathf.Clamp01(1f - dist / _maxDistance);

        Color current = Color.Lerp(Color.black, _targetColor, t);
        _material.color = current;
        _material.SetColor("_EmissionColor", current);
    }
}
