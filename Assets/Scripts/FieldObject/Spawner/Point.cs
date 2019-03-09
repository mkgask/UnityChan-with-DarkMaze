using UnityEngine;
using System.Collections.Generic;

namespace sgffu.FieldObject.Spawner
{

    public class Point {
        public Vector3 position = new Vector3(0f, 0f, 0f);
        public Vector3 direction = new Vector3(0f, 0f, 0f);
    }

    public class List {
        public static Dictionary<string, Point> all = new Dictionary<string, Point> {
            { "front", new Point { position = new Vector3(0.5f,  0f, 5f), direction = new Vector3(0f, 0f, 0f) }},
            { "left",  new Point { position = new Vector3(-3.5f, 0f, 0f), direction = new Vector3(0f, -90f, 0f) }},
        };
    }

}