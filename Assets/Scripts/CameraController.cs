using Assets;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Threading;
using UnityEngine;

public class CameraController : MonoBehaviour
{
    TilemapController tmapController;
    float mouswheelTimerLength = 0.15f;
    float mouswheelTimer = 0;
    float cameraOffsetTimerLength = 0.05f;
    float cameraOffsetTimer = 0;
    float mouseScrollValueDelta = 0;
    Vector3 cameraOffset = new Vector3(0,0,0);
    private float speed = 100f;

    // Start is called before the frame update
    void Start()
    {
        tmapController = TilemapController.Instance;
    }

    // Update is called once per frame
    void Update()
    {
        float mouseScrollCurrentDelta = Input.GetAxis("Mouse ScrollWheel");
        if (mouseScrollCurrentDelta != 0)
        {
            if (mouswheelTimer<=0)
            {
                StartCoroutine(HandleMouseWheelTimer());
            }
            mouswheelTimer = mouswheelTimerLength;
            mouseScrollValueDelta += mouseScrollCurrentDelta;
        }

        Vector3 cameraCurrentOffset = GetCurrentOffset();
        if (cameraCurrentOffset.x!=0 || cameraCurrentOffset.y != 0)
        {
            transform.position += cameraOffset;
            cameraOffset.Set(0, 0, 0);
            if (cameraOffsetTimer <= 0)
            {
                StartCoroutine(HandleCameraOffsetTimer());
            }
            cameraOffsetTimer = cameraOffsetTimerLength;
            cameraOffset += cameraCurrentOffset;
        }
    }

    private Vector3 GetCurrentOffset()
    {
        if (Input.GetKey(KeyCode.UpArrow) && Input.GetKey(KeyCode.RightArrow))
            return new Vector3(speed * Time.deltaTime, speed * Time.deltaTime, 0);
        else
        if (Input.GetKey(KeyCode.DownArrow) && Input.GetKey(KeyCode.RightArrow))
            return new Vector3(speed * Time.deltaTime, -(speed * Time.deltaTime), 0);
        else
        if (Input.GetKey(KeyCode.UpArrow) && Input.GetKey(KeyCode.LeftArrow))
            return new Vector3(-(speed * Time.deltaTime), speed * Time.deltaTime, 0);
        else
        if (Input.GetKey(KeyCode.DownArrow) && Input.GetKey(KeyCode.LeftArrow))
            return new Vector3(-(speed * Time.deltaTime), -(speed * Time.deltaTime), 0);
        else
            if (Input.GetKey(KeyCode.RightArrow))
            return new Vector3(speed * Time.deltaTime, 0, 0);
        else
        if (Input.GetKey(KeyCode.LeftArrow))
            return new Vector3(-(speed * Time.deltaTime), 0, 0);
        else
        if (Input.GetKey(KeyCode.DownArrow))
            return new Vector3(0, -(speed * Time.deltaTime), 0);
        else
        if (Input.GetKey(KeyCode.UpArrow))
            return new Vector3(0, speed * Time.deltaTime, 0);
        else
        return new Vector3(0, 0, 0);
    }

    private IEnumerator HandleMouseWheelTimer()
    {
        mouswheelTimer = mouswheelTimerLength;
        while (mouswheelTimer>0)
        {
            mouswheelTimer -= Time.deltaTime;
            yield return 0;
        }

        if (mouseScrollValueDelta != 0f)
        {
            float f = (-mouseScrollValueDelta/110);
            tmapController.noiseWrapper.IncreaseFrequencyt(f);
            mouseScrollValueDelta = 0;
            tmapController.UpdateTiles();
        }
    }

    private IEnumerator HandleCameraOffsetTimer()
    {
        cameraOffsetTimer = cameraOffsetTimerLength;
        while (cameraOffsetTimer > 0)
        {
            cameraOffsetTimer -= Time.deltaTime;
            yield return 0;
        }

        if (cameraOffset.magnitude != 0)
        {
            tmapController.UpdateTiles();
        }
    }
}
