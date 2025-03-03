﻿using ProjectSky.Core;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Text;
using System.Text.Json.Serialization;
using System.Threading.Tasks;

namespace ProjectSky.Models
{
    public class Personal
    {
        public class BaseStats : ViewModel
        {
            [JsonIgnore]
            private int _HP;
            public int HP
            {
                get => _HP;
                set
                {
                    _HP = value;
                    OnPropertyChanged();
                    OnPropertyChanged(nameof(BST));
                }
            }
            [JsonIgnore]
            private int _ATK;
            public int ATK
            {
                get => _ATK;
                set
                {
                    _ATK = value;
                    OnPropertyChanged();
                    OnPropertyChanged(nameof(BST));
                }
            }
            [JsonIgnore]
            private int _DEF;
            public int DEF
            {
                get => _DEF;
                set
                {
                    _DEF = value;
                    OnPropertyChanged();
                    OnPropertyChanged(nameof(BST));
                }
            }
            [JsonIgnore]
            private int _SPA;
            public int SPA
            {
                get => _SPA;
                set
                {
                    _SPA = value;
                    OnPropertyChanged();
                    OnPropertyChanged(nameof(BST));
                }
            }
            [JsonIgnore]
            private int _SPD;
            public int SPD
            {
                get => _SPD;
                set
                {
                    _SPD = value;
                    OnPropertyChanged();
                    OnPropertyChanged(nameof(BST));
                }
            }
            [JsonIgnore]
            private int _SPE;
            public int SPE
            {
                get => _SPE;
                set
                {
                    _SPE = value;
                    OnPropertyChanged();
                    OnPropertyChanged(nameof(BST));
                }
            }
            [JsonIgnore]
            private int _BST;
            [JsonIgnore]
            public int BST
            {
                get
                {
                    return HP + ATK + DEF + SPA + SPD + SPE;
                }
                set
                {
                    _BST = value;
                    OnPropertyChanged();
                }
            }
        }

        public class Dex
        {
            public int index { get; set; }
            public int group { get; set; }
        }

        public class EggHatch
        {
            public int species { get; set; }
            public int form { get; set; }
            public int form_flags { get; set; }
            public int form_everstone { get; set; }
        }

        public class Entry : ViewModel
        {
            public Species species { get; set; }
            public bool is_present { get; set; }
            public int kitakami_dex { get; set; }
            public int blueberry_dex { get; set; }
            public int type_1 { get; set; }
            public int type_2 { get; set; }
            public int ability_1 { get; set; }
            public int ability_2 { get; set; }
            public int ability_hidden { get; set; }
            public int xp_growth { get; set; }
            public int catch_rate { get; set; }
            public Gender gender { get; set; }
            public int egg_group_1 { get; set; }
            public int egg_group_2 { get; set; }
            public EggHatch egg_hatch { get; set; }
            public int egg_hatch_steps { get; set; }
            public int base_friendship { get; set; }
            public int exp_addend { get; set; }
            public int evo_stage { get; set; }
            public bool unk_flag { get; set; }
            public EvYield ev_yield { get; set; }
            public BaseStats base_stats { get; set; }
            [JsonIgnore]
            private ObservableCollection<EvoDatum> _evo_data;
            public ObservableCollection<EvoDatum> evo_data
            {
                get => _evo_data;
                set
                {
                    _evo_data = value;
                    OnPropertyChanged();
                }
            }
            public List<int> tm_moves { get; set; }
            public List<int> egg_moves { get; set; }
            public List<object> reminder_moves { get; set; }
            [JsonIgnore]
            private ObservableCollection<LevelupMove> _levelup_moves;
            public ObservableCollection<LevelupMove> levelup_moves {
                get => _levelup_moves;
                set
                {
                    _levelup_moves = value;
                    OnPropertyChanged();
                }
            }
            public Dex dex { get; set; }
        }

        public class EvoDatum
        {
            public int level { get; set; }
            public int condition { get; set; }
            public int parameter { get; set; }
            public int reserved3 { get; set; }
            public int reserved4 { get; set; }
            public int reserved5 { get; set; }
            public int species { get; set; }
            public int form { get; set; }
        }

        public class EvYield
        {
            public int HP { get; set; }
            public int ATK { get; set; }
            public int DEF { get; set; }
            public int SPA { get; set; }
            public int SPD { get; set; }
            public int SPE { get; set; }
        }

        public class Gender
        {
            public int group { get; set; }
            public int ratio { get; set; }
        }

        public class LevelupMove : ViewModel
        {
            [JsonIgnore]
            private int _move;
            public int move
            {
                get => _move;
                set
                {
                    _move = value;
                    OnPropertyChanged();
                }
            }
            public int level { get; set; }
        }

        public class PersonalArray
        {
            public List<Entry> entry { get; set; }
        }

        public class Species
        {
            public int species { get; set; }
            public int form { get; set; }
            public int model { get; set; }
            public int color { get; set; }
            public int body_type { get; set; }
            public int height { get; set; }
            public int weight { get; set; }
            public int reserved { get; set; }
            public int reserved1 { get; set; }
            public int reserved2 { get; set; }
        }


    }
}
