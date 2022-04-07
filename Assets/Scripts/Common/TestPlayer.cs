using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace RandomMap
{
    public class TestPlayer : MonoBehaviour
    {

        public float speed = 200f;
        Rigidbody rigid;
        Vector3 move;
        private void Awake()
        {
            rigid = GetComponent<Rigidbody>();
            Cursor.SetCursor(null, Vector2.zero, CursorMode.Auto);
            DontDestroyOnLoad(this.gameObject);
        }

        private void Update()
        {
            this.transform.rotation = Quaternion.Euler(-(Input.mousePosition.y), Input.mousePosition.x, 0);

            move = ((transform.forward * Input.GetAxis("Vertical")) + (transform.right * Input.GetAxis("Horizontal"))) * speed * Time.deltaTime;
            rigid.velocity = move;
        }

    }
}