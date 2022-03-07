using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using YooAsset;

public static class SceneManager
{
	private static AssetOperationHandle _operationHandle;
	private static bool _isLoadScene = false;

	public static void ChangeScene(string location)
	{
		if (_isLoadScene && _operationHandle.IsDone == false)
		{
			Debug.LogWarning("场景正在加载中！");
			return;
		}

		// 释放之前加载的场景
		if (_isLoadScene)
		{
			_isLoadScene = false;
			_operationHandle.Release();
		}

		// 加载新的场景
		SceneInstanceParam param = new SceneInstanceParam();
		param.LoadMode = UnityEngine.SceneManagement.LoadSceneMode.Single;
		param.ActivateOnLoad = true;
		_operationHandle = YooAssets.LoadSceneAsync(location, param);
		_isLoadScene = true;
	}
}