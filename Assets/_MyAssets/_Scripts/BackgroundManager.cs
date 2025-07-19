using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BackgroundManager : MonoBehaviour
{
    [SerializeField] Transform[] backgrounds;
    [SerializeField] Transform[] midgrounds;
    [SerializeField] Transform[] foregrounds;
    [SerializeField] float[] moveSpeeds;
    private float[] sizes;
    private float[] backgroundStarts;
    private float[] midgroundStarts;
    private float[] foregroundStarts;

    // Start is called before the first frame update
    void Start()
    {
        sizes = new float[3];
        sizes[0] = backgrounds[0].GetComponent<SpriteRenderer>().bounds.size.x;
        sizes[1] = midgrounds[0].GetComponent<SpriteRenderer>().bounds.size.x;
        sizes[2] = foregrounds[0].GetComponent<SpriteRenderer>().bounds.size.x;

        backgroundStarts = new float[backgrounds.Length];
        for (int i = 0; i < backgrounds.Length; i++)
            backgroundStarts[i] = backgrounds[i].transform.position.x;

        midgroundStarts = new float[midgrounds.Length];
        for (int i = 0; i < midgrounds.Length; i++)
            midgroundStarts[i] = midgrounds[i].transform.position.x;

        foregroundStarts = new float[foregrounds.Length];
        for (int i = 0; i < foregrounds.Length; i++)
            foregroundStarts[i] = foregrounds[i].transform.position.x;

        // Fixed interval versions that won't use physics engine.
        // Start the Coroutine version.
        // StartCoroutine("MoveBackgroundCoroutine");
        // Start the InvokeRepeating version.
        InvokeRepeating("MoveBackgrounds", 0f, Time.fixedDeltaTime);
    }

    void Update()
    {
        // MoveBackgrounds(); // The non-fixed way.
    }

    void FixedUpdate()
    {
        // MoveBackgrounds(); // The "don't use the physics engine for this" way.
    }

    IEnumerator MoveBackgroundCoroutine()
    {
        while (true) // We've essentially created our own Update.
        {
            MoveBackgrounds();
            yield return new WaitForSeconds(Time.fixedDeltaTime);
        }
    }

    private void MoveBackgrounds()
    {
        // Scroll the backgrounds.
        foreach (var background in backgrounds)
        {
            background.Translate(moveSpeeds[0] * Time.fixedDeltaTime, 0.0f, 0.0f);
        }
        foreach (var midground in midgrounds)
        {
            midground.Translate(moveSpeeds[1] * Time.fixedDeltaTime, 0.0f, 0.0f);
        }
        foreach (var foreground in foregrounds)
        {
            foreground.Translate(moveSpeeds[2] * Time.fixedDeltaTime, 0.0f, 0.0f);
        }
        // Bounce the backgrounds back.
        if (backgrounds[0].transform.position.x <= -sizes[0])
        {
            for (int i = 0; i < backgrounds.Length; i++)
            {
                backgrounds[i].transform.position = new Vector3(backgroundStarts[i], 0f, 0f);
            }
        }
        if (midgrounds[0].transform.position.x <= -sizes[1])
        {
            for (int i = 0; i < midgrounds.Length; i++)
            {
                midgrounds[i].transform.position = new Vector3(midgroundStarts[i], 0f, 0f);
            }
        }
        if (foregrounds[0].transform.position.x <= -sizes[2])
        {
            for (int i = 0; i < foregrounds.Length; i++)
            {
                foregrounds[i].transform.position = new Vector3(foregroundStarts[i], -16f, 0f);
            }
        }
    }
}
