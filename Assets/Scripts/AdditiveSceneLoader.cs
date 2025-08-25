using UnityEngine;
using UnityEngine.SceneManagement;
using System.Collections; // 用于协程

public class AdditiveSceneLoader : MonoBehaviour
{
    void Start()
    {
        //TODO: 添加UI场景

        StartCoroutine(LoadScenesInOrder());
    }
    IEnumerator LoadScenesInOrder()
    {
        int i = 0;
        while (true)
        {
            i++;
            string scenePath = SceneUtility.GetScenePathByBuildIndex(i); // 确保场景路径有效
            if (string.IsNullOrEmpty(scenePath))
            {
                Debug.LogError("Scene path is empty or invalid for index " + i + " .");
                break; // 如果路径无效，退出循环
            }
            yield return StartCoroutine(LoadAdditiveSceneAsync(scenePath));
        }
    }

    IEnumerator LoadAdditiveSceneAsync(string scenePath)
    {
        if (SceneUtility.GetBuildIndexByScenePath(scenePath) == -1)
        {
            Debug.LogError("Scene '" + scenePath + "' is not in Build Settings!");
            yield break;
        }

        // 开始异步附加加载
        AsyncOperation asyncLoad = SceneManager.LoadSceneAsync(scenePath, LoadSceneMode.Additive);

        // 等待加载完成
        while (!asyncLoad.isDone)
        {
            Debug.Log("Loading " + scenePath + " progress: " + asyncLoad.progress * 100 + "%");
            yield return null; // 等待下一帧
        }

        Debug.Log("Scene '" + scenePath + "' loaded additively.");

        // 设置场景位置
        // 后来我放弃了
        bool a = false;
        if (a)
        {
            Scene loadedScene = SceneManager.GetSceneAt(SceneUtility.GetBuildIndexByScenePath(scenePath));
            if (!loadedScene.IsValid() || !loadedScene.isLoaded)
            {
                Debug.LogError("Scene Load Error");
                yield break;
            }
            var obj = loadedScene.GetRootGameObjects()[loadedScene.GetRootGameObjects().Length - 1];
            var levelData = obj.GetComponent<LevelData>();
            if (levelData == null)
            {
                Debug.LogError("LevelData Load Error");
                yield break;
            }
            var levelHalfScale = obj.GetComponent<BoxCollider2D>().size / 2;

            var preScene = SceneManager.GetSceneAt(levelData.preLevel);
            if (preScene == null || !preScene.isLoaded || !preScene.IsValid())
            {
                Debug.LogError("preScene Load Error");
                yield break;
            }
            var prelevelRoot = preScene.GetRootGameObjects()[preScene.GetRootGameObjects().Length - 1];
            var prePos = prelevelRoot.transform.position;
            var preHalfScale = prelevelRoot.GetComponent<BoxCollider2D>().size / 2;
            Vector3 newPosition = new Vector3(prePos.x, prePos.y, 0);
            Debug.Log("newPosition" + newPosition);
            Debug.Log(loadedScene);
            switch (levelData.direction)
            {
                case LevelData.Direction.Up:
                    newPosition += new Vector3(0, preHalfScale.y + levelHalfScale.y, 0);
                    break;
                case LevelData.Direction.Down:
                    newPosition += new Vector3(0, -(preHalfScale.y + levelHalfScale.y), 0);
                    break;
                case LevelData.Direction.Left:
                    newPosition += new Vector3(-(preHalfScale.x + levelHalfScale.x), 0, 0);
                    break;
                case LevelData.Direction.Right:
                    newPosition += new Vector3(preHalfScale.x + levelHalfScale.x, 0, 0);
                    break;
            }
            obj.transform.position = newPosition;
            Debug.Log("Set position of " + obj.name + " to " + newPosition);
            Debug.Log(preScene.name);
            obj.SetActive(true); // 激活场景中的对象
        }
    }

    // 卸载一个附加加载的场景
    public void UnloadAdditiveScene(string sceneName)
    {
        // 检查场景是否已加载
        Scene scene = SceneManager.GetSceneByName(sceneName);
        if (scene.isLoaded)
        {
            SceneManager.UnloadSceneAsync(sceneName);
            Debug.Log("Scene '" + sceneName + "' unloaded.");
        }
        else
        {
            Debug.LogWarning("Scene '" + sceneName + "' is not currently loaded.");
        }
    }
}
