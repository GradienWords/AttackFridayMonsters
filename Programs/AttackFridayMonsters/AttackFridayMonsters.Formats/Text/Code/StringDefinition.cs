//  Copyright (c) 2020 Benito Palacios Sánchez
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
namespace AttackFridayMonsters.Formats.Text.Code
{
    using System.Collections.ObjectModel;

    public class StringDefinition
    {
        public long Address { get; set; }

        public int Size { get; set; }

        public string Encoding { get; set; }

        public Collection<long> Pointers { get; set; }

        public Collection<Subpointer> Subpointers { get; set; }
    }
}
