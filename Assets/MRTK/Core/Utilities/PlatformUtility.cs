// Copyright (c) Microsoft Corporation. All rights reserved.
// Licensed under the MIT License. See LICENSE in the project root for license information.

using UnityEngine;

namespace Microsoft.MixedReality.Toolkit.Utilities
{
    public static class PlatformUtility
    {
        public static bool IsPlatformSupported(this RuntimePlatform runtimePlatform, SupportedPlatforms platforms)
        {
            SupportedPlatforms target = GetSupportedPlatformMask(runtimePlatform);
            return IsPlatformSupported(target, platforms);
        }

        private static SupportedPlatforms GetSupportedPlatformMask(RuntimePlatform runtimePlatform)
        {
            SupportedPlatforms supportedPlatforms = 0;

            switch (runtimePlatform)
            {
                case RuntimePlatform.WindowsPlayer:
                case RuntimePlatform.WindowsEditor:
                    supportedPlatforms |= SupportedPlatforms.WindowsStandalone;
                    break;
                case RuntimePlatform.WSAPlayerARM:
                case RuntimePlatform.WSAPlayerX86:
                case RuntimePlatform.WSAPlayerX64:
                    supportedPlatforms |= SupportedPlatforms.WindowsUniversal;
                    break;
                case RuntimePlatform.OSXPlayer:
                case RuntimePlatform.OSXEditor:
                    supportedPlatforms |= SupportedPlatforms.MacStandalone;
                    break;
                case RuntimePlatform.LinuxPlayer:
                case RuntimePlatform.LinuxEditor:
                    supportedPlatforms |= SupportedPlatforms.LinuxStandalone;
                    break;
                case RuntimePlatform.Android:
                    supportedPlatforms |= SupportedPlatforms.Android;
                    break;
                case RuntimePlatform.IPhonePlayer:
                    supportedPlatforms |= SupportedPlatforms.IOS;
                    break;
                case RuntimePlatform.WebGLPlayer:
                    supportedPlatforms |= SupportedPlatforms.Web;
                    break;
                case RuntimePlatform.Lumin:
                    supportedPlatforms |= SupportedPlatforms.Lumin;
                    break;
            }

            return supportedPlatforms;
        }

        private static bool IsPlatformSupported(SupportedPlatforms target, SupportedPlatforms supported)
        {
            return (target & supported) > 0;
        }

#if UNITY_EDITOR
        public static bool IsPlatformSupported(this UnityEditor.BuildTarget editorBuildTarget, SupportedPlatforms platforms)
        {
            SupportedPlatforms target = GetSupportedPlatformMask(editorBuildTarget);
            return IsPlatformSupported(target, platforms);
        }

        private static SupportedPlatforms GetSupportedPlatformMask(UnityEditor.BuildTarget editorBuildTarget)
        {
            SupportedPlatforms supportedPlatforms = 0;

            // Editor platforms
            switch (Application.platform)
            {
                case RuntimePlatform.WindowsEditor:
                    supportedPlatforms |= SupportedPlatforms.WindowsEditor;
                    break;

                case RuntimePlatform.OSXEditor:
                    supportedPlatforms |= SupportedPlatforms.MacEditor;
                    break;

                case RuntimePlatform.LinuxEditor:
                    supportedPlatforms |= SupportedPlatforms.LinuxEditor;
                    break;
            }

            // Build target platforms
            switch (editorBuildTarget)
            {
                case UnityEditor.BuildTarget.StandaloneWindows:
                case UnityEditor.BuildTarget.StandaloneWindows64:
                    supportedPlatforms |= SupportedPlatforms.WindowsStandalone;
                    break;
                case UnityEditor.BuildTarget.WSAPlayer:
                    supportedPlatforms |= SupportedPlatforms.WindowsUniversal;
                    break;
                case UnityEditor.BuildTarget.StandaloneOSX:
                    supportedPlatforms |= SupportedPlatforms.MacStandalone;
                    break;
#if !UNITY_2019_2_OR_NEWER
                case UnityEditor.BuildTarget.StandaloneLinux:
                case UnityEditor.BuildTarget.StandaloneLinuxUniversal:
#endif
                case UnityEditor.BuildTarget.StandaloneLinux64:
                    supportedPlatforms |= SupportedPlatforms.LinuxStandalone;
                    break;
                case UnityEditor.BuildTarget.Android:
                    supportedPlatforms |= SupportedPlatforms.Android;
                    break;
                case UnityEditor.BuildTarget.iOS:
                    supportedPlatforms |= SupportedPlatforms.IOS;
                    break;
                case UnityEditor.BuildTarget.WebGL:
                    supportedPlatforms |= SupportedPlatforms.Web;
                    break;
                case UnityEditor.BuildTarget.Lumin:
                    supportedPlatforms |= SupportedPlatforms.Lumin;
                    break;
            }

            return supportedPlatforms;
        }
#endif
    }
}