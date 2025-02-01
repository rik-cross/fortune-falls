using System;
using System.IO;
using System.Collections.Generic;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Audio;
using Microsoft.Xna.Framework.Media;
using MonoGame.Extended.Tiled;

namespace Engine
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
				var texture = Texture2D.FromStream(EngineGlobals.graphicsDevice, stream);
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

		public static Song LoadSong(string file)
		{
			if (LoadedAssets.TryGetValue(file, out var asset))
			{
				if (asset is Song song)
					return song;
			}

			string absolutePath = ProjectPath + ContentLocation + file;
            try
            {
				var song = Song.FromUri(file, new Uri(absolutePath));
				LoadedAssets[file] = song;

				return song;
			}
			catch (FileNotFoundException ex)
			{
				Console.WriteLine($"File not found: {ex}");
			}
            catch (FileLoadException ex)
            {
				Console.WriteLine($"Error loading song: {ex}");
			}
			return null;

			/*using (var stream = File.OpenRead(absolutePath))
			{
				var song = Song.FromUri(file, new Uri(absolutePath));
				LoadedAssets[file] = song;

				return song;
			}*/
		}

		/*public static TiledMap LoadTiledMap(string file)
		{
			if (LoadedAssets.TryGetValue(file, out var asset))
			{
				if (asset is TiledMap map)
					return map;
			}

			string absolutePath = ProjectPath + ContentLocation + file;
			using (var stream = TitleContainer.OpenStream(absolutePath))
			{
				//var map = 
			}
		}*/

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
