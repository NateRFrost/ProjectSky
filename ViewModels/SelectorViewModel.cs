﻿using CliWrap;
using CliWrap.Buffered;
using ProjectSky.Core;
using ProjectSky.Models;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Diagnostics;
using System.Drawing;
using System.IO;
using System.IO.Compression;
using System.Linq;
using System.Reflection;
using System.Text;
using System.Text.Json;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Media;
using System.Windows.Media.Imaging;

namespace ProjectSky.ViewModels
{
    public class SelectorViewModel : ViewModel
    {
        private INavigationService _navigationService;
        public INavigationService NavigationService
        {
            get => _navigationService;
            set
            {
                _navigationService = value;
                OnPropertyChanged();
            }
        }

        private string _searchText = "Search";
        public string SearchText {
            get => _searchText;
            set
            {
                _searchText = value;
                Filter();
            }
        }

        private Personal.PersonalArray _personal;

        private PokeData.DataArray _pdata;

        private Plib.PlibArray _plib;

        public ItemDevID.DevID ItemDevID;
        public PokeDevID.DevID PokeDevID;

        List<string> pictureNames = new(){ "bulbasaur", "ivysaur", "venusaur", "venusaurmega", "charmander", "charmeleon", "charizard", "charizardmegax", "charizardmegay", "squirtle", "wartortle", "blastoise", "blastoisemega", "caterpie", "metapod", "butterfree", "weedle", "kakuna", "beedrill", "beedrillmega", "pidgey", "pidgeotto", "pidgeot", "pidgeotmega", "rattata", "rattataalola", "raticate", "raticatealola", "spearow", "fearow", "ekans", "arbok", "pikachu", "pikachu", "pikachu", "pikachu", "pikachu", "pikachu", "pikachu", "pikachu", "pikachu", "pikachu", "raichu", "raichualola", "sandshrew", "sandshrewalola", "sandslash", "sandslashalola", "nidoranf", "nidorina", "nidoqueen", "nidoranm", "nidorino", "nidoking", "clefairy", "clefable", "vulpix", "vulpixalola", "ninetales", "ninetalesalola", "jigglypuff", "wigglytuff", "zubat", "golbat", "oddish", "gloom", "vileplume", "paras", "parasect", "venonat", "venomoth", "diglett", "diglettalola", "dugtrio", "dugtrioalola", "meowth", "meowthalola", "meowthgalar", "persian", "persianalola", "psyduck", "golduck", "mankey", "primeape", "growlithe", "growlithehisui", "arcanine", "arcaninehisui", "poliwag", "poliwhirl", "poliwrath", "abra", "kadabra", "alakazam", "alakazammega", "machop", "machoke", "machamp", "bellsprout", "weepinbell", "victreebel", "tentacool", "tentacruel", "geodude", "geodudealola", "graveler", "graveleralola", "golem", "golemalola", "ponyta", "ponytagalar", "rapidash", "rapidashgalar", "slowpoke", "slowpokegalar", "slowbro", "slowbromega", "slowbrogalar", "magnemite", "magneton", "farfetchd", "farfetchdgalar", "doduo", "dodrio", "seel", "dewgong", "grimer", "grimeralola", "muk", "mukalola", "shellder", "cloyster", "gastly", "haunter", "gengar", "gengarmega", "onix", "drowzee", "hypno", "krabby", "kingler", "voltorb", "voltorbhisui", "electrode", "electrodehisui", "exeggcute", "exeggutor", "exeggutoralola", "cubone", "marowak", "marowakalola", "hitmonlee", "hitmonchan", "lickitung", "koffing", "weezing", "weezinggalar", "rhyhorn", "rhydon", "chansey", "tangela", "kangaskhan", "kangaskhanmega", "horsea", "seadra", "goldeen", "seaking", "staryu", "starmie", "mrmime", "mrmimegalar", "scyther", "jynx", "electabuzz", "magmar", "pinsir", "pinsirmega", "tauros", "taurospaldea", "taurospaldeafire", "taurospaldeawater", "magikarp", "gyarados", "gyaradosmega", "lapras", "ditto", "eevee", "eevee", "vaporeon", "jolteon", "flareon", "porygon", "omanyte", "omastar", "kabuto", "kabutops", "aerodactyl", "aerodactylmega", "snorlax", "articuno", "articunogalar", "zapdos", "zapdosgalar", "moltres", "moltresgalar", "dratini", "dragonair", "dragonite", "mewtwo", "mewtwomegax", "mewtwomegay", "mew", "chikorita", "bayleef", "meganium", "cyndaquil", "quilava", "typhlosion", "typhlosionhisui", "totodile", "croconaw", "feraligatr", "sentret", "furret", "hoothoot", "noctowl", "ledyba", "ledian", "spinarak", "ariados", "crobat", "chinchou", "lanturn", "pichu", "cleffa", "igglybuff", "togepi", "togetic", "natu", "xatu", "mareep", "flaaffy", "ampharos", "ampharosmega", "bellossom", "marill", "azumarill", "sudowoodo", "politoed", "hoppip", "skiploom", "jumpluff", "aipom", "sunkern", "sunflora", "yanma", "wooper", "wooperpaldea", "quagsire", "espeon", "umbreon", "murkrow", "slowking", "slowkinggalar", "misdreavus", "unown", "unown", "unown", "unown", "unown", "unown", "unown", "unown", "unown", "unown", "unown", "unown", "unown", "unown", "unown", "unown", "unown", "unown", "unown", "unown", "unown", "unown", "unown", "unown", "unown", "unown", "unown", "unown", "wobbuffet", "girafarig", "pineco", "forretress", "dunsparce", "gligar", "steelix", "steelixmega", "snubbull", "granbull", "qwilfish", "qwilfishhisui", "scizor", "scizormega", "shuckle", "heracross", "heracrossmega", "sneasel", "sneaselhisui", "teddiursa", "ursaring", "slugma", "magcargo", "swinub", "piloswine", "corsola", "corsolagalar", "remoraid", "octillery", "delibird", "mantine", "skarmory", "houndour", "houndoom", "houndoommega", "kingdra", "phanpy", "donphan", "porygon2", "stantler", "smeargle", "tyrogue", "hitmontop", "smoochum", "elekid", "magby", "miltank", "blissey", "raikou", "entei", "suicune", "larvitar", "pupitar", "tyranitar", "tyranitarmega", "lugia", "hooh", "celebi", "treecko", "grovyle", "sceptile", "sceptilemega", "torchic", "combusken", "blaziken", "blazikenmega", "mudkip", "marshtomp", "swampert", "swampertmega", "poochyena", "mightyena", "zigzagoon", "zigzagoongalar", "linoone", "linoonegalar", "wurmple", "silcoon", "beautifly", "cascoon", "dustox", "lotad", "lombre", "ludicolo", "seedot", "nuzleaf", "shiftry", "taillow", "swellow", "wingull", "pelipper", "ralts", "kirlia", "gardevoir", "gardevoirmega", "surskit", "masquerain", "shroomish", "breloom", "slakoth", "vigoroth", "slaking", "nincada", "ninjask", "shedinja", "whismur", "loudred", "exploud", "makuhita", "hariyama", "azurill", "nosepass", "skitty", "delcatty", "sableye", "sableyemega", "mawile", "mawilemega", "aron", "lairon", "aggron", "aggronmega", "meditite", "medicham", "medichammega", "electrike", "manectric", "manectricmega", "plusle", "minun", "volbeat", "illumise", "roselia", "gulpin", "swalot", "carvanha", "sharpedo", "sharpedomega", "wailmer", "wailord", "numel", "camerupt", "cameruptmega", "torkoal", "spoink", "grumpig", "spinda", "trapinch", "vibrava", "flygon", "cacnea", "cacturne", "swablu", "altaria", "altariamega", "zangoose", "seviper", "lunatone", "solrock", "barboach", "whiscash", "corphish", "crawdaunt", "baltoy", "claydol", "lileep", "cradily", "anorith", "armaldo", "feebas", "milotic", "castform", "castform", "castform", "castform", "kecleon", "shuppet", "banette", "banettemega", "duskull", "dusclops", "tropius", "chimecho", "absol", "absolmega", "wynaut", "snorunt", "glalie", "glaliemega", "spheal", "sealeo", "walrein", "clamperl", "huntail", "gorebyss", "relicanth", "luvdisc", "bagon", "shelgon", "salamence", "salamencemega", "beldum", "metang", "metagross", "metagrossmega", "regirock", "regice", "registeel", "latias", "latiasmega", "latios", "latiosmega", "kyogre", "kyogre", "groudon", "groudon", "rayquaza", "rayquazamega", "jirachi", "deoxys", "deoxys", "deoxys", "deoxys", "turtwig", "grotle", "torterra", "chimchar", "monferno", "infernape", "piplup", "prinplup", "empoleon", "starly", "staravia", "staraptor", "bidoof", "bibarel", "kricketot", "kricketune", "shinx", "luxio", "luxray", "budew", "roserade", "cranidos", "rampardos", "shieldon", "bastiodon", "burmy", "burmy", "burmy", "wormadam", "wormadam", "wormadam", "mothim", "mothim", "mothim", "combee", "vespiquen", "pachirisu", "buizel", "floatzel", "cherubi", "cherrim", "cherrim", "shellos", "shellos", "gastrodon", "gastrodon", "ambipom", "drifloon", "drifblim", "buneary", "lopunny", "lopunnymega", "mismagius", "honchkrow", "glameow", "purugly", "chingling", "stunky", "skuntank", "bronzor", "bronzong", "bonsly", "mimejr", "happiny", "chatot", "spiritomb", "gible", "gabite", "garchomp", "garchompmega", "munchlax", "riolu", "lucario", "lucariomega", "hippopotas", "hippowdon", "skorupi", "drapion", "croagunk", "toxicroak", "carnivine", "finneon", "lumineon", "mantyke", "snover", "abomasnow", "abomasnowmega", "weavile", "magnezone", "lickilicky", "rhyperior", "tangrowth", "electivire", "magmortar", "togekiss", "yanmega", "leafeon", "glaceon", "gliscor", "mamoswine", "porygonz", "gallade", "gallademega", "probopass", "dusknoir", "froslass", "rotom", "rotom", "rotom", "rotom", "rotom", "rotom", "uxie", "mesprit", "azelf", "dialga", "dialga", "palkia", "palkia", "heatran", "regigigas", "giratina", "giratina", "cresselia", "phione", "manaphy", "darkrai", "shaymin", "shaymin", "arceus", "arceus", "arceus", "arceus", "arceus", "arceus", "arceus", "arceus", "arceus", "arceus", "arceus", "arceus", "arceus", "arceus", "arceus", "arceus", "arceus", "arceus", "victini", "snivy", "servine", "serperior", "tepig", "pignite", "emboar", "oshawott", "dewott", "samurott", "samurotthisui", "patrat", "watchog", "lillipup", "herdier", "stoutland", "purrloin", "liepard", "pansage", "simisage", "pansear", "simisear", "panpour", "simipour", "munna", "musharna", "pidove", "tranquill", "unfezant", "blitzle", "zebstrika", "roggenrola", "boldore", "gigalith", "woobat", "swoobat", "drilbur", "excadrill", "audino", "audinomega", "timburr", "gurdurr", "conkeldurr", "tympole", "palpitoad", "seismitoad", "throh", "sawk", "sewaddle", "swadloon", "leavanny", "venipede", "whirlipede", "scolipede", "cottonee", "whimsicott", "petilil", "lilligant", "lilliganthisui", "basculin", "basculin", "basculin", "sandile", "krokorok", "krookodile", "darumaka", "darumakagalar", "darmanitan", "darmanitangalar", "darmanitan", "darmanitan", "maractus", "dwebble", "crustle", "scraggy", "scrafty", "sigilyph", "yamask", "yamaskgalar", "cofagrigus", "tirtouga", "carracosta", "archen", "archeops", "trubbish", "garbodor", "zorua", "zoruahisui", "zoroark", "zoroarkhisui", "minccino", "cinccino", "gothita", "gothorita", "gothitelle", "solosis", "duosion", "reuniclus", "ducklett", "swanna", "vanillite", "vanillish", "vanilluxe", "deerling", "deerling", "deerling", "deerling", "sawsbuck", "sawsbuck", "sawsbuck", "sawsbuck", "emolga", "karrablast", "escavalier", "foongus", "amoonguss", "frillish", "jellicent", "alomomola", "joltik", "galvantula", "ferroseed", "ferrothorn", "klink", "klang", "klinklang", "tynamo", "eelektrik", "eelektross", "elgyem", "beheeyem", "litwick", "lampent", "chandelure", "axew", "fraxure", "haxorus", "cubchoo", "beartic", "cryogonal", "shelmet", "accelgor", "stunfisk", "stunfiskgalar", "mienfoo", "mienshao", "druddigon", "golett", "golurk", "pawniard", "bisharp", "bouffalant", "rufflet", "braviary", "braviaryhisui", "vullaby", "mandibuzz", "heatmor", "durant", "deino", "zweilous", "hydreigon", "larvesta", "volcarona", "cobalion", "terrakion", "virizion", "tornadus", "tornadus", "thundurus", "thundurus", "reshiram", "zekrom", "landorus", "landorus", "kyurem", "kyurem", "kyurem", "keldeo", "keldeo", "meloetta", "meloetta", "genesect", "genesect", "genesect", "genesect", "genesect", "chespin", "quilladin", "chesnaught", "fennekin", "braixen", "delphox", "froakie", "frogadier", "greninja", "greninja", "greninja", "bunnelby", "diggersby", "fletchling", "fletchinder", "talonflame", "scatterbug", "scatterbug", "scatterbug", "scatterbug", "scatterbug", "scatterbug", "scatterbug", "scatterbug", "scatterbug", "scatterbug", "scatterbug", "scatterbug", "scatterbug", "scatterbug", "scatterbug", "scatterbug", "scatterbug", "scatterbug", "scatterbug", "scatterbug", "spewpa", "spewpa", "spewpa", "spewpa", "spewpa", "spewpa", "spewpa", "spewpa", "spewpa", "spewpa", "spewpa", "spewpa", "spewpa", "spewpa", "spewpa", "spewpa", "spewpa", "spewpa", "spewpa", "spewpa", "vivillon", "vivillon", "vivillon", "vivillon", "vivillon", "vivillon", "vivillon", "vivillon", "vivillon", "vivillon", "vivillon", "vivillon", "vivillon", "vivillon", "vivillon", "vivillon", "vivillon", "vivillon", "vivillon", "vivillon", "litleo", "pyroar", "flabebe", "flabebe", "flabebe", "flabebe", "flabebe", "floette", "floette", "floette", "floette", "floette", "floette", "florges", "florges", "florges", "florges", "florges", "skiddo", "gogoat", "pancham", "pangoro", "furfrou", "furfrou", "furfrou", "furfrou", "furfrou", "furfrou", "furfrou", "furfrou", "furfrou", "furfrou", "espurr", "meowstic", "meowstic", "honedge", "doublade", "aegislash", "aegislash", "spritzee", "aromatisse", "swirlix", "slurpuff", "inkay", "malamar", "binacle", "barbaracle", "skrelp", "dragalge", "clauncher", "clawitzer", "helioptile", "heliolisk", "tyrunt", "tyrantrum", "amaura", "aurorus", "sylveon", "hawlucha", "dedenne", "carbink", "goomy", "sliggoo", "sliggoohisui", "goodra", "goodrahisui", "klefki", "phantump", "trevenant", "pumpkaboo", "pumpkaboo", "pumpkaboo", "pumpkaboo", "gourgeist", "gourgeist", "gourgeist", "gourgeist", "bergmite", "avalugg", "avalugghisui", "noibat", "noivern", "xerneas", "xerneas", "yveltal", "zygarde", "zygarde", "zygarde", "zygarde", "zygarde", "diancie", "dianciemega", "hoopa", "hoopa", "volcanion", "rowlet", "dartrix", "decidueye", "decidueyehisui", "litten", "torracat", "incineroar", "popplio", "brionne", "primarina", "pikipek", "trumbeak", "toucannon", "yungoos", "gumshoos", "grubbin", "charjabug", "vikavolt", "crabrawler", "crabominable", "oricorio", "oricorio", "oricorio", "oricorio", "cutiefly", "ribombee", "rockruff", "rockruff", "lycanroc", "lycanroc", "lycanroc", "wishiwashi", "wishiwashi", "mareanie", "toxapex", "mudbray", "mudsdale", "dewpider", "araquanid", "fomantis", "lurantis", "morelull", "shiinotic", "salandit", "salazzle", "stufful", "bewear", "bounsweet", "steenee", "tsareena", "comfey", "oranguru", "passimian", "wimpod", "golisopod", "sandygast", "palossand", "pyukumuku", "typenull", "silvally", "silvally", "silvally", "silvally", "silvally", "silvally", "silvally", "silvally", "silvally", "silvally", "silvally", "silvally", "silvally", "silvally", "silvally", "silvally", "silvally", "silvally", "minior", "minior", "minior", "minior", "minior", "minior", "minior", "minior", "minior", "minior", "minior", "minior", "minior", "minior", "komala", "turtonator", "togedemaru", "mimikyu", "mimikyu", "bruxish", "drampa", "dhelmise", "jangmoo", "hakamoo", "kommoo", "tapukoko", "tapulele", "tapubulu", "tapufini", "cosmog", "cosmoem", "solgaleo", "lunala", "nihilego", "buzzwole", "pheromosa", "xurkitree", "celesteela", "kartana", "guzzlord", "necrozma", "necrozma", "necrozma", "necrozma", "magearna", "magearna", "marshadow", "poipole", "naganadel", "stakataka", "blacephalon", "zeraora", "meltan", "melmetal", "grookey", "thwackey", "rillaboom", "scorbunny", "raboot", "cinderace", "sobble", "drizzile", "inteleon", "skwovet", "greedent", "rookidee", "corvisquire", "corviknight", "blipbug", "dottler", "orbeetle", "nickit", "thievul", "gossifleur", "eldegoss", "wooloo", "dubwool", "chewtle", "drednaw", "yamper", "boltund", "rolycoly", "carkol", "coalossal", "applin", "flapple", "appletun", "silicobra", "sandaconda", "cramorant", "cramorant", "cramorant", "arrokuda", "barraskewda", "toxel", "toxtricity", "toxtricity", "sizzlipede", "centiskorch", "clobbopus", "grapploct", "sinistea", "sinistea", "polteageist", "polteageist", "hatenna", "hattrem", "hatterene", "impidimp", "morgrem", "grimmsnarl", "obstagoon", "perrserker", "cursola", "sirfetchd", "mrrime", "runerigus", "milcery", "alcremie", "alcremie", "alcremie", "alcremie", "alcremie", "alcremie", "alcremie", "alcremie", "alcremie", "falinks", "pincurchin", "snom", "frosmoth", "stonjourner", "eiscue", "eiscue", "indeedee", "indeedee", "morpeko", "morpeko", "cufant", "copperajah", "dracozolt", "arctozolt", "dracovish", "arctovish", "duraludon", "dreepy", "drakloak", "dragapult", "zacian", "zacian", "zamazenta", "zamazenta", "eternatus", "eternatus", "kubfu", "urshifu", "urshifu", "zarude", "zarude", "regieleki", "regidrago", "glastrier", "spectrier", "calyrex", "calyrex", "calyrex", "wyrdeer", "kleavor", "ursaluna", "ursaluna-bloodmoon", "basculegion", "basculegion", "sneasler", "overqwil", "enamorus", "enamorus", "sprigatito", "floragato", "meowscarada", "fuecoco", "crocalor", "skeledirge", "quaxly", "quaxwell", "quaquaval", "lechonk", "oinkologne", "oinkologne", "dudunsparce", "dudunsparce", "tarountula", "spidops", "nymble", "lokix", "rellor", "rabsca", "greavard", "houndstone", "flittle", "espathra", "farigiraf", "wiglett", "wugtrio", "dondozo", "veluza", "finizen", "palafin", "palafin", "smoliv", "dolliv", "arboliva", "capsakid", "scovillain", "tadbulb", "bellibolt", "varoom", "revavroom", "orthworm", "tandemaus", "maushold", "maushold", "cetoddle", "cetitan", "frigibax", "arctibax", "baxcalibur", "tatsugiri", "tatsugiri", "tatsugiri", "cyclizar", "pawmi", "pawmo", "pawmot", "wattrel", "kilowattrel", "bombirdier", "squawkabilly", "squawkabilly", "squawkabilly", "squawkabilly", "flamigo", "klawf", "nacli", "naclstack", "garganacl", "glimmet", "glimmora", "shroodle", "grafaiai", "fidough", "dachsbun", "maschiff", "mabosstiff", "bramblin", "brambleghast", "gimmighoul", "gimmighoul", "gholdengo", "greattusk", "brutebonnet", "walkingwake", "sandyshocks", "screamtail", "fluttermane", "slitherwing", "roaringmoon", "irontreads", "ironleaves", "ironmoth", "ironhands", "ironjugulis", "ironthorns", "ironbundle", "ironvaliant", "tinglu", "chienpao", "wochien", "chiyu", "koraidon", "koraidon", "koraidon", "koraidon", "koraidon", "miraidon", "miraidon", "miraidon", "miraidon", "miraidon", "tinkatink", "tinkatuff", "tinkaton", "charcadet", "armarouge", "ceruledge", "toedscool", "toedscruel", "kingambit", "clodsire", "annihilape", "ogerpon", "ogerpon-wellspring", "ogerpon-hearthflame", "ogerpon-cornerstone", "ogerpon", "ogerpon-wellspring", "ogerpon-hearthflame", "ogerpon-cornerstone", "dipplin", "hydrapple", "okidogi", "munkidori", "fezandipiti", "gougingfire", "ragingbolt", "ironcrown", "ironboulder", "terapagos", "terapagos", "terapagos", "pecharunt", "archaludon", "poltchageist", "poltchageist", "sinistcha", "sinistcha" };
        List<string> buttonNames = new() { "Bulbasaur", "Ivysaur", "Venusaur", "Mega Venusaur", "Charmander", "Charmeleon", "Charizard", "Mega Charizard X", "Mega Charizard Y", "Squirtle", "Wartortle", "Blastoise", "Mega Blastoise", "Caterpie", "Metapod", "Butterfree", "Weedle", "Kakuna", "Beedrill", "Mega Beedrill", "Pidgey", "Pidgeotto", "Pidgeot", "Mega Pidgeot", "Rattata", "Alolan Rattata", "Raticate", "Alolan Raticate", "Spearow", "Fearow", "Ekans", "Arbok", "Pikachu", "Pikachu", "Pikachu", "Pikachu", "Pikachu", "Pikachu", "Pikachu", "Pikachu", "Pikachu", "Pikachu", "Raichu", "Alolan Raichu", "Sandshrew", "Alolan Sandshrew", "Sandslash", "Alolan Sandslash", "Nidoran♀", "Nidorina", "Nidoqueen", "Nidoran♂", "Nidorino", "Nidoking", "Clefairy", "Clefable", "Vulpix", "Alolan Vulpix", "Ninetales", "Alolan Ninetales", "Jigglypuff", "Wigglytuff", "Zubat", "Golbat", "Oddish", "Gloom", "Vileplume", "Paras", "Parasect", "Venonat", "Venomoth", "Diglett", "Alolan Diglett", "Dugtrio", "Alolan Dugtrio", "Meowth", "Alolan Meowth", "Alolan Meowth", "Persian", "Alolan Persian", "Psyduck", "Golduck", "Mankey", "Primeape", "Growlithe", "Hisuian Growlithe", "Arcanine", "Hisuian Arcanine", "Poliwag", "Poliwhirl", "Poliwrath", "Abra", "Kadabra", "Alakazam", "Mega Alakazam", "Machop", "Machoke", "Machamp", "Bellsprout", "Weepinbell", "Victreebel", "Tentacool", "Tentacruel", "Geodude", "Alolan Geodude", "Graveler", "Alolan Graveler", "Golem", "Alolan Golem", "Ponyta", "Galarian Ponyta", "Rapidash", "Galarian Rapidash", "Slowpoke", "Galarian Slowpoke", "Slowbro", "Galarian Slowbro", "Galarian Slowbro", "Magnemite", "Magneton", "Farfetch’d", "Farfetch’d", "Doduo", "Dodrio", "Seel", "Dewgong", "Grimer", "Alolan Grimer", "Muk", "Alolan Muk", "Shellder", "Cloyster", "Gastly", "Haunter", "Gengar", "Mega Gengar", "Onix", "Drowzee", "Hypno", "Krabby", "Kingler", "Voltorb", "Hisuian Voltorb", "Electrode", "Hisuian Electrode", "Exeggcute", "Exeggutor", "Alolan Exeggutor", "Cubone", "Marowak", "Alolan Marowak", "Hitmonlee", "Hitmonchan", "Lickitung", "Koffing", "Weezing", "Galarian Weezing", "Rhyhorn", "Rhydon", "Chansey", "Tangela", "Kangaskhan", "Mega Kangaskhan", "Horsea", "Seadra", "Goldeen", "Seaking", "Staryu", "Starmie", "Mr. Mime", "Mr. Mime", "Scyther", "Jynx", "Electabuzz", "Magmar", "Pinsir", "Mega Pinsir", "Tauros", "Tauros", "Tauros", "Tauros", "Magikarp", "Gyarados", "Mega Gyarados", "Lapras", "Ditto", "Eevee", "Eevee", "Vaporeon", "Jolteon", "Flareon", "Porygon", "Omanyte", "Omastar", "Kabuto", "Kabutops", "Aerodactyl", "Mega Aerodactyl", "Snorlax", "Articuno", "Galarian Articuno", "Zapdos", "Galarian Zapdos", "Moltres", "Galarian Moltres", "Dratini", "Dragonair", "Dragonite", "Mewtwo", "Mega Mewtwo X", "Mega Mewtwo Y", "Mew", "Chikorita", "Bayleef", "Meganium", "Cyndaquil", "Quilava", "Typhlosion", "Hisuian Typhlosion", "Totodile", "Croconaw", "Feraligatr", "Sentret", "Furret", "Hoothoot", "Noctowl", "Ledyba", "Ledian", "Spinarak", "Ariados", "Crobat", "Chinchou", "Lanturn", "Pichu", "Cleffa", "Igglybuff", "Togepi", "Togetic", "Natu", "Xatu", "Mareep", "Flaaffy", "Ampharos", "Mega Ampharos", "Bellossom", "Marill", "Azumarill", "Sudowoodo", "Politoed", "Hoppip", "Skiploom", "Jumpluff", "Aipom", "Sunkern", "Sunflora", "Yanma", "Wooper", "Paldean Wooper", "Quagsire", "Espeon", "Umbreon", "Murkrow", "Slowking", "Galarian Slowking", "Misdreavus", "Unown", "Unown", "Unown", "Unown", "Unown", "Unown", "Unown", "Unown", "Unown", "Unown", "Unown", "Unown", "Unown", "Unown", "Unown", "Unown", "Unown", "Unown", "Unown", "Unown", "Unown", "Unown", "Unown", "Unown", "Unown", "Unown", "Unown", "Unown", "Wobbuffet", "Girafarig", "Pineco", "Forretress", "Dunsparce", "Gligar", "Steelix", "Mega Steelix", "Snubbull", "Granbull", "Qwilfish", "Hisuian Qwilfish", "Scizor", "Mega Scizor", "Shuckle", "Heracross", "Mega Heracross", "Sneasel", "Hisuian Sneasel", "Teddiursa", "Ursaring", "Slugma", "Magcargo", "Swinub", "Piloswine", "Corsola", "Galarian Corsola", "Remoraid", "Octillery", "Delibird", "Mantine", "Skarmory", "Houndour", "Houndoom", "Mega Houndoom", "Kingdra", "Phanpy", "Donphan", "Porygon2", "Stantler", "Smeargle", "Tyrogue", "Hitmontop", "Smoochum", "Elekid", "Magby", "Miltank", "Blissey", "Raikou", "Entei", "Suicune", "Larvitar", "Pupitar", "Tyranitar", "Mega Tyranitar", "Lugia", "Ho-Oh", "Celebi", "Treecko", "Grovyle", "Sceptile", "Mega Sceptile", "Torchic", "Combusken", "Blaziken", "Mega Blaziken", "Mudkip", "Marshtomp", "Swampert", "Mega Swampert", "Poochyena", "Mightyena", "Zigzagoon", "Galarian Zigzagoon", "Linoone", "Galarian Linoone", "Wurmple", "Silcoon", "Beautifly", "Cascoon", "Dustox", "Lotad", "Lombre", "Ludicolo", "Seedot", "Nuzleaf", "Shiftry", "Taillow", "Swellow", "Wingull", "Pelipper", "Ralts", "Kirlia", "Gardevoir", "Mega Gardevoir", "Surskit", "Masquerain", "Shroomish", "Breloom", "Slakoth", "Vigoroth", "Slaking", "Nincada", "Ninjask", "Shedinja", "Whismur", "Loudred", "Exploud", "Makuhita", "Hariyama", "Azurill", "Nosepass", "Skitty", "Delcatty", "Sableye", "Mega Sableye", "Mawile", "Mega Mawile", "Aron", "Lairon", "Aggron", "Mega Aggron", "Meditite", "Medicham", "Mega Medicham", "Electrike", "Manectric", "Mega Manectric", "Plusle", "Minun", "Volbeat", "Illumise", "Roselia", "Gulpin", "Swalot", "Carvanha", "Sharpedo", "Mega Sharpedo", "Wailmer", "Wailord", "Numel", "Camerupt", "Mega Camerupt", "Torkoal", "Spoink", "Grumpig", "Spinda", "Trapinch", "Vibrava", "Flygon", "Cacnea", "Cacturne", "Swablu", "Altaria", "Mega Altaria", "Zangoose", "Seviper", "Lunatone", "Solrock", "Barboach", "Whiscash", "Corphish", "Crawdaunt", "Baltoy", "Claydol", "Lileep", "Cradily", "Anorith", "Armaldo", "Feebas", "Milotic", "Castform", "Castform", "Castform", "Castform", "Kecleon", "Shuppet", "Banette", "Mega Banette", "Duskull", "Dusclops", "Tropius", "Chimecho", "Absol", "Mega Absol", "Wynaut", "Snorunt", "Glalie", "Mega Glalie", "Spheal", "Sealeo", "Walrein", "Clamperl", "Huntail", "Gorebyss", "Relicanth", "Luvdisc", "Bagon", "Shelgon", "Salamence", "Mega Salamence", "Beldum", "Metang", "Metagross", "Mega Metagross", "Regirock", "Regice", "Registeel", "Latias", "Mega Latias", "Latios", "Mega Latios", "Kyogre", "Primal Kyogre", "Groudon", "Primal Groudon", "Rayquaza", "Mega Rayquaza", "Jirachi", "Deoxys", "Deoxys", "Deoxys", "Deoxys", "Turtwig", "Grotle", "Torterra", "Chimchar", "Monferno", "Infernape", "Piplup", "Prinplup", "Empoleon", "Starly", "Staravia", "Staraptor", "Bidoof", "Bibarel", "Kricketot", "Kricketune", "Shinx", "Luxio", "Luxray", "Budew", "Roserade", "Cranidos", "Rampardos", "Shieldon", "Bastiodon", "Burmy", "Burmy", "Burmy", "Wormadam", "Wormadam", "Wormadam", "Mothim", "Mothim", "Mothim", "Combee", "Vespiquen", "Pachirisu", "Buizel", "Floatzel", "Cherubi", "Cherrim", "Cherrim", "Shellos", "Shellos", "Gastrodon", "Gastrodon", "Ambipom", "Drifloon", "Drifblim", "Buneary", "Lopunny", "Mega Lopunny", "Mismagius", "Honchkrow", "Glameow", "Purugly", "Chingling", "Stunky", "Skuntank", "Bronzor", "Bronzong", "Bonsly", "Mime Jr.", "Happiny", "Chatot", "Spiritomb", "Gible", "Gabite", "Garchomp", "Mega Garchomp", "Munchlax", "Riolu", "Lucario", "Mega Lucario", "Hippopotas", "Hippowdon", "Skorupi", "Drapion", "Croagunk", "Toxicroak", "Carnivine", "Finneon", "Lumineon", "Mantyke", "Snover", "Abomasnow", "Mega Abomasnow", "Weavile", "Magnezone", "Lickilicky", "Rhyperior", "Tangrowth", "Electivire", "Magmortar", "Togekiss", "Yanmega", "Leafeon", "Glaceon", "Gliscor", "Mamoswine", "Porygon-Z", "Gallade", "Mega Gallade", "Probopass", "Dusknoir", "Froslass", "Rotom", "Rotom", "Rotom", "Rotom", "Rotom", "Rotom", "Uxie", "Mesprit", "Azelf", "Dialga", "Origin Dialga", "Palkia", "Origin Palkia", "Heatran", "Regigigas", "Giratina", "Origin Giratina", "Cresselia", "Phione", "Manaphy", "Darkrai", "Shaymin", "Shaymin", "Arceus", "Arceus", "Arceus", "Arceus", "Arceus", "Arceus", "Arceus", "Arceus", "Arceus", "Arceus", "Arceus", "Arceus", "Arceus", "Arceus", "Arceus", "Arceus", "Arceus", "Arceus", "Victini", "Snivy", "Servine", "Serperior", "Tepig", "Pignite", "Emboar", "Oshawott", "Dewott", "Samurott", "Hisuian Samurott", "Patrat", "Watchog", "Lillipup", "Herdier", "Stoutland", "Purrloin", "Liepard", "Pansage", "Simisage", "Pansear", "Simisear", "Panpour", "Simipour", "Munna", "Musharna", "Pidove", "Tranquill", "Unfezant", "Blitzle", "Zebstrika", "Roggenrola", "Boldore", "Gigalith", "Woobat", "Swoobat", "Drilbur", "Excadrill", "Audino", "Mega Audino", "Timburr", "Gurdurr", "Conkeldurr", "Tympole", "Palpitoad", "Seismitoad", "Throh", "Sawk", "Sewaddle", "Swadloon", "Leavanny", "Venipede", "Whirlipede", "Scolipede", "Cottonee", "Whimsicott", "Petilil", "Lilligant", "Hisuian Lilligant", "Basculin", "Basculin", "Basculin", "Sandile", "Krokorok", "Krookodile", "Darumaka", "Galarian Darumaka", "Darmanitan", "Galarian Darmanitan", "Galarian Darmanitan", "Galarian Darmanitan", "Maractus", "Dwebble", "Crustle", "Scraggy", "Scrafty", "Sigilyph", "Yamask", "Galarian Yamask", "Cofagrigus", "Tirtouga", "Carracosta", "Archen", "Archeops", "Trubbish", "Garbodor", "Zorua", "Hisuian Zorua", "Zoroark", "Hisuian Zoroark", "Minccino", "Cinccino", "Gothita", "Gothorita", "Gothitelle", "Solosis", "Duosion", "Reuniclus", "Ducklett", "Swanna", "Vanillite", "Vanillish", "Vanilluxe", "Deerling", "Deerling", "Deerling", "Deerling", "Sawsbuck", "Sawsbuck", "Sawsbuck", "Sawsbuck", "Emolga", "Karrablast", "Escavalier", "Foongus", "Amoonguss", "Frillish", "Jellicent", "Alomomola", "Joltik", "Galvantula", "Ferroseed", "Ferrothorn", "Klink", "Klang", "Klinklang", "Tynamo", "Eelektrik", "Eelektross", "Elgyem", "Beheeyem", "Litwick", "Lampent", "Chandelure", "Axew", "Fraxure", "Haxorus", "Cubchoo", "Beartic", "Cryogonal", "Shelmet", "Accelgor", "Stunfisk", "Galarian Stunfisk", "Mienfoo", "Mienshao", "Druddigon", "Golett", "Golurk", "Pawniard", "Bisharp", "Bouffalant", "Rufflet", "Braviary", "Hisuian Braviary", "Vullaby", "Mandibuzz", "Heatmor", "Durant", "Deino", "Zweilous", "Hydreigon", "Larvesta", "Volcarona", "Cobalion", "Terrakion", "Virizion", "Tornadus", "Tornadus", "Thundurus", "Thundurus", "Reshiram", "Zekrom", "Landorus", "Landorus", "Kyurem", "Kyurem", "Kyurem", "Keldeo", "Keldeo", "Meloetta", "Meloetta", "Genesect", "Genesect", "Genesect", "Genesect", "Genesect", "Chespin", "Quilladin", "Chesnaught", "Fennekin", "Braixen", "Delphox", "Froakie", "Frogadier", "Greninja", "Greninja", "Greninja", "Bunnelby", "Diggersby", "Fletchling", "Fletchinder", "Talonflame", "Scatterbug", "Scatterbug", "Scatterbug", "Scatterbug", "Scatterbug", "Scatterbug", "Scatterbug", "Scatterbug", "Scatterbug", "Scatterbug", "Scatterbug", "Scatterbug", "Scatterbug", "Scatterbug", "Scatterbug", "Scatterbug", "Scatterbug", "Scatterbug", "Scatterbug", "Scatterbug", "Spewpa", "Spewpa", "Spewpa", "Spewpa", "Spewpa", "Spewpa", "Spewpa", "Spewpa", "Spewpa", "Spewpa", "Spewpa", "Spewpa", "Spewpa", "Spewpa", "Spewpa", "Spewpa", "Spewpa", "Spewpa", "Spewpa", "Spewpa", "Vivillon", "Vivillon", "Vivillon", "Vivillon", "Vivillon", "Vivillon", "Vivillon", "Vivillon", "Vivillon", "Vivillon", "Vivillon", "Vivillon", "Vivillon", "Vivillon", "Vivillon", "Vivillon", "Vivillon", "Vivillon", "Vivillon", "Vivillon", "Litleo", "Pyroar", "Flabébé", "Flabébé", "Flabébé", "Flabébé", "Flabébé", "Floette", "Floette", "Floette", "Floette", "Floette", "Floette", "Florges", "Florges", "Florges", "Florges", "Florges", "Skiddo", "Gogoat", "Pancham", "Pangoro", "Furfrou", "Furfrou", "Furfrou", "Furfrou", "Furfrou", "Furfrou", "Furfrou", "Furfrou", "Furfrou", "Furfrou", "Espurr", "Meowstic", "Meowstic", "Honedge", "Doublade", "Aegislash", "Aegislash", "Spritzee", "Aromatisse", "Swirlix", "Slurpuff", "Inkay", "Malamar", "Binacle", "Barbaracle", "Skrelp", "Dragalge", "Clauncher", "Clawitzer", "Helioptile", "Heliolisk", "Tyrunt", "Tyrantrum", "Amaura", "Aurorus", "Sylveon", "Hawlucha", "Dedenne", "Carbink", "Goomy", "Sliggoo", "Hisuian Sliggoo", "Goodra", "Hisuian Goodra", "Klefki", "Phantump", "Trevenant", "Pumpkaboo", "Pumpkaboo", "Pumpkaboo", "Pumpkaboo", "Gourgeist", "Gourgeist", "Gourgeist", "Gourgeist", "Bergmite", "Avalugg", "Hisuian Avalugg", "Noibat", "Noivern", "Xerneas", "Xerneas", "Yveltal", "Zygarde", "Zygarde", "Zygarde", "Zygarde", "Zygarde", "Diancie", "Mega Diancie", "Hoopa", "Hoopa", "Volcanion", "Rowlet", "Dartrix", "Decidueye", "Hisuian Decidueye", "Litten", "Torracat", "Incineroar", "Popplio", "Brionne", "Primarina", "Pikipek", "Trumbeak", "Toucannon", "Yungoos", "Gumshoos", "Grubbin", "Charjabug", "Vikavolt", "Crabrawler", "Crabominable", "Oricorio", "Oricorio", "Oricorio", "Oricorio", "Cutiefly", "Ribombee", "Rockruff", "Rockruff", "Lycanroc", "Lycanroc", "Lycanroc", "Wishiwashi", "Wishiwashi", "Mareanie", "Toxapex", "Mudbray", "Mudsdale", "Dewpider", "Araquanid", "Fomantis", "Lurantis", "Morelull", "Shiinotic", "Salandit", "Salazzle", "Stufful", "Bewear", "Bounsweet", "Steenee", "Tsareena", "Comfey", "Oranguru", "Passimian", "Wimpod", "Golisopod", "Sandygast", "Palossand", "Pyukumuku", "Type: Null", "Silvally", "Silvally", "Silvally", "Silvally", "Silvally", "Silvally", "Silvally", "Silvally", "Silvally", "Silvally", "Silvally", "Silvally", "Silvally", "Silvally", "Silvally", "Silvally", "Silvally", "Silvally", "Minior", "Minior", "Minior", "Minior", "Minior", "Minior", "Minior", "Minior", "Minior", "Minior", "Minior", "Minior", "Minior", "Minior", "Komala", "Turtonator", "Togedemaru", "Mimikyu", "Mimikyu", "Bruxish", "Drampa", "Dhelmise", "Jangmo-o", "Hakamo-o", "Kommo-o", "Tapu Koko", "Tapu Lele", "Tapu Bulu", "Tapu Fini", "Cosmog", "Cosmoem", "Solgaleo", "Lunala", "Nihilego", "Buzzwole", "Pheromosa", "Xurkitree", "Celesteela", "Kartana", "Guzzlord", "Necrozma", "Necrozma", "Necrozma", "Necrozma", "Magearna", "Magearna", "Marshadow", "Poipole", "Naganadel", "Stakataka", "Blacephalon", "Zeraora", "Meltan", "Melmetal", "Grookey", "Thwackey", "Rillaboom", "Scorbunny", "Raboot", "Cinderace", "Sobble", "Drizzile", "Inteleon", "Skwovet", "Greedent", "Rookidee", "Corvisquire", "Corviknight", "Blipbug", "Dottler", "Orbeetle", "Nickit", "Thievul", "Gossifleur", "Eldegoss", "Wooloo", "Dubwool", "Chewtle", "Drednaw", "Yamper", "Boltund", "Rolycoly", "Carkol", "Coalossal", "Applin", "Flapple", "Appletun", "Silicobra", "Sandaconda", "Cramorant", "Cramorant", "Cramorant", "Arrokuda", "Barraskewda", "Toxel", "Toxtricity", "Toxtricity", "Sizzlipede", "Centiskorch", "Clobbopus", "Grapploct", "Sinistea", "Sinistea", "Polteageist", "Polteageist", "Hatenna", "Hattrem", "Hatterene", "Impidimp", "Morgrem", "Grimmsnarl", "Obstagoon", "Perrserker", "Cursola", "Sirfetch’d", "Mr. Rime", "Runerigus", "Milcery", "Alcremie", "Alcremie", "Alcremie", "Alcremie", "Alcremie", "Alcremie", "Alcremie", "Alcremie", "Alcremie", "Falinks", "Pincurchin", "Snom", "Frosmoth", "Stonjourner", "Eiscue", "Eiscue", "Indeedee", "Indeedee", "Morpeko", "Morpeko", "Cufant", "Copperajah", "Dracozolt", "Arctozolt", "Dracovish", "Arctovish", "Duraludon", "Dreepy", "Drakloak", "Dragapult", "Zacian", "Zacian", "Zamazenta", "Zamazenta", "Eternatus", "Eternatus", "Kubfu", "Urshifu", "Urshifu", "Zarude", "Zarude", "Regieleki", "Regidrago", "Glastrier", "Spectrier", "Calyrex", "Calyrex", "Calyrex", "Wyrdeer", "Kleavor", "Ursaluna", "Bloodmoon Ursaluna", "Basculegion", "Basculegion", "Sneasler", "Overqwil", "Enamorus", "Enamorus", "Sprigatito", "Floragato", "Meowscarada", "Fuecoco", "Crocalor", "Skeledirge", "Quaxly", "Quaxwell", "Quaquaval", "Lechonk", "Oinkologne", "Oinkologne", "Dudunsparce", "Dudunsparce", "Tarountula", "Spidops", "Nymble", "Lokix", "Rellor", "Rabsca", "Greavard", "Houndstone", "Flittle", "Espathra", "Farigiraf", "Wiglett", "Wugtrio", "Dondozo", "Veluza", "Finizen", "Palafin", "Palafin", "Smoliv", "Dolliv", "Arboliva", "Capsakid", "Scovillain", "Tadbulb", "Bellibolt", "Varoom", "Revavroom", "Orthworm", "Tandemaus", "Maushold", "Maushold", "Cetoddle", "Cetitan", "Frigibax", "Arctibax", "Baxcalibur", "Tatsugiri", "Tatsugiri", "Tatsugiri", "Cyclizar", "Pawmi", "Pawmo", "Pawmot", "Wattrel", "Kilowattrel", "Bombirdier", "Squawkabilly", "Squawkabilly", "Squawkabilly", "Squawkabilly", "Flamigo", "Klawf", "Nacli", "Naclstack", "Garganacl", "Glimmet", "Glimmora", "Shroodle", "Grafaiai", "Fidough", "Dachsbun", "Maschiff", "Mabosstiff", "Bramblin", "Brambleghast", "Gimmighoul", "Gimmighoul", "Gholdengo", "Great Tusk", "Brute Bonnet", "Walking Wake", "Sandy Shocks", "Scream Tail", "Flutter Mane", "Slither Wing", "Roaring Moon", "Iron Treads", "Iron Leaves", "Iron Moth", "Iron Hands", "Iron Jugulis", "Iron Thorns", "Iron Bundle", "Iron Valiant", "Ting-Lu", "Chien-Pao", "Wo-Chien", "Chi-Yu", "Koraidon", "Koraidon", "Koraidon", "Koraidon", "Koraidon", "Miraidon", "Miraidon", "Miraidon", "Miraidon", "Miraidon", "Tinkatink", "Tinkatuff", "Tinkaton", "Charcadet", "Armarouge", "Ceruledge", "Toedscool", "Toedscruel", "Kingambit", "Clodsire", "Annihilape", "Ogerpon", "Ogerpon Wellspring", "Ogerpon Hearthflame", "Ogerpon Cornerstone" , "Ogerpon", "Ogerpon Wellspring", "Ogerpon Hearthflame", "Ogerpon Cornerstone", "Dipplin", "Hydrapple", "Okidogi", "Munkidori", "Fezandipiti", "Gouging Fire", "Raging Bolt", "Iron Crown", "Iron Boulder", "Terapagos", "Terapagos", "Terapagos", "Pecharunt", "Archaludon", "Poltchageist", "Poltchageist", "Sinistcha", "Sinistcha" };
        public bool ComingBack { get; set; } = false;
        private PokeParamsToSend paramsFromThing { get; set; } = null;

