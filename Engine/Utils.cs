using System.IO;
using System.Collections.Generic;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Audio;
using MonoGame.Extended.Tiled;

namespace AdventureGame.Engine
{
    public static class Utils
    {
		public static Dictionary<string, object> LoadedAssets = new Dictionary<string, object>();
        public static string ProjectPath = ProjectSourcePath.Value;
        public static string ContentLocation = "Content/";

        public static Texture2D LoadTexture(string file)
        {
			if (LoadedAssets.TryGetValue(file, out var asset))
			{
				if (asset is Texture2D texture)
					return texture;
			}

			string absolutePath = ProjectPath + ContentLocation + file;
			using (var stream = File.OpenRead(absolutePath))
			{
				var texture = Texture2D.FromStream(Globals.graphicsDevice, stream);
				texture.Name = file;
				LoadedAssets[file] = texture;

				return texture;
			}
		}

		public static SoundEffect LoadSoundEffect(string file)
		{
			if (LoadedAssets.TryGetValue(file, out var asset))
			{
				if (asset is SoundEffect sound)
					return sound;
			}

			string absolutePath = ProjectPath + ContentLocation + file;
			using (var stream = File.OpenRead(absolutePath))
			{
				var sound = SoundEffect.FromStream(stream);
				LoadedAssets[file] = sound;

				return sound;
			}
		}

		/*
		/// <summary>
		/// loads a Tiled map
		/// </summary>
		public TmxMap LoadTiledMap(string name)
		{
			if (LoadedAssets.TryGetValue(name, out var asset))
			{
				if (asset is TmxMap map)
					return map;
			}

			var tiledMap = new TmxMap().LoadTmxMap(name);

			LoadedAssets[name] = tiledMap;
			DisposableAssets.Add(tiledMap);

			return tiledMap;
		}

		/// <summary>
		/// Loads a BitmapFont
		/// </summary>
		public BitmapFont LoadBitmapFont(string name, bool premultiplyAlpha = false)
		{
			if (LoadedAssets.TryGetValue(name, out var asset))
			{
				if (asset is BitmapFont bmFont)
					return bmFont;
			}

			var font = BitmapFontLoader.LoadFontFromFile(name, premultiplyAlpha);

			LoadedAssets.Add(name, font);
			DisposableAssets.Add(font);

			return font;
		}
		*/
	}
}
