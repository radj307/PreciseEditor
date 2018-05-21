using System;
using UnityEngine;
using KSP.UI.Screens;

namespace PreciseEditor
{
    [KSPAddon (KSPAddon.Startup.EditorAny, false)]
	public class AppLauncherButton : MonoBehaviour
	{
		private ApplicationLauncherButton button = null;

        public static AppLauncherButton Instance;

		const string texPathDefault = "PreciseEditor/Textures/AppLauncherIcon";

        private void Awake()
        {
            if (AppLauncherButton.Instance == null)
            {
                GameEvents.onGUIApplicationLauncherReady.Add(this.OnGuiAppLauncherReady);
                Instance = this;
            }
        }

        private void Start ()
		{
			if (button == null)
            {
				OnGuiAppLauncherReady();
			}
		}

		private void OnDestroy()
		{
			GameEvents.onGUIApplicationLauncherReady.Remove(this.OnGuiAppLauncherReady);
			if (this.button != null)
            {
				ApplicationLauncher.Instance.RemoveModApplication(this.button);
			}
		}

        static string[] imgSuffixes = new string[] { ".png", ".jpg", ".gif", ".PNG", ".JPG", ".GIF" };

        public static Boolean LoadImageFromFile(ref Texture2D tex, String fileNamePath)
        {
            Boolean blnReturn = false;

            try
            {
                string path = fileNamePath;

                if (!System.IO.File.Exists(fileNamePath))
                {
                    for (int i = 0; i < imgSuffixes.Length; i++)
                    {
                        if (System.IO.File.Exists(fileNamePath + imgSuffixes[i]))
                        {
                            path = fileNamePath + imgSuffixes[i];
                            break;
                        }
                    }
                }

                if (System.IO.File.Exists(path))
                {
                    try
                    {
                        tex.LoadImage(System.IO.File.ReadAllBytes(path));
                        blnReturn = true;
                    }
                    catch (Exception ex)
                    {
                        Log.Error("Failed to load the texture:" + path);
                        Log.Error(ex.Message);
                    }
                }
                else
                {
                    Log.Error("Cannot find texture to load:" + fileNamePath);
                }
            }
            catch (Exception ex)
            {
                Log.Error("Failed to load (are you missing a file):" + fileNamePath);
                Log.Error(ex.Message);
            }

            return blnReturn;
        }

        Texture2D GetTexture(string path, bool b)
        {
            Texture2D tex = new Texture2D(16, 16, TextureFormat.ARGB32, false);

            LoadImageFromFile(ref tex, KSPUtil.ApplicationRootPath + "GameData/" + path);

            return tex;
        }

        private void OnGuiAppLauncherReady()
		{
			if (this.button == null)
            {
				try
                {
					this.button = ApplicationLauncher.Instance.AddModApplication(
                        () => { PreciseEditor.Instance.vesselWindow.Toggle(); }, //RUIToggleButton.onTrue
                        () => {}, //RUIToggleButton.onFalse
                        () => {}, //RUIToggleButton.OnHover
                        () => {}, //RUIToggleButton.onHoverOut                                  
                        () => {}, //RUIToggleButton.onEnable
                        () => {}, //RUIToggleButton.onDisable
                        ApplicationLauncher.AppScenes.VAB | ApplicationLauncher.AppScenes.SPH,
                        GetTexture(texPathDefault, false)
					);
				}
                catch (Exception ex)
                {
					Log.Error("Error adding ApplicationLauncher button: " + ex.Message);
				}
			}

		}

		private void Update()
		{
			if (this.button == null)
            {
				return;
			}

			try
            {
				if (EditorLogic.fetch != null)
                {
                    if (this.button.enabled)
                    {
                        this.button.SetFalse();
                    }
                    else
                    {
                        this.button.SetTrue();
                    }
				}
                else if (this.button.enabled)
                {
					this.button.Disable();
				}
			}
            catch (Exception ex)
            {
				Log.Error ("Error updating ApplicationLauncher button: " + ex.Message);
			}
		}
	}
}