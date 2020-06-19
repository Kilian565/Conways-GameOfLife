using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ConwayGameOfLife : MonoBehaviour
{

    public Texture2D texture;
    float tickTime = 0.1f;
    float time;


    int birth = 3;
    int lone = 3;
    int pop = 4;

    bool[,] pixelRead;
    bool[,] pixelWrite;

    private void Start()
    {
        SetRandomColorPerlin();
        pixelRead = new bool[texture.width, texture.height];
        pixelWrite = new bool[texture.width, texture.height];
    }

    private void Update()
    {
        TickTimer();
    }

    public bool SetRandomColorPerlin()
    {
        Debug.Log("Width: " + texture.width + " " + "Height: " + texture.height);

        for (int i = 0; i < texture.width; i++)
        {
            for (int k = 0; k < texture.height; k++)
            {
                int scale = 200;
                float perlin_X = ((float)i / (float)texture.height * (float)scale);
                float perlin_Y = ((float)k / (float)texture.width * (float)scale);
                if (Mathf.PerlinNoise(perlin_X, perlin_Y) >= 0.5f)
                {
                    texture.SetPixel(i, k, Color.black);
                }
                else
                {
                    texture.SetPixel(i, k, Color.white);
                }
            }
        }
        texture.Apply();
        return true;
    }


    private void TickTimer()
    {
        time += Time.deltaTime;
        if (time >= tickTime)
        {
            time = 0;
            GameOfLife();

        }
    }

    private void ReadTex()
    {
        for (int i = 0; i < texture.width; i++)
        {
            for (int k = 0; k < texture.height; k++)
            {
                if (texture.GetPixel(i, k) == Color.black)
                {
                    pixelRead[i, k] = true;
                    pixelWrite[i, k] = true;
                }
                else
                {
                    pixelRead[i, k] = false;
                    pixelWrite[i, k] = false;
                }
            }
        }
    }

    public void WriteTex()
    {
        for (int i = 0; i < texture.width; i++)
        {
            for (int k = 0; k < texture.height; k++)
            {
                if (pixelWrite[i, k] == true)
                {
                    texture.SetPixel(i, k, Color.black);
                }
                else
                {
                    texture.SetPixel(i, k, Color.white);
                }
            }
        }
        texture.Apply();
    }

    private void GameOfLife()
    {
        ReadTex();
        for (int i = 0; i <= pixelRead.GetUpperBound(0); i++)
        {
            for (int k = 0; k <= pixelRead.GetUpperBound(1); k++)
            {
                if (CountBlackNeighbors(pixelRead, i, k) == birth)
                {
                    pixelWrite[i, k] = true;
                }
                else if (CountBlackNeighbors(pixelRead, i, k) < lone)
                {
                    pixelWrite[i, k] = false;
                }
                else if (CountBlackNeighbors(pixelRead, i, k) >= lone && CountBlackNeighbors(pixelRead, i, k) <= pop)
                {

                }
                else if (CountBlackNeighbors(pixelRead, i, k) > pop)
                {
                    pixelWrite[i, k] = false;
                }
            }
        }
        WriteTex();
    }


    public static int CountBlackNeighbors(bool[,] pix, int x, int y)
    {
        int returnValue = 0;
        int aX, bY;

        for (int i = -1; i <= 1; i++)
        {
            for (int k = -1; k <= 1; k++)
            {
                if (i == 0 && k == 0)
                {
                    continue;
                }
                aX = x + i;
                bY = y + k;

                if (aX < 0)
                {
                    aX = pix.GetUpperBound(0);
                }
                if (aX >= pix.GetUpperBound(0))
                {
                    aX = 0;
                }
                if (bY < 0)
                {
                    bY = pix.GetUpperBound(1);
                }
                if (bY >= pix.GetUpperBound(1))
                {
                    bY = 0;
                }


                if (pix[aX, bY])
                {
                    returnValue++;
                }

            }
        }

        return returnValue;
    }

}
