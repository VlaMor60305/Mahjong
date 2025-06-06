using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AutoSolver : MonoBehaviour
{
    
    public void StartSolving()
    {
        StartCoroutine(Solving());
    }
    IEnumerator Solving()
    {
        GameEvents.AutosolvingStateChange(true);
        List<int> ignoreIndexes = new List<int>();
        while (GameManager.instance.tilesCount > 0)
        {
            for (int i = GameManager.instance.gameConfig.depth - 1; i >= 0; i--)
            {
                for (int j = 0; j < GameManager.instance.gameConfig.width; j++)
                {
                    for (int k = 0; k < GameManager.instance.gameConfig.height; k++)
                    {
                        if (GameManager.instance.slots[j, k, i] != null && GameManager.instance.slots[j, k, i].tile != null)
                        {
                            if (GameManager.instance.slots[j, k, i].tile.isAvaible)
                            {
                                if (GameManager.instance.selectedTile != null)
                                {
                                    if (GameManager.instance.slots[j, k, i].tile != GameManager.instance.selectedTile && GameManager.instance.selectedTile.tileTypeIndex == GameManager.instance.slots[j, k, i].tile.tileTypeIndex)
                                    {
                                        GameManager.instance.SelectTile(GameManager.instance.slots[j, k, i].tile);
                                        i = GameManager.instance.gameConfig.depth - 1;
                                        j = 0;
                                        k = 0;
                                        ignoreIndexes.Clear();
                                        yield return new WaitForSeconds(GameManager.instance.gameConfig.stepDelay);
                                    }
                                }
                                else if(!ignoreIndexes.Contains(GameManager.instance.slots[j, k, i].tile.tileTypeIndex))
                                {
                                    GameManager.instance.SelectTile(GameManager.instance.slots[j, k, i].tile);
                                    yield return new WaitForSeconds(GameManager.instance.gameConfig.stepDelay);
                                }
                            }
                        }
                    }
                }
            }
            if(GameManager.instance.tilesCount > 0 && GameManager.instance.selectedTile != null)
            {
                ignoreIndexes.Add(GameManager.instance.selectedTile.tileTypeIndex);
                GameManager.instance.ResetSelection();
            }
        }
        GameEvents.AutosolvingStateChange(false);
    }
}
