using System;
using System.Collections;
using System.Threading.Tasks;
using UnityEngine;
using UnityEngine.U2D;
using UnityEngine.UI;
using YooAsset;

public class GameScene : MonoBehaviour
{
	public GameObject CanvasRoot;

	void Awake()
	{
		// 同步加载
		SyncLoad();

		// 协程加载
		StartCoroutine(CoroutineLoad());
	}
	async void Start()
	{
		// 异步编程
		await AsyncLoad();
	}

	/// <summary>
	/// 同步加载方式
	/// </summary>
	void SyncLoad()
	{
		// 加载Unity官方生成的图集
		{
			var btn = CanvasRoot.transform.Find("unity_atlas/btn").GetComponent<Button>();
			var icon = CanvasRoot.transform.Find("unity_atlas/icon").GetComponent<Image>();
			btn.onClick.AddListener(() =>
			{
				AssetOperationHandle handle = YooAssets.LoadAssetSync<SpriteAtlas>("UIAtlas/unityAtlas");
				SpriteAtlas atlas = handle.AssetObject as SpriteAtlas;
				icon.sprite = atlas.GetSprite("Icon_Arrows_128");
			});
		}

		// 加载TexturePacker生成的图集
		{
			var btn = CanvasRoot.transform.Find("tp_atlas/btn").GetComponent<Button>();
			var icon = CanvasRoot.transform.Find("tp_atlas/icon").GetComponent<Image>();
			btn.onClick.AddListener(() =>
			{
				SubAssetsOperationHandle handle = YooAssets.LoadSubAssetsSync<Sprite>("UIAtlas/tpAtlas");
				icon.sprite = handle.GetSubAssetObject<Sprite>("Icon_Arrows_128");
			});
		}

		// 加载预制体
		{
			var btn = CanvasRoot.transform.Find("entity/btn").GetComponent<Button>();
			var icon = CanvasRoot.transform.Find("entity/icon").GetComponent<Image>();
			btn.onClick.AddListener(() =>
			{
				AssetOperationHandle handle = YooAssets.LoadAssetSync<GameObject>("Entity/Level1/footman_Blue");
				GameObject go = handle.InstantiateSync(icon.transform);
				go.transform.localPosition = new Vector3(0, -50, -100);
				go.transform.localRotation = Quaternion.EulerAngles(0, 180, 0);
				go.transform.localScale = Vector3.one * 50;
			});
		}

		// 加载原生文件
		{
			var btn = CanvasRoot.transform.Find("config/btn").GetComponent<Button>();
			
			btn.onClick.AddListener(() =>
			{
				string savePath = $"{YooAssets.GetSandboxRoot()}/config1.txt";
				RawFileOperation operation = YooAssets.LoadRawFileAsync("Config/config1.txt", savePath);
				operation.Completed += Operation_Completed;
			});
		}
	}
	private void Operation_Completed(AsyncOperationBase operation)
	{
		var hint = CanvasRoot.transform.Find("config/icon/hint").GetComponent<Text>();
		RawFileOperation op = operation as RawFileOperation;
		hint.text = op.GetFileText();
	}

	/// <summary>
	/// 协程加载方式
	/// </summary>
	IEnumerator CoroutineLoad()
	{
		// 加载背景音乐
		{
			var audioSource = CanvasRoot.transform.Find("music").GetComponent<AudioSource>();
			AssetOperationHandle handle = YooAssets.LoadAssetAsync<AudioClip>("Music/town.mp3");
			yield return handle;
			audioSource.clip = handle.AssetObject as AudioClip;
			audioSource.Play();
		}
	}

	/// <summary>
	/// 异步编程方式
	/// </summary>
	async Task AsyncLoad()
	{
		// 加载背景图片
		{
			var rawImage = CanvasRoot.transform.Find("texture").GetComponent<RawImage>();
			AssetOperationHandle handle = YooAssets.LoadAssetAsync<Texture>("Texture/bg2.jpeg");
			await handle.Task;
			rawImage.texture = handle.AssetObject as Texture;
		}
	}
}