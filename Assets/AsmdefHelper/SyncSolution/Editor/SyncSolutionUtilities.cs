﻿using System.Collections.Generic;
using System.IO;
using System.Linq;
using UnityEditor;
using UnityEditor.VisualStudioIntegration;
using UnityEngine;

// https://forum.unity.com/threads/solved-unity-not-generating-sln-file-from-assets-open-c-project.538487/
// Thank you Flexford!
namespace AsmdeffHelper.SyncSolution {
    public static class SyncSolutionUtilities {
        static SolutionSynchronizer synchronizer;

        static SyncSolutionUtilities() {
            synchronizer = SyncVS.Synchronizer;
        }

        [MenuItem("Window/Asmdef Helper/Sync C# Solution", priority = 1000000)]
        public static void Sync() {
            Sync(true);
        }

        static void Sync(bool logsEnabled) {
            CleanOldFiles(logsEnabled);
            Call_SyncSolution(logsEnabled);
            Call_SynchronizerSync(logsEnabled);
        }

        static void CleanOldFiles(bool logsEnabled) {
            var assetsDirectoryInfo = new DirectoryInfo(Application.dataPath);
            var projectDirectoryInfo = assetsDirectoryInfo.Parent;

            var files = GetFilesByExtensions(projectDirectoryInfo, "*.sln", "*.csproj");
            foreach (var file in files) {
                if (logsEnabled) {
                    Debug.Log($"Remove old solution file: {file.Name}");
                }
                file.Delete();
            }
        }

        static void Call_SyncSolution(bool logsEnabled) {
            if (logsEnabled) {
                Debug.Log($"Coll method: SyncVS.Sync()");
            }
            SyncVS.SyncSolution();
        }

        static void Call_SynchronizerSync(bool logsEnabled) {
            if (logsEnabled) {
                Debug.Log($"Coll method: SyncVS.Synchronizer.Sync()");
            }
            synchronizer?.Sync();
        }

        static IEnumerable<FileInfo> GetFilesByExtensions(DirectoryInfo dir, params string[] extensions) {
            extensions = extensions ?? new[] { "*" };
            var files = Enumerable.Empty<FileInfo>();
            return extensions.Aggregate(files, (current, ext) => current.Concat(dir.GetFiles(ext)));
        }
    }
}