        private ObservableCollection<ButtonModel> _imageButtons;

        public ObservableCollection<ButtonModel> ImageButtons
        {
            get => _imageButtons;
            set
            {
                _imageButtons = value;
                OnPropertyChanged();
            }
        }
        public ObservableCollection<ButtonModel> FilteredButtons { get; set; }
        private Config configVals;

        public SelectorViewModel(INavigationService navService)
        {
            NavigationService = navService;
            NavigationService.NavigatedToViewModel += OnNavigatedToViewModel;
            ImageButtons = new ObservableCollection<ButtonModel>();
            var configLocation = Path.Combine(Path.GetDirectoryName(Assembly.GetExecutingAssembly().Location), "config.json");
            using (var r = new StreamReader(configLocation))
            {
                var conf = r.ReadToEnd();
                configVals = JsonSerializer.Deserialize<Config>(conf);
            }
            FillButtonList();
        }

        private void OnNavigatedToViewModel(object sender, Type viewModelType)
        {
            if (viewModelType == typeof(SelectorViewModel))
            {
                try
                {
                    InstantiateArrays();
                } catch
                {
                    throw;
                }
            }
        }

        private void FillButtonList()
        {
            for (int num = 1; num < pictureNames.Count + 1; num++)
            {
                try
                {
                    var image = new System.Windows.Controls.Image();
                    image.Source = new BitmapImage(new Uri($"pack://application:,,,/Assets/Sprites/{pictureNames[num - 1]}.png"));
                    image.HorizontalAlignment = HorizontalAlignment.Center;
                    StackPanel panel = new();
                    panel.Height = 105;
                    panel.Width = 90;
                    panel.Children.Add(image);
                    TextBlock tb = new();
                    tb.Text = buttonNames[num - 1];
                    tb.HorizontalAlignment = HorizontalAlignment.Center;
                    tb.FontFamily = (System.Windows.Media.FontFamily)Application.Current.FindResource("Gill");
                    tb.Foreground = new SolidColorBrush(System.Windows.Media.Color.FromArgb(255,255,255,255));
                    panel.Children.Add(tb);
                    var monName = pictureNames[num-1] == "yanmega" || pictureNames[num-1] == "meganium" || pictureNames[num-1] == "mew" ? pictureNames[num-1] : pictureNames[num - 1].Replace("fire", "").Replace("water", "").Replace("alola", "").Replace("galar", "").Replace("paldea", "").Replace("megax", "").Replace("megay", "").Replace("mega", "").Replace("hisui", "").Replace("origin", "").Replace("primal", "").Replace("-wellspring", "").Replace("-hearthflame", "").Replace("-cornerstone", "").Replace("-bloodmoon", "").Replace("therian", "");
                    int formNum1 = 0;
                    for (int num2 = 0; num2 < num - 1; num2 ++)
                    {
                        if (pictureNames[num2].Contains(monName) && monName != "mew" && monName != "porygon" && monName != "porygon2" && monName != "porygonz" && monName != "pidgeotto")
                        {
                            formNum1++;
                        }
                    }
                    int currentNum = num;
                    int formNum = formNum1;
                    ImageButtons.Add(new ButtonModel(panel, new RelayCommand(o => { NavigationService.NavigateTo<PokeEditorViewModel>(new SelectorParamsToSend { PokeNum = currentNum, FormNum = formNum, Personal = _personal, Plib = _plib, PokeData = _pdata, PokeName = buttonNames[currentNum - 1] }); }, o => true)));
                }
                catch (Exception ex)
                {
                    Debug.WriteLine(ex);
                }
            }
            FilteredButtons = new ObservableCollection<ButtonModel>(ImageButtons);
        }

