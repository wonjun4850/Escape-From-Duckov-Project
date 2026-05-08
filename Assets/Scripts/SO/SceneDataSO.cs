using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(menuName = "Data (SO)/SceneDataSO", fileName = "SceneDataSO")]
public class SceneDataSO : ScriptableObject
{
    [System.Serializable]
    public struct SceneInfo
    {
        #region 인스펙터
        [SerializeField] private string _sceneID;
        [SerializeField] private string _sceneName;
        #endregion

        #region 프로퍼티
        public string SceneID => _sceneID;
        public string SceneName => _sceneName;
        #endregion
    }

    #region 인스펙터
    [SerializeField] private List<SceneInfo> _sceneInfos = new List<SceneInfo>();
    #endregion

    #region 프로퍼티
    public IReadOnlyList<SceneInfo> SceneInfos => _sceneInfos;
    #endregion

    #region 외부 호출 함수
    public string GetSceneNameById(string id)
    {
        SceneInfo info = _sceneInfos.Find(x => x.SceneID == id);

        if (!string.IsNullOrEmpty(info.SceneID))
        {
            return info.SceneName;
        }

        Debug.LogError($"id 확인 필요");
        return null;
    }
    #endregion
}