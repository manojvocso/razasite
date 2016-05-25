using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace MvcApplication1.Controllers
{
    public class FlagDictonary
    {

        public static IDictionary<int,string> CreateDictinary()
        {
            string pre = "/images/";
            IDictionary<int, string> flagdict = new Dictionary<int, string>();
            flagdict.Add(1, pre + "af-icon.png"); //Afghanistan
            flagdict.Add(387, pre + "alaska.png"); // Alaska
            flagdict.Add(2, pre + "albania.png"); // Albania
            flagdict.Add(3, pre + "algeria.png"); // Algeria
            flagdict.Add(4, pre + "american_samoa.png"); // American Samoa
            flagdict.Add(5, pre + "andorra.png");   // Andorra
            flagdict.Add(6, pre + "angola.png"); // Angola
            flagdict.Add(7, pre + "anguilla.png"); // Anguilla
            flagdict.Add(8, pre + "antarctica.png"); // Antarctica
            flagdict.Add(9, pre + "antigua-barbuda.png"); // image for Antigua Barbuda
            flagdict.Add(10, pre + "argentina.png"); // Argentina
            flagdict.Add(11, pre + "argentina.png");
            flagdict.Add(12, pre + "argentina.png");
            flagdict.Add(16, pre + "argentina.png");
            flagdict.Add(390, pre + "argentina.png"); // Argentina Mendoza
            flagdict.Add(15, pre + "argentina.png");
            flagdict.Add(17, pre + "armenia.png"); // Armenia
            flagdict.Add(497, pre + "armenia.png"); // Armenia Yerevan
            flagdict.Add(18, pre + "aruba.png");  // Aruba
            flagdict.Add(371, pre + "aruba.png");
            flagdict.Add(498, pre + "ascension-island_flag.png");  // image for ascension island

            flagdict.Add(19, pre + "australia.png"); //Australia
            flagdict.Add(22, pre + "austria.png"); //Austria

            flagdict.Add(24, pre + "azerbaijan.png"); // => Azerbaijan
            flagdict.Add(25, pre + "bahamas.png"); // => Bahamas
            flagdict.Add(26, pre + "bahrain.png"); //=> Bahrain
            flagdict.Add(27, pre + "bangladesh.png"); //=> Bangladesh
            flagdict.Add(33, pre + "barbados.png"); //=> Barbados
            flagdict.Add(34, pre + "belarus.png"); // => Belarus
            flagdict.Add(35, pre + "belgium.png"); // => Belgium
            flagdict.Add(36, pre + "belize.png"); // => Belize
            flagdict.Add(37, pre + "benin.png");  //37 => Benin
            flagdict.Add(38, pre + "bermuda.png"); //38 => Bermuda
            flagdict.Add(39, pre + "bhutan_flag.png"); //39 => Bhutan
            flagdict.Add(40, pre + "bolivia.png"); //40 => Bolivia
            flagdict.Add(41, pre + "bosnia.png"); //41 => Bosnia and Herzegovina
            flagdict.Add(42, pre + "botswana.png"); //42 => Botswana
            flagdict.Add(43, pre + "brazil.png"); //43 => Brazil
            flagdict.Add(47, pre + "british_island.png"); //47 => British Virgin Islands
            flagdict.Add(48, pre + "brunei.png"); //48 => Brunei
            flagdict.Add(49, pre + "bulgaria.png"); //49 => Bulgaria
            flagdict.Add(54, pre + "burundi.png"); //54 => Burundi
            flagdict.Add(55, pre + "cambodja.png"); //55 => Cambodia
            flagdict.Add(56, pre + "cameroon.png"); //56 => Cameroon
            flagdict.Add(57, pre + "canada.png"); //57 => Canada
            flagdict.Add(58, pre + "cape-verde.png"); //58 => Cape Verde Islands
            flagdict.Add(59, pre + "cayman-islands.png"); //59 => Cayman Islands
            flagdict.Add(60, pre + "central-african-republic.png"); //60 => Central African Rep
            flagdict.Add(62, pre + "chad.png"); //62 => Chad Republic
            flagdict.Add(63, pre + "chile.png"); //63 => Chile
            flagdict.Add(64, pre + "china.png"); //64 => China
            flagdict.Add(404, pre + "christmas_island.png"); //404 => Christmas Islands
            flagdict.Add(345, pre + "cocos.png"); //345 => Cocos Islands
            flagdict.Add(67, pre + "colombia.png"); //67 => Colombia
            flagdict.Add(68, pre + "comoros.png"); //68 => Comoros Island
            flagdict.Add(70, pre + "congo-brazzaville.png"); //70 => Congo
            flagdict.Add(71, pre + "cook-islands.png"); //71 => Cook Islands
            flagdict.Add(72, pre + "costa-rica.png"); //72 => Costa Rica
            flagdict.Add(73, pre + "croatia.png"); //73 => Croatia
            flagdict.Add(74, pre + "cuba.png"); //74 => Cuba
            flagdict.Add(75, pre + "cyprus.png"); //75 => Cyprus
            flagdict.Add(381, pre + "czech-republic.png"); //381 => Czech Republic
            flagdict.Add(327, pre + "congo-kinshasa(zaire).png"); //327 => Dem. Rep. Of Congo
            flagdict.Add(80, pre + "denmark.png"); //80 => Denmark
            flagdict.Add(453, pre + "denmark.png"); //453 => Denmark Mobile
            flagdict.Add(81, pre + "christmas_island.png"); //81 => Diego Garcia
            flagdict.Add(82, pre + "djibouti.png"); //82 => Djibouti
            flagdict.Add(83, pre + "dominica.png"); //83 => Dominica
            flagdict.Add(84, pre + "dominican-republic.png"); //84 => Dominican Republic
            flagdict.Add(85, pre + "ecuador_new.png"); //85 => Ecuador
            flagdict.Add(87, pre + "egypt.png"); //87 => Egypt
            flagdict.Add(88, pre + "el-salvador.png"); //88 => El Salvador
            flagdict.Add(552, pre + "equatorial-guinea.png"); //552 => Equatorial Guinea
            flagdict.Add(89, pre + "eritrea.png"); //89 => Eritrea
            flagdict.Add(90, pre + "estonia.png"); //90 => Estonia
            flagdict.Add(91, pre + "ethiopia.png"); //91 => Ethiopia
            flagdict.Add(93, pre + "falkland.png"); //93 => Falkland Islands
            flagdict.Add(92, pre + "faroes.png"); //92 => Faroe Islands
            flagdict.Add(382, pre + "fiji.png"); //382 => Fiji Islands
            flagdict.Add(95, pre + "finland.png"); //95 => Finland
            flagdict.Add(96, pre + "france.png"); //96 => France
            flagdict.Add(99, pre + "flag_netherlandsantillies_t.jpg"); //99 => French Antilles
            flagdict.Add(100, pre + "F-Guiana.jpg"); //100 => French Guiana
            flagdict.Add(101, pre + "pf-flag.png"); //101 => French Polynesia
            flagdict.Add(102, pre + "gabon.png"); //102 => Gabon
            flagdict.Add(103, pre + "gambia.png"); //103 => Gambia
            flagdict.Add(104, pre + "georgia.png"); //104 => Georgia
            flagdict.Add(105, pre + "germany.png"); //105 => Germany
            flagdict.Add(110, pre + "ghana.png"); //110 => Ghana
            flagdict.Add(111, pre + "gibraltar.png"); //111 => Gibraltar
            flagdict.Add(113, pre + "greece.png"); //113 => Greece
            flagdict.Add(115, pre + "greenland.png"); //115 => Greenland
            flagdict.Add(116, pre + "grenada.png"); //116 => Grenada
            flagdict.Add(117, pre + "guadeloupe.png"); //117 => Guadeloupe
            flagdict.Add(118, pre + "guam.png"); //118 => Guam
            flagdict.Add(119, pre + "finland_flag.png"); //119 => Guantanamo Bay
            flagdict.Add(120, pre + "guademala.png"); //120 => Guatemala
            flagdict.Add(122, pre + "guinea.png"); //122 => Guinea
            flagdict.Add(123, pre + "guyana_flag.png"); //123 => Guyana
            flagdict.Add(124, pre + "haiti.png"); //124 => Haiti
            flagdict.Add(411, pre + "hawaii_flag.png"); //411 => Hawaii
            flagdict.Add(125, pre + "honduras.png"); //125 => Honduras
            flagdict.Add(126, pre + "hong-kong.png"); //126 => Hong Kong
            flagdict.Add(128, pre + "hungary.png"); //128 => Hungary
            flagdict.Add(129, pre + "iceland.png"); //129 => Iceland
            flagdict.Add(130, pre + "india.png"); //130 => India
            flagdict.Add(166, pre + "indonesia.png"); //166 => Indonesia
            flagdict.Add(311, pre + "indonesia.png"); //311 => Indonesia Jakarta
            flagdict.Add(167, pre + "iran.png"); //167 => Iran
            flagdict.Add(169, pre + "iraq.png"); //169 => Iraq
            flagdict.Add(170, pre + "ireland.png"); //170 => Ireland
            flagdict.Add(172, pre + "italy.png"); //172 => Italy
            flagdict.Add(175, pre + "ivory_coast.png"); //175 => Ivory Coast
            flagdict.Add(176, pre + "jamaica.png"); //176 => Jamaica
            flagdict.Add(177, pre + "japan.png"); //177 => Japan
            flagdict.Add(178, pre + "jordan.png"); //178 => Jordan
            flagdict.Add(179, pre + "kazakhstan.png"); //179 => Kazakhstan
            flagdict.Add(180, pre + "kenya.png"); //180 => Kenya
            flagdict.Add(414, pre + "kiribati.png"); //414 => Kiribati
            flagdict.Add(670, pre + "kosovo_flag.png"); //670 => Kosovo
            flagdict.Add(185, pre + "kuwait.png"); //185 => Kuwait
            flagdict.Add(186, pre + "kyrgyzstan.png"); //186 => Kyrgyzstan
            flagdict.Add(187, pre + "laos_flag.png"); //187 => Laos
            flagdict.Add(188, pre + "latvia.png"); //188 => Latvia
            flagdict.Add(189, pre + "lebanon.png"); //189 => Lebanon
            flagdict.Add(190, pre + "lesotho.png"); //190 => Lesotho
            flagdict.Add(191, pre + "liberia.png"); //191 => Liberia
            flagdict.Add(192, pre + "libya.png"); //192 => Libya
            flagdict.Add(193, pre + "liechtenstein.png"); //193 => Liechtenstein
            flagdict.Add(194, pre + "lithuania.png"); //194 => Lithuania
            flagdict.Add(195, pre + "luxembourg.png"); //195 => Luxembourg
            flagdict.Add(196, pre + "macao.png"); //196 => Macao
            flagdict.Add(197, pre + "macedonia.png"); //197 => Macedonia
            flagdict.Add(198, pre + "madagascar.png"); //198 => Madagascar
            flagdict.Add(199, pre + "malawi.png"); //199 => Malawi
            flagdict.Add(200, pre + "malaysia.png"); //200 => Malaysia
            flagdict.Add(203, pre + "maldives.png"); //203 => Maldives
            flagdict.Add(204, pre + "mali.png"); //204 => Mali Republic
            flagdict.Add(205, pre + "malta.png"); //205 => Malta
            flagdict.Add(206, pre + "marshall-islands.png"); //206 => Marshall Islands
            flagdict.Add(207, pre + "mauritania.png"); //207 => Mauritania
            flagdict.Add(378, pre + "mauritius.png"); //378 => Mauritius
            flagdict.Add(597, pre + "mayotte-flag.png"); //597 => Mayotte Island
            flagdict.Add(209, pre + "mexico.png"); //209 => Mexico
            flagdict.Add(214, pre + "micronesia.png"); //214 => Micronesia
            flagdict.Add(215, pre + "moldova.png"); //215 => Moldova
            flagdict.Add(216, pre + "monaco.png"); //216 => Monaco
            flagdict.Add(217, pre + "mongolia.png"); //217 => Mongolia
            flagdict.Add(667, pre + "montenegro.png"); //667 => Montenegro
            flagdict.Add(218, pre + "montserrat.png"); //218 => Montserrat
            flagdict.Add(219, pre + "morocco.png"); //219 => Morocco
            flagdict.Add(220, pre + "mozambique.png"); //220 => Mozambique
            flagdict.Add(221, pre + "myanmar.png"); //221 => Myanmar
            flagdict.Add(222, pre + "namibia.png"); //222 => Namibia
            flagdict.Add(223, pre + "nauru.png"); //223 => Nauru
            flagdict.Add(224, pre + "nepal.png"); //224 => Nepal
            flagdict.Add(227, pre + "netherlands.png"); //227 => Netherlands
            flagdict.Add(226, pre + "netherlands-antilles.png"); //226 => Netherlands Antilles
            flagdict.Add(228, pre + "new-caledonia.png"); //228 => New Caledonia
            flagdict.Add(229, pre + "new-zealand.png"); //229 => New Zealand
            flagdict.Add(230, pre + "nicaragua.png"); //230 => Nicaragua
            flagdict.Add(231, pre + "niger.png"); //231 => Niger
            flagdict.Add(232, pre + "nigeria-flag.png"); //232 => Nigeria
            flagdict.Add(235, pre + "niue_flag.png"); //235 => Niue Island
            flagdict.Add(427, pre + "norfolk_flag.png"); //427 => Norfolk Island
            flagdict.Add(428, pre + "north_korea_flag.png"); //428 => North Korea
            flagdict.Add(236, pre + "norway.png"); //236 => Norway
            flagdict.Add(237, pre + "oman.png"); //237 => Oman
            flagdict.Add(238, pre + "pakistan.png"); //238 => Pakistan
            flagdict.Add(242, pre + "palau.png"); //242 => Palau
            flagdict.Add(385, pre + "palestine.png"); //385 => Palestine
            flagdict.Add(243, pre + "panama.png"); //243 => Panama
            flagdict.Add(244, pre + "papua-new-guinea.png"); //244 => Papua New Guinea
            flagdict.Add(245, pre + "paraguay.png"); //245 => Paraguay
            flagdict.Add(246, pre + "peru.png"); //246 => Peru
            flagdict.Add(247, pre + "peru.png"); //247 => Peru Lima
            flagdict.Add(248, pre + "philippines.png"); //248 => Philippines
            flagdict.Add(249, pre + "poland_flag.png"); //249 => Poland
            flagdict.Add(251, pre + "portugal.png"); //251 => Portugal
            flagdict.Add(252, pre + "Portrico.jpg"); //252 => Puerto Rico
            flagdict.Add(253, pre + "qatar.png"); //253 => Qatar
            flagdict.Add(255, pre + "reunion.png"); //255 => Reunion Island
            flagdict.Add(256, pre + "romania.png"); //256 => Romania
            flagdict.Add(257, pre + "romania.png"); //257 => Romania Bucharest
            flagdict.Add(258, pre + "russian-federation.png"); //258 => Russia
            flagdict.Add(260, pre + "rwanda.png"); //260 => Rwanda
            flagdict.Add(250, pre + "saipan.png"); //250 => Saipan
            flagdict.Add(261, pre + "san-marino.png"); //261 => San Marino
            flagdict.Add(627, pre + "sao-tome.png"); //627 => Sao Tome
            flagdict.Add(265, pre + "saudi-arabia.png"); //265 => Saudi Arabia
            flagdict.Add(266, pre + "senegal.png"); //266 => Senegal
            flagdict.Add(267, pre + "serbia.png"); //267 => Serbia
            flagdict.Add(268, pre + "seyshelles.png"); //268 => Seychelles Island
            flagdict.Add(269, pre + "sierra-leone.png"); //269 => Sierra Leone
            flagdict.Add(270, pre + "singapore.png"); //270 => Singapore
            flagdict.Add(271, pre + "slovakia.png"); //271 => Slovakia
            flagdict.Add(272, pre + "slovenia.png"); //272 => Slovenia
            flagdict.Add(273, pre + "solomon-islands.png"); //273 => Solomon Island
            flagdict.Add(274, pre + "somalia.png"); //274 => Somalia
            flagdict.Add(275, pre + "south-afriica.png"); //275 => South Africa
            flagdict.Add(277, pre + "korea_flag.png"); //277 => South Korea
            flagdict.Add(278, pre + "spain.png"); //278 => Spain
            flagdict.Add(279, pre + "spain.png"); //279 => Spain Barcelona
            flagdict.Add(280, pre + "spain.png"); //280 => Spain Madrid
            flagdict.Add(281, pre + "sri-lanka.png"); //281 => Sri Lanka
            flagdict.Add(284, pre + "helena_flag.png"); //284 => St. Helena
            flagdict.Add(285, pre + "kitts_nevis.png"); //285 => St. Kitts & Nevis
            flagdict.Add(286, pre + "lucia_flag.png"); //286 => St. Lucia
            flagdict.Add(287, pre + "finland_flag.png"); //287 => St. Martin
            flagdict.Add(288, pre + "pierre_mequelon_flag.png"); //288 => St. Pierre & Mequelon
            flagdict.Add(379, pre + "vincent_flag.png"); //379 => St. Vincent
            flagdict.Add(289, pre + "sudan.png"); //289 => Sudan
            flagdict.Add(290, pre + "suriname.png"); //290 => Suriname
            flagdict.Add(291, pre + "swaziland.png"); //291 => Swaziland
            flagdict.Add(292, pre + "sweden.png"); //292 => Sweden
            flagdict.Add(293, pre + "sweden.png"); //293 => Sweden Stockholm
            flagdict.Add(294, pre + "switzerland.png"); //294 => Switzerland
            flagdict.Add(295, pre + "syria.png"); //295 => Syria
            flagdict.Add(296, pre + "taiwan.png"); //296 => Taiwan
            flagdict.Add(297, pre + "tajikistan.png"); //297 => Tajikistan
            flagdict.Add(493, pre + "tajikistan.png"); //493 => Tajikistan Vahdat
            flagdict.Add(679, pre + "tanzania.png"); //679 => Tanzania
            flagdict.Add(299, pre + "thailand.png"); //299 => Thailand
            flagdict.Add(301, pre + "togo.png"); //301 => Togo
            flagdict.Add(302, pre + "tonga.png"); //302 => Tonga
            flagdict.Add(303, pre + "trinidad.png"); //303 => Trinidad & Tobago
            flagdict.Add(304, pre + "tunisia.png"); //304 => Tunisia
            flagdict.Add(305, pre + "turkey.png"); //305 => Turkey
            flagdict.Add(307, pre + "turk.png"); //307 => Turkmenistan
            flagdict.Add(308, pre + "tuvalu.png"); //308 => Tuvalu
            flagdict.Add(315, pre + "uganda.png"); //315 => Uganda
            flagdict.Add(316, pre + "ukraine.png"); //316 => Ukraine
            flagdict.Add(309, pre + "united-arab-emirates.png"); //309 => United Arab Emirates
            flagdict.Add(312, pre + "united-Kingdom_flat.png"); //312 => United Kingdom
            flagdict.Add(317, pre + "uruguay.png"); //317 => Uruguay
            flagdict.Add(313, pre + "virgin-islands-british.png"); //313 => US Virgin Islands
            flagdict.Add(314, pre + "us.png"); //314 => USA
            flagdict.Add(318, pre + "uzbekistan.png"); //318 => Uzbekistan
            flagdict.Add(319, pre + "vanutau.png"); //319 => Vanuatu
            flagdict.Add(320, pre + "vatican.png"); //320 => Vatican City
            flagdict.Add(321, pre + "venezuela.png"); //321 => Venezuela
            flagdict.Add(323, pre + "viet-nam.png"); //323 => Vietnam
            flagdict.Add(663, pre + "wales.png"); //663 => Wallis & Futuna
            flagdict.Add(324, pre + "west_samoa.png"); //324 => West Samoa
            flagdict.Add(325, pre + "yemen.png"); //325 => Yemen
            flagdict.Add(326, pre + "yugoslavia.png"); //326 => Yugoslavia
            flagdict.Add(328, pre + "zambia.png"); //328 => Zambia
            flagdict.Add(448, pre + "zimbabwe.png"); //448 => Zimbabwe


            return flagdict;
        }


        public static IDictionary<string, string> GetCurrencycode()
        {
            var currencyCode = new Dictionary<string, string> {{"USD", "$"}, {"CAD", "$"}, {"GBP", "£"}};

            return currencyCode;
        }


        public static IDictionary<int, string> RatePerMinSign()
        {
            var currencySign = new Dictionary<int, string> { { 1, "¢/min" }, { 2, "¢/min" }, { 3, "p/min" } };

            return currencySign;
        }
       
        
        public static IDictionary<int, string> GetCurrencycodebyCountry()
        {
            var currencyCode = new Dictionary<int, string> {{1, "USD"}, {2, "CAD"}, {3, "GBP"}};

            return currencyCode;
        }


        public static IDictionary<int, string> GetHomePageCountryFlag()
        {
            const string pre = "/images/";
            IDictionary<int, string> flagDict = new Dictionary<int, string>();

            flagDict.Add(1, pre + "afganistan.png");
            flagDict.Add(27, pre + "ban-icon.jpg");
            flagDict.Add(57, pre + "can-icon.jpg");
            flagDict.Add(87, pre + "egy-icon.jpg");
            flagDict.Add(110, pre + "gha-icon.jpg");
            flagDict.Add(130, pre + "ind-icon.jpg");
            flagDict.Add(178, pre + "jor-icon.jpg");
            flagDict.Add(180, pre + "key-icon.jpg");
            flagDict.Add(224, pre + "nep-icon.jpg");
            flagDict.Add(232, pre + "neg-icon.jpg");
            flagDict.Add(238, pre + "pak-icon.jpg");
            flagDict.Add(248, pre + "philipens.png");
            flagDict.Add(265, pre + "arab.png");
            flagDict.Add(281, pre + "sri1.png");
            flagDict.Add(309, pre + "uae1.png");
            flagDict.Add(312, pre + "shutterstock_128190683.png");

            return flagDict;
        }


        public static IDictionary<string, string> FlagDictonaryforRate()
        {
           // string pre = "/images/";
            IDictionary<string, string> flagdict = new Dictionary<string, string>();
            flagdict.Add("1", "/images/af-icon.png"); // Afghanistan (93)
            flagdict.Add("347", "/images/af-icon.png"); // Afghanistan Mobile (93)
            flagdict.Add("387", "/images/alaska.png"); // Alaska (1)
            flagdict.Add("2", "/images/albania.png"); // Albania (355)
            flagdict.Add("335", "/images/albania.png"); // Albania Mobile (355)
            flagdict.Add("681", "/images/albania.png"); // Albania Mobile - Vodafone (355)
            flagdict.Add("682", "/images/albania.png"); // Albania Olo (355)
            flagdict.Add("3", "/images/algeria.png"); // Algeria (213)
            flagdict.Add("386", "/images/algeria.png"); // Algeria mobile (213)
            flagdict.Add("683", "/images/algeria.png"); // Algeria Mobile - Mobilis (213)
            flagdict.Add("684", "/images/algeria.png"); // Algeria Mobile - MPTA (213)
            flagdict.Add("685", "/images/algeria.png"); // Algeria Mobile - Orascom (213)
            flagdict.Add("686", "/images/algeria.png"); // Algeria Mobile - Wataniya (213)
            flagdict.Add("4", "/images/american_samoa.png"); // American Samoa (684)
            flagdict.Add("336", "/images/american_samoa.png"); // American Samoa Mobile (684)
            flagdict.Add("5", "/images/andorra.png"); // Andorra (376)
            flagdict.Add("388", "/images/andorra.png"); // Andorra Mobile (376)
            flagdict.Add("6", "/images/angola.png"); // Angola (244)
            flagdict.Add("337", "/images/angola.png"); // Angola Mobile (244)
            flagdict.Add("7", "/images/anguilla.png"); // Anguilla (1-264)
            flagdict.Add("389", "/images/anguilla.png"); // Anguilla Mobile (1-264)
            flagdict.Add("687", "/images/anguilla.png"); // Anguilla Mobile - Digicel (1-264)
            flagdict.Add("8", "/images/antarctica.png"); // Antarctica (672-12)
            flagdict.Add("496", "/images/antarctica.png"); // Antarctica Mobile (672-12)
            flagdict.Add("9", "/images/antigua-barbuda.png"); // Antigua Barbuda (1-268)
            flagdict.Add("333", "/images/antigua-barbuda.png"); // Antigua Barbuda Mobile (1-268)
            flagdict.Add("10", "/images/argentina.png"); // Argentina (54)
            flagdict.Add("12", "/images/argentina.png"); // Argentina Buenos Aires (54)
            flagdict.Add("11", "/images/argentina.png"); // Argentina Cordoba (54)
            flagdict.Add("16", "/images/argentina.png"); // Argentina Del Plata (54)
            flagdict.Add("13", "/images/argentina.png"); // Argentina La Plata (54)
            flagdict.Add("390", "/images/argentina.png"); // Argentina Mendoza (54)
            flagdict.Add("391", "/images/argentina.png"); // Argentina Mobile (54)
            flagdict.Add("15", "/images/argentina.png"); // Argentina Rosario (54)
            flagdict.Add("17", "/images/armenia.png"); // Armenia (374)
            flagdict.Add("680", "/images/armenia.png"); // Armenia Karabakh (374)
            flagdict.Add("338", "/images/armenia.png"); // Armenia Mobile (374)
            flagdict.Add("688", "/images/armenia.png"); // Armenia Mobile - Armentel (374)
            flagdict.Add("689", "/images/armenia.png"); // Armenia Mobile - Karabakh (374)
            flagdict.Add("690", "/images/armenia.png"); // Armenia Mobile - Orange (374)
            flagdict.Add("691", "/images/armenia.png"); // Armenia Mobile - Vivacell (374)
            flagdict.Add("497", "/images/armenia.png"); // Armenia Yerevan (374)
            flagdict.Add("18", "/images/aruba.png"); // Aruba (297)
            flagdict.Add("371", "/images/aruba.png"); // Aruba Mobile (297)
            flagdict.Add("498", "/images/ascension-island_flag.png"); // Ascension Island (247)
            flagdict.Add("499", "/images/ascension-island_flag.png"); // Ascension Island Mobile (247)
            flagdict.Add("19", "/images/australia.png"); // Australia (61)
            flagdict.Add("500", "/images/australia.png"); // Australia Melbourne (61)
            flagdict.Add("20", "/images/australia.png"); // Australia Mobile (61)
            flagdict.Add("692", "/images/australia.png"); // Australia Satellite (61)
            flagdict.Add("693", "/images/australia.png"); // Australia Special Services (61)
            flagdict.Add("339", "/images/australia.png"); // Australia Sydney (61)
            flagdict.Add("694", "/images/australia.png"); // Australia Territories 672 (672)
            flagdict.Add("22", "/images/austria.png"); // Austria (43)
            flagdict.Add("392", "/images/austria.png"); // Austria Mobile (43)
            flagdict.Add("695", "/images/austria.png"); // Austria Mobile - H3G (43)
            flagdict.Add("696", "/images/austria.png"); // Austria Mobile - Mobilkom (43)
            flagdict.Add("697", "/images/austria.png"); // Austria Mobile - One (43)
            flagdict.Add("698", "/images/austria.png"); // Austria Mobile - Special Services (43)
            flagdict.Add("699", "/images/austria.png"); // Austria Mobile - Telering (43)
            flagdict.Add("23", "/images/austria.png"); // Austria Vienna (43)
            flagdict.Add("24", "/images/azerbaijan.png"); // Azerbaijan (994)
            flagdict.Add("393", "/images/azerbaijan.png"); // Azerbaijan Mobile (994)
            flagdict.Add("25", "/images/bahamas.png"); // Bahamas (1-284)
            flagdict.Add("394", "/images/bahamas.png"); // Bahamas Mobile (1-242)
            flagdict.Add("26", "/images/bahrain.png"); // Bahrain (973)
            flagdict.Add("340", "/images/bahrain.png"); // Bahrain Mobile (973)
            flagdict.Add("700", "/images/bahrain.png"); // Bahrain Mobile - Stc (973)
            flagdict.Add("701", "/images/bahrain.png"); // Bahrain Mobile - Stc (973)
            flagdict.Add("27", "/images/bangladesh.png"); // Bangladesh (880)
            flagdict.Add("30", "/images/bangladesh.png"); // Bangladesh Chittagong (880)
            flagdict.Add("31", "/images/bangladesh.png"); // Bangladesh Dhaka (880)
            flagdict.Add("501", "/images/bangladesh.png"); // Bangladesh Khulna (880)
            flagdict.Add("29", "/images/bangladesh.png"); // Bangladesh Mobile (880)
            flagdict.Add("702", "/images/bangladesh.png"); // Bangladesh Mobile - Grameen (880)
            flagdict.Add("28", "/images/bangladesh.png"); // Bangladesh Sylhet (880)
            flagdict.Add("33", "/images/barbados.png"); // Barbados (1-246)
            flagdict.Add("395", "/images/barbados.png"); // Barbados Mobile (1-246)
            flagdict.Add("34", "/images/belarus.png"); // Belarus (375)
            flagdict.Add("396", "/images/belarus.png"); // Belarus Mobile (375)
            flagdict.Add("703", "/images/belarus.png"); // Belarus Telematic (375)
            flagdict.Add("35", "/images/belgium.png"); // Belgium (32)
            flagdict.Add("502", "/images/belgium.png"); // Belgium Antwerp (32)
            flagdict.Add("503", "/images/belgium.png"); // Belgium Brussels (32)
            flagdict.Add("341", "/images/belgium.png"); // Belgium Mobile (32)
            flagdict.Add("704", "/images/belgium.png"); // Belgium Mobile - Base (32)
            flagdict.Add("705", "/images/belgium.png"); // Belgium Mobile - Mobistar (32)
            flagdict.Add("706", "/images/belgium.png"); // Belgium Mobile - Proximus (32)
            flagdict.Add("36", "/images/belize.png"); // Belize (501)
            flagdict.Add("397", "/images/belize.png"); // Belize Mobile (501)
            flagdict.Add("37", "/images/benin.png"); // Benin (229)
            flagdict.Add("459", "/images/benin.png"); // Benin Mobile (229)
            flagdict.Add("38", "/images/bermuda.png"); // Bermuda (1-441)
            flagdict.Add("398", "/images/bermuda.png"); // Bermuda Mobile (1-441)
            flagdict.Add("39", "/images/bhutan_flag.png"); // Bhutan (975)
            flagdict.Add("504", "/images/bhutan_flag.png"); // Bhutan Mobile (975)
            flagdict.Add("40", "/images/bolivia.png"); // Bolivia (591)
            flagdict.Add("505", "/images/bolivia.png"); // Bolivia Cochabamba (591)
            flagdict.Add("506", "/images/bolivia.png"); // Bolivia La Paz (591)
            flagdict.Add("342", "/images/bolivia.png"); // Bolivia Mobile (591)
            flagdict.Add("707", "/images/bolivia.png"); // Bolivia Oruro (591)
            flagdict.Add("708", "/images/bolivia.png"); // Bolivia Potosi (591)
            flagdict.Add("711", "/images/bolivia.png"); // Bolivia Potosi (591)
            flagdict.Add("507", "/images/bolivia.png"); // Bolivia Santa Cruz (591)
            flagdict.Add("709", "/images/bolivia.png"); // Bolivia Sucre (591)
            flagdict.Add("710", "/images/bolivia.png"); // Bolivia Tarija (591)
            flagdict.Add("712", "/images/bosnia.png"); // Bosnia & Herzegovina Sarajevo (387)
            flagdict.Add("41", "/images/bosnia.png"); // Bosnia and Herzegovina (387)
            flagdict.Add("508", "/images/bosnia.png"); // Bosnia and Herzegovina Mobile (387)
            flagdict.Add("42", "/images/botswana.png"); // Botswana (267)
            flagdict.Add("509", "/images/botswana.png"); // Botswana Mobile (267)
            flagdict.Add("43", "/images/brazil.png"); // Brazil (55)
            flagdict.Add("46", "/images/brazil.png"); // Brazil Belo Horizonte (55)
            flagdict.Add("510", "/images/brazil.png"); // Brazil Brasilia (55)
            flagdict.Add("511", "/images/brazil.png"); // Brazil Campinas (55)
            flagdict.Add("512", "/images/brazil.png"); // Brazil Curitibia (55)
            flagdict.Add("513", "/images/brazil.png"); // Brazil Goiania (55)
            flagdict.Add("451", "/images/brazil.png"); // Brazil Mobile (55)
            flagdict.Add("713", "/images/brazil.png"); // Brazil Mobile - Claro (55)
            flagdict.Add("714", "/images/brazil.png"); // Brazil Mobile - Tim (55)
            flagdict.Add("514", "/images/brazil.png"); // Brazil Recife (55)
            flagdict.Add("44", "/images/brazil.png"); // Brazil Rio De Janeiro (55)
            flagdict.Add("45", "/images/brazil.png"); // Brazil Sao Paulo (55)
            flagdict.Add("515", "/images/brazil.png"); // Brazil Vitoria (55)
            flagdict.Add("47", "/images/bahamas.png"); // British Virgin Islands (1-284)
            flagdict.Add("399", "/images/bahamas.png"); // British Virgin Islands Mobile (1-284)
            flagdict.Add("715", "/images/bahamas.png"); // British Virgin Islands Mobile - CWBV (1-284)
            flagdict.Add("716", "/images/bahamas.png"); // British Virgin Islands Mobile - Digicel (1-284)
            flagdict.Add("717", "/images/bahamas.png"); // British Virgin Islands Mobile - Mio (1-284)
            flagdict.Add("48", "/images/brunei.png"); // Brunei (673)
            flagdict.Add("516", "/images/brunei.png"); // Brunei Mobile (673)
            flagdict.Add("49", "/images/bulgaria.png"); // Bulgaria (359)
            flagdict.Add("50", "/images/bulgaria.png"); // Bulgaria Mobile (359)
            flagdict.Add("718", "/images/bulgaria.png"); // Bulgaria Mobile - Btc (359)
            flagdict.Add("719", "/images/bulgaria.png"); // Bulgaria Mobile - Globul (359)
            flagdict.Add("720", "/images/bulgaria.png"); // Bulgaria Mobile - Mobikom (359)
            flagdict.Add("721", "/images/bulgaria.png"); // Bulgaria Mobile - Mobiltel (359)
            flagdict.Add("722", "/images/bulgaria.png"); // Bulgaria Mobile - Tetra (359)
            flagdict.Add("723", "/images/bulgaria.png"); // Bulgaria Mobile - Wimax (359)
            flagdict.Add("51", "/images/bulgaria.png"); // Bulgaria Sofia (359)
            flagdict.Add("724", "/images/bulgaria.png"); // Bulgaria Special Services (359)
            flagdict.Add("52", "/images/Burkina-Faso.png"); // Burkina Faso (226)
            flagdict.Add("517", "/images/Burkina-Faso.png"); // Burkina Faso Mobile (226)
            flagdict.Add("54", "/images/burundi.png"); // Burundi (257)
            flagdict.Add("400", "/images/burundi.png"); // Burundi Mobile (257)
            flagdict.Add("725", "/images/bulgaria.png"); // Burundi Mobile - Onatel (359)
            flagdict.Add("55", "/images/cambodja.png"); // Cambodia (855)
            flagdict.Add("343", "/images/cambodja.png"); // Cambodia Mobile (855)
            flagdict.Add("56", "/images/cameroon.png"); // Cameroon (237)
            flagdict.Add("518", "/images/cameroon.png"); // Cameroon Douala (237)
            flagdict.Add("519", "/images/cameroon.png"); // Cameroon Mobile (237)
            flagdict.Add("726", "/images/cameroon.png"); // Cameroon Spacemob (237)
            flagdict.Add("57", "/images/canada.png"); // Canada (1)
            flagdict.Add("727", "/images/canada.png"); // Canada 867 Yukon And N.W.T. (1)
            flagdict.Add("728", "/images/canada.png"); // Canada 867 Yukon And N.W.T. (1)
            flagdict.Add("58", "/images/cape-verde.png"); // Cape Verde Islands (238)
            flagdict.Add("401", "/images/cape-verde.png"); // Cape Verde Islands Mobile (238)
            flagdict.Add("59", "/images/cayman-islands.png"); // Cayman Islands (1-345)
            flagdict.Add("344", "/images/cayman-islands.png"); // Cayman Islands Mobile (1-345)
            flagdict.Add("60", "/images/central-african-republic.png"); // Central African Rep (236)
            flagdict.Add("520", "/images/central-african-republic.png"); // Central African Rep Mobile (236)
            flagdict.Add("729", "/images/central-african-republic.png"); // Central African Republic Mobile (236)
            flagdict.Add("730", "/images/central-african-republic.png"); // Central African Republic Mobile (236)
            flagdict.Add("731", "/images/central-african-republic.png"); // Central African Republic Mobile - Acell (236)
            flagdict.Add("732", "/images/central-african-republic.png"); // Central African Republic Mobile - Nationlink (236)
            flagdict.Add("733", "/images/central-african-republic.png"); // Central African Republic Mobile - Orange (236)
            flagdict.Add("734", "/images/central-african-republic.png"); // Central African Republic Mobile - Telecel (236)
            flagdict.Add("735", "/images/chad.png"); // Chad Mobile (235)
            flagdict.Add("736", "/images/chad.png"); // Chad Mobile - Millicom (235)
            flagdict.Add("737", "/images/chad.png"); // Chad Mobile - Sotel (235)
            flagdict.Add("738", "/images/chad.png"); // Chad Mobile - Zain Celtel (235)
            flagdict.Add("62", "/images/chad.png"); // Chad Republic (235)
            flagdict.Add("402", "/images/chad.png"); // Chad Republic Mobile (235)
            flagdict.Add("63", "/images/chile.png"); // Chile (56)
            flagdict.Add("739", "/images/chile.png"); // Chile All Country (56)
            flagdict.Add("740", "/images/chile.png"); // Chile Coyhaique (56)
            flagdict.Add("521", "/images/chile.png"); // Chile Easter Island (56)
            flagdict.Add("403", "/images/chile.png"); // Chile Mobile (56)
            flagdict.Add("741", "/images/chile.png"); // Chile Rural (56)
            flagdict.Add("742", "/images/chile.png"); // Chile Santiago (56)
            flagdict.Add("743", "/images/chile.png"); // Chile Special Services (56)
            flagdict.Add("64", "/images/china.png"); // China (86)
            flagdict.Add("65", "/images/china.png"); // China Beijing (86)
            flagdict.Add("522", "/images/china.png"); // China Changchun (86)
            flagdict.Add("523", "/images/china.png"); // China Chengdu (86)
            flagdict.Add("524", "/images/china.png"); // China Chongging (86)
            flagdict.Add("525", "/images/china.png"); // China Dalian (86)
            flagdict.Add("526", "/images/china.png"); // China Fuzhou (86)
            flagdict.Add("527", "/images/china.png"); // China Guangzhou (86)
            flagdict.Add("452", "/images/china.png"); // China Mobile (86)
            flagdict.Add("528", "/images/china.png"); // China Qing Dao (86)
            flagdict.Add("66", "/images/china.png"); // China Shanghai (86)
            flagdict.Add("404", "/images/christmas_island.png"); // Christmas Islands (61-964)
            flagdict.Add("529", "/images/christmas_island.png"); // Christmas Islands Mobile (61-964)
            flagdict.Add("345", "/images/cocos.png"); // Cocos Islands (672-2)
            flagdict.Add("530", "/images/cocos.png"); // Cocos Islands Mobile (672-2)
            flagdict.Add("67", "/images/colombia.png"); // Colombia (57)
            flagdict.Add("531", "/images/colombia.png"); // Colombia Armenia (57)
            flagdict.Add("532", "/images/colombia.png"); // Colombia Barranquilla (57)
            flagdict.Add("533", "/images/colombia.png"); // Colombia Bogota (57)
            flagdict.Add("534", "/images/colombia.png"); // Colombia Buenaventura (57)
            flagdict.Add("535", "/images/colombia.png"); // Colombia Cali (57)
            flagdict.Add("536", "/images/colombia.png"); // Colombia Cartagena (57)
            flagdict.Add("537", "/images/colombia.png"); // Colombia Cartago (57)
            flagdict.Add("538", "/images/colombia.png"); // Colombia Manizales (57)
            flagdict.Add("539", "/images/colombia.png"); // Colombia Medellin (57)
            flagdict.Add("69", "/images/colombia.png"); // Colombia Mobile (57)
            flagdict.Add("744", "/images/colombia.png"); // Colombia Mobile - Movistar (57)
            flagdict.Add("540", "/images/colombia.png"); // Colombia Palmira (57)
            flagdict.Add("541", "/images/colombia.png"); // Colombia Pereira (57)
            flagdict.Add("68", "/images/comoros.png"); // Comoros Island (269)
            flagdict.Add("542", "/images/comoros.png"); // Comoros Island Mobile (269)
            flagdict.Add("70", "/images/congo-brazzaville.png"); // Congo (242)
            flagdict.Add("543", "/images/congo-brazzaville.png"); // Congo Mobile (242)
            flagdict.Add("745", "/images/congo-brazzaville.png"); // Congo Mobile - Warid (242)
            flagdict.Add("746", "/images/congo-brazzaville.png"); // Congo Mobile - Zain Celtel (242)
            flagdict.Add("71", "/images/cook-islands.png"); // Cook Islands (682)
            flagdict.Add("544", "/images/cook-islands.png"); // Cook Islands Mobile (682)
            flagdict.Add("72", "/images/costa-rica.png"); // Costa Rica (506)
            flagdict.Add("545", "/images/costa-rica.png"); // Costa Rica Mobile (506)
            flagdict.Add("73", "/images/croatia.png"); // Croatia (385)
            flagdict.Add("405", "/images/croatia.png"); // Croatia Mobile (385)
            flagdict.Add("747", "/images/croatia.png"); // Croatia Mobile - Tele2 (385)
            flagdict.Add("748", "/images/croatia.png"); // Croatia Mobile - TMobile (385)
            flagdict.Add("749", "/images/croatia.png"); // Croatia Mobile - Vipnet (385)
            flagdict.Add("74", "/images/cuba.png"); // Cuba (53)
            flagdict.Add("750", "/images/cuba.png"); // Cuba Guantanamo (53)
            flagdict.Add("751", "/images/cuba.png"); // Cuba Guantanamo Bay (53)
            flagdict.Add("460", "/images/cuba.png"); // Cuba Mobile (53)
            flagdict.Add("75", "/images/cyprus.png"); // Cyprus (357)
            flagdict.Add("346", "/images/cyprus.png"); // Cyprus Mobile (357)
            flagdict.Add("752", "/images/cyprus.png"); // Cyprus Mobile - Areeba (357)
            flagdict.Add("753", "/images/cyprus.png"); // Cyprus Mobile - CYTA (357)
            flagdict.Add("754", "/images/cyprus.png"); // Cyprus Mobile - CYTA Vodafone (357)
            flagdict.Add("381", "/images/czech-republic.png"); // Czech Republic (420)
            flagdict.Add("76", "/images/czech-republic.png"); // Czech Republic Brno (420)
            flagdict.Add("77", "/images/czech-republic.png"); // Czech Republic Mobile (420)
            flagdict.Add("755", "/images/czech-republic.png"); // Czech Republic Mobile - Mobilkom (420)
            flagdict.Add("756", "/images/czech-republic.png"); // Czech Republic Mobile - Telefonica O2 (420)
            flagdict.Add("757", "/images/czech-republic.png"); // Czech Republic Mobile - TMobile (420)
            flagdict.Add("758", "/images/czech-republic.png"); // Czech Republic Mobile - Vodafone (420)
            flagdict.Add("78", "/images/czech-republic.png"); // Czech Republic Prague (420)
            flagdict.Add("327", "/images/congo-kinshasa(zaire).png"); // Dem. Rep. Of Congo (243)
            flagdict.Add("446", "/images/congo-kinshasa(zaire).png"); // Dem. Rep. Of Congo Mobile (243)
            flagdict.Add("759", "/images/congo-kinshasa(zaire).png"); // Dem. Rep. Of Congo Mobile - Cct (243)
            flagdict.Add("760", "/images/congo-kinshasa(zaire).png"); // Dem. Rep. Of Congo Mobile - Celtel (243)
            flagdict.Add("761", "/images/congo-kinshasa(zaire).png"); // Dem. Rep. Of Congo Mobile - MTN (243)
            flagdict.Add("762", "/images/congo-kinshasa(zaire).png"); // Dem. Rep. Of Congo Mobile - Tigo (243)
            flagdict.Add("763", "/images/congo-kinshasa(zaire).png"); // Dem. Rep. Of Congo Mobile - Vodacom Congo (243)
            flagdict.Add("764", "/images/congo-kinshasa(zaire).png"); // Dem. Rep. Of Congo Premium (243)
            flagdict.Add("80", "/images/denmark.png"); // Denmark (45)
            flagdict.Add("765", "/images/denmark.png"); // Denmark Copenhagen (45)
            flagdict.Add("453", "/images/denmark.png"); // Denmark Mobile (45)
            flagdict.Add("766", "/images/denmark.png"); // Denmark Mobile - Barablu (45)
            flagdict.Add("767", "/images/denmark.png"); // Denmark Mobile - Hi3G (45)
            flagdict.Add("768", "/images/denmark.png"); // Denmark Mobile - Sonofon (45)
            flagdict.Add("769", "/images/denmark.png"); // Denmark Mobile - Tdc (45)
            flagdict.Add("770", "/images/denmark.png"); // Denmark Mobile - Telia (45)
            flagdict.Add("81", "/images/christmas_island.png"); // Diego Garcia (246)
            flagdict.Add("461", "/images/christmas_island.png"); // Diego Garcia Mobile (246)
            flagdict.Add("82", "/images/djibouti.png"); // Djibouti (253)
            flagdict.Add("546", "/images/djibouti.png"); // Djibouti Mobile (253)
            flagdict.Add("83", "/images/dominica.png"); // Dominica (1-767)
            flagdict.Add("547", "/images/dominica.png"); // Dominica Mobile (1-767)
            flagdict.Add("84", "/images/dominican-republic.png"); // Dominican Republic (1-809)
            flagdict.Add("548", "/images/dominican-republic.png"); // Dominican Republic Mobile (1-809)
            flagdict.Add("771", "/images/dominican-republic.png"); // Dominican Republic Pager (1-809)
            flagdict.Add("772", "/images/dominican-republic.png"); // Dominican Republic Santiago (1-809)
            flagdict.Add("773", "/images/dominican-republic.png"); // Dominican Republic Santo Domingo (1-809)
            flagdict.Add("85", "/images/ecuador_new.png"); // Ecuador (593)
            flagdict.Add("406", "/images/ecuador_new.png"); // Ecuador Cuenca (593)
            flagdict.Add("407", "/images/ecuador_new.png"); // Ecuador Guayaquil (593)
            flagdict.Add("86", "/images/ecuador_new.png"); // Ecuador Mobile (593)
            flagdict.Add("774", "/images/ecuador_new.png"); // Ecuador Mobile - Bellsouth Movistar (593)
            flagdict.Add("549", "/images/ecuador_new.png"); // Ecuador Quito (593)
            flagdict.Add("775", "/images/ecuador_new.png"); // Ecuador Quito Andinatel (593)
            flagdict.Add("87", "/images/egypt.png"); // Egypt (20)
            flagdict.Add("550", "/images/egypt.png"); // Egypt Alexandria (20)
            flagdict.Add("551", "/images/egypt.png"); // Egypt Cairo (20)
            flagdict.Add("454", "/images/egypt.png"); // Egypt Mobile (20)
            flagdict.Add("88", "/images/el-salvador.png"); // El Salvador (503)
            flagdict.Add("776", "/images/el-salvador.png"); // El Salvador CTE (503)
            flagdict.Add("462", "/images/el-salvador.png"); // El Salvador Mobile (503)
            flagdict.Add("777", "/images/el-salvador.png"); // El Salvador Mobile - CTE Personal (503)
            flagdict.Add("778", "/images/el-salvador.png"); // El Salvador Mobile - Personal (503)
            flagdict.Add("552", "/images/equatorial-guinea.png"); // Equatorial Guinea (240)
            flagdict.Add("553", "/images/equatorial-guinea.png"); // Equatorial Guinea Mobile (240)
            flagdict.Add("89", "/images/eritrea.png"); // Eritrea (291)
            flagdict.Add("463", "/images/eritrea.png"); // Eritrea Mobile (291)
            flagdict.Add("90", "/images/estonia.png"); // Estonia (372)
            flagdict.Add("464", "/images/estonia.png"); // Estonia Mobile (372)
            flagdict.Add("779", "/images/estonia.png"); // Estonia Mobile - Elisa (372)
            flagdict.Add("780", "/images/estonia.png"); // Estonia Mobile - EMT (372)
            flagdict.Add("781", "/images/estonia.png"); // Estonia Mobile - Tele2 (372)
            flagdict.Add("782", "/images/estonia.png"); // Estonia Premium (372)
            flagdict.Add("91", "/images/ethiopia.png"); // Ethiopia (251)
            flagdict.Add("783", "/images/ethiopia.png"); // Ethiopia Addis Ababa (251)
            flagdict.Add("784", "/images/ethiopia.png"); // Ethiopia All Country (251)
            flagdict.Add("408", "/images/ethiopia.png"); // Ethiopia Mobile (251)
            flagdict.Add("93", "/images/falkland.png"); // Falkland Islands (500)
            flagdict.Add("554", "/images/falkland.png"); // Falkland Islands Mobile (500)
            flagdict.Add("92", "/images/faroes.png"); // Faroe Islands (298)
            flagdict.Add("555", "/images/faroes.png"); // Faroe Islands Mobile (298)
            flagdict.Add("382", "/images/fiji.png"); // Fiji Islands (679)
            flagdict.Add("94", "/images/fiji.png"); // Fiji Islands Mobile (679)
            flagdict.Add("95", "/images/finland.png"); // Finland (358)
            flagdict.Add("556", "/images/finland.png"); // Finland Helsinki (358)
            flagdict.Add("348", "/images/finland.png"); // Finland Mobile (358)
            flagdict.Add("96", "/images/france.png"); // France (33)
            flagdict.Add("785", "/images/france.png"); // France CLEC (33)
            flagdict.Add("97", "/images/france.png"); // France Mobile (336)
            flagdict.Add("786", "/images/france.png"); // France Mobile - Bouygues (336)
            flagdict.Add("787", "/images/france.png"); // France Mobile - Globalstar (336)
            flagdict.Add("788", "/images/france.png"); // France Mobile - Orange (336)
            flagdict.Add("789", "/images/france.png"); // France Mobile - Sfr (336)
            flagdict.Add("98", "/images/france.png"); // France Paris (33)
            flagdict.Add("99", "/images/france.png"); // French Antilles (596)
            flagdict.Add("557", "/images/france.png"); // French Antilles Mobile (596)
            flagdict.Add("100", "/images/F-Guiana.jpg"); // French Guiana (594)
            flagdict.Add("558", "/images/F-Guiana.jpg"); // French Guiana Mobile (594)
            flagdict.Add("790", "/images/F-Guiana.jpg"); // French Guiana Mobile - Digicel (336)
            flagdict.Add("101", "/images/pf-flag.png"); // French Polynesia (689)
            flagdict.Add("559", "/images/pf-flag.png"); // French Polynesia Mobile (689)
            flagdict.Add("102", "/images/gabon.png"); // Gabon (241)
            flagdict.Add("409", "/images/gabon.png"); // Gabon Mobile (241)
            flagdict.Add("103", "/images/gambia.png"); // Gambia (220)
            flagdict.Add("560", "/images/gambia.png"); // Gambia Mobile (220)
            flagdict.Add("104", "/images/georgia.png"); // Georgia (995)
            flagdict.Add("455", "/images/georgia.png"); // Georgia Mobile (995)
            flagdict.Add("791", "/images/georgia.png"); // Georgia Mobile - Geocell (995)
            flagdict.Add("792", "/images/georgia.png"); // Georgia Mobile - Magticom (995)
            flagdict.Add("793", "/images/georgia.png"); // Georgia Mobile - Mobitel (995)
            flagdict.Add("794", "/images/georgia.png"); // Georgia Tbilisi (995)
            flagdict.Add("105", "/images/germany.png"); // Germany (49)
            flagdict.Add("561", "/images/germany.png"); // Germany Berlin (49)
            flagdict.Add("107", "/images/germany.png"); // Germany Dusseldorf (49)
            flagdict.Add("108", "/images/germany.png"); // Germany Frankfurt (49)
            flagdict.Add("109", "/images/germany.png"); // Germany Hamburg (49)
            flagdict.Add("106", "/images/germany.png"); // Germany Mobile (4916)
            flagdict.Add("795", "/images/germany.png"); // Germany Mobile - 02 (49)
            flagdict.Add("796", "/images/germany.png"); // Germany Mobile - E-Plus 3G Luxb (49)
            flagdict.Add("797", "/images/germany.png"); // Germany Mobile - E-Plus Mobilfunk (49)
            flagdict.Add("798", "/images/germany.png"); // Germany Mobile - Group 3G (49)
            flagdict.Add("799", "/images/germany.png"); // Germany Mobile - Non-Geo32 (49)
            flagdict.Add("800", "/images/germany.png"); // Germany Mobile - Reserve PNS (49)
            flagdict.Add("801", "/images/germany.png"); // Germany Mobile - TMobile (49)
            flagdict.Add("802", "/images/germany.png"); // Germany Mobile - Vistream (49)
            flagdict.Add("803", "/images/germany.png"); // Germany Mobile - Vodafone (49)
            flagdict.Add("562", "/images/germany.png"); // Germany Munich (49)
            flagdict.Add("804", "/images/germany.png"); // Germany Personal Numbers (49)
            flagdict.Add("110", "/images/ghana.png"); // Ghana (233)
            flagdict.Add("563", "/images/ghana.png"); // Ghana Accra (233)
            flagdict.Add("350", "/images/ghana.png"); // Ghana Mobile (233)
            flagdict.Add("805", "/images/ghana.png"); // Ghana Mobile - MTN (233)
            flagdict.Add("806", "/images/ghana.png"); // Ghana Mobile - Onetouch Vodafone (233)
            flagdict.Add("807", "/images/ghana.png"); // Ghana Mobile - Zain (233)
            flagdict.Add("111", "/images/gibraltar.png"); // Gibraltar (350)
            flagdict.Add("112", "/images/gibraltar.png"); // Gibraltar Mobile (1-473)
            flagdict.Add("113", "/images/greece.png"); // Greece (30)
            flagdict.Add("564", "/images/greece.png"); // Greece Athens (30)
            flagdict.Add("114", "/images/greece.png"); // Greece Mobile (30)
            flagdict.Add("808", "/images/greece.png"); // Greece Mobile - Cosmote (30)
            flagdict.Add("809", "/images/greece.png"); // Greece Mobile - Ote (30)
            flagdict.Add("810", "/images/greece.png"); // Greece Mobile - Roam (30)
            flagdict.Add("811", "/images/greece.png"); // Greece Mobile - Vodafone (30)
            flagdict.Add("812", "/images/greece.png"); // Greece Mobile - Wind Telecommunications (30)
            flagdict.Add("565", "/images/greece.png"); // Greece Saloniki (30)
            flagdict.Add("115", "/images/greenland.png"); // Greenland (399)
            flagdict.Add("566", "/images/greenland.png"); // Greenland Mobile (399)
            flagdict.Add("116", "/images/dominican-republic.png"); // Grenada (1-809)
            flagdict.Add("351", "/images/dominican-republic.png"); // Grenada Mobile (1-809)
            flagdict.Add("813", "/images/dominican-republic.png"); // Grenada Mobile - Digicel (1-809)
            flagdict.Add("117", "/images/guadeloupe.png"); // Guadeloupe (590)
            flagdict.Add("567", "/images/guadeloupe.png"); // Guadeloupe Mobile (590)
            flagdict.Add("814", "/images/guadeloupe.png"); // Guadeloupe Mobile - Digicel (590)
            flagdict.Add("815", "/images/guadeloupe.png"); // Guadeloupe Mobile - Mio (590)
            flagdict.Add("816", "/images/guadeloupe.png"); // Guadeloupe Mobile - Orange (590)
            flagdict.Add("118", "/images/guam.png"); // Guam (1-671)
            flagdict.Add("119", "/images/finland_flag.png"); // Guantanamo Bay (53-99)
            flagdict.Add("568", "/images/finland_flag.png"); // Guantanamo Bay Mobile (53-99)
            flagdict.Add("120", "/images/guademala.png"); // Guatemala (502)
            flagdict.Add("352", "/images/guademala.png"); // Guatemala Mobile (502)
            flagdict.Add("817", "/images/guademala.png"); // Guatemala Mobile - BELL (502)
            flagdict.Add("818", "/images/guademala.png"); // Guatemala Mobile - Comcel (502)
            flagdict.Add("819", "/images/guademala.png"); // Guatemala Mobile - PCS (502)
            flagdict.Add("820", "/images/guademala.png"); // Guatemala Mobile - Telefonica (502)
            flagdict.Add("821", "/images/guademala.png"); // Guatemala Telgua (502)
            flagdict.Add("122", "/images/guinea.png"); // Guinea (224)
            flagdict.Add("121", "/images/guinea.png"); // Guinea Bissau (245)
            flagdict.Add("570", "/images/guinea.png"); // Guinea Bissau Mobile (245)
            flagdict.Add("822", "/images/guinea.png"); // Guinea Bissau Mobile - Guinetel (245)
            flagdict.Add("823", "/images/guinea.png"); // Guinea Bissau Mobile - Mtn (245)
            flagdict.Add("824", "/images/guinea.png"); // Guinea Bissau Mobile - Orange (224)
            flagdict.Add("825", "/images/guinea.png"); // Guinea Bissau Premium Services (224)
            flagdict.Add("569", "/images/guinea.png"); // Guinea Mobile (224)
            flagdict.Add("826", "/images/guinea.png"); // Guinea Mobile - Cellcom (224)
            flagdict.Add("827", "/images/guinea.png"); // Guinea Mobile - Intercel (224)
            flagdict.Add("828", "/images/guinea.png"); // Guinea Mobile - Mai (224)
            flagdict.Add("829", "/images/guinea.png"); // Guinea Mobile - Mtn (224)
            flagdict.Add("830", "/images/guinea.png"); // Guinea Mobile - Orange (224)
            flagdict.Add("831", "/images/guinea.png"); // Guinea Mobile - Sotelgui (224)
            flagdict.Add("832", "/images/guinea.png"); // Guinea Mobile - Spacetel (224)
            flagdict.Add("123", "/images/guyana_flag.png"); // Guyana (592)
            flagdict.Add("354", "/images/guyana_flag.png"); // Guyana Mobile (592)
            flagdict.Add("833", "/images/guyana_flag.png"); // Guyana Mobile - Digicel (592)
            flagdict.Add("124", "/images/haiti.png"); // Haiti (509)
            flagdict.Add("410", "/images/haiti.png"); // Haiti Mobile (509)
            flagdict.Add("834", "/images/haiti.png"); // Haiti Mobile - Haitel (509)
            flagdict.Add("835", "/images/haiti.png"); // Haiti Special Services (509)
            flagdict.Add("411", "/images/hawaii_flag.png"); // Hawaii (1-808)
            flagdict.Add("125", "/images/honduras.png"); // Honduras (504)
            flagdict.Add("412", "/images/honduras.png"); // Honduras Mobile (504)
            flagdict.Add("836", "/images/honduras.png"); // Honduras Mobile - Hondutel (504)
            flagdict.Add("837", "/images/honduras.png"); // Honduras Mobile - Megatel (504)
            flagdict.Add("571", "/images/honduras.png"); // Honduras San Pedro Sula (504)
            flagdict.Add("572", "/images/honduras.png"); // Honduras Tegucigalpa (504)
            flagdict.Add("126", "/images/hong-kong.png"); // Hong Kong (852)
            flagdict.Add("127", "/images/hong-kong.png"); // Hong Kong Mobile (852)
            flagdict.Add("128", "/images/hungary.png"); // Hungary (36)
            flagdict.Add("573", "/images/hungary.png"); // Hungary Budapest (36)
            flagdict.Add("466", "/images/hungary.png"); // Hungary Mobile (36)
            flagdict.Add("838", "/images/hungary.png"); // Hungary Mobile - Pannon (36)
            flagdict.Add("839", "/images/hungary.png"); // Hungary Mobile - Tmobile (36)
            flagdict.Add("840", "/images/hungary.png"); // Hungary Mobile - Vodafone (36)
            flagdict.Add("129", "/images/iceland.png"); // Iceland (354)
            flagdict.Add("467", "/images/iceland.png"); // Iceland Mobile (354)
            flagdict.Add("841", "/images/iceland.png"); // Iceland Mobile - LMC (354)
            flagdict.Add("842", "/images/iceland.png"); // Iceland Mobile - Telecom (354)
            flagdict.Add("843", "/images/iceland.png"); // Iceland Mobile - Vodafone (354)
            flagdict.Add("130", "/images/india.png"); // India (91)
            flagdict.Add("132", "/images/india.png"); // India Ahmedabad (91)
            flagdict.Add("672", "/images/india.png"); // India Andhra Pradesh (91)
            flagdict.Add("159", "/images/india.png"); // India Bangalore (91)
            flagdict.Add("574", "/images/india.png"); // India Baroda (91)
            flagdict.Add("161", "/images/india.png"); // India Calcutta (91)
            flagdict.Add("163", "/images/india.png"); // India Chennai (91)
            flagdict.Add("131", "/images/india.png"); // India Gujrat (91)
            flagdict.Add("162", "/images/india.png"); // India Hyderabad (91)
            flagdict.Add("145", "/images/india.png"); // India Kerala (91)
            flagdict.Add("146", "/images/india.png"); // India Ludhiana (91)
            flagdict.Add("133", "/images/india.png"); // India Mobile (91)
            flagdict.Add("844", "/images/india.png"); // India Mobile - 94 Bsnl (91)
            flagdict.Add("160", "/images/india.png"); // India Mumbai (91)
            flagdict.Add("164", "/images/india.png"); // India New Delhi (91)
            flagdict.Add("575", "/images/india.png"); // India Pune (91)
            flagdict.Add("355", "/images/india.png"); // India Punjab (91)
            flagdict.Add("673", "/images/india.png"); // India Tamil Nadu (91)
            flagdict.Add("166", "/images/indonesia.png"); // Indonesia (62)
            flagdict.Add("845", "/images/indonesia.png"); // Indonesia All Country (62)
            flagdict.Add("311", "/images/indonesia.png"); // Indonesia Jakarta (62)
            flagdict.Add("449", "/images/indonesia.png"); // Indonesia Mobile (62)
            flagdict.Add("846", "/images/indonesia.png"); // Indonesia Mobile - Excelcom (62)
            flagdict.Add("847", "/images/indonesia.png"); // Indonesia Mobile - Indosat (62)
            flagdict.Add("848", "/images/indonesia.png"); // Indonesia Mobile - Telkomsel (62)
            flagdict.Add("167", "/images/iran.png"); // Iran (98)
            flagdict.Add("168", "/images/iran.png"); // Iran Mobile (98)
            flagdict.Add("576", "/images/iran.png"); // Iran Tehran (98)
            flagdict.Add("169", "/images/iraq.png"); // Iraq (964)
            flagdict.Add("577", "/images/iraq.png"); // Iraq Baghdad (964)
            flagdict.Add("468", "/images/iraq.png"); // Iraq Mobile (964)
            flagdict.Add("849", "/images/iraq.png"); // Iraq Mobile - Asiacell (964)
            flagdict.Add("850", "/images/iraq.png"); // Iraq Mobile - Atheer (964)
            flagdict.Add("851", "/images/iraq.png"); // Iraq Mobile - Irqna (964)
            flagdict.Add("852", "/images/iraq.png"); // Iraq Mobile - Korektel (964)
            flagdict.Add("853", "/images/iraq.png"); // Iraq Mobile - Kurdish (964)
            flagdict.Add("854", "/images/iraq.png"); // Iraq Mobile - Mobitel (964)
            flagdict.Add("855", "/images/iraq.png"); // Iraq Mobile - Orascom (964)
            flagdict.Add("856", "/images/iraq.png"); // Iraq Mobile - Sana (964)
            flagdict.Add("857", "/images/iraq.png"); // Iraq Mobile - Zain (964)
            flagdict.Add("170", "/images/ireland.png"); // Ireland (353)
            flagdict.Add("578", "/images/ireland.png"); // Ireland Dublin (353)
            flagdict.Add("469", "/images/ireland.png"); // Ireland Mobile (353)
            flagdict.Add("858", "/images/ireland.png"); // Ireland Mobile - Meteor (353)
            flagdict.Add("859", "/images/ireland.png"); // Ireland Mobile - O2 (353)
            flagdict.Add("860", "/images/ireland.png"); // Ireland Mobile - Vodafone (353)
            flagdict.Add("861", "/images/ireland.png"); // Ireland Non Geographic (353)
            flagdict.Add("862", "/images/ireland.png"); // Ireland Personal Number Service (353)
            flagdict.Add("863", "/images/ireland.png"); // Ireland Shannon (353)
            flagdict.Add("171", "/images/Israel.png"); // Israel (972)
            flagdict.Add("579", "/images/Israel.png"); // Israel Haifa (972)
            flagdict.Add("581", "/images/Israel.png"); // Israel Jerusalem (972)
            flagdict.Add("413", "/images/Israel.png"); // Israel Mobile (972)
            flagdict.Add("864", "/images/Israel.png"); // Israel Mobile - Cellcom (972)
            flagdict.Add("865", "/images/Israel.png"); // Israel Mobile - Mirs (972)
            flagdict.Add("866", "/images/Israel.png"); // Israel Mobile - Partner (972)
            flagdict.Add("867", "/images/Israel.png"); // Israel Mobile - Pelephone (972)
            flagdict.Add("580", "/images/Israel.png"); // Israel Palestinian Authority (972)
            flagdict.Add("582", "/images/Israel.png"); // Israel Tel Aviv (972)
            flagdict.Add("172", "/images/italy.png"); // Italy (39)
            flagdict.Add("583", "/images/italy.png"); // Italy Milan (39)
            flagdict.Add("173", "/images/italy.png"); // Italy Mobile (39)
            flagdict.Add("868", "/images/italy.png"); // Italy Mobile - Elsacom (39)
            flagdict.Add("869", "/images/italy.png"); // Italy Mobile - H3G (39)
            flagdict.Add("870", "/images/italy.png"); // Italy Mobile - Tim (39)
            flagdict.Add("871", "/images/italy.png"); // Italy Mobile - Vodafone (39)
            flagdict.Add("872", "/images/italy.png"); // Italy Mobile - Wind (39)
            flagdict.Add("174", "/images/italy.png"); // Italy Rome (39)
            flagdict.Add("175", "/images/ivory_coast.png"); // Ivory Coast (225)
            flagdict.Add("584", "/images/ivory_coast.png"); // Ivory Coast Abidjan (225)
            flagdict.Add("470", "/images/ivory_coast.png"); // Ivory Coast Mobile (225)
            flagdict.Add("176", "/images/jamaica.png"); // Jamaica (1-876)
            flagdict.Add("331", "/images/jamaica.png"); // Jamaica Mobile (1-876)
            flagdict.Add("873", "/images/jamaica.png"); // Jamaica Mobile - Centennial (1-876)
            flagdict.Add("874", "/images/jamaica.png"); // Jamaica Mobile - CWJ (1-876)
            flagdict.Add("875", "/images/jamaica.png"); // Jamaica Mobile - Digicel (1-876)
            flagdict.Add("177", "/images/japan.png"); // Japan (81)
            flagdict.Add("585", "/images/japan.png"); // Japan Military (81)
            flagdict.Add("158", "/images/japan.png"); // Japan Mobile (81)
            flagdict.Add("876", "/images/japan.png"); // Japan Mobile - Softbank (81)
            flagdict.Add("586", "/images/japan.png"); // Japan Nagoya (81)
            flagdict.Add("587", "/images/japan.png"); // Japan Okinawa (81)
            flagdict.Add("588", "/images/japan.png"); // Japan Osaka (81)
            flagdict.Add("589", "/images/japan.png"); // Japan Tokyo (81)
            flagdict.Add("590", "/images/japan.png"); // Japan Yokohama (81)
            flagdict.Add("178", "/images/jordan.png"); // Jordan (962)
            flagdict.Add("456", "/images/jordan.png"); // Jordan Amman (962)
            flagdict.Add("457", "/images/jordan.png"); // Jordan Mobile (962)
            flagdict.Add("877", "/images/jordan.png"); // Jordan Mobile - Fastlink (962)
            flagdict.Add("878", "/images/jordan.png"); // Jordan Mobile - Mobilcom (962)
            flagdict.Add("879", "/images/jordan.png"); // Jordan Mobile - Umniah (962)
            flagdict.Add("179", "/images/kazakhstan.png"); // Kazakhstan (7-336)
            flagdict.Add("880", "/images/kazakhstan.png"); // Kazakhstan Almaty (7-336)
            flagdict.Add("881", "/images/kazakhstan.png"); // Kazakhstan Astana (7-336)
            flagdict.Add("882", "/images/kazakhstan.png"); // Kazakhstan Karaganda (7-336)
            flagdict.Add("458", "/images/kazakhstan.png"); // Kazakhstan Mobile (7-336)
            flagdict.Add("883", "/images/kazakhstan.png"); // Kazakhstan Mobile - Altel (7-336)
            flagdict.Add("884", "/images/kazakhstan.png"); // Kazakhstan Mobile - K Cell (7-336)
            flagdict.Add("885", "/images/kazakhstan.png"); // Kazakhstan Mobile - Kartel (7-336)
            flagdict.Add("886", "/images/kazakhstan.png"); // Kazakhstan Mobile - TNS (7-336)
            flagdict.Add("887", "/images/kazakhstan.png"); // Kazakhstan Nursat (7-336)
            flagdict.Add("180", "/images/kenya.png"); // Kenya (254)
            flagdict.Add("182", "/images/kenya.png"); // Kenya Mobile (254)
            flagdict.Add("888", "/images/kenya.png"); // Kenya Mobile - Orange (254)
            flagdict.Add("889", "/images/kenya.png"); // Kenya Mobile - Safaricom (254)
            flagdict.Add("890", "/images/kenya.png"); // Kenya Mobile - Telkom Kenya (254)
            flagdict.Add("891", "/images/kenya.png"); // Kenya Mobile - Yu Econet (254)
            flagdict.Add("892", "/images/kenya.png"); // Kenya Mobile - Zain Celtel (254)
            flagdict.Add("181", "/images/kenya.png"); // Kenya Nairobi (254)
            flagdict.Add("893", "/images/kenya.png"); // Kenya Premium Services (254)
            flagdict.Add("414", "/images/kiribati.png"); // Kiribati (686)
            flagdict.Add("471", "/images/kiribati.png"); // Kiribati Mobile (686)
            flagdict.Add("670", "/images/kosovo_flag.png"); // Kosovo (381)
            flagdict.Add("671", "/images/kosovo_flag.png"); // Kosovo-Mobile (381-4)
            flagdict.Add("185", "/images/kuwait.png"); // Kuwait (965)
            flagdict.Add("472", "/images/kuwait.png"); // Kuwait Mobile (965)
            flagdict.Add("186", "/images/kyrgyzstan.png"); // Kyrgyzstan (996)
            flagdict.Add("894", "/images/kyrgyzstan.png"); // Kyrgyzstan Bishkek (996)
            flagdict.Add("473", "/images/kyrgyzstan.png"); // Kyrgyzstan Mobile (996)
            flagdict.Add("187", "/images/laos_flag.png"); // Laos (856)
            flagdict.Add("415", "/images/laos_flag.png"); // Laos Mobile (856)
            flagdict.Add("188", "/images/latvia.png"); // Latvia (371)
            flagdict.Add("474", "/images/latvia.png"); // Latvia Mobile (371)
            flagdict.Add("895", "/images/latvia.png"); // Latvia Mobile - Bite (371)
            flagdict.Add("896", "/images/latvia.png"); // Latvia Mobile - Lmt (371)
            flagdict.Add("897", "/images/latvia.png"); // Latvia Mobile - Radiocoms (371)
            flagdict.Add("898", "/images/latvia.png"); // Latvia Mobile - Rigatta (371)
            flagdict.Add("899", "/images/latvia.png"); // Latvia Mobile - Tele2 (371)
            flagdict.Add("900", "/images/latvia.png"); // Latvia Riga (371)
            flagdict.Add("189", "/images/lebanon.png"); // Lebanon (961)
            flagdict.Add("901", "/images/lebanon.png"); // Lebanon Beirut (961)
            flagdict.Add("475", "/images/lebanon.png"); // Lebanon Mobile (961)
            flagdict.Add("190", "/images/lesotho.png"); // Lesotho (266)
            flagdict.Add("591", "/images/lesotho.png"); // Lesotho Mobile (266)
            flagdict.Add("902", "/images/lesotho.png"); // Lesotho Mobile - Econet (266)
            flagdict.Add("903", "/images/lesotho.png"); // Lesotho Mobile - Vodacom (266)
            flagdict.Add("191", "/images/liberia.png"); // Liberia (231)
            flagdict.Add("416", "/images/liberia.png"); // Liberia Mobile (231)
            flagdict.Add("904", "/images/liberia.png"); // Liberia Mobile - Cellcom (231)
            flagdict.Add("905", "/images/liberia.png"); // Liberia Mobile - Comium (231)
            flagdict.Add("906", "/images/liberia.png"); // Liberia Mobile - Libercell (231)
            flagdict.Add("907", "/images/liberia.png"); // Liberia Mobile - Lonestar (231)
            flagdict.Add("908", "/images/liberia.png"); // Liberia Mobile - Mtn (231)
            flagdict.Add("909", "/images/liberia.png"); // Liberia Premium Services (231)
            flagdict.Add("192", "/images/libya.png"); // Libya (218)
            flagdict.Add("417", "/images/libya.png"); // Libya Mobile (218)
            flagdict.Add("193", "/images/liechtenstein.png"); // Liechtenstein (423)
            flagdict.Add("592", "/images/liechtenstein.png"); // Liechtenstein Mobile (423)
            flagdict.Add("910", "/images/liechtenstein.png"); // Liechtenstein Special Services (423)
            flagdict.Add("194", "/images/lithuania.png"); // Lithuania (370)
            flagdict.Add("911", "/images/liechtenstein.png"); // Lithuania Freephone (423)
            flagdict.Add("476", "/images/lithuania.png"); // Lithuania Mobile (370)
            flagdict.Add("912", "/images/liechtenstein.png"); // Lithuania Mobile - Bite (423)
            flagdict.Add("913", "/images/liechtenstein.png"); // Lithuania Mobile - Omnitel (423)
            flagdict.Add("914", "/images/lithuania.png"); // Lithuania Mobile - Tele2 (370)
            flagdict.Add("915", "/images/lithuania.png"); // Lithuania Personal (370)
            flagdict.Add("195", "/images/luxembourg.png"); // Luxembourg (352)
            flagdict.Add("593", "/images/luxembourg.png"); // Luxembourg Mobile (352)
            flagdict.Add("916", "/images/luxembourg.png"); // Luxembourg Mobile - Tango (352)
            flagdict.Add("917", "/images/luxembourg.png"); // Luxembourg Mobile - VOX (352)
            flagdict.Add("196", "/images/macao.png"); // Macao (853)
            flagdict.Add("418", "/images/macao.png"); // Macao Mobile (853)
            flagdict.Add("197", "/images/macedonia.png"); // Macedonia (389)
            flagdict.Add("477", "/images/macedonia.png"); // Macedonia Mobile (389-7)
            flagdict.Add("918", "/images/macedonia.png"); // Macedonia Mobile - Cosmofone (389-7)
            flagdict.Add("919", "/images/macedonia.png"); // Macedonia Mobile - T Mobile (389-7)
            flagdict.Add("920", "/images/macedonia.png"); // Macedonia Mobile - Vip (389-7)
            flagdict.Add("198", "/images/madagascar.png"); // Madagascar (261)
            flagdict.Add("478", "/images/madagascar.png"); // Madagascar Mobile (261)
            flagdict.Add("921", "/images/madagascar.png"); // Madagascar Mobile - Celtel (261)
            flagdict.Add("922", "/images/madagascar.png"); // Madagascar Mobile - Intercel (261)
            flagdict.Add("923", "/images/madagascar.png"); // Madagascar Mobile - Orange (261)
            flagdict.Add("924", "/images/madagascar.png"); // Madagascar Mobile - Spacetel (261)
            flagdict.Add("925", "/images/madagascar.png"); // Madagascar Mobile - Telma (261)
            flagdict.Add("199", "/images/malawi.png"); // Malawi (265)
            flagdict.Add("594", "/images/malawi.png"); // Malawi Mobile (265)
            flagdict.Add("200", "/images/malaysia.png"); // Malaysia (60)
            flagdict.Add("377", "/images/malaysia.png"); // Malaysia Kuala Lumpur (60)
            flagdict.Add("201", "/images/malaysia.png"); // Malaysia Mobile (60)
            flagdict.Add("203", "/images/maldives.png"); // Maldives (960)
            flagdict.Add("479", "/images/maldives.png"); // Maldives Mobile (960)
            flagdict.Add("926", "/images/mali.png"); // Mali Orange (223)
            flagdict.Add("204", "/images/mali.png"); // Mali Republic (223)
            flagdict.Add("419", "/images/mali.png"); // Mali Republic Mobile (223)
            flagdict.Add("205", "/images/malta.png"); // Malta (356)
            flagdict.Add("420", "/images/malta.png"); // Malta Mobile (356)
            flagdict.Add("927", "/images/malta.png"); // Malta Mobile - 3G (356)
            flagdict.Add("928", "/images/malta.png"); // Malta Mobile - Gomobile (356)
            flagdict.Add("929", "/images/malta.png"); // Malta Mobile - Vodafone (356)
            flagdict.Add("206", "/images/marshall-islands.png"); // Marshall Islands (692)
            flagdict.Add("595", "/images/marshall-islands.png"); // Marshall Islands Mobile (692)
            flagdict.Add("207", "/images/mauritania.png"); // Mauritania (222)
            flagdict.Add("596", "/images/mauritania.png"); // Mauritania Mobile (222)
            flagdict.Add("378", "/images/mauritius.png"); // Mauritius (230)
            flagdict.Add("208", "/images/mauritius.png"); // Mauritius Mobile (230)
            flagdict.Add("597", "/images/comoros.png"); // Mayotte Island (269)
            flagdict.Add("598", "/images/comoros.png"); // Mayotte Island Mobile (269)
            flagdict.Add("209", "/images/mexico.png"); // Mexico (52)
            flagdict.Add("930", "/images/mexico.png"); // Mexico Acaponeta 325 (52)
            flagdict.Add("931", "/images/mexico.png"); // Mexico Acapulco 744 (52)
            flagdict.Add("932", "/images/mexico.png"); // Mexico Actopan 772 (52)
            flagdict.Add("933", "/images/mexico.png"); // Mexico Agua Prieta 633 (52)
            flagdict.Add("934", "/images/mexico.png"); // Mexico Aguascalientes 449 (52)
            flagdict.Add("935", "/images/mexico.png"); // Mexico Allende 862 (52)
            flagdict.Add("936", "/images/mexico.png"); // Mexico Apatzingan 453 (52)
            flagdict.Add("937", "/images/mexico.png"); // Mexico Apizaco 241 (52)
            flagdict.Add("938", "/images/mexico.png"); // Mexico Arcelia 732 (52)
            flagdict.Add("939", "/images/mexico.png"); // Mexico Arcelia 732 (52)
            flagdict.Add("940", "/images/mexico.png"); // Mexico Atlacomulco 712 (52)
            flagdict.Add("941", "/images/mexico.png"); // Mexico Atlixco 244 (52)
            flagdict.Add("942", "/images/mexico.png"); // Mexico Autlan 317 (52)
            flagdict.Add("943", "/images/mexico.png"); // Mexico Bahia De Huatulco 958 (52)
            flagdict.Add("944", "/images/mexico.png"); // Mexico Caborca 637 (52)
            flagdict.Add("945", "/images/mexico.png"); // Mexico Cadereyta 828 (52)
            flagdict.Add("946", "/images/mexico.png"); // Mexico Campeche 981 (52)
            flagdict.Add("947", "/images/mexico.png"); // Mexico Cananea 645 (52)
            flagdict.Add("948", "/images/mexico.png"); // Mexico Cancun 998 (52)
            flagdict.Add("949", "/images/mexico.png"); // Mexico Celaya 461 (52)
            flagdict.Add("950", "/images/mexico.png"); // Mexico Cerralvo 892 (52)
            flagdict.Add("951", "/images/mexico.png"); // Mexico Chetumal 983 (52 )
            flagdict.Add("952", "/images/mexico.png"); // Mexico Chihuahua 614 (52)
            flagdict.Add("953", "/images/mexico.png"); // Mexico Chilapa 756 (52 )
            flagdict.Add("954", "/images/mexico.png"); // Mexico Chilpancingo 747 (52)
            flagdict.Add("955", "/images/mexico.png"); // Mexico China 823 (52 )
            flagdict.Add("956", "/images/mexico.png"); // Mexico Cintalapa 968 (52 )
            flagdict.Add("666", "/images/mexico.png"); // Mexico Cities (52)
            flagdict.Add("211", "/images/mexico.png"); // Mexico City (52)
            flagdict.Add("957", "/images/mexico.png"); // Mexico Ciudad Acu±a 877 (52)
            flagdict.Add("958", "/images/mexico.png"); // Mexico Ciudad Altamirano 767 (52 )
            flagdict.Add("959", "/images/mexico.png"); // Mexico Ciudad Camargo 648 (52)
            flagdict.Add("960", "/images/mexico.png"); // Mexico Ciudad Camargo 891 (52)
            flagdict.Add("961", "/images/mexico.png"); // Mexico Ciudad Constitucion 613 (52)
            flagdict.Add("962", "/images/mexico.png"); // Mexico Ciudad Cuauhtemoc 625 (52)
            flagdict.Add("963", "/images/mexico.png"); // Mexico Ciudad Del Carmen 938 (52)
            flagdict.Add("964", "/images/mexico.png"); // Mexico Ciudad Delicias 639 (52)
            flagdict.Add("965", "/images/mexico.png"); // Mexico Ciudad Guzman 341 (52)
            flagdict.Add("966", "/images/mexico.png"); // Mexico Ciudad Hidalgo 786 (52)
            flagdict.Add("967", "/images/mexico.png"); // Mexico Ciudad Juarez 656 (52 )
            flagdict.Add("968", "/images/mexico.png"); // Mexico Ciudad Lazaro Cardenas 753 (52)
            flagdict.Add("969", "/images/mexico.png"); // Mexico Ciudad Mante 831 (52)
            flagdict.Add("970", "/images/mexico.png"); // Mexico Ciudad Obregon 644 (52)
            flagdict.Add("971", "/images/mexico.png"); // Mexico Ciudad Sahagun 791 (52)
            flagdict.Add("972", "/images/mexico.png"); // Mexico Ciudad Valles 481 (52)
            flagdict.Add("973", "/images/mexico.png"); // Mexico Ciudad Victoria 834 (52)
            flagdict.Add("974", "/images/mexico.png"); // Mexico Coatzacoalcos 921 (52)
            flagdict.Add("975", "/images/mexico.png"); // Mexico Colima 312 (52)
            flagdict.Add("976", "/images/mexico.png"); // Mexico Cordoba 271 (52)
            flagdict.Add("977", "/images/mexico.png"); // Mexico Cosamaloapan 288 (52)
            flagdict.Add("978", "/images/mexico.png"); // Mexico Cozumel 987 (52)
            flagdict.Add("979", "/images/mexico.png"); // Mexico Cuautla 735 (52)
            flagdict.Add("980", "/images/mexico.png"); // Mexico Cuernavaca 777 (52)
            flagdict.Add("981", "/images/mexico.png"); // Mexico Culiacan 667 (52)
            flagdict.Add("982", "/images/mexico.png"); // Mexico Durango 618 (52)
            flagdict.Add("983", "/images/mexico.png"); // Mexico Encarnacion De Diaz 475 (52 )
            flagdict.Add("984", "/images/mexico.png"); // Mexico Ensenada 646 (52)
            flagdict.Add("985", "/images/mexico.png"); // Mexico Fresnillo 493 (52)
            flagdict.Add("986", "/images/mexico.png"); // Mexico Guadalajara (52)
            flagdict.Add("210", "/images/mexico.png"); // Mexico Guadalajara (52)
            flagdict.Add("987", "/images/mexico.png"); // Mexico Guadalupe Victoria 676 (52 )
            flagdict.Add("988", "/images/mexico.png"); // Mexico Guamuchil 673 (52)
            flagdict.Add("989", "/images/mexico.png"); // Mexico Guanajuato 473 (52)
            flagdict.Add("990", "/images/mexico.png"); // Mexico Guasave 687 (52)
            flagdict.Add("991", "/images/mexico.png"); // Mexico Guaymas 622 (52)
            flagdict.Add("992", "/images/mexico.png"); // Mexico Hermosillo 662 (52)
            flagdict.Add("993", "/images/mexico.png"); // Mexico Hidalgo 829 (52)
            flagdict.Add("994", "/images/mexico.png"); // Mexico Huatabampo 647 (52)
            flagdict.Add("995", "/images/mexico.png"); // Mexico Huetamo 435 (52)
            flagdict.Add("996", "/images/mexico.png"); // Mexico Huimanguillo 917 (52)
            flagdict.Add("997", "/images/mexico.png"); // Mexico Huitzuco 727 (52)
            flagdict.Add("998", "/images/mexico.png"); // Mexico Iguala 733 (52)
            flagdict.Add("999", "/images/mexico.png"); // Mexico Irapuato 462 (52)
            flagdict.Add("1000", "/images/mexico.png"); // Mexico Ixtapan De La Sal 721 (52)
            flagdict.Add("1001", "/images/mexico.png"); // Mexico Ixtlan Del Rio 324 (52)
            flagdict.Add("1002", "/images/mexico.png"); // Mexico Izucar De Matamoros 243 (52)
            flagdict.Add("1003", "/images/mexico.png"); // Mexico Jalapa 228 (52 )
            flagdict.Add("1004", "/images/mexico.png"); // Mexico Jalpa 463 (52)
            flagdict.Add("1005", "/images/mexico.png"); // Mexico Jerez De Garcia Salinas 494 (52)
            flagdict.Add("1006", "/images/mexico.png"); // Mexico Jojutla 734 (52)
            flagdict.Add("1007", "/images/mexico.png"); // Mexico La Barca 393 (52)
            flagdict.Add("1008", "/images/mexico.png"); // Mexico La Paz 612 (52)
            flagdict.Add("1009", "/images/mexico.png"); // Mexico La Piedad 352 (52)
            flagdict.Add("1010", "/images/mexico.png"); // Mexico Lagos De Moreno 474 (52)
            flagdict.Add("1011", "/images/mexico.png"); // Mexico Leon 477 (52)
            flagdict.Add("1012", "/images/mexico.png"); // Mexico Lerdo De Tejada 284 (52)
            flagdict.Add("1013", "/images/mexico.png"); // Mexico Lerma 728 (52)
            flagdict.Add("1014", "/images/mexico.png"); // Mexico Linares 821 (52)
            flagdict.Add("1015", "/images/mexico.png"); // Mexico Los Mochis 668 (52)
            flagdict.Add("1016", "/images/mexico.png"); // Mexico Los Reyes 354 (52)
            flagdict.Add("1017", "/images/mexico.png"); // Mexico Magdalena 632 (52)
            flagdict.Add("1018", "/images/mexico.png"); // Mexico Manzanillo 314 (52)
            flagdict.Add("1019", "/images/mexico.png"); // Mexico Martinez De La Torre 232 (52)
            flagdict.Add("1020", "/images/mexico.png"); // Mexico Matamoros 868 (52)
            flagdict.Add("1021", "/images/mexico.png"); // Mexico Matehuala 488 (52)
            flagdict.Add("1022", "/images/mexico.png"); // Mexico Mazatlan 669 (52)
            flagdict.Add("1023", "/images/mexico.png"); // Mexico Merida 999 (52)
            flagdict.Add("1024", "/images/mexico.png"); // Mexico Mexicali 686 (52)
            flagdict.Add("1025", "/images/mexico.png"); // Mexico Mexico City (52)
            flagdict.Add("1026", "/images/mexico.png"); // Mexico Minatitlan 922 (52)
            flagdict.Add("480", "/images/mexico.png"); // Mexico Mobile (52)
            flagdict.Add("1027", "/images/mexico.png"); // Mexico Mobile - Iusacell (52)
            flagdict.Add("1028", "/images/mexico.png"); // Mexico Mobile - MGM (52)
            flagdict.Add("1029", "/images/mexico.png"); // Mexico Mobile - Movistar (52)
            flagdict.Add("1030", "/images/mexico.png"); // Mexico Mobile - Telcel (52)
            flagdict.Add("1031", "/images/mexico.png"); // Mexico Monclova 866 (52)
            flagdict.Add("1032", "/images/mexico.png"); // Mexico Montemorelos 826 (52)
            flagdict.Add("212", "/images/mexico.png"); // Mexico Monterey (52)
            flagdict.Add("1033", "/images/mexico.png"); // Mexico Monterrey (52)
            flagdict.Add("1034", "/images/mexico.png"); // Mexico Morelia 443 (52)
            flagdict.Add("1035", "/images/mexico.png"); // Mexico Moroleon 445 (52)
            flagdict.Add("1036", "/images/mexico.png"); // Mexico Nacozari 634 (52)
            flagdict.Add("1037", "/images/mexico.png"); // Mexico Navojoa 642 (52)
            flagdict.Add("1038", "/images/mexico.png"); // Mexico Nogales 631 (52 )
            flagdict.Add("1039", "/images/mexico.png"); // Mexico Nuevo Casas Grandes 636 (52)
            flagdict.Add("1040", "/images/mexico.png"); // Mexico Nuevo Laredo 867 (52)
            flagdict.Add("1041", "/images/mexico.png"); // Mexico Oaxaca 951 (52)
            flagdict.Add("1042", "/images/mexico.png"); // Mexico Ocotlan 392 (52)
            flagdict.Add("1043", "/images/mexico.png"); // Mexico Ojinaga 626 (52)
            flagdict.Add("1044", "/images/mexico.png"); // Mexico Ometepec 741 (52)
            flagdict.Add("1045", "/images/mexico.png"); // Mexico Orizaba 272 (52)
            flagdict.Add("1046", "/images/mexico.png"); // Mexico Ozumba 597 (52)
            flagdict.Add("1047", "/images/mexico.png"); // Mexico Pachuca 771 (52)
            flagdict.Add("1048", "/images/mexico.png"); // Mexico Palenque 916 (52)
            flagdict.Add("1049", "/images/mexico.png"); // Mexico Parral 627 (52)
            flagdict.Add("1050", "/images/mexico.png"); // Mexico Parras De La Fuente 842 (52)
            flagdict.Add("1051", "/images/mexico.png"); // Mexico Patzcuaro 434 (52)
            flagdict.Add("1052", "/images/mexico.png"); // Mexico Penjamo 469 (52)
            flagdict.Add("1053", "/images/mexico.png"); // Mexico Petatlan 758 (52)
            flagdict.Add("1054", "/images/mexico.png"); // Mexico Piedras Negras 878 (52)
            flagdict.Add("1055", "/images/mexico.png"); // Mexico Poza Rica 782 (52)
            flagdict.Add("213", "/images/mexico.png"); // Mexico Puebla (52)
            flagdict.Add("1056", "/images/mexico.png"); // Mexico Puebla 222 (52)
            flagdict.Add("1057", "/images/mexico.png"); // Mexico Puerto Pe±asco 638 (52)
            flagdict.Add("1058", "/images/mexico.png"); // Mexico Puerto Vallarta 322 (52)
            flagdict.Add("1059", "/images/mexico.png"); // Mexico Puruandiro 438 (52)
            flagdict.Add("1060", "/images/mexico.png"); // Mexico Queretaro 442 (52)
            flagdict.Add("1061", "/images/mexico.png"); // Mexico Reynosa 899 (52)
            flagdict.Add("1062", "/images/mexico.png"); // Mexico Rio Grande 498 (52)
            flagdict.Add("1063", "/images/mexico.png"); // Mexico Rio Verde 487 (52)
            flagdict.Add("1064", "/images/mexico.png"); // Mexico Rosarito 661 (52)
            flagdict.Add("1065", "/images/mexico.png"); // Mexico Sabinas 861 (52)
            flagdict.Add("1066", "/images/mexico.png"); // Mexico Sahuayo 353 (52)
            flagdict.Add("1067", "/images/mexico.png"); // Mexico Salamanca 464 (52)
            flagdict.Add("1068", "/images/mexico.png"); // Mexico Salina Cruz 971 (52)
            flagdict.Add("1069", "/images/mexico.png"); // Mexico Saltillo 844 (52)
            flagdict.Add("1070", "/images/mexico.png"); // Mexico Salvatierra 466 (52)
            flagdict.Add("1071", "/images/mexico.png"); // Mexico San Andres Tuxtla 294 (52)
            flagdict.Add("1072", "/images/mexico.png"); // Mexico San Cristobal De Las Casas 967 (52)
            flagdict.Add("1073", "/images/mexico.png"); // Mexico San Fernando 841 (52)
            flagdict.Add("1074", "/images/mexico.png"); // Mexico San Jose De Gracia 381 (52)
            flagdict.Add("1075", "/images/mexico.png"); // Mexico San Jose Del Cabo 624 (52)
            flagdict.Add("1076", "/images/mexico.png"); // Mexico San Juan Del Rio 427 (52)
            flagdict.Add("1077", "/images/mexico.png"); // Mexico San Luis De La Paz 468 (52)
            flagdict.Add("1078", "/images/mexico.png"); // Mexico San Luis Potosi 444 (52)
            flagdict.Add("1079", "/images/mexico.png"); // Mexico San Luis Rio Colorado 653 (52)
            flagdict.Add("1080", "/images/mexico.png"); // Mexico San Miguel Allende 415 (52)
            flagdict.Add("1081", "/images/mexico.png"); // Mexico San Quintin 616 (52)
            flagdict.Add("1082", "/images/mexico.png"); // Mexico Santa Ana 641 (52)
            flagdict.Add("1083", "/images/mexico.png"); // Mexico Santa Rosalia 615 (52)
            flagdict.Add("1084", "/images/mexico.png"); // Mexico Santiago Ixcuintla 323 (52)
            flagdict.Add("1085", "/images/mexico.png"); // Mexico Santiago Papasquiaro 674 (52)
            flagdict.Add("1086", "/images/mexico.png"); // Mexico Santiago Tianguistenco 713 (52)
            flagdict.Add("1087", "/images/mexico.png"); // Mexico Silao 472 (52)
            flagdict.Add("1088", "/images/mexico.png"); // Mexico Tala 384 (52)
            flagdict.Add("1089", "/images/mexico.png"); // Mexico Tampico 833 (52)
            flagdict.Add("1090", "/images/mexico.png"); // Mexico Tapachula 962 (52)
            flagdict.Add("1091", "/images/mexico.png"); // Mexico Taxco 762 (52)
            flagdict.Add("1092", "/images/mexico.png"); // Mexico Tecate 665 (52)
            flagdict.Add("1093", "/images/mexico.png"); // Mexico Tecoman 313 (52)
            flagdict.Add("1094", "/images/mexico.png"); // Mexico Tecpan De Galeana 742 (52)
            flagdict.Add("1095", "/images/mexico.png"); // Mexico Tecuala 389 (52)
            flagdict.Add("1096", "/images/mexico.png"); // Mexico Tehuacan 238 (52)
            flagdict.Add("1097", "/images/mexico.png"); // Mexico Teloloapan 736 (52)
            flagdict.Add("1098", "/images/mexico.png"); // Mexico Tenancingo 714 (52)
            flagdict.Add("1099", "/images/mexico.png"); // Mexico Tepatitlan 378 (52)
            flagdict.Add("1100", "/images/mexico.png"); // Mexico Tepic 311 (52)
            flagdict.Add("1101", "/images/mexico.png"); // Mexico Tequila 374 (52)
            flagdict.Add("1102", "/images/mexico.png"); // Mexico Texcoco 595 (52)
            flagdict.Add("1103", "/images/mexico.png"); // Mexico Teziutlan 231 (52)
            flagdict.Add("1104", "/images/mexico.png"); // Mexico Ticul 997 (52)
            flagdict.Add("1105", "/images/mexico.png"); // Mexico Tijuana 664 (52)
            flagdict.Add("1134", "/images/mexico.png"); // Mexico Tixtla 754 (52)
            flagdict.Add("1106", "/images/mexico.png"); // Mexico Tizimin 986 (52)
            flagdict.Add("1107", "/images/mexico.png"); // Mexico Tlapa De Comonfort 757 (52)
            flagdict.Add("1108", "/images/mexico.png"); // Mexico Tlaxcala 246 (52)
            flagdict.Add("1109", "/images/mexico.png"); // Mexico Toluca 722 (52)
            flagdict.Add("1110", "/images/mexico.png"); // Mexico Torreon 871 (52)
            flagdict.Add("1111", "/images/mexico.png"); // Mexico Tula 773 (52)
            flagdict.Add("1112", "/images/mexico.png"); // Mexico Tulancingo 775 (52)
            flagdict.Add("1113", "/images/mexico.png"); // Mexico Tuxpan - Veracruz 783 (52)
            flagdict.Add("1114", "/images/mexico.png"); // Mexico Tuxtepec 287 (52)
            flagdict.Add("1115", "/images/mexico.png"); // Mexico Tuxtla Gutierrez 961 (52)
            flagdict.Add("1116", "/images/mexico.png"); // Mexico Ures 623 (52)
            flagdict.Add("1117", "/images/mexico.png"); // Mexico Uruapan 452 (52)
            flagdict.Add("1118", "/images/mexico.png"); // Mexico Valle De Bravo 726 (52)
            flagdict.Add("1119", "/images/mexico.png"); // Mexico Veracruz 229 (52)
            flagdict.Add("1120", "/images/mexico.png"); // Mexico Villa Aldama 836 (52)
            flagdict.Add("1121", "/images/mexico.png"); // Mexico Villa Flores 965 (52)
            flagdict.Add("1122", "/images/mexico.png"); // Mexico Villahermosa 993 (52)
            flagdict.Add("1123", "/images/mexico.png"); // Mexico Yurecuaro 356 (52)
            flagdict.Add("1124", "/images/mexico.png"); // Mexico Zacapu 436 (52)
            flagdict.Add("1125", "/images/mexico.png"); // Mexico Zacatecas 492 (52)
            flagdict.Add("1126", "/images/mexico.png"); // Mexico Zamora 351 (52)
            flagdict.Add("1127", "/images/mexico.png"); // Mexico Zihuatanejo 755 (52)
            flagdict.Add("1128", "/images/mexico.png"); // Mexico Zinapecuaro 451 (52)
            flagdict.Add("1129", "/images/mexico.png"); // Mexico Zitacuaro 715 (52)
            flagdict.Add("1130", "/images/mexico.png"); // Mexico Zumpango 591 (52)
            flagdict.Add("214", "/images/micronesia.png"); // Micronesia (691)
            flagdict.Add("599", "/images/micronesia.png"); // Micronesia Mobile (691)
            flagdict.Add("215", "/images/moldova.png"); // Moldova (373)
            flagdict.Add("422", "/images/moldova.png"); // Moldova Mobile (373)
            flagdict.Add("1131", "/images/moldova.png"); // Moldova Mobile (373)
            flagdict.Add("1132", "/images/moldova.png"); // Moldova Mobile - Eventis (373)
            flagdict.Add("1133", "/images/moldova.png"); // Moldova Mobile - Moldcell (373)
            flagdict.Add("1135", "/images/moldova.png"); // Moldova Mobile - Moldtelecom (373)
            flagdict.Add("1136", "/images/moldova.png"); // Moldova Mobile - Orange (373)
            flagdict.Add("1137", "/images/moldova.png"); // Moldova Mobile - Transnistria (373)
            flagdict.Add("1138", "/images/moldova.png"); // Moldova Mobile - Voxtel (373)
            flagdict.Add("216", "/images/monaco.png"); // Monaco (377)
            flagdict.Add("423", "/images/monaco.png"); // Monaco Mobile (377)
            flagdict.Add("217", "/images/mongolia.png"); // Mongolia (976)
            flagdict.Add("481", "/images/mongolia.png"); // Mongolia Mobile (976)
            flagdict.Add("667", "/images/montenegro.png"); // Montenegro (382)
            flagdict.Add("668", "/images/montenegro.png"); // montenegro Mobile (382-6)
            flagdict.Add("218", "/images/dominican-republic.png"); // Montserrat (1-809)
            flagdict.Add("424", "/images/dominican-republic.png"); // Montserrat Mobile (1-809)
            flagdict.Add("219", "/images/morocco.png"); // Morocco (212)
            flagdict.Add("1139", "/images/morocco.png"); // Morocco All Country (212)
            flagdict.Add("1140", "/images/morocco.png"); // Morocco Casablanca (212)
            flagdict.Add("600", "/images/morocco.png"); // Morocco Casablanca (212)
            flagdict.Add("1141", "/images/morocco.png"); // Morocco Fes (212)
            flagdict.Add("363", "/images/morocco.png"); // Morocco Mobile (212)
            flagdict.Add("1142", "/images/morocco.png"); // Morocco Mobile - Global Star (212)
            flagdict.Add("1143", "/images/morocco.png"); // Morocco Mobile - Maroc (212)
            flagdict.Add("1144", "/images/morocco.png"); // Morocco Mobile - Meditel (212)
            flagdict.Add("1145", "/images/morocco.png"); // Morocco Mobile - Wana (212)
            flagdict.Add("1146", "/images/morocco.png"); // Morocco Rabat (212)
            flagdict.Add("220", "/images/mozambique.png"); // Mozambique (258)
            flagdict.Add("601", "/images/mozambique.png"); // Mozambique Maputo (258)
            flagdict.Add("425", "/images/mozambique.png"); // Mozambique Mobile (258)
            flagdict.Add("1147", "/images/mozambique.png"); // Mozambique Mobile - Mcel (258)
            flagdict.Add("1148", "/images/mozambique.png"); // Mozambique Mobile - Vodacom (258)
            flagdict.Add("221", "/images/myanmar.png"); // Myanmar (95)
            flagdict.Add("482", "/images/myanmar.png"); // Myanmar Mobile (95)
            flagdict.Add("222", "/images/namibia.png"); // Namibia (264)
            flagdict.Add("364", "/images/namibia.png"); // Namibia Mobile (264)
            flagdict.Add("223", "/images/nauru.png"); // Nauru (674)
            flagdict.Add("602", "/images/nauru.png"); // Nauru Mobile (674)
            flagdict.Add("224", "/images/nepal.png"); // Nepal (977)
            flagdict.Add("225", "/images/nepal.png"); // Nepal Kathmandu (977)
            flagdict.Add("330", "/images/nepal.png"); // Nepal Mobile (977)
            flagdict.Add("1149", "/images/nepal.png"); // Nepal Mobile - Mero (977)
            flagdict.Add("1150", "/images/nepal.png"); // Nepal Mobile - Ntc (977)
            flagdict.Add("227", "/images/netherlands.png"); // Netherlands (31)
            flagdict.Add("603", "/images/netherlands.png"); // Netherlands Amsterdam (31)
            flagdict.Add("226", "/images/netherlands-antilles.png"); // Netherlands Antilles (599)
            flagdict.Add("1151", "/images/netherlands-antilles.png"); // Netherlands Antilles Curacao (559)
            flagdict.Add("1152", "/images/netherlands-antilles.png"); // Netherlands Antilles Curacao Mobile - Digicel (559)
            flagdict.Add("604", "/images/netherlands-antilles.png"); // Netherlands Antilles Mobile (599)
            flagdict.Add("1153", "/images/netherlands-antilles.png"); // Netherlands Antilles St Maarten (559)
            flagdict.Add("1154", "/images/netherlands-antilles.png"); // Netherlands Antilles St Marteens Mobile - ECC (559)
            flagdict.Add("426", "/images/netherlands.png"); // Netherlands Mobile (31)
            flagdict.Add("1155", "/images/netherlands.png"); // Netherlands Mobile - Kpn (31)
            flagdict.Add("1156", "/images/netherlands.png"); // Netherlands Mobile - Orange (31)
            flagdict.Add("1157", "/images/netherlands.png"); // Netherlands Mobile - Tele2 (31)
            flagdict.Add("1158", "/images/netherlands.png"); // Netherlands Mobile - Telfort (31)
            flagdict.Add("1159", "/images/netherlands.png"); // Netherlands Mobile - Tmobile (31)
            flagdict.Add("1160", "/images/netherlands.png"); // Netherlands Mobile - Vodafone (31)
            flagdict.Add("605", "/images/netherlands.png"); // Netherlands Rotterdam (31)
            flagdict.Add("228", "/images/new-caledonia.png"); // New Caledonia (687)
            flagdict.Add("606", "/images/new-caledonia.png"); // New Caledonia Mobile (687)
            flagdict.Add("229", "/images/new-zealand.png"); // New Zealand (64)
            flagdict.Add("607", "/images/new-zealand.png"); // New Zealand Mobile (64)
            flagdict.Add("1161", "/images/new-zealand.png"); // New Zealand Mobile - Nz Communications (64)
            flagdict.Add("1162", "/images/new-zealand.png"); // New Zealand Mobile - Telecom Nz (64)
            flagdict.Add("1163", "/images/new-zealand.png"); // New Zealand Mobile - Vodafone Nz (64)
            flagdict.Add("230", "/images/nicaragua.png"); // Nicaragua (505)
            flagdict.Add("608", "/images/nicaragua.png"); // Nicaragua Mobile (505)
            flagdict.Add("1164", "/images/nicaragua.png"); // Nicaragua Mobile - Bell South (505)
            flagdict.Add("1165", "/images/nicaragua.png"); // Nicaragua Mobile - Enitel (505)
            flagdict.Add("231", "/images/niger.png"); // Niger (227)
            flagdict.Add("609", "/images/niger.png"); // Niger Mobile (227)
            flagdict.Add("232", "/images/nigeria-flag.png"); // Nigeria (234)
            flagdict.Add("365", "/images/nigeria-flag.png"); // Nigeria Lagos (234)
            flagdict.Add("233", "/images/nigeria-flag.png"); // Nigeria Mobile (234)
            flagdict.Add("1166", "/images/nigeria-flag.png"); // Nigeria Mobile - Etisalat (234)
            flagdict.Add("1167", "/images/nigeria-flag.png"); // Nigeria Mobile - Globacom (234)
            flagdict.Add("1168", "/images/nigeria-flag.png"); // Nigeria Mobile - Mtel (234)
            flagdict.Add("1169", "/images/nigeria-flag.png"); // Nigeria Mobile - Mtn (234)
            flagdict.Add("1170", "/images/nigeria-flag.png"); // Nigeria Mobile - Zain Econet (234)
            flagdict.Add("235", "/images/niue_flag.png"); // Niue Island (683)
            flagdict.Add("610", "/images/niue_flag.png"); // Niue Island Mobile (683)
            flagdict.Add("427", "/images/norfolk_flag.png"); // Norfolk Island (672-3)
            flagdict.Add("611", "/images/norfolk_flag.png"); // Norfolk Island Mobile (672-3)
            flagdict.Add("428", "/images/north_korea_flag.png"); // North Korea (850)
            flagdict.Add("483", "/images/north_korea_flag.png"); // North Korea Mobile (850)
            flagdict.Add("236", "/images/norway.png"); // Norway (47)
            flagdict.Add("429", "/images/norway.png"); // Norway Mobile (47)
            flagdict.Add("1171", "/images/norway.png"); // Norway Mobile - Netcom (47)
            flagdict.Add("1172", "/images/norway.png"); // Norway Mobile - Tele2 (47)
            flagdict.Add("1173", "/images/norway.png"); // Norway Mobile - Telenor (47)
            flagdict.Add("1174", "/images/norway.png"); // Norway Oslo (47)
            flagdict.Add("1175", "/images/norway.png"); // Norway Special (47)
            flagdict.Add("237", "/images/oman.png"); // Oman (968)
            flagdict.Add("234", "/images/oman.png"); // Oman Mobile (968)
            flagdict.Add("238", "/images/pakistan.png"); // Pakistan (92)
            flagdict.Add("612", "/images/pakistan.png"); // Pakistan Islamabad (92)
            flagdict.Add("366", "/images/pakistan.png"); // Pakistan Karachi (92)
            flagdict.Add("239", "/images/pakistan.png"); // Pakistan Lahore (92)
            flagdict.Add("240", "/images/pakistan.png"); // Pakistan Mobile (92)
            flagdict.Add("242", "/images/palau.png"); // Palau (680)
            flagdict.Add("613", "/images/palau.png"); // Palau Mobile (680)
            flagdict.Add("385", "/images/palestine.png"); // Palestine (970)
            flagdict.Add("485", "/images/palestine.png"); // Palestine Mobile (970)
            flagdict.Add("243", "/images/panama.png"); // Panama (507)
            flagdict.Add("484", "/images/panama.png"); // Panama Mobile (507)
            flagdict.Add("614", "/images/panama.png"); // Panama Panama City (507)
            flagdict.Add("244", "/images/papua-new-guinea.png"); // Papua New Guinea (675)
            flagdict.Add("615", "/images/papua-new-guinea.png"); // Papua New Guinea Mobile (675)
            flagdict.Add("245", "/images/paraguay.png"); // Paraguay (595)
            flagdict.Add("430", "/images/paraguay.png"); // Paraguay Mobile (595)
            flagdict.Add("246", "/images/peru.png"); // Peru (51)
            flagdict.Add("1176", "/images/peru.png"); // Peru All Country (51)
            flagdict.Add("247", "/images/peru.png"); // Peru Lima (51)
            flagdict.Add("1177", "/images/peru.png"); // Peru Mobile (51)
            flagdict.Add("1178", "/images/peru.png"); // Peru Mobile - Claro (51)
            flagdict.Add("1179", "/images/peru.png"); // Peru Mobile - Lima (51)
            flagdict.Add("1180", "/images/peru.png"); // Peru Mobile - Movistar (51)
            flagdict.Add("1181", "/images/peru.png"); // Peru Mobile - Nextel (51)
            flagdict.Add("1182", "/images/peru.png"); // Peru Mobile - Tesam (51)
            flagdict.Add("431", "/images/peru.png"); // Peru Mobile Hudashka (51)
            flagdict.Add("1183", "/images/peru.png"); // Peru Rural (51)
            flagdict.Add("248", "/images/philippines.png"); // Philippines (63)
            flagdict.Add("616", "/images/philippines.png"); // Philippines Manila (63)
            flagdict.Add("617", "/images/philippines.png"); // Philippines Manila Bayantel (63)
            flagdict.Add("241", "/images/philippines.png"); // Philippines Mobile (63)
            flagdict.Add("1184", "/images/philippines.png"); // Philippines Mobile - Digitel (63)
            flagdict.Add("1185", "/images/philippines.png"); // Philippines Mobile - Globe (63)
            flagdict.Add("1186", "/images/philippines.png"); // Philippines Mobile - Smart (63)
            flagdict.Add("1187", "/images/philippines.png"); // Philippines Mobile - Touch (63)
            flagdict.Add("249", "/images/poland_flag.png"); // Poland (48)
            flagdict.Add("367", "/images/poland_flag.png"); // Poland Mobile (48)
            flagdict.Add("1188", "/images/poland_flag.png"); // Poland Mobile - Centernet (48)
            flagdict.Add("1189", "/images/poland_flag.png"); // Poland Mobile - Orange (48)
            flagdict.Add("1190", "/images/poland_flag.png"); // Poland Mobile - P4 (48)
            flagdict.Add("1191", "/images/poland_flag.png"); // Poland Mobile - Polkomtel (48)
            flagdict.Add("1192", "/images/poland_flag.png"); // Poland Mobile - Polsat (48)
            flagdict.Add("1193", "/images/poland_flag.png"); // Poland Mobile - PTC (48)
            flagdict.Add("618", "/images/poland_flag.png"); // Poland Warsaw (48)
            flagdict.Add("251", "/images/portugal.png"); // Portugal (351)
            flagdict.Add("619", "/images/portugal.png"); // Portugal Lisbon (351)
            flagdict.Add("620", "/images/portugal.png"); // Portugal Madeira Island (351)
            flagdict.Add("432", "/images/portugal.png"); // Portugal Mobile (351)
            flagdict.Add("621", "/images/portugal.png"); // Portugal Porto (351)
            flagdict.Add("252", "/images/Portrico.jpg"); // Puerto Rico (1-787)
            flagdict.Add("486", "/images/Portrico.jpg"); // Puerto Rico Mobile (1-787)
            flagdict.Add("253", "/images/qatar.png"); // Qatar (974)
            flagdict.Add("368", "/images/qatar.png"); // Qatar Mobile (974)
            flagdict.Add("255", "/images/reunion.png"); // Reunion Island (262)
            flagdict.Add("622", "/images/reunion.png"); // Reunion Island Mobile (262)
            flagdict.Add("256", "/images/romania.png"); // Romania (40)
            flagdict.Add("257", "/images/romania.png"); // Romania Bucharest (40)
            flagdict.Add("369", "/images/romania.png"); // Romania Mobile (40)
            flagdict.Add("1194", "/images/romania.png"); // Romania Mobile - Romtelecom (40)
            flagdict.Add("258", "/images/russian-federation.png"); // Russia (7)
            flagdict.Add("370", "/images/russian-federation.png"); // Russia Mobile (7)
            flagdict.Add("1195", "/images/russian-federation.png"); // Russia Mobile - Beeline (7)
            flagdict.Add("1196", "/images/russian-federation.png"); // Russia Mobile - Megafon (7)
            flagdict.Add("1197", "/images/russian-federation.png"); // Russia Mobile - Mts (7)
            flagdict.Add("259", "/images/russian-federation.png"); // Russia Moscow (7)
            flagdict.Add("623", "/images/russian-federation.png"); // Russia Sakhalin (7)
            flagdict.Add("624", "/images/russian-federation.png"); // Russia St Petersburg (7)
            flagdict.Add("260", "/images/rwanda.png"); // Rwanda (250)
            flagdict.Add("433", "/images/rwanda.png"); // Rwanda Mobile (250)
            flagdict.Add("1198", "/images/russian-federation.png"); // Rwanda Mobile - Tigo (7)
            flagdict.Add("250", "/images/saipan.png"); // Saipan (670)
            flagdict.Add("625", "/images/saipan.png"); // Saipan Mobile (670)
            flagdict.Add("261", "/images/san-marino.png"); // San Marino (378)
            flagdict.Add("626", "/images/san-marino.png"); // San Marino Mobile (378)
            flagdict.Add("627", "/images/sao-tome.png"); // Sao Tome (239)
            flagdict.Add("628", "/images/sao-tome.png"); // Sao Tome Mobile (239)
            flagdict.Add("265", "/images/saudi-arabia.png"); // Saudi Arabia (966)
            flagdict.Add("362", "/images/saudi-arabia.png"); // Saudi Arabia Jeddah (966)
            flagdict.Add("264", "/images/saudi-arabia.png"); // Saudi Arabia Mobile (966)
            flagdict.Add("262", "/images/saudi-arabia.png"); // Saudi Arabia Riyadh (966)
            flagdict.Add("266", "/images/senegal.png"); // Senegal (221)
            flagdict.Add("434", "/images/senegal.png"); // Senegal Mobile (221)
            flagdict.Add("1199", "/images/senegal.png"); // Senegal Mobile - Orange (221)
            flagdict.Add("1200", "/images/senegal.png"); // Senegal Mobile - Sentel (221)
            flagdict.Add("1201", "/images/senegal.png"); // Senegal Mobile - Sudatel (221)
            flagdict.Add("267", "/images/serbia.png"); // Serbia (38)
            flagdict.Add("1202", "/images/serbia.png"); // Serbia Mobile - Mobilkom (381-6)
            flagdict.Add("1203", "/images/serbia.png"); // Serbia Mobile - Telcom Serbia (381-6)
            flagdict.Add("1204", "/images/serbia.png"); // Serbia Mobile - Telenor (381-6)
            flagdict.Add("669", "/images/serbia.png"); // Serbia-Mobile (381-6)
            flagdict.Add("268", "/images/seyshelles.png"); // Seychelles Island (248)
            flagdict.Add("629", "/images/seyshelles.png"); // Seychelles Island Mobile (248)
            flagdict.Add("269", "/images/sierra-leone.png"); // Sierra Leone (232)
            flagdict.Add("630", "/images/sierra-leone.png"); // Sierra Leone Mobile (232)
            flagdict.Add("270", "/images/singapore.png"); // Singapore (65)
            flagdict.Add("263", "/images/singapore.png"); // Singapore Mobile (65)
            flagdict.Add("271", "/images/slovakia.png"); // Slovakia (421)
            flagdict.Add("487", "/images/slovakia.png"); // Slovakia Mobile (421)
            flagdict.Add("1205", "/images/slovakia.png"); // Slovakia Mobile - Orange (421)
            flagdict.Add("1206", "/images/slovakia.png"); // Slovakia Mobile - Telefonica O2 (421)
            flagdict.Add("1207", "/images/slovakia.png"); // Slovakia Mobile - Tmobile (421)
            flagdict.Add("1208", "/images/slovakia.png"); // Slovakia Vas (421)
            flagdict.Add("272", "/images/slovenia.png"); // Slovenia (836)
            flagdict.Add("488", "/images/slovenia.png"); // Slovenia Mobile (836)
            flagdict.Add("1209", "/images/slovenia.png"); // Slovenia Mobile - Ipko (836)
            flagdict.Add("1210", "/images/slovakia.png"); // Slovenia Mobile - Mobitel (421)
            flagdict.Add("1211", "/images/slovenia.png"); // Slovenia Mobile - Simobil (836)
            flagdict.Add("1212", "/images/slovenia.png"); // Slovenia Mobile - T2 (836)
            flagdict.Add("1213", "/images/slovenia.png"); // Slovenia Mobile - Tus (836)
            flagdict.Add("273", "/images/solomon-islands.png"); // Solomon Island (677)
            flagdict.Add("489", "/images/solomon-islands.png"); // Solomon Island Mobile (677)
            flagdict.Add("274", "/images/somalia.png"); // Somalia (252)
            flagdict.Add("1214", "/images/somalia.png"); // Somalia Golis (252)
            flagdict.Add("1215", "/images/somalia.png"); // Somalia Hormud (252)
            flagdict.Add("490", "/images/somalia.png"); // Somalia Mobile (252)
            flagdict.Add("1216", "/images/somalia.png"); // Somalia Soltelco (252)
            flagdict.Add("1217", "/images/somalia.png"); // Somalia Somtel (252)
            flagdict.Add("1218", "/images/somalia.png"); // Somalia Telecom (252)
            flagdict.Add("275", "/images/south-afriica.png"); // South Africa (27)
            flagdict.Add("631", "/images/south-afriica.png"); // South Africa Cape Town (27)
            flagdict.Add("632", "/images/south-afriica.png"); // South Africa Johannesburg (27)
            flagdict.Add("361", "/images/south-afriica.png"); // South Africa Mobile (27)
            flagdict.Add("1219", "/images/south-afriica.png"); // South Africa Mobile - Vodacom (27)
            flagdict.Add("277", "/images/korea_flag.png"); // South Korea (82)
            flagdict.Add("383", "/images/korea_flag.png"); // South Korea Mobile (82)
            flagdict.Add("633", "/images/korea_flag.png"); // South Korea Seoul (82)
            flagdict.Add("278", "/images/spain.png"); // Spain (34)
            flagdict.Add("634", "/images/spain.png"); // Spain Balearic Island (34)
            flagdict.Add("279", "/images/spain.png"); // Spain Barcelona (34)
            flagdict.Add("635", "/images/spain.png"); // Spain Canary Islands (34)
            flagdict.Add("280", "/images/spain.png"); // Spain Madrid (34)
            flagdict.Add("276", "/images/spain.png"); // Spain Mobile (34)
            flagdict.Add("1220", "/images/spain.png"); // Spain Mobile - Yoigo (34)
            flagdict.Add("281", "/images/sri-lanka.png"); // Sri Lanka (94)
            flagdict.Add("283", "/images/sri-lanka.png"); // Sri Lanka Colombo (94)
            flagdict.Add("360", "/images/sri-lanka.png"); // Sri Lanka Mobile (94)
            flagdict.Add("1221", "/images/sri-lanka.png"); // Sri Lanka Mobile - Celltel (94)
            flagdict.Add("1222", "/images/sri-lanka.png"); // Sri Lanka Mobile - Hutch (94)
            flagdict.Add("1223", "/images/kitts_nevis.png"); // St Kitts & Nevis Mobile - C&W (1-869)
            flagdict.Add("1224", "/images/kitts_nevis.png"); // St Kitts & Nevis Mobile - Digicel (1-869)
            flagdict.Add("1225", "/images/dominican-republic.png"); // St Lucia Mobile - C&W (1-809)
            flagdict.Add("1226", "/images/dominican-republic.png"); // St Lucia Mobile - Digicel (1-809)
            flagdict.Add("1227", "/images/St-Vincent-&-the-Grenadines.png"); // St Vincent & Grenadines Mobile (1-784)
            flagdict.Add("1228", "/images/St-Vincent-&-the-Grenadines.png"); // St Vincent & Grenadines Mobile - Digicel (1-784)
            flagdict.Add("284", "/images/helena_flag.png"); // St. Helena (290)
            flagdict.Add("636", "/images/helena_flag.png"); // St. Helena Mobile (290)
            flagdict.Add("285", "/images/kitts_nevis.png"); // St. Kitts & Nevis (1-869)
            flagdict.Add("435", "/images/kitts_nevis.png"); // St. Kitts & Nevis Mobile (1-869)
            flagdict.Add("286", "/images/dominican-republic.png"); // St. Lucia (1-809)
            flagdict.Add("436", "/images/dominican-republic.png"); // St. Lucia Mobile (1-809)
            flagdict.Add("287", "/images/finland_flag.png"); // St. Martin (000)
            flagdict.Add("288", "/images/pierre_mequelon_flag.png"); // St. Pierre & Mequelon (508)
            flagdict.Add("637", "/images/pierre_mequelon_flag.png"); // St. Pierre & Mequelon Mobile (508)
            flagdict.Add("379", "/images/dominican-republic.png"); // St. Vincent (1-809)
            flagdict.Add("437", "/images/dominican-republic.png"); // St. Vincent Mobile (1-809)
            flagdict.Add("289", "/images/sudan.png"); // Sudan (249)
            flagdict.Add("491", "/images/sudan.png"); // Sudan Mobile (249)
            flagdict.Add("1229", "/images/sudan.png"); // Sudan Mobile - Now (249)
            flagdict.Add("290", "/images/suriname.png"); // Suriname (597)
            flagdict.Add("638", "/images/suriname.png"); // Suriname Mobile (597)
            flagdict.Add("1230", "/images/suriname.png"); // Suriname Mobile - Digicel (597)
            flagdict.Add("291", "/images/swaziland.png"); // Swaziland (268)
            flagdict.Add("492", "/images/swaziland.png"); // Swaziland Mobile (268)
            flagdict.Add("292", "/images/sweden.png"); // Sweden (46)
            flagdict.Add("374", "/images/sweden.png"); // Sweden Mobile (46)
            flagdict.Add("293", "/images/sweden.png"); // Sweden Stockholm (46)
            flagdict.Add("294", "/images/switzerland.png"); // Switzerland (41)
            flagdict.Add("372", "/images/switzerland.png"); // Switzerland Mobile (41)
            flagdict.Add("1231", "/images/switzerland.png"); // Switzerland Mobile - 3G (41)
            flagdict.Add("1232", "/images/switzerland.png"); // Switzerland Mobile - Orange (41)
            flagdict.Add("1233", "/images/switzerland.png"); // Switzerland Mobile - Sunrise (41)
            flagdict.Add("1234", "/images/switzerland.png"); // Switzerland Mobile - Swisscom (41)
            flagdict.Add("1235", "/images/switzerland.png"); // Switzerland Mobile - Tele2 (41)
            flagdict.Add("1236", "/images/switzerland.png"); // Switzerland Mobile - TNet (41)
            flagdict.Add("1237", "/images/switzerland.png"); // Switzerland Special (41)
            flagdict.Add("639", "/images/switzerland.png"); // Switzerland Zurich (41)
            flagdict.Add("295", "/images/syria.png"); // Syria (963)
            flagdict.Add("438", "/images/syria.png"); // Syria Mobile (963)
            flagdict.Add("296", "/images/taiwan.png"); // Taiwan (886)
            flagdict.Add("640", "/images/taiwan.png"); // Taiwan Kaohsiung (886)
            flagdict.Add("439", "/images/taiwan.png"); // Taiwan Mobile (886)
            flagdict.Add("641", "/images/taiwan.png"); // Taiwan Taipei (886)
            flagdict.Add("297", "/images/tajikistan.png"); // Tajikistan (992)
            flagdict.Add("677", "/images/tajikistan.png"); // Tajikistan Babilon Mobile (992 918)
            flagdict.Add("674", "/images/tajikistan.png"); // Tajikistan Dushanbe (992 372)
            flagdict.Add("298", "/images/tajikistan.png"); // Tajikistan Khorog (992 35222)
            flagdict.Add("675", "/images/tajikistan.png"); // Tajikistan Kulob (992 3322)
            flagdict.Add("493", "/images/tajikistan.png"); // Tajikistan Vahdat (992 3631)
            flagdict.Add("676", "/images/tajikistan.png"); // Tajikitan Indigo Mobile (992 93)
            flagdict.Add("679", "/images/tanzania.png"); // Tanzania (255)
            flagdict.Add("642", "/images/tanzania.png"); // Tanzania Dar es Salaam (255)
            flagdict.Add("375", "/images/tanzania.png"); // Tanzania Mobile (255)
            flagdict.Add("1238", "/images/tanzania.png"); // Tanzania Mobile - Zain Celtel (255)
            flagdict.Add("299", "/images/thailand.png"); // Thailand (66)
            flagdict.Add("300", "/images/thailand.png"); // Thailand Bangkok (66)
            flagdict.Add("643", "/images/thailand.png"); // Thailand Chang Mai (66)
            flagdict.Add("440", "/images/thailand.png"); // Thailand Mobile (66)
            flagdict.Add("301", "/images/togo.png"); // Togo (228)
            flagdict.Add("441", "/images/togo.png"); // Togo Mobile (228)
            flagdict.Add("1239", "/images/togo.png"); // Togo Mobile - Telecel (228)
            flagdict.Add("1240", "/images/togo.png"); // Togo Mobile - Togocel (228)
            flagdict.Add("302", "/images/tonga.png"); // Tonga (676)
            flagdict.Add("644", "/images/tonga.png"); // Tonga Mobile (676)
            flagdict.Add("303", "/images/trinidad.png"); // Trinidad & Tobago (1-868)
            flagdict.Add("373", "/images/trinidad.png"); // Trinidad & Tobago Mobile (1-868)
            flagdict.Add("1241", "/images/trinidad.png"); // Trinidad & Tobago Mobile - Digicel (1-868)
            flagdict.Add("304", "/images/tunisia.png"); // Tunisia (216)
            flagdict.Add("443", "/images/tunisia.png"); // Tunisia Mobile (216)
            flagdict.Add("305", "/images/turkey.png"); // Turkey (90)
            flagdict.Add("645", "/images/turkey.png"); // Turkey Ankara (90)
            flagdict.Add("306", "/images/turkey.png"); // Turkey Istanbul (90)
            flagdict.Add("282", "/images/turkey.png"); // Turkey Mobile (90)
            flagdict.Add("1242", "/images/turkey.png"); // Turkey Mobile - Avea (90)
            flagdict.Add("1243", "/images/turkey.png"); // Turkey Mobile - Aycell (90)
            flagdict.Add("1244", "/images/turkey.png"); // Turkey Mobile - Telsim (90)
            flagdict.Add("1245", "/images/turkey.png"); // Turkey Mobile - Turkcell (90)
            flagdict.Add("1246", "/images/turkey.png"); // Turkey Mobile - Vodafone (90)
            flagdict.Add("1247", "/images/turkey.png"); // Turkey North Cyprus Mobile (90)
            flagdict.Add("1248", "/images/turkey.png"); // Turkey North Cyprus Mobile - Turkcell (90)
            flagdict.Add("1249", "/images/turkey.png"); // Turkey North Cyprus Mobile - Vodafone (90)
            flagdict.Add("307", "/images/turk.png"); // Turkmenistan (993)
            flagdict.Add("646", "/images/turk.png"); // Turkmenistan Mobile (993)
            flagdict.Add("308", "/images/tuvalu.png"); // Tuvalu (688)
            flagdict.Add("647", "/images/tuvalu.png"); // Tuvalu Mobile (688)
            flagdict.Add("315", "/images/uganda.png"); // Uganda (256)
            flagdict.Add("357", "/images/uganda.png"); // Uganda Mobile (256)
            flagdict.Add("1250", "/images/uganda.png"); // Uganda Mobile - Celtel Uganda (256)
            flagdict.Add("1251", "/images/uganda.png"); // Uganda Mobile - Gemtel (256)
            flagdict.Add("1252", "/images/uganda.png"); // Uganda Mobile - Mtn (256)
            flagdict.Add("1253", "/images/uganda.png"); // Uganda Mobile - Orange (256)
            flagdict.Add("1254", "/images/uganda.png"); // Uganda Mobile - Uganda Telecom (256)
            flagdict.Add("1255", "/images/uganda.png"); // Uganda Mobile - Warid (256)
            flagdict.Add("316", "/images/ukraine.png"); // Ukraine (380)
            flagdict.Add("648", "/images/ukraine.png"); // Ukraine Dnepropetrovsk (380)
            flagdict.Add("649", "/images/ukraine.png"); // Ukraine Donetsk (380)
            flagdict.Add("650", "/images/ukraine.png"); // Ukraine Kharkov (380)
            flagdict.Add("651", "/images/ukraine.png"); // Ukraine Kiev (380)
            flagdict.Add("652", "/images/ukraine.png"); // Ukraine Lviv (380)
            flagdict.Add("358", "/images/ukraine.png"); // Ukraine Mobile (380)
            flagdict.Add("1256", "/images/ukraine.png"); // Ukraine Mobile - Astelit (380)
            flagdict.Add("1257", "/images/ukraine.png"); // Ukraine Mobile - Beeline (380)
            flagdict.Add("1258", "/images/ukraine.png"); // Ukraine Mobile - Golden Telecom (380)
            flagdict.Add("1259", "/images/ukraine.png"); // Ukraine Mobile - Kiyvstar (380)
            flagdict.Add("1260", "/images/ukraine.png"); // Ukraine Mobile - Ukr Telecom (380)
            flagdict.Add("1261", "/images/ukraine.png"); // Ukraine Mobile - Umc (380)
            flagdict.Add("653", "/images/ukraine.png"); // Ukraine Nicolaev (380)
            flagdict.Add("654", "/images/ukraine.png"); // Ukraine Odessa (380)
            flagdict.Add("309", "/images/united-arab-emirates.png"); // United Arab Emirates (971)
            flagdict.Add("376", "/images/united-arab-emirates.png"); // United Arab Emirates Mobile (971)
            flagdict.Add("312", "/images/united-Kingdom_flat.png"); // United Kingdom (44)
            flagdict.Add("356", "/images/united-Kingdom_flat.png"); // United Kingdom London (44)
            flagdict.Add("310", "/images/united-Kingdom_flat.png"); // United Kingdom Mobile (44)
            flagdict.Add("1262", "/images/united-Kingdom_flat.png"); // United Kingdom Mobile - PNS (44)
            flagdict.Add("1263", "/images/united-Kingdom_flat.png"); // United Kingdom Special NTS 843 (44)
            flagdict.Add("1264", "/images/united-Kingdom_flat.png"); // United Kingdom Special NTS 844 (44)
            flagdict.Add("1265", "/images/united-Kingdom_flat.png"); // United Kingdom Special NTS 845 (44)
            flagdict.Add("1266", "/images/united-Kingdom_flat.png"); // United Kingdom Special NTS 870 (44)
            flagdict.Add("1267", "/images/united-Kingdom_flat.png"); // United Kingdom Special NTS 871 (44)
            flagdict.Add("1268", "/images/united-Kingdom_flat.png"); // United Kingdom Special NTS 872 (44)
            flagdict.Add("1269", "/images/united-Kingdom_flat.png"); // United Kingdom Special NTS 873 (44)
            flagdict.Add("317", "/images/uruguay.png"); // Uruguay (598)
            flagdict.Add("444", "/images/uruguay.png"); // Uruguay Mobile (598)
            flagdict.Add("313", "/images/virgin-islands-british.png"); // US Virgin Islands (1-340)
            flagdict.Add("1270", "/images/virgin-islands-british.png"); // US Virgin Islands All Country (1-340)
            flagdict.Add("314", "/images/us.png"); // USA (1)
            flagdict.Add("1271", "/images/alaska.png"); // USA - Alaska (1)
            flagdict.Add("1272", "/images/us.png"); // USA - Domestic (1)
            flagdict.Add("1273", "/images/hawaii_flag.png"); // USA - Hawaii (1-808)
            flagdict.Add("318", "/images/russian-federation.png"); // Uzbekistan (7)
            flagdict.Add("655", "/images/russian-federation.png"); // Uzbekistan Mobile (7)
            flagdict.Add("1274", "/images/russian-federation.png"); // Uzbekistan Tashkent (7)
            flagdict.Add("319", "/images/vanutau.png"); // Vanuatu (678)
            flagdict.Add("656", "/images/vanutau.png"); // Vanuatu Mobile (678)
            flagdict.Add("320", "/images/vatican.png"); // Vatican City (379)
            flagdict.Add("657", "/images/vatican.png"); // Vatican City Mobile (379)
            flagdict.Add("321", "/images/venezuela.png"); // Venezuela (58)
            flagdict.Add("658", "/images/venezuela.png"); // Venezuela Barquisimeto (58)
            flagdict.Add("322", "/images/venezuela.png"); // Venezuela Caracas (58)
            flagdict.Add("659", "/images/venezuela.png"); // Venezuela Cristbol (58)
            flagdict.Add("660", "/images/venezuela.png"); // Venezuela Maricabo (58)
            flagdict.Add("661", "/images/venezuela.png"); // Venezuela Maricay (58)
            flagdict.Add("445", "/images/venezuela.png"); // Venezuela Mobile (58)
            flagdict.Add("323", "/images/viet-nam.png"); // Vietnam (84)
            flagdict.Add("662", "/images/viet-nam.png"); // Vietnam Ho Chi Minh (84)
            flagdict.Add("359", "/images/viet-nam.png"); // Vietnam Mobile (84)
            flagdict.Add("663", "/images/wales.png"); // Wallis & Futuna (681)
            flagdict.Add("664", "/images/wales.png"); // Wallis & Futuna Mobile (681)
            flagdict.Add("324", "/images/west_samoa.png"); // West Samoa (685)
            flagdict.Add("665", "/images/west_samoa.png"); // West Samoa Mobile (685)
            flagdict.Add("325", "/images/yemen.png"); // Yemen (969)
            flagdict.Add("384", "/images/yemen.png"); // Yemen Mobile (969)
            flagdict.Add("326", "/images/kosovo_flag.png"); // Yugoslavia (381)
            flagdict.Add("494", "/images/kosovo_flag.png"); // Yugoslavia Mobile (381)
            flagdict.Add("328", "/images/zambia.png"); // Zambia (260)
            flagdict.Add("447", "/images/zambia.png"); // Zambia Mobile (260)
            flagdict.Add("448", "/images/zimbabwe.png"); // Zimbabwe (263)
            flagdict.Add("329", "/images/zimbabwe.png"); // Zimbabwe Mobile (263)
            flagdict.Add("1275", "/images/zimbabwe.png"); // Zimbabwe Mobile - Econet (263)
            flagdict.Add("1276", "/images/zimbabwe.png"); // Zimbabwe Mobile - Netone (263)
            flagdict.Add("1277", "/images/zimbabwe.png"); // Zimbabwe Mobile - Telecel (263)

            return flagdict;

        }

    }
}