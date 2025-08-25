using UnityEngine;
using UnityEngine.SceneManagement;
using System.Collections; // ����Э��

public class AdditiveSceneLoader : MonoBehaviour
{
    void Start()
    {
        //TODO: ���UI����

        StartCoroutine(LoadScenesInOrder());
    }
    IEnumerator LoadScenesInOrder()
    {
        int i = 0;
        while (true)
        {
            i++;
            string scenePath = SceneUtility.GetScenePathByBuildIndex(i); // ȷ������·����Ч
            if (string.IsNullOrEmpty(scenePath))
            {
                Debug.LogError("Scene path is empty or invalid for index " + i + " .");
                break; // ���·����Ч���˳�ѭ��
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

        // ��ʼ�첽���Ӽ���
        AsyncOperation asyncLoad = SceneManager.LoadSceneAsync(scenePath, LoadSceneMode.Additive);

        // �ȴ��������
        while (!asyncLoad.isDone)
        {
            Debug.Log("Loading " + scenePath + " progress: " + asyncLoad.progress * 100 + "%");
            yield return null; // �ȴ���һ֡
        }

        Debug.Log("Scene '" + scenePath + "' loaded additively.");

        // ���ó���λ��
        // �����ҷ�����
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
            obj.SetActive(true); // ������еĶ���
        }
    }

    // ж��һ�����Ӽ��صĳ���
    public void UnloadAdditiveScene(string sceneName)
    {
        // ��鳡���Ƿ��Ѽ���
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
