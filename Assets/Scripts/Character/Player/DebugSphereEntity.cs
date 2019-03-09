using UnityEngine;



class DebugSphereEntity
{
    public bool enable = false;

    public Ray ray = new Ray();

    public Vector3 position = Vector3.zero;

    public float radius = 1f;

    public float max_distance = 100f;

    public Vector3 target_position = Vector3.zero;

    public float draw_color_alpha = 1f;

    public float remaining_time = 0f;

    public Vector3 velocity = Vector3.zero;
}