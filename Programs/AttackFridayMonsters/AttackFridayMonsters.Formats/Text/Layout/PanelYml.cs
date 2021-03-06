// Copyright (c) 2019 SceneGate Team
//
// This program is free software: you can redistribute it and/or modify
// it under the terms of the GNU General Public License as published by
// the Free Software Foundation, either version 3 of the License, or
// (at your option) any later version.
//
// This program is distributed in the hope that it will be useful,
// but WITHOUT ANY WARRANTY; without even the implied warranty of
// MERCHANTABILITY or FITNESS FOR A PARTICULAR PURPOSE.  See the
// GNU General Public License for more details.
//
// You should have received a copy of the GNU General Public License
// along with this program.  If not, see <http://www.gnu.org/licenses/>.
namespace AttackFridayMonsters.Formats.Text.Layout
{
    using YamlDotNet.Serialization;

    internal sealed class PanelYml
    {
        [YamlMember(Order = 0)]
        public string Name { get; set; }

        [YamlMember(Order = 1)]
        public string Type { get; set; }

        [YamlMember(Order = 2)]
        public Vector3 Position { get; set; }

        [YamlMember(Order = 4)]
        public Vector2 Scale { get; set; }

        [YamlMember(Order = 3)]
        public Size Size { get; set; }
    }
}
