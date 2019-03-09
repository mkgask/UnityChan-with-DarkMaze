using System.Collections.Generic;
using System;
//using System.Linq;
//using System.Reflection;
using UnityEngine;
using UniRx;
using sgffu.EventMessage;

namespace sgffu.Input {

    public class Service
    {

        private static Dictionary<string, InputEntity> entities;

        private static Dictionary<string, float> old_axis = new Dictionary<string, float> {
            { Mouse.X, 0f },
            { Mouse.Y, 0f },
            { Mouse.Wheel, 0f },
        };

        private static string in_enter_key = "";

        private const float event_trigger_idle_wait_time = 5f;

        private static float idle_wait_time = 0f;

        private static bool idle_wait_toggle = false;

        public static void init()
        {
            //entities = InputConfigFactory.loadFile(InputConfigFactory.createDefault());
            entities = InputConfigFactory.createDefault();
        }

        public static void reload(Dictionary<string, InputEntity> load_entities) {
            entities = load_entities;
        }

        public static void inputCheck()
        {
            bool input_key = false;

            foreach (KeyValuePair<string, InputEntity> pair in entities) {
                switch(pair.Value.type) {
                    case Type.Key:
                        if (pair.Value.key_code.Count < 1) { continue; }

                        foreach (KeyCode key in pair.Value.key_code) {
                            if (UnityEngine.Input.GetKeyDown(key)) {
                                //Debug.Log("UnityEngine.Input.GetKeyDown(key): " + key);

                                if (in_enter_key == pair.Value.name) { continue; }

                                MessageBroker.Default.Publish(new InputEvent {
                                    name = pair.Value.name,
                                    key_code = pair.Value.key_code,
                                    type = pair.Value.type,
                                    key_value = true,
                                });

                                in_enter_key = pair.Value.name;
                                input_key = true;
                            }
                            if (UnityEngine.Input.GetKeyUp(key)) {
                                //Debug.Log("UnityEngine.Input.GetKeyDown(key): " + key);
                                
                                MessageBroker.Default.Publish(new InputEvent {
                                    name = pair.Value.name,
                                    key_code = pair.Value.key_code,
                                    type = pair.Value.type,
                                    key_value = false,
                                });

                                in_enter_key = "";
                                input_key = true;
                            }
                        }
                        
                        break;
                    case Type.Axis:
                        float value = UnityEngine.Input.GetAxis(pair.Value.name);

                        if (old_axis[pair.Value.name] != value) {
                            MessageBroker.Default.Publish(new InputEvent {
                                name = pair.Value.name,
                                key_code = pair.Value.key_code,
                                type = pair.Value.type,
                                axis_value = value,
                            });

                            old_axis[pair.Value.name] = value;
                            input_key = true;
                        }
                        break;
                }
            }

            if (input_key) {
                idle_wait_time = 0f;
                //Debug.Log("idle_wait_time = 0f");
            } else
            if (event_trigger_idle_wait_time < idle_wait_time) {
                if (!idle_wait_toggle) {
                    MessageBroker.Default.Publish(new InputEvent {
                        name = Actions.Idle,
                    });
                }
                idle_wait_toggle = !idle_wait_toggle;
                idle_wait_time = 0f;
                //Debug.Log("idle_wait_time = 0f");
            } else {
                idle_wait_time += Time.deltaTime;
                //Debug.Log("idle_wait_time = " + idle_wait_time);
            }
            
        }

    }

}
