using System.Collections;
using UnityEngine;

public static class SpawnFormat
{
    public static IEnumerator Line(string tag, int capacity, float spacing, Vector3 pos, float spawnRangeMin, float spawnRangeMax, float durationAfter)
    {
        var randomPos = Random.Range(spawnRangeMin, spawnRangeMax);
        var distance = 0f;
        for (int i = 0; i < capacity; i++)
        {
            Vector2 newPos = new Vector2(randomPos + distance, pos.y);
            ObjectPool.Instance.SpawnFromPool(tag, newPos, Quaternion.identity);
            distance += spacing;
        }

        yield return new WaitForSeconds(durationAfter);
    }

    public static IEnumerator Rectangle(string tag, float spacing, Vector3 pos, float spawnRangeMin, float spawnRangeMax, float durationAfter)
    {
        var randomPos = Random.Range(spawnRangeMin, spawnRangeMax);
        var distanceX = 0f;
        var distanceY = 0f;

        for (int i = 0; i < 2; i++)
        {
            for (int j = 0; j < 3; j++)
            {
                var currentNum = j + 1;
                Vector2 newPos = new Vector2(randomPos, pos.y + distanceY);
                
                if (currentNum % 2 == 0)
                    newPos.x += distanceX;
                else
                {
                    if (currentNum > 2) newPos.x -= distanceX - spacing;
                    else newPos.x -= distanceX;
                }

                distanceX += spacing;

                ObjectPool.Instance.SpawnFromPool(tag, newPos, Quaternion.identity);
            }

            distanceX = 0;
            distanceY += spacing;
        }

        yield return new WaitForSeconds(durationAfter);
    }

    public static IEnumerator Polygon(string tag, int rows, float spacing, Vector3 pos, float spawnRangeMin, float spawnRangeMax, float durationAfter)
    {
        var randomPos = Random.Range(spawnRangeMin, spawnRangeMax);
        var distanceX = 0f;
        var distanceY = 0f;
        var columns = 3; // Start with one column

        for (int i = 0; i < rows; i++)
        {
            for (int j = 0; j < columns; j++)
            {
                var currentNum = j + 1;
                Vector2 newPos = new Vector2(randomPos + distanceX, pos.y + distanceY);
                ObjectPool.Instance.SpawnFromPool(tag, newPos, Quaternion.identity);

                // Calculate the X distance based on the column number and spacing
                distanceX += spacing + spacing;
            }

            // Calculate the Y distance based on the row number and spacing
            distanceY += spacing * 2;
            // Increase the number of columns for the next row
            columns += 3;
            // Reset the X distance for the next row
            distanceX = -spacing * columns / 2f;
        }

        yield return new WaitForSeconds(durationAfter);
    }

    public static IEnumerator Repeated(string tag, int amoungToSpawn, Vector3 pos, float frequency, float durationAfter)
    {
        for (int i = 0; i < amoungToSpawn; i++)
        {
            ObjectPool.Instance.SpawnFromPool(tag, pos, Quaternion.identity);
            yield return new WaitForSeconds(frequency);
        }

        yield return new WaitForSeconds(-durationAfter);
    }
}