        private void InstantiateArrays()
        {
            try
            {
                var personalFile = File.Exists(Path.Combine(configVals.outPath, "personal_array.json")) ? File.Open(Path.Combine(configVals.outPath, "personal_array.json"), FileMode.Open) : Application.GetResourceStream(new Uri($"pack://application:,,,/Assets/JSON/personal_array.json")).Stream;
                var plibFile = File.Exists(Path.Combine(configVals.outPath, "plib_item_conversion_array.json")) ? File.Open(Path.Combine(configVals.outPath, "plib_item_conversion_array.json"), FileMode.Open) : Application.GetResourceStream(new Uri($"pack://application:,,,/Assets/JSON/plib_item_conversion_array.json")).Stream;
                var pokeDataFile = File.Exists(Path.Combine(configVals.outPath, "pokedata_array.json")) ? File.Open(Path.Combine(configVals.outPath, "pokedata_array.json"), FileMode.Open) : Application.GetResourceStream(new Uri($"pack://application:,,,/Assets/JSON/pokedata_array.json")).Stream;
                using var personalReader = new StreamReader(personalFile);
                using var plibReader = new StreamReader(plibFile);
                using var pdataReader = new StreamReader(pokeDataFile);
                var personalJson = personalReader.ReadToEnd();
                var plibJson = plibReader.ReadToEnd();
                var pdataJson = pdataReader.ReadToEnd();

                _personal = JsonSerializer.Deserialize<Personal.PersonalArray>(personalJson);
                _plib = JsonSerializer.Deserialize<Plib.PlibArray>(plibJson);
                _pdata = JsonSerializer.Deserialize<PokeData.DataArray>(pdataJson);
                Application.Current.Properties["plib"] = _plib;

                foreach (var x in _personal.entry)
                {
                    // go through everything that can have a -1 value and set it to 0 just to be completely sure its fine
                    if (x.ability_1 == -1) x.ability_1 = 0;
                    if (x.ability_2 == -1) x.ability_2 = 0;
                    if (x.ability_hidden == -1) x.ability_hidden = 0;
                    if (x.type_1 == -1) x.type_1 = 0;
                    if (x.type_2 == -1) x.type_2 = 0;

                    foreach (var y in x.evo_data)
                    {
                        if (y.parameter == -1) y.parameter = 0;
                        if (y.condition == -1) y.condition = 0;
                        if (y.species == -1) y.species = 0;
                    }
                    foreach (var y in x.levelup_moves)
                    {
                        if (y.move == -1) y.move = 0;
                    }

                    x.tm_moves.RemoveAll(y => y == -1);
                }
            } catch (Exception ex)
            {
                throw ex;
            }
        }

