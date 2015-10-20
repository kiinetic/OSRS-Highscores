using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace OSRS_Highscores {
    class Player {

        public int attack, defence, strength, hitpoints, range, prayer, magic, cooking, woodcutting, fletching, fishing, firemaking, crafting, smithing, mining, herblore, agility, thieving, slayer, farming, runecrafting, hunter, construction;
        public string playerName;

        public Player(int attack, int defence, int strength, int hitpoints, int range, int prayer, int magic, int cooking, int woodcutting, int fletching, int fishing,
            int firemaking, int crafting, int smithing, int mining, int herblore, int agility, int thieving, int slayer, int farming, int runecrafting, int hunter,
            int construction, string playerName) {
                this.attack = attack;
                this.defence = defence;
                this.strength = strength;
                this.hitpoints = hitpoints;
                this.range = range;
                this.prayer = prayer;
                this.magic = magic;
                this.cooking = cooking;
                this.woodcutting = woodcutting;
                this.fletching = fletching;
                this.fishing = fishing;
                this.firemaking = firemaking;
                this.crafting = crafting;
                this.smithing = smithing;
                this.mining = mining;
                this.agility = agility;
                this.thieving = thieving;
                this.slayer = slayer;
                this.farming = farming;
                this.runecrafting = runecrafting;
                this.hunter = hunter;
                this.construction = construction;
                this.herblore = herblore;
                this.playerName = playerName;
        }
    }
}
