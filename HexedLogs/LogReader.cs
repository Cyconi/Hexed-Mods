using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using TMPro;
using UnityEngine;
using System.IO;
using System.Threading;
using System.Runtime.InteropServices;
using VRC;
using System.Net;
using System.Text.RegularExpressions;
using UnityEngine.UI;
using BestHTTP.SecureProtocol.Org.BouncyCastle.Utilities;
using UnityEngine.EventSystems;
using UnityEngine.Events;

namespace HexedLogs
{
    public class LogReader
    {
        public TextMeshProUGUI logText; // Assign your TextMeshProUGUI component here

        private long lastLength = 0;
        private string filePath = "C:\\HexedTools\\Log.txt";
        private FileSystemWatcher watcher;
        private static ScrollRect scrollRect;
        internal static string guiPos = "topleft";
        internal static Vector2 canvasSize = new(400, 800);
        // Define the maximum number of lines that can fit in the viewport
        private int maxLines = 80; // Adjust this value as needed
        // Use a Queue to store the lines of text
        private Queue<string> lines = new();
        internal void LogInit()
        {

            GameObject parentObject = GameObject.Find("UnscaledUI/HudContent/HUD_UI 2(Clone)/VR Canvas/Container/Left");
            GameObject logObject = new("HexedGUILog");
            logObject.transform.SetParent(parentObject.transform, false);

            // Create a ScrollRect
            scrollRect = logObject.AddComponent<ScrollRect>();
            scrollRect.horizontal = false; // Disable horizontal scrolling

            // Create a viewport as a child of the ScrollRect
            GameObject viewport = new("Viewport");
            viewport.transform.SetParent(logObject.transform, false);
            RectTransform viewportRect = viewport.AddComponent<RectTransform>();
            viewportRect.sizeDelta = canvasSize; // Set the size of the viewport
            viewport.AddComponent<CanvasRenderer>();
            viewport.AddComponent<Image>(); // Add Image component for Mask to work
            viewport.AddComponent<Mask>().showMaskGraphic = false; // Add Mask component
            scrollRect.viewport = viewportRect; // Assign the viewport to the ScrollRect

            // Create the TextMeshProUGUI as a child of the viewport
            GameObject content = new("Content");
            content.transform.SetParent(viewport.transform, false);
            RectTransform rectTransform = content.AddComponent<RectTransform>();
            rectTransform.sizeDelta = canvasSize; // Set the size of the text area

            // Create an Image as a child of the Content to serve as the box
            GameObject box = new("Box");
            box.transform.SetParent(content.transform, false);
            Image boxImage = box.AddComponent<Image>();
            boxImage.color = new(0, 0, 0, 0.5f); // Set the color of the box to semi-transparent black
            RectTransform boxRect = box.GetComponent<RectTransform>();
            boxRect.sizeDelta = canvasSize; // Set the size of the box
            boxRect.anchorMin = Vector2.zero; // Anchor the box to the bottom-left corner
            boxRect.anchorMax = Vector2.one; // Stretch the box to fill the entire text area

            // Create the TextMeshProUGUI as a child of the Content, after the Box
            logText = content.AddComponent<TMPro.TextMeshProUGUI>();
            logText.text = ""; // Initialize with empty text
            logText.fontSize = 12;
            scrollRect.content = rectTransform; // Assign the text area as the content of the ScrollRect

            Vector2 anchorVector;
            switch (guiPos)
            {
                case "bottomleft":
                    anchorVector = new(0, 0);
                    logObject.transform.localPosition = new(500, 600);
                    break;
                case "bottomright":
                    anchorVector = new(1, 0);
                    logObject.transform.localPosition = new(500, -600);
                    break;
                case "topleft":
                    anchorVector = new(0, 1);
                    logObject.transform.localPosition = new(-500, 600);
                    break;
                case "topright":
                    anchorVector = new(1, 1);
                    logObject.transform.localPosition = new(-500, -600);
                    break;
                default:
                    anchorVector = Vector2.zero;
                    break;
            }
            viewportRect.anchorMin = anchorVector;
            viewportRect.anchorMax = anchorVector;
            viewportRect.pivot = anchorVector;
            rectTransform.anchoredPosition = Vector2.zero;

            for (; !File.Exists(filePath); )
                Thread.Sleep(100);

            watcher = new FileSystemWatcher
            {
                Path = Path.GetDirectoryName(filePath),
                Filter = Path.GetFileName(filePath),
                NotifyFilter = NotifyFilters.LastWrite | NotifyFilters.Size,
                EnableRaisingEvents = true,
            };

            watcher.Changed += (sender, e) =>
            {
                Thread.Sleep(100);
                string newText = ReadNewTextFromFile(filePath);
                if (!string.IsNullOrEmpty(newText))
                {
                    logText.text += newText; // Append the new text to the TextMeshProUGUI component
                    scrollRect.verticalNormalizedPosition = 0; // Scroll to the bottom
                }
            };
        }
        private string ReadNewTextFromFile(string filePath)
        {
            string newText = "";
            try
            {
                long newLength = new FileInfo(filePath).Length;

                if (newLength > lastLength)
                {
                    using FileStream fs = new(filePath, FileMode.Open, FileAccess.Read, FileShare.ReadWrite);
                    fs.Seek(lastLength, SeekOrigin.Begin);
                    byte[] buffer = new byte[newLength - lastLength];
                    fs.Read(buffer, 0, buffer.Length);
                    newText = System.Text.Encoding.Default.GetString(buffer);
                    newText = ConvertToUnityRichText(newText);
                    lastLength = newLength;
                }
            }
            catch (Exception ex) { Console.WriteLine("[Hexed Logs] Error reading file: " + ex.Message); }

            // Split the new text into lines
            string[] newLines = newText.Split(new[] { "\r\n", "\r", "\n" }, StringSplitOptions.None);

            // Add each new line to the queue
            foreach (string line in newLines)
            {
                lines.Enqueue(line);

                // If the queue is too big, dequeue the oldest line
                if (lines.Count > maxLines)
                    lines.Dequeue();
            }

            logText.text = string.Join("\n", lines);

            return newText;
        }
        internal static void OnUpdate()
        {
            if (Input.mouseScrollDelta.y != 0)
                OnScroll(Input.mouseScrollDelta.y);
        }

        private static void OnScroll(float scrollDelta)
        {
            scrollRect.verticalNormalizedPosition += scrollDelta * 0.1f;
        }
        private string ConvertToUnityRichText(string ansiText)
        {
            // Replace ANSI color codes with corresponding Unity rich text color tags
            ansiText = Regex.Replace(ansiText, @"\x1b\[38;2;(\d+);(\d+);(\d+)m", m => {
                int r = int.Parse(m.Groups[1].Value);
                int g = int.Parse(m.Groups[2].Value);
                int b = int.Parse(m.Groups[3].Value);
                return (m.Index > 0 ? "</color>" : "") + $"<color=#{r:X2}{g:X2}{b:X2}>";
            });

            return ansiText;
        }
    }
}
