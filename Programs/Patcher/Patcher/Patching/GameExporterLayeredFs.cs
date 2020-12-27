//  Copyright (c) 2020 GradienWords
//
//  This program is free software: you can redistribute it and/or modify
//  it under the terms of the GNU General Public License as published by
//  the Free Software Foundation, either version 3 of the License, or
//  (at your option) any later version.
//
//  This program is distributed in the hope that it will be useful,
//  but WITHOUT ANY WARRANTY; without even the implied warranty of
//  MERCHANTABILITY or FITNESS FOR A PARTICULAR PURPOSE.  See the
//  GNU General Public License for more details.
//
//  You should have received a copy of the GNU General Public License
//  along with this program.  If not, see <http://www.gnu.org/licenses/>.
namespace Patcher.Patching
{
    using System;
    using System.IO;
    using System.Threading.Tasks;
    using SceneGate.Lemon.Containers.Converters;
    using Yarhl.FileSystem;
    using Yarhl.IO;

    public class GameExporterLayeredFs
    {
        static readonly string HomePath = System.Environment.GetEnvironmentVariable("HOME");
        static readonly string CitraPathWindows = @$"{HomePath}\AppData\Roaming\Citra\load\mods";
        static readonly string CitraPathUnix = $"{HomePath}/.local/share/citra-emu/load/mods";
        static readonly string CitraPath = (System.Environment.OSVersion.Platform == PlatformID.Win32NT)
            ? CitraPathWindows
            : CitraPathUnix;

        public GameExporterLayeredFs(GameNode game) => Game = game;

        public GameNode Game { get; }

        public event EventHandler<double> ProgressChanged;

        public async Task ExportToDirectoryAsync(string output)
        {
            await Task.Run(() => {
                try {
                    Unpack();

                    string lumaBase = Path.Combine(output, "luma", "titles");
                    Logger.Log($"Luma base: {lumaBase}");
                    ExtractLayeredFs(lumaBase);
                } catch (Exception ex) {
                    Logger.Log(ex.ToString());
                }
            }).ConfigureAwait(false);
            ProgressChanged?.Invoke(this, 1);
        }

        public async Task ExportToCitraAsync()
        {
            await Task.Delay(1000).ConfigureAwait(false);
            ProgressChanged?.Invoke(this, 1);
        }

        private void ExtractLayeredFs(string outputBaseDir)
        {
            string layeredBase = Path.Combine(outputBaseDir, Game.PatchInfo.TitleId);
            Logger.Log($"LayeredFS base: {layeredBase}");

            int numFiles = Game.PatchInfo.LayeredFs.Count;
            for (int i = 0; i < numFiles; i++) {
                string path = Game.PatchInfo.LayeredFs[i];
                var node = Navigator.SearchNode(Game.Root, path);
                if (node == null) {
                    Logger.Log($"ERROR: Missing node: {path}");
                    continue;
                }

                if (node.Stream == null) {
                    Logger.Log($"ERROR: Node has null stream: {path}");
                    continue;
                }

                string layeredPath = GetLayeredFsPath(node.Path);
                string output = Path.Combine(layeredBase, layeredPath);
                Logger.Log($"Writing {path} to {output}");
                node.Stream.WriteTo(output);
                ProgressChanged?.Invoke(this, (double)i / numFiles);
            }
        }

        private void Unpack()
        {
            var programNode = Game.Root.Children["content"].Children["program"];
            if (programNode.Format is BinaryFormat) {
                Logger.Log("Converting binary NCCH");
                programNode.Stream.Position = 0;
                programNode.TransformWith<Binary2Ncch>();

                Logger.Log("Converting romFS");
                programNode.Children["rom"].Stream.Position = 0;
                programNode.Children["rom"].TransformWith<BinaryIvfc2NodeContainer>();

                Logger.Log("Converting exeFS");
                programNode.Children["system"].Stream.Position = 0;
                programNode.Children["system"].TransformWith<BinaryExeFs2NodeContainer>();
            }
        }

        private static string GetLayeredFsPath(string nodePath)
        {
            const string RomFsBase = "/root/content/program/rom/";
            const string ExHeader = "/root/content/program/extended_header";
            const string CodeBin = "/root/content/program/system/.code";

            if (nodePath.StartsWith(RomFsBase, StringComparison.Ordinal)) {
                string relative = nodePath
                    .Replace(RomFsBase, string.Empty)
                    .Replace('/', Path.DirectorySeparatorChar);
                return Path.Combine("romfs", relative);
            } else if (nodePath == ExHeader) {
                return "exheader.bin";
            } else if (nodePath == CodeBin) {
                return "code.bin";
            }

            throw new NotSupportedException($"Unknown file: {nodePath}");
        }
    }
}