        private void Filter()
        {
            FilteredButtons.Clear();
            foreach (var x in ImageButtons)
            {
                TextBlock tb = (TextBlock)x.Content.Children[1];
                if (tb.Text.ToLower().Contains(SearchText.ToLower()))
                {
                    FilteredButtons.Add(x);
                }
            }
        }

        public async void Exit()
        {
            // create bins
            if (File.Exists(Path.Combine(configVals.outPath, "personal_array.json")))
            {
                var flatcExe = Application.GetResourceStream(new Uri($"pack://application:,,,/Assets/Flatc/flatc.exe")).Stream;
                var personalFBS = Application.GetResourceStream(new Uri($"pack://application:,,,/Assets/Flatc/personal_array.fbs")).Stream;
                var plibBFBS = Application.GetResourceStream(new Uri($"pack://application:,,,/Assets/Flatc/plib_item_conversion_array.bfbs")).Stream;
                var pdataBFBS = Application.GetResourceStream(new Uri($"pack://application:,,,/Assets/Flatc/pokedata_array.bfbs")).Stream;

                var tempExePath = Path.Combine(configVals.outPath, "flatc.exe");
                var personalFBSPath = Path.Combine(configVals.outPath, "personal_array.fbs");
                var plibBFBSPath = Path.Combine(configVals.outPath, "plib_item_conversion_array.bfbs");
                var pdataBFBSPath = Path.Combine(configVals.outPath, "pokedata_array.bfbs");

                if (File.Exists(personalFBSPath))
                {
                    File.Delete(personalFBSPath);
                }
                if (File.Exists(tempExePath))
                {
                    File.Delete(tempExePath);
                }
                if (File.Exists(plibBFBSPath))
                {
                    File.Delete(plibBFBSPath);
                }
                if (File.Exists(pdataBFBSPath))
                {
                    File.Delete(pdataBFBSPath);
                }

                byte[] exebytes = new byte[(int)flatcExe.Length];
                byte[] pbytes = new byte[(int)personalFBS.Length];
                byte[] plbytes = new byte[(int)plibBFBS.Length];
                byte[] pdbytes = new byte[(int)pdataBFBS.Length];

                flatcExe.Read(exebytes, 0, exebytes.Length);
                personalFBS.Read(pbytes, 0, pbytes.Length);
                plibBFBS.Read(plbytes, 0, plbytes.Length);
                pdataBFBS.Read(pdbytes, 0, pdbytes.Length);

                File.WriteAllBytesAsync(tempExePath, exebytes);
                File.WriteAllBytesAsync(personalFBSPath, pbytes);
                File.WriteAllBytesAsync(plibBFBSPath, plbytes);
                File.WriteAllBytesAsync(pdataBFBSPath, pdbytes);

                var pcmd = Cli.Wrap(tempExePath).WithArguments("-o romfs/avalon/data/ -b personal_array.fbs personal_array.json").WithWorkingDirectory(configVals.outPath);
                var plcmd = Cli.Wrap(tempExePath).WithArguments("-o romfs/world/data/battle/plib_item_conversion/ -b plib_item_conversion_array.bfbs plib_item_conversion_array.json").WithWorkingDirectory(configVals.outPath);
                var pdcmd = Cli.Wrap(tempExePath).WithArguments("-o romfs/world/data/encount/pokedata/pokedata/ -b pokedata_array.bfbs pokedata_array.json").WithWorkingDirectory(configVals.outPath);

                if (File.Exists(Path.Combine(configVals.outPath, "personal_array.json")))
                {
                    await pcmd.ExecuteBufferedAsync();
                }

                if (File.Exists(Path.Combine(configVals.outPath, "plib_item_conversion_array.json")))
                {
                    await plcmd.ExecuteBufferedAsync();
                }

                if (File.Exists(Path.Combine(configVals.outPath, "pokedata_array.json")))
                {
                    await pdcmd.ExecuteBufferedAsync();
                }

                File.Delete(tempExePath);
                File.Delete(personalFBSPath);
                File.Delete(plibBFBSPath);
                File.Delete(pdataBFBSPath);

                var zipDirectories = Path.Combine(configVals.outPath, "romfs/");
                var zipPath = Path.Combine(configVals.outPath, "project_sky_pokemon_mod.zip");

                if (File.Exists(zipPath))
                {
                    File.Delete(zipPath);
                }

                ZipFile.CreateFromDirectory(zipDirectories, zipPath);
            }
        }
    }
}
