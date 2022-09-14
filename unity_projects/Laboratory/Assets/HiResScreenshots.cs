using System;
using System.Collections.Generic;
using System.IO;
using MyBox;
using UnityEditor;
using UnityEditor.Experimental.GraphView;
using UnityEngine;
using UnityEngine.Tilemaps;

namespace LoadedLions.Infrastructure
{
    public class HiResScreenshots : MonoBehaviour
    {
        public enum Resolution
        {
            _1080,
            _2k,
            _4k,
            _8k,
            Custom
        }

        private static Vector2Int HD = new Vector2Int(720, 1280);
        private static Vector2Int FullHD = new Vector2Int(1080, 1920);

        private static Dictionary<Resolution, Vector2Int> Resolutions = new Dictionary<Resolution, Vector2Int>()
        {
            {Resolution._1080, FullHD},
            {Resolution._2k, 2 * HD},
            {Resolution._4k, 2 * FullHD},
            {Resolution._8k, 4 * FullHD},
        };

        [SerializeField] private Camera _camera;
        [SerializeField] private Orientation _orientation = Orientation.Horizontal;
        [SerializeField] private Resolution _resolution = Resolution._8k;

        [ConditionalField(nameof(_resolution), false, Resolution.Custom)]
        [SerializeField] private Vector2Int _customResolution = new Vector2Int(1080, 1920) * 2 * 2;

        private void Reset() =>
            _camera = Camera.main;

        [ButtonMethod]
        private void CreateScreenshot()
        {
            var resolution = GetResolution();
            resolution = _orientation == Orientation.Vertical ? resolution : new Vector2Int(resolution.y, resolution.x);
            var rt = new RenderTexture(resolution.x, resolution.y, 24);
            _camera.targetTexture = rt;
            var screenShot = new Texture2D(resolution.x, resolution.y, TextureFormat.RGB24, false);
            _camera.Render();
            RenderTexture.active = rt;
            screenShot.ReadPixels(new Rect(0, 0, resolution.x, resolution.y), 0, 0);
            _camera.targetTexture = null;
            RenderTexture.active = null; // JC: added to avoid errors

            if (Application.isPlaying)
                Destroy(rt);
            else
                DestroyImmediate(rt);

            byte[] bytes = screenShot.EncodeToPNG();
            string screenshotPath = ScreenshotPath(resolution.x, resolution.y);
            if (!Directory.Exists(Path.GetDirectoryName(screenshotPath)))
                Directory.CreateDirectory(Path.GetDirectoryName(screenshotPath) ?? string.Empty);
            File.WriteAllBytes(screenshotPath, bytes);
            Debug.Log($"Took screenshot to: {screenshotPath}");
        }

        private Vector2Int GetResolution() =>
            _resolution switch
            {
                Resolution.Custom => _customResolution,
                _ => Resolutions[_resolution]
            };

        private static string ScreenshotPath(int width, int height) =>
            $"{Application.dataPath}/screenshots/screen_{width}x{height}_" +
            $"{DateTime.Now:yyyy-MM-dd_HH-mm-ss}.png";
    }
}
