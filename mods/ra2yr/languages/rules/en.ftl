## Player
options-tech-level =
   .one = 1
   .two = 2
   .three = 3
   .four = 4
   .five = 5

checkbox-redeployable-mcvs =
   .label = Redeployable MCVs
   .description = Allow undeploying Construction Yard

checkbox-free-minimap =
   .label = Free Minimap
   .description = Minimap is active without a building to enable it

checkbox-limit-super-weapons =
   .label = Limit Super Weapons
   .description = Only 1 of each super weapon can be built by a player

checkbox-tech-build-area =
   .label = Tech Build Area
   .description = Allow building placement around tech structures

checkbox-instant-capture =
   .label = Instant Capture
   .description = Engineers can enter a building without waiting to capture

checkbox-multiqueue =
   .label = MultiQueue
   .description = Each production facility can produce individually

checkbox-upgrades-option =
   .label = Upgrades
   .description = Enables researching upgrades that improve existing units

checkbox-domination-option =
   .label = Domination
   .description = Control the flags on the map to win

checkbox-megawealth-option =
   .label = Megawealth
   .description = Removes all the Ore on the map and makes the economy dependent on Oil Derricks

checkbox-show-owner-name =
   .label = Show Owner Name
   .description = Show name and flag of the owner of a unit on its tooltip

checkbox-sudden-death =
   .label = Sudden Death
   .description = Players can't build another MCV and get defeated when they lose it

checkbox-king-of-the-hill =
   .label = King of the Hill
   .description = Capture and hold the Psychic Beacon on the map to win

checkbox-regicide =
   .label = Regicide
   .description = Kill enemy leader to win the game

notification-insufficient-funds = Insufficient funds.
notification-new-construction-options = New construction options.
notification-cannot-deploy-here = Cannot deploy here.
notification-low-power = Low power.
notification-base-under-attack = Our base is under attack.
notification-ally-under-attack = Our ally is under attack.
notification-ore-miner-under-attack = Ore miner under attack.
notification-insufficient-silos = Insufficient silos.

## World
options-starting-units =
   .no-bases = No Bases
   .mcv-only = MCV Only
   .mcv-and-dog = MCV and Dog
   .light-support = Light Support
   .medium-support = Medium Support
   .heavy-support = Heavy Support
   .unholy-alliance = Unholy Alliance

## Defaults
notification-unit-lost = Unit lost.
notification-unit-promoted = Unit promoted.
notification-primary-building-selected = Primary building selected.
notification-building-captured = Building captured.
notification-tech-building-captured = Tech building captured.
notification-tech-building-lost = Tech building lost.

## Structures
notification-construction-complete = Construction complete.
notification-unit-ready = Unit ready.
notification-unable-to-build-more = Unable to build more.
notification-unable-to-comply-building-in-progress = Unable to comply. Building in progress.
notification-new-rally-point-established = New rally point established.
notification-repairing = Repairing.
notification-unit-repaired = Unit repaired.
notification-select-target = Select target.
notification-spy-plane-ready = Spy plane ready.
notification-paratroopers-ready = Paratroopers ready.
notification-force-shield-ready = Force Shield ready.
notification-force-shield-activated = Force Shield activated.
notification-lightning-storm-ready = Lightning Storm ready.
notification-lightning-storm-created = Warning: Lightning Storm created.
notification-weather-control-device-detected = Warning: Weather Control Device detected.
notification-chronosphere-ready = Chronosphere ready.
notification-chronosphere-activated = Warning: Chronosphere activated.
notification-chronosphere-detected = Warning: Chronosphere detected.
notification-iron-curtain-ready = Iron Curtain ready.
notification-iron-curtain-activated = Warning: Iron Curtain activated.
notification-iron-curtain-detected = Warning: Iron Curtain detected.
notification-nuclear-missile-ready = Nuclear Missile ready.
notification-nuclear-missile-launched = Warning: Nuclear Missile launched.
notification-nuclear-silo-detected = Warning: Nuclear Silo detected.
notification-genetic-mutator-ready = Genetic Mutator ready.
notification-genetic-mutator-activated = Warning: Genetic Mutator activated.
notification-genetic-mutator-detected = Warning: Genetic Mutator detected.
notification-psychic-dominator-ready = Psychic Dominator ready.
notification-psychic-dominator-activated = Warning: Psychic Dominator activated.
notification-psychic-dominator-detected = Warning: Psychic Dominator detected.

## aircraft.yaml
actor-shad =
   .name = Night Hawk
   .description = Infantry Transport Helicopter.
    
      Strong vs Infantry
      Weak vs Vehicles, Aircraft
    
    Abilties:
    - Can detect Stealth units
    - Invisible on radar

actor-zep =
   .name = Kirov Airship
   .description = Massive zeppelins that are the aerial terror of the Soviet force.
    
      Strong vs Buildings
      Weak vs Infantry, Vehicles
    
    Upgradeable with:
    - Radioactive Warheads
    - Advanced Irradiators (Iraq)
    - Aerial Propaganda (Vietnam)

actor-orca =
   .name = Harrier
   .description = Fast assault fighter.
    Cannot be built more than landing pads available.
    
      Strong vs Buildings, Infantry, Vehicles
      Weak vs Aircraft
    
    Upgradeable with:
    - Air-to-Air Missile Systems
    - Predator Missiles

actor-beag =
   .name = Black Eagle
   .description = Aircraft armed with EMP bomb.
    Cannot be built more than landing pads available.
      Strong vs Buildings, Vehicles
      Weak vs Infantry, Aircraft

actor-pdplane =
   .name = Cargo Plane
actor-hornet =
   .name = Hornet
actor-asw =
   .name = Osprey
actor-spyp =
   .name = Spy Plane

actor-schp =
   .name = Siege Chopper
   .description = Helicopter capable of deploying into a long range artillery.
    
    Mobile:
      Strong vs Infantry
      Weak vs Vehicles
    Deployed:
      Strong vs Buildings, Vehicles
      Weak vs Aircraft
    
    Abilities:
    - Can detect Stealth units
    
    Upgradeable with:
    - Armor-Piercing Bullets

actor-bpln =
   .name = MiG

actor-disk =
   .name = Leech Disc
   .description = Floating Disc armed with lasers.
    Can disable Power Plants and powered Defenses.
    Can steal resources from enemy Refineries.
    Can steal technology from enemy Battle Labs.
    
      Strong vs Infantry, Aircraft
      Weak vs Vehicles
    
    Upgradeable with:
    - Disc Armor (Lunar Eclipse)
    - Laser Capacitors (Lazarus Corps)

## allied-infantry.yaml
actor-engineer =
   .name = Engineer
   .description = Captures enemy structures, repairs damaged buildings and bridges.
    Disarms explosives.
    
    Abilities:
    - Can swim
    
      Unarmed

actor-dog =
   .name = Attack Dog
   .description = Anti-infantry unit.
    Can be deployed to stun nearby infantry for a short while.
    
      Strong vs Infantry
      Weak vs Vehicles, Aircraft
    
    Abilities:
    - Can swim
    - Can detect Stealth units
    - Can detect Spies

actor-e1 =
   .name = G.I.
   .description = General-purpose infantry.
    Can deploy to gain more range and damage.
    
      Strong vs Infantry
      Weak vs Vehicles, Aircraft
    
    Upgradeable with:
    - Fiber-Reinforced Fortifications
    - Advanced Training
    - Portable Weaponry (United Kingdom)

actor-ggi =
   .name = Guardian G.I.
   .description = Anti-tank and anti-air infantry.
    Can deploy to gain more range.
    
      Strong vs Vehicles, Aircraft
      Weak vs Buildings
    
    Upgradeable with:
    - Fiber-Reinforced Fortifications
    - Advanced Training
    - Boost-Gliding Systems (United States)
    - Portable Weaponry (United Kingdom)

actor-snipe =
   .name = Sniper
   .description = Special anti-infantry unit.
    
      Strong vs Infantry
      Weak vs Vehicles, Aircraft
    
    Abilities:
    - Can kill garrisoned infantry

actor-spy =
   .name = Spy
   .description = Infiltrates enemy structures for intel or sabotage.
    Exact effect depends on the
    building infiltrated.
    
      Unarmed
    
    Abilities:
    - Can swim
    - Can disguise as an enemy infantry

actor-ghost =
   .name = Navy SEAL
   .description = Elite commando infantry, armed with a sub machine gun
    and C4 charges.
    
      Strong vs Infantry, Buildings
      Weak vs Vehicles, Aircraft
    
    Abilities:
    - Can swim
    - Can place C4 on buildings
    
    Upgradeable with:
    - Advanced Training

actor-ccomand =
   .name = Chrono Commando
   .description = Elite commando infantry, armed with
    a sub machine gun and C4 charges.
    
      Strong vs Infantry, Buildings
      Weak vs Vehicles, Aircraft
    
    Abilities:
    - Can place C4 on buildings
    - Can teleport anywhere on the map
    
    Upgradeable with:
    - Advanced Training

actor-ptroop =
   .name = Psi Commando
   .description = Psychic infantry. Can mind control enemy units.
    
      Strong vs Infantry, Vehicles, Buildings
      Weak vs Dogs, Terror Drones, Aircraft
    
    Abilities:
    - Can place C4 on buildings
    
    Upgradeable with:
    - Mastery of Mind (Antarctica)

actor-tany =
   .name = Tanya Adams
   .description = Elite commando infantry, armed with
    dual pistols and C4 charges.
    
      Strong vs Infantry, Vehicles, Buildings
      Weak vs Aircraft
    
    Abilities:
    - Can swim
    - Can place C4 on buildings and vehicles
    
    Upgradeable with:
    - Advanced Training
    
      Maximum 1 can be trained

actor-jumpjet =
   .name = Rocketeer
   .description = Airborne soldier.
    
      Strong vs Infantry, Aircraft
      Weak vs Vehicles
    
    Upgradeable with:
    - Advanced training

actor-cleg =
   .name = Chrono Legionnaire
   .description = High-tech soldier capable of erasing enemy units.
      Strong vs Infantry, Vehicles, Buildings
      Weak vs Aircraft
    
    Abilities:
    - Can teleport anywhere on the map

## allied-naval.yaml
actor-dest =
   .name = Destroyer
   .description = Allied Main Battle Ship armed with cannons and
    an Osprey helicopter.
      Strong vs Naval units
      Weak vs Ground units, Aircraft
    
    Abilities:
    - Can detect Stealth units

actor-aegis =
   .name = Aegis Cruiser
   .description = Anti-Air naval unit.
    
      Strong vs Aircraft
      Weak vs Grounds units, Ships
    
    Upgradeable with:
    - Boost-Gliding Systems (United States)

actor-dlph =
   .name = Dolphin
   .description = Trained dolphin armed with sonic beams.
    
      Strong vs Ships
    
    Abilities:
    - Can remove Squids from ships by attacking them
    - Can detect Stealth units

actor-carrier =
   .name = Aircraft Carrier
   .description = Aircraft carrier ship.
    
      Strong vs Tanks, Structures
      Weak vs Infantry

actor-adest =
   .name = Assault Destroyer
   .description = Heavy vehicle armed with a cannon that can move over both land and water.
      Strong vs Vehicles, Ships
      Weak vs Infantry, Aircraft
    
    Abilities:
    - Can crush enemy land vehicles
    - Can detect Stealth units

## allied-structures.yaml
actor-gapowr =
   .name = Power Plant
   .description = Provides power for other structures.
actor-gapile =
   .description = Trains Allied infantry.
    Can heal nearby infantry.
    
      Cannot be placed on water.
      Can be rotated.

actor-gaairc =
   .name = Airforce Command Headquarters
   .description = Provides radar.
    Produces Allied aircraft.
    Supports 4 aircraft.
    Researches basic upgrades.
    
    Provides a different support power depening on the subfaction:
    - Airborne (United States)
    - Carpet Bombing (United Kingdom)
    - Force Shield (France)
    - Chrono Grizzly (Germany)
    - Chronobomb (Korea)
    
    Abilities:
    - Comes with 3 repair drones.

actor-gaweap =
   .description = Produces Allied vehicles.
    
    Abilities:
    - Comes with 3 repair drones.
    
      Cannot be placed on water.
      Can be rotated.

actor-gayard =
   .description = Produces Allied ships, and other naval units.
    
    Abilities:
    - Comes with 3 repair drones.
    
      Can only be placed on water.

actor-gawall =
   .name = Allied Wall
   .description = Heavy wall capable of blocking units and projectiles.
    
    Upgradeable with:
    - Grand Cannon Protocols (France)
    
      Cannot be placed on water.

actor-gapill =
   .name = Pill Box
   .description = Automated anti-infantry defense.
    
      Strong vs Infantry
      Weak vs Vehicles, Aircraft
    
    Abilities:
    - Can detect Steath units
    
    Upgradeable with:
    - Grand Cannon Protocols (France)

actor-nasam =
   .name = Patriot Missile System
   .description = Automated anti-aircraft defense.
    
      Strong vs Aircraft
      Weak vs Ground units
    
    Abilities:
    - Can detect Steath units
    
    Upgradeable with:
    - Boost-Gliding Systems (United States)
    - Grand Cannon Protocols (France)
    
      Requires power to operate.

actor-gtgcan =
   .name = Grand Cannon
   .description = Automated, long ranged anti-ground defense.
    
      Strong vs Buildings, Vehicles
      Weak vs Aircraft
    
      Requires power to operate.

actor-gaorep =
   .name = Ore Purifier
   .megawealth-name = Oil Purifier
   .description = Refines income from ores and gems by 25%.
    
      Maximum 1 can be built.

actor-gaspysat =
   .name = Spy Satellite Uplink
   .description = Provides Satellite Scan power, which reveals the map for a while.
    Provides Radar.
    
      Requires power to operate.

actor-gagap =
   .name = Gap Generator
   .description = Obscures the enemy's view with shroud.
    
      Requires power to operate.

actor-gaweat =
   .name = Weather Controller
   .description = Play God with deadly weather!
    
      Requires power to operate.

actor-gacsph =
   .name = Chronosphere
   .description = Allows teleportation of vehicles.
    Kills infantry.
    
      Requires power to operate.
      Can be rotated.

actor-atesla =
   .name = Prism Tower
   .description = Advanced base defense.
    Can buff at most 3 other Prism Towers nearby.
    
      Strong vs Infantry, Vehicles
      Weak vs Aircraft
    
    Abilities:
    - Can detect Steath units
    
    Upgradeable with:
    - Grand Cannon Protocols (France)
    
      Requires power to operate.

actor-garobo =
   .name = Robot Control Center
   .description = Allows production of Robot Tanks.
    Required for Robot Tanks to function.

## allied-vehicles.yaml
actor-cmin =
   .name = Chrono Miner
   .description = Gathers Ore and Gems.
    
      Unarmed
    
    Abilities:
    - Can move over water
    - Cannot be mind controlled
    - Can teleport back to refineries

actor-mtnk =
   .name = Grizzly Battle Tank
   .description = Allied Main Battle Tank.
    
      Strong vs Vehicles, Ships
      Weak vs Infantry, Aircraft
    
    Upgradeable with:
    - Composite Armor
    - Advanced Gun Systems (Germany)

actor-tnkd =
   .name = Tank Destroyer
   .description = Special anti-armor unit.
    
      Strong vs Vehicles, Ships
      Weak vs Infantry, Aircraft
    
    Upgradeable with:
    - Composite Armor
    - Advanced Gun Systems (Germany)

actor-fv =
   .name = Infantry Fighting Vehicle
   .mg-name = Machine Gun IFV
   .init-name = Ignitor IFV
   .rocket-name = Rocket IFV
   .gren-name = Grenade IFV
   .mortar-name = Mortar IFV
   .engineer-name = Repair IFV
   .medic-name = Ambulance IFV
   .dog-name = Detector IFV
   .hijack-name = Grinder IFV
   .sniper-name = Sniper IFV
   .virus-name = Virus IFV
   .pyro-name = Flamethrower IFV
   .flak-name = Flak IFV
   .gatling-name = Gatling IFV
   .tesla-name = Tesla IFV
   .desolator-name = Desolator IFV
   .demo-name = Demolition IFV
   .seal-name = Sub-Machine Gun IFV
   .tanya-name = Tanya IFV
   .boris-name = Boris IFV
   .chrono-name = Chrono IFV
   .yuri-name = Psi IFV
   .iron-name = Iron Curtain IFV
   .lazarus-name = Lazarus IFV
   .toxin-name = Toxin Sprayer IFV
   .crkt-name = Chaos Rocket IFV
   .description = Multi-Purpose Vehicle.
    
    Without passenger:
      Strong vs Infantry, Aircraft
      Weak vs Vehicles, Ships
    
    Abilities:
    - Armament changes depending on passenger
    - Can detect Stealth units
    
    Upgradeable with:
    - Boost-Gliding Systems (United States)

actor-sref =
   .name = Prism Tank
   .description = Fires deadly beams of light.
    
      Strong vs Infantry, Vehicles
      Weak vs Aircraft

actor-mgtk =
   .name = Mirage Tank
   .description = Tank that appears as a tree while immobile.
    
      Strong vs Infantry, Vehicles
      Weak vs Aircraft
    
    Upgradeable with:
    - Composite Armor

actor-bfrt =
   .name = Battle Fortress
   .description = Large vehicle with 5 fireports for infantry in to fire.
    
    Abilities:
    - Can crush enemy vehicles
    
    Upgradeable with:
    - Composite Armor

actor-robo =
   .name = Robot Tank
   .description = Remote controlled vehicle.
    Requires an Allied War Factory to operate.
    
      Strong vs Infantry
      Weak vs Vehicles, Aircraft
    
    Abilities:
    - Can move over water
    - Cannot be mind controlled

## animals.yaml
actor-cow =
   .name = Cow
actor-all =
   .name = Alligator
actor-polarb =
   .name = Polar Bear
actor-josh =
   .name = Monkey
actor-caml =
   .name = Camel
actor-dnoa =
   .name = T-Rex
actor-dnob =
   .name = Animal Bront

## bridges.yaml
actor-cabhut =
   .name = Bridge Repair Hut
meta-wooden-bridge =
   .name = Wooden Bridge
meta-concrete-bridge =
   .name = Concrete Bridge
meta-dead-bridge =
   .name = Dead Bridge
meta-bridge-ramp =
   .name = Bridge Ramp

## civilian-flags.yaml
actor-cadmfgl =
   .name = Flag
actor-causfgl =
   .name = American Flag
actor-caukfgl =
   .name = British Flag
actor-cafrfgl =
   .name = French Flag
actor-cagefgl =
   .name = German Flag
actor-caskfgl =
   .name = Korean Flag
actor-carufgl =
   .name = Soviet Flag
actor-cairfgl =
   .name = Iraqi Flag
actor-cavnfgl =
   .name = Vietnamese Flag
actor-cacufgl =
   .name = Cuban Flag
actor-calbfgl =
   .name = Libyan Flag
actor-capcfgl =
   .name = Yurigrad Flag
actor-caplfgl =
   .name = Lazarus Corps Flag
actor-capsfgl =
   .name = Antarctic Flag
actor-captfgl =
   .name = Transylvanian Flag
actor-capmfgl =
   .name = Lunar Eclipse Flag
actor-catcfgl =
   .name = Transcaucasian Flag
actor-catmfgl =
   .name = Turkmen Flag
actor-catvfgl =
   .name = Tuvan Flag
actor-carffgl =
   .name = Russian Flag
actor-casmfgl =
   .name = Serbo-Montenegrin Flag
actor-cajpfgl =
   .name = Japanese Flag
actor-cablfgl =
   .name = Belarusian Flag
actor-capofgl =
   .name = Polish Flag
actor-cauafgl =
   .name = Ukrainian Flag
actor-cachfgl =
   .name = Chinese Flag
actor-caaufgl =
   .name = Australian Flag
actor-catrfgl =
   .name = Turkish Flag
actor-cacnfgl =
   .name = Canadian Flag
actor-caclfgl =
   .name = Chilean Flag
actor-camxfgl =
   .name = Mexican Flag
actor-camofgl =
   .name = Mongolian Flag
actor-caarfgl =
   .name = Armenian SSR Flag
actor-caazfgl =
   .name = Azerbaijan SSR Flag
actor-cagofgl =
   .name = Georgian SSR Flag
actor-cakzfgl =
   .name = Kazakh SSR Flag
actor-cakyfgl =
   .name = Kyrgyz SSR Flag
actor-carsfgl =
   .name = Russian SFSR Flag
actor-catjfgl =
   .name = Tajik SSR Flag
actor-cauzfgl =
   .name = Uzbek SSR Flag
actor-caatfgl =
   .name = Austrian Flag
actor-cabefgl =
   .name = Belgian Flag
actor-cabrfgl =
   .name = Brazilian Flag
actor-cacyfgl =
   .name = Cypriot Flag
actor-caczfgl =
   .name = Czechoslovak Flag
actor-cadnfgl =
   .name = Danish Flag
actor-canlfgl =
   .name = Dutch Flag
actor-caesfgl =
   .name = Estonian Flag
actor-caphfgl =
   .name = Filipino Flag
actor-cafifgl =
   .name = Finnish Flag
actor-cagrfgl =
   .name = Greek Flag
actor-cahufgl =
   .name = Hungarian Flag
actor-cainfgl =
   .name = Indonesian Flag
actor-caeifgl =
   .name = Irish Flag
actor-caitfgl =
   .name = Italian Flag
actor-calafgl =
   .name = Latvian Flag
actor-calefgl =
   .name = Lebanese Flag
actor-califgl =
   .name = Lithuanian Flag
actor-calxfgl =
   .name = Luxembourgish Flag
actor-camlfgl =
   .name = Maltese Flag
actor-camrfgl =
   .name = Moroccan Flag
actor-canzfgl =
   .name = New Zealander Flag
actor-canwfgl =
   .name = Norwegian Flag
actor-caomfgl =
   .name = Omani Flag
actor-caslfgl =
   .name = Slovenian Flag
actor-caspfgl =
   .name = Spanish Flag
actor-cazafgl =
   .name = Zairian Flag
actor-caalfgl =
   .name = Albanian Flag
actor-cabmfgl =
   .name = Burmese Flag
actor-cacgfgl =
   .name = Congolese Flag
actor-cakmfgl =
   .name = Kampuchean Flag
actor-calofgl =
   .name = Laotian Flag
actor-cancfgl =
   .name = Nicaraguan Flag
actor-caprfgl =
   .name = Peruvian Flag
actor-casofgl =
   .name = Somalian Flag
actor-casyfgl =
   .name = Syrian Flag
actor-caymfgl =
   .name = Yemeni Flag
actor-capafgl =
   .name = Australian Psi-Corps Flag
actor-cabhfgl =
   .name = Bhutanese Flag
actor-cabufgl =
   .name = Bulgarian Flag
actor-cabunfgl =
   .name = Bulgarian Naval Ensign
actor-caicfgl =
   .name = Icelandic Flag
actor-cajrfgl =
   .name = Jordanian Flag
actor-cangfgl =
   .name = Nigerian Flag
actor-capnfgl =
   .name = Panamanian Flag
actor-capgfgl =
   .name = Portuguese Flag
actor-caswfgl =
   .name = Swedish Flag
actor-caszfgl =
   .name = Swiss Flag
actor-cahlfgl2 =
   .name = Angriverian Flag
actor-caeqfgl =
   .name = Equestrian Flag
actor-cahlfgl =
   .name = Herzlander Flag
actor-capvfgl =
   .name = Ponyvillian Flag

## civilian-naval.yaml
actor-tug =
   .name = Tug Boat
actor-cruise =
   .name = Cruise Ship

## civilian-props.yaml
actor-camisc01 =
   .name = Barrels
actor-camisc02 =
   .name = Barrel
actor-camisc03 =
   .name = Dumpster
actor-camisc04 =
   .name = Mailbox
actor-camisc05 =
   .name = Construction Pipes
actor-camisc06 =
   .name = V3 Rack
actor-camsc11 =
   .name = Tires
actor-camsc12 =
   .name = Bullseye
actor-camsc13 =
   .name = Derelict Tank
actor-ammocrat =
   .name = Ammo Crates
actor-camsc01 =
   .name = Hot Dog Stand
actor-camsc02 =
   .name = Beach Umbrellas
actor-camsc03 =
   .name = Beach Umbrellas
actor-camsc04 =
   .name = Beach Towels
actor-camsc05 =
   .name = Beach Towels
actor-camsc06 =
   .name = Camp Fire
actor-caeuro05 =
   .name = Statue
actor-capark01 =
   .name = Park Bench
actor-capark02 =
   .name = Swing Set
actor-capark03 =
   .name = Merry Go Round
actor-capark04 =
   .name = Park Bench
actor-capark05 =
   .name = Park Bench
actor-capark06 =
   .name = Park Bench
actor-castrt01 =
   .name = Traffic Light
actor-castrt02 =
   .name = Traffic Light
actor-castrt03 =
   .name = Traffic Light
actor-castrt04 =
   .name = Traffic Light
actor-castrt05 =
   .name = Bus Stop
actor-camov01 =
   .name = Drive In Movie Screen
actor-camov02 =
   .name = Drive In Movie Concession Stand
actor-pole01 =
   .name = Utility Pole
actor-pole02 =
   .name = Utility Pole
actor-hdstn01 =
   .name = Alrington Stones
actor-spkr01 =
   .name = Drive-In Speaker
actor-carus02c =
   .name = Kremlin Walls
actor-carus02d =
   .name = Kremlin Walls
actor-carus02e =
   .name = Kremlin Walls
actor-carus02f =
   .name = Kremlin Walls
actor-cakrmw =
   .name = Kremlin Walls
actor-gagate-a =
   .name = Guard Border Crossing
actor-cabarr01 =
   .name = Barricade
actor-cabarr02 =
   .name = Barricade
actor-casin03e =
   .name = Construction Sign
actor-caurb01 =
   .name = Telephone Booth
actor-caurb02 =
   .name = Fire Hydrant
actor-caurb03 =
   .name = Spotlight
actor-cagatene =
   .name = Guard Border Crossing
actor-cagatenw =
   .name = Guard Border Crossing
actor-cagatesw =
   .name = Guard Border Crossing
actor-capark07 =
   .name = Picnic Table

## civilian-structures.yaml
actor-cafncb =
   .name = Black Fence
actor-cafncw =
   .name = White Fence
actor-cafnck =
   .name = Brown Fence
actor-cafncy =
   .name = Yellow Fence
actor-cafncg =
   .name = Green Fence
actor-cafncm =
   .name = Purple Fence
actor-cabarb =
   .name = Barbed Wire Fence
actor-gasand =
   .name = Sandbags
actor-cafncp =
   .name = Prison Camp Fence
actor-cawt01 =
   .name = Water Tower
actor-cats01 =
   .name = Twin Silos
actor-cabarn02 =
   .name = Barn
actor-cawash01 =
   .name = White House
actor-cawsh12 =
   .name = Washington Monument
actor-cawash14 =
   .name = Jefferson Memorial
actor-cawash15 =
   .name = Lincoln Memorial
actor-cawash16 =
   .name = Smithsonian Castle
actor-cawash17 =
   .name = Smithsonian Natural History Museum
actor-cawash18 =
   .name = Fountain
actor-cawash19 =
   .name = Iwo Jima Memorial
actor-cawa2a =
   .name = Pentagon
actor-cawa2b =
   .name = Pentagon
actor-cawa2c =
   .name = Pentagon
actor-cawa2d =
   .name = Pentagon
actor-canewy04 =
   .name = Statue of Liberty
actor-canewy05 =
   .name = World Trade Center
actor-canewy06 =
   .name = Wall Street Office
actor-canewy07 =
   .name = Wall Street Office
actor-canewy08 =
   .name = Wall Street Office
actor-canewy20 =
   .name = Warehouse
actor-canewy21 =
   .name = Warehouse
actor-caarmy01 =
   .name = Army Tent
actor-caarmy02 =
   .name = Army Tent
actor-caarmy03 =
   .name = Army Tent
actor-caarmy04 =
   .name = Army Tent
actor-cafarm01 =
   .name = Farm
actor-cafarm02 =
   .name = Farm Silo
actor-cafarm06 =
   .name = Lighthouse
actor-cacolo01 =
   .name = Air Force Academy Chapel
actor-caind01 =
   .name = Factory
actor-calab =
   .name = Einstein's Lab
actor-cagas01 =
   .name = Gas Station
actor-galite =
   .name = Light Post
actor-city05 =
   .name = Battersea Power Station
actor-catech01 =
   .name = Communications Center
actor-catexs02 =
   .name = Alamo
actor-capars01 =
   .name = Eiffel Tower
actor-capars07 =
   .name = Phone Booth
actor-capars10 =
   .name = Bistro
actor-capars11 =
   .name = Arc de Triumphe
actor-capars12 =
   .name = Notre Dame
actor-capars13 =
   .name = Bistro
actor-capars14 =
   .name = Bistro
actor-cafrma =
   .name = Farm House
actor-cafrmb =
   .name = Outhouse
actor-caprs03 =
   .name = Louvre
actor-cagard01 =
   .name = Guard Shack
actor-cagard02 =
   .name = Guard Shack
actor-carus01 =
   .name = St. Basil's Cathedral
actor-carus02a =
   .name = Kremlin Walls
actor-carus02b =
   .name = Kremlin Walls
actor-carus02g =
   .name = Kremlin Wall Clock Tower
actor-carus03 =
   .name = Kremlin Palace
actor-camiam04 =
   .name = Lifeguard Hut
actor-camiam08 =
   .name = Arizona Memorial
actor-camex01 =
   .name = Mayan Pyramid
actor-camex02 =
   .name = Mayan Castillo
actor-camex03 =
   .name = Mayan Minor Temple
actor-camex04 =
   .name = Mayan Large Temple
actor-camex05 =
   .name = Mayan Platfrom
actor-caeur1 =
   .name = Cottage
actor-caeur2 =
   .name = Cottage
actor-cachig04 =
   .name = Associates Center
actor-cachig05 =
   .name = Sears Tower
actor-cachig06 =
   .name = Water Tower
actor-castl04 =
   .name = St Louis Arch
actor-castl05a =
   .name = Stadium
actor-castl05b =
   .name = Stadium
actor-castl05c =
   .name = Stadium
actor-castl05d =
   .name = Stadium
actor-castl05e =
   .name = Stadium
actor-castl05f =
   .name = Stadium
actor-castl05g =
   .name = Stadium
actor-castl05h =
   .name = Stadium
actor-camsc07 =
   .name = Hut
actor-camsc08 =
   .name = Hut
actor-camsc09 =
   .name = Hut
actor-camsc10 =
   .name = McBurger Kong
actor-cabunk01 =
   .name = Concrete Bunker
actor-cabunk02 =
   .name = Concrete Bunker
actor-cabunk03 =
   .name = Concrete Bunker
actor-cabunk04 =
   .name = Concrete Bunker
actor-cala03 =
   .name = Hollywood Sign
actor-cala04 =
   .name = Hollywood Bowl
actor-cala05 =
   .name = LAX
actor-cala06 =
   .name = LAX Control Tower
actor-cala07 =
   .name = Movie Theater
actor-cala08 =
   .name = Car Dealership
actor-cala09 =
   .name = Convenience Store
actor-cala10 =
   .name = Billboard
actor-cala11 =
   .name = Hollywood Bowl
actor-cala12 =
   .name = Hollywood Bowl
actor-cala13 =
   .name = Hollywood Sign
actor-cala14 =
   .name = Mini Mall
actor-cala15 =
   .name = Mini Mall
actor-caegyp01 =
   .name = Pyramid
actor-caegyp02 =
   .name = Pyramid
actor-caegyp03 =
   .name = Sphinx
actor-caegyp04 =
   .name = Pyramid
actor-caegyp05 =
   .name = Pyramid
actor-caegyp06 =
   .name = Pyramid
actor-calond03 =
   .name = Pub
actor-calond04 =
   .name = British Parlaiment
actor-calond05 =
   .name = Big Ben
actor-calond06 =
   .name = Tower of London
actor-casanf04 =
   .name = Golden Gate Bridge
actor-casanf05 =
   .name = Alcatraz
actor-casanf09 =
   .name = Golden Gate Bridge
actor-casanf10 =
   .name = Golden Gate Bridge
actor-casanf11 =
   .name = Golden Gate Bridge
actor-casanf12 =
   .name = Golden Gate Bridge
actor-casanf13 =
   .name = Golden Gate Bridge
actor-casanf14 =
   .name = Golden Gate Bridge
actor-casanf15 =
   .name = Alcatraz Water Tower
actor-casanf16 =
   .name = Light House
actor-casydn02 =
   .name = McRoo Burger
actor-casydn03 =
   .name = Sydney Opera House

actor-calunr01 =
   .name = Lunar Lander
   .dune-name = Dunar Lander

actor-calunr02 =
   .name = American Flag
actor-caseat01 =
   .name = Seattle Space Needle
actor-caseat02 =
   .name = MassiveSoft Campus
actor-catran01 =
   .name = Crypt
actor-catran02 =
   .name = Crypt
actor-catran03 =
   .name = Yuri's Fortress
actor-catime01 =
   .name = Time Machine
actor-catime02 =
   .name = Time Machine
actor-caeast01 =
   .name = Moai
actor-caeast02 =
   .name = Yuri Bust

## civilian-vehicles.yaml
actor-bus =
   .name = School Bus
actor-limo =
   .name = Limousine
actor-pick =
   .name = Pickup Truck
actor-car =
   .name = Automobile
actor-wini =
   .name = Recreational Vehicle
actor-propa =
   .name = Propaganda Truck
actor-cop =
   .name = Police Car
actor-euroc =
   .name = Automobile
actor-cona =
   .name = Excavator
actor-trucka =
   .name = Truck
actor-truckb =
   .name = Truck
actor-suvb =
   .name = Automobile
actor-suvw =
   .name = Automobile
actor-stang =
   .name = Automobile
actor-ptruck =
   .name = Pickup Truck
actor-taxi =
   .name = Taxi
actor-ambu =
   .name = Ambulance
actor-bcab =
   .name = Black Cab
actor-cblc =
   .name = Cable Car
actor-ddbx =
   .name = Bus
actor-doly =
   .name = Camera Dolly
actor-ftrk =
   .name = Fire Truck
actor-jeep =
   .name = Pickup Truck
actor-ycab =
   .name = Yellow Cab
actor-civp =
   .name = Passenger Plane
actor-truckc =
   .name = Truck
actor-car2 =
   .name = Automobile
actor-car3 =
   .name = Automobile
actor-mixer =
   .name = Cement Mixer
actor-flata =
   .name = Missile Truck
actor-flatb =
   .name = Empty Truck

## civilians.yaml
actor-vladimir =
   .name = General Vladimir
actor-pentgen =
   .name = General Pentagon
actor-ssrv =
   .name = Secret Service
actor-pres =
   .name = President
actor-rmnv =
   .name = Romanov
actor-eins =
   .name = Albert Einstein
actor-mumy =
   .name = Evil Mummy
actor-arnd =
   .name = Arnnie Frankenfurter
actor-stln =
   .name = Sammy Stallion

## default-naval.yaml
meta-amphibioustransport =
   .name = Amphibious Transport
   .description = General-purpose naval transport.
    Can carry infantry and vehicles.
    
      Unarmed

## default-structures.yaml
meta-constructionyard =
   .name = Construction Yard
   .description = Allows construction of base structures.

meta-barracks =
   .name = Barracks
   .description = Trains infantry.
    Can heal nearby infantry.
    
      Cannot be placed on water.
      Can be rotated.

meta-refinery =
   .name = Ore Refinery
   .description = Processes ore into credits.
    
      Can be rotated.


meta-warfactory =
   .name = War Factory
   .description = Produces vehicles.
    
    Abilities:
    - Comes with 3 repair drones.
    
      Cannot be placed on water.
      Can be rotated.

meta-shipyard =
   .name = Naval Yard
   .description = Produces ships, submarines, and other naval units.
    
    Abilities:
    - Comes with 3 repair drones.
    
      Can only be placed on water.

meta-servicedepot =
   .name = Service Depot
   .description = Repairs vehicles and removes Terror Drones for a price.
meta-battlelab =
   .name = Battle Lab
   .description = Allows deployment of advanced units.
    Researches advanced upgrades.
    
      Can be rotated.

## default-vehicles.yaml
meta-miner =
   .name = Ore Miner
   .description = Gathers Ore and Gems.
    
      Unarmed
    
    Abilities:
    - Can move over water
    - Cannot be mind controlled

meta-constructionvehicle =
   .name = Mobile Construction Vehicle
   .description = Deploys into a Construction Yard.
    
      Unarmed
    
    Abilities:
    - Can move over water

## defaults.yaml
meta-civbuilding =
   .name = Civilian Building
meta-flag =
   .name = Flag
meta-wall =
   .name = Wall
meta-fence =
   .name = Fence
meta-gate =
   .name = Gate
   .description = Automated barrier that opens for allied units.
    
      Cannot be placed on water.
      Can be rotated.

meta-rubble =
   .name = Rubble
meta-civilianinfantry =
   .name = Civilian
meta-oredrill =
   .name = Ore Drill
meta-tree =
   .name = Tree
meta-streetsign =
   .name = Street Sign
meta-trafficlight =
   .name = Traffic Light
meta-streetlight =
   .name = Street Light
meta-telephonepole =
   .name = Utility Pole
meta-rock =
   .name = Rock
meta-crate =
   .name = Crate

## misc.yaml
actor-ambient-bird-jungle-1-name = Jungle Bird Ambient Sound 1
actor-ambient-bird-jungle-2-name = Jungle Bird Ambient Sound 2
actor-ambient-bird-morning-name = Morning Bird Ambient Sound
actor-ambient-bird-park-name = Park Bird Ambient Sound
actor-ambient-bird-temperate-1-name = Temperate Bird Ambient Sound 1
actor-ambient-bird-temperate-2-name = Temperate Bird Ambient Sound 2
actor-ambient-cricket-1-name = Cricket Ambient Sound 1
actor-ambient-cricket-2-name = Cricket Ambient Sound 2
actor-ambient-cricket-3-name = Cricket Ambient Sound 3
actor-ambient-hawk-name = Hawk Ambient Sound
actor-ambient-seagull-1-name = Seagull Ambient Sound 1
actor-ambient-seagull-2-name = Seagull Ambient Sound 2
actor-ambient-owl-name = Owl Ambient Sound
actor-ambient-river-name = River Ambient Sound
actor-ambient-traffic-name = Traffic Ambient Sound
actor-ambient-urban-1-name = Urban Ambient Sound 1
actor-ambient-urban-2-name = Urban Ambient Sound 2
actor-ambient-wave-1-name = Wave Ambient Sound 1
actor-ambient-wave-2-name = Wave Ambient Sound 2
actor-ambient-wave-3-name = Wave Ambient Sound 3
actor-ambient-wind-1-name = Wind Ambient Sound 1
actor-ambient-wind-2-name = Wind Ambient Sound 2
actor-camera-name = (reveals area to owner)
actor-sonar-name = (support power proxy camera)
actor-camera-satscan-name = Satellite Scan
actor-magnetic-beam-1-name = Magnetic Beam
meta-lamppost-name = (Invisible Light Post)
actor-galite-white-name = (Invisible Light Post)
actor-galite-black-name = (Invisible Negative Light Post)
actor-galite-red-name = (Invisible Red Light Post)
actor-galite-cyan-name = (Invisible Negative Red Light Post)
actor-galite-green-name = (Invisible Green Light Post)
actor-galite-blue-name = (Invisible Blue Light Post)
actor-galite-yellow-name = (Invisible Yellow Light Post)
actor-galite-orange-name = (Invisible Orange Light Post)
actor-galite-purple-name = (Invisible Purple Light Post)
actor-galite-morning-temp-name = (Invisible Temperate Morning Light Post)
actor-galite-day-temp-name = (Invisible Temperate Day Light Post)
actor-galite-dusk-temp-name = (Invisible Temperate Dusk Light Post)
actor-galite-night-temp-name = (Invisible Temperate Night Light Post)
actor-galite-morning-snow-name = (Invisible Snow Morning Light Post)
actor-galite-day-snow-name = (Invisible Snow Day Light Post)
actor-galite-dusk-snow-name = (Invisible Snow Dusk Light Post)
actor-galite-dusk-night-name = (Invisible Snow Night Light Post)

## soviet-infantry.yaml
actor-e2 =
   .name = Conscript
   .description = Cheap rifle infantry.
    
      Strong vs Infantry
      Weak vs Vehicles, Aircraft
    
    Upgradeable with:
    - Bullet-Proof Coats
    - Molotov Cocktails
    - Armor-Piercing Bullets

actor-flakt =
   .name = Flak Trooper
   .description = Anti-Air and anti-Infantry unit.
    
      Strong vs Infantry, Aircraft
      Weak vs Vehicles
    
    Upgradeable with:
    - Bullet-Proof Coats

actor-shk =
   .name = Tesla Trooper
   .description = Special armored unit using electricity.
    
      Strong vs Infantry, Tanks
      Weak vs Aircraft
    
    Abilities:
    - Can charge Tesla Coils
    
    Upgradeable with:
    - Overcharge (Soviet Union)

actor-terror =
   .name = Terrorist
   .description = Carries C4 charges taped to his body and kamikazes enemies
    blowing them up quickly and efficiently.
    
      Strong vs Ground units
      Weak vs Aircraft
    
    Upgradeable with:
    - Targeted Explosives (Cuba)

actor-deso =
   .name = Desolator
   .description = Carries a radiation-emitting weapon.
    Can deploy for area-of-effect damage.
    
      Strong vs Infantry, Light vehicles
      Weak vs Tanks, Aircraft
    
    Abilities:
    - Immune to Radiation
    
    Upgradeable with:
    - Advanced Irradiators (Iraq)

actor-ivan =
   .name = Crazy Ivan
   .description = Specialist for explosives. Can plant a Bomb on anything, even Cows.
    
    Upgradeable with:
    - Targeted Explosives (Cuba)
    - High Explosive Bombs (Libya)

actor-civan =
   .name = Chrono Ivan
   .description = Specialist for explosives. Can plant a Bomb on anything, even Cows.
    
    Abilities:
    - Can teleport anywhere on the map
    
    Upgradeable with:
    - High Explosive Bombs (Libya)

actor-boris =
   .name = Boris Bukov
   .description = Elite commando infantry, armed with
    AKM rifle and signal flare.
    
      Strong vs Infantry, Vehicles, Buildings
      Weak vs Aircraft
    
    Abilities:
    - Can call airstrike against Buildings
    - Can move over water
    
    Upgradeable with:
    - Armor-Piercing Bullets
    
      Maximum 1 can be trained.

## soviet-naval.yaml
actor-sub =
   .name = Typhoon Attack Submarine
   .description = Submerged anti-ship unit armed with torpedoes.
    
      Strong vs Ships
      Weak vs Ground units, Aircraft
    
    Abilities:
    - Can detect Stealth units

actor-hyd =
   .name = Sea Scorpion
   .description = Anti-Air and Anti-Infantry naval unit.
    
      Strong vs Infantry, Aircraft
      Weak vs Vehicles, Naval units

actor-sqd =
   .name = Giant Squid
   .generic-name = Squid
   .description = Large ocean creature capable of grabbing and sinking ships.
    
      Strong vs Ships
    
    Abilities:
    - Can be deployed to remove other Squids from nearby ships
    - Can detect Stealth units

actor-dred =
   .name = Dreadnought
   .description = Long-range rocket artillery ship.
    
      Strong vs Buildings, Infantry
      Weak vs Ships, Aircraft
    
    Upgradeable with:
    - Radioactive Warheads
    - Advanced Irradiators (Iraq)
    - High Explosive Bombs (Libya)

actor-dmisl =
   .name = Dreadnought Missile

## soviet-structures.yaml
actor-napowr =
   .name = Tesla Reactor
   .description = Provides power for other structures.
    
      Can be rotated.

actor-naradr =
   .name = Radar Tower
   .description = Provides radar.
    Researches basic upgrades.
    
    Provides a different support power depening on the subfaction:
    - Tesla Drop (Soviet Union)
    - Radiation Missile (Iraq)
    - Instant Bunker (Vietnam)
    - Death Bombs (Cuba)
    - Ambush (Libya)

actor-nanrct =
   .name = Nuclear Reactor
   .description = Provides a large amount of power for other structures.
    
    Upgradeable with:
    - Advanced Irradiators (Iraq)
    
      Can be rotated.

actor-naclon =
   .name = Cloning Vats
   .description = Clones most trained infantry.
    
      Cannot be placed on water.
      Maximum 1 can be built.

actor-napsis =
   .name = Psychic Sensor
   .description = Detects enemy units and structures.
    
      Requires power to operate.

actor-nairon =
   .name = Iron Curtain Device
   .description = Grants invulnerability to vehicles and structures.
    Kills infantry.
    
      Requires power to operate.

actor-namisl =
   .name = Nuclear Missile Silo
   .description = Provides an atomic bomb.
    
    Upgradeable with:
    - Advanced Irradiators (Iraq)
    
      Requires power to operate.

actor-nawall =
   .name = Soviet Wall
   .description = Heavy wall capable of blocking units and projectiles.
    
      Cannot be placed on water.
 
actor-naflak =
   .name = Flak Cannon
   .description = Automated anti-aircraft defense.
    
      Strong vs Aircraft
      Weak vs Ground units
    
    Abilities:
    - Can detect Steath units
    
      Requires power to operate.

actor-tesla =
   .name = Tesla Coil
   .description = Advanced base defense.
    Can be buffed or made work during low power by Tesla Troopers.
    
      Strong vs Infantry, Vehicles
      Weak vs Aircraft
    
    Abilities:
    - Can detect Steath units
    
    Upgradeable with:
    - Overcharge (Russia)
    
      Requires power to operate.

actor-nalasr =
   .name = Sentry Gun
   .description = Automated anti-infantry defense.
    
      Strong vs Infantry
      Weak vs Vehicles, Aircraft
    
    Abilities:
    - Can detect Steath units
    
    Upgradeable with:
    - Armor-Piercing Bullets
 
actor-nabnkr =
   .name = Battle Bunker
   .description = Static defense with fireports for 6 garrisoned soldiers.
    Infantry inside cannot be killed by garrison killers.
    Comes with a Conscript inside.
    
      Cannot be placed on water.

actor-naindp =
   .name = Industrial Plant
   .description = Decreases vehicle costs by 25%.
    
      Maximum 1 can be built.

## soviet-vehicles.yaml
actor-harv =
   .name = War Miner
   .description = Gathers Ore and Gems.
    
      Strong vs Infantry
      Weak vs Vehicles, Aircraft
    
    Abilities:
    - Can move over water
    - Cannot be mind controlled
    
    Upgradeable with:
    - Armor-Piercing Bullets

actor-dron =
   .name = Terror Drone
   .description = Small vehicle that can infect enemy vehicles and slowly kill them.
    
      Strong vs Infantry, Vehicles
      Weak vs Aircraft
    
    Abilities:
    - Immune to Radiation
    - Cannot be mind controlled

actor-htk =
   .name = Flak Track
   .description = Anti-Air and Anti-Infantry vehicle capable of transporting Infantry.
    
      Strong vs Infantry, Aircraft
      Weak vs Vehicles
    
    Abilities:
    - Can detect Stealth units

actor-htnk =
   .name = Rhino Heavy Tank
   .description = Soviet Main Battle Tank.
    
      Strong vs Vehicles
      Weak vs Infantry, Aircraft
    
    Upgradeable with:
    - Nuclear Engines
    - Uranium Shells
    - Advanced Irradiators (Iraq)

actor-apoc =
   .name = Apocalypse Tank
   .description = Soviet Advanced Battle Tank with Double Barrel
    and Anti-Aircraft Missile Launcher.
    
      Strong vs Vehicles, Aircraft
      Weak vs Infantry
    
    Abilities:
    - Can crush enemy vehicles
    
    Upgradeable with:
    - Nuclear Engines
    - Uranium Shells
    - Advanced Irradiators (Iraq)

actor-ttnk =
   .name = Tesla Tank
   .description = Russian special tank armed with dual small Tesla Coils.
    
      Strong vs Infantry, Vehicles
      Weak vs Aircraft
    
    Upgradeable with:
    - Overcharge (Soviet Union)

actor-dtruck =
   .name = Demolition Truck
   .description = Demolition Truck, actively armed with nuclear explosives.
    
      Strong vs Infantry, Vehicles, Buildings
      Weak vs Aircraft
    
    Upgradeable with:
    - High Explosive Bombs (Libya)

actor-v3 =
   .name = V3 Launcher
   .description = Long-range rocket artillery.
    
      Strong vs Buildings, Infantry
      Weak vs Aircraft
    
    Upgradeable with:
    - Radioactive Warheads
    - Advanced Irradiators (Iraq)
    - High Explosive Bombs (Libya)


actor-v3rocket =
   .name = V3 Rocket

## tech-structures.yaml
actor-caoild =
   .name = Tech Oil Derrick
   .description = Periodically provides cash.
actor-caoild-mwspawner =
   .name = Megawealth Only Oil Derrick
actor-caoild-nonmwspawner =
   .name = Non-Megawealth Only Oil Derrick
actor-caairp =
   .name = Tech Airport
   .description = Provides Paradrop support power.
actor-cahosp =
   .name = Tech Hospital
   .description = Allows infantry to self-heal.
actor-caoutp =
   .name = Tech Outpost
   .description-1 = Provides repairing ground for vehicles.
   .description-2 = Armed with a missile launcher.
   .description-3 = Provides build area.
actor-capowr =
   .name = Tech Power Plant
   .description = Provides 400 power.
actor-camach =
   .name = Tech Machine Shop
   .description = Allows vehicles to self-repair.
actor-caslab =
   .name = Tech Secret Lab
   .description = Allow construction of a new 3rd tier vehicle.

## trees.yaml
actor-tibtre04 =
   .name = Gem Drill
actor-tibtre05 =
   .name = Fast Ore Drill

## yuri-infantry.yaml
actor-slav =
   .name = Slave
   .description = Gathers Ore and Gems.

actor-init =
   .name = Initiate
   .description = Basic Yuri Infantry.
    
      Strong vs Infantry
      Weak vs Vehicles, Aircraft
    
    Upgradeable with:
    - Camouflage

actor-brute =
   .name = Brute
   .description = Powerful soldiers.
    
      Strong vs Infantry, Vehicles
      Weak vs Aircraft
    
    Abilities:
    - Cannot be eaten by Attack Dogs
    
    Upgradeable with:
    - DNA Boosters (Transylvania)

actor-virus =
   .name = Virus
   .description = Sniper infantry armed with toxic bullets.
    Killed units leave toxin clouds.
    
      Strong vs Infantry
      Weak vs Vehicles, Aircraft
    
    Abilities:
    - Can kill garrisoned infantry
    
    Upgradeable with:
    - Camouflage

actor-yuri =
   .name = Yuri Clone
   .description = Psychic infantry. Can mind control enemy units.
    
      Strong vs Infantry, Vehicles
      Weak vs Terror Drones, Aircraft, Buildings
    
    Upgradeable with:
    - Mastery of Mind (Antarctica)

actor-yuripr =
   .name = Yuri Prime
   .description = Psychic infantry. Can mind control enemy units and structures.
    Can be deployed to unleash a powerful psychic wave.
    
      Strong vs Infantry, Vehicles, Buildings
      Weak vs Terror Drones, Aircraft
    
    Abilities:
    - Cannot be eaten by Attack Dogs
    - Can move over water
    
    Upgradeable with:
    - Mastery of Mind (Antarctica)
    
      Maximum 1 can be trained.

actor-lunr =
   .name = Cosmonaut
   .description = Airborne soldier.
    
      Strong vs Infantry, Aircraft
      Weak vs Vehicles

## yuri-naval.yaml
actor-bsub =
   .name = Boomer Submarine
   .description = Submerged anti-ship and anti-structure armed with
    double torpedo and missile launchers.
    
      Strong vs Ships, Buildings
      Weak vs Ground units, Aircraft
    
    Abilities:
    - Can detect Stealth units

actor-cmisl =
   .name = Cruise Missile

## yuri-structures.yaml
actor-yapowr =
   .name = Bio Reactor
   .description = Provides power for other structures.
    Can be occupied with up to 5 infantry for 50 more power each.

actor-yarefn =
   .name = Slave Miner
   .description = Gathers and processes ore.

actor-yayard =
   .name = Submarine Pen
   .description = Produces Psi-Corps submarines, and other naval units.
    
    Abilities:
    - Comes with 3 repair drones.
    
      Can only be placed on water.

actor-yagrnd =
   .name = Grinder
   .description = Converts Infantry and Vehicles back to credits.

actor-yapsis =
   .name = Psychic Sensor
   .description = Detects enemy units and structures.
    
      Requires power to operate.

actor-yaclon =
   .name = Cloning Vats
   .description = Clones most trained infantry.
    
      Cannot be placed on water.
      Maximum 1 can be built.

actor-yawall =
   .name = Citadel Wall
   .description = Heavy wall capable of blocking units and projectiles.
    
      Cannot be placed on water.

actor-yaggun =
   .name = Gatling Cannon
   .description = Automated anti-infantry and anti-air defense.
    
      Strong vs Infantry, Aircraft
      Weak vs Vehicles
    
    Abilities:
    - Can detect Steath units
    
    Upgradeable with:
    - Chainguns
    
      Requires power to operate.

actor-natbnk =
   .name = Tank Bunker
   .description = Static defense with fireports for a vehicle to garrison.
    Provides increased firepower, fire speed and range to the vehicle.
    
      Cannot be placed on water.

actor-yapsyt =
   .name = Psychic Tower
   .description = Tower capable of mind controlling up to 3 enemy units.
    
      Strong vs Infantry, Vehicles
      Weak vs Buildings, Aircraft
    
    Abilities:
    - Can detect Steath units
    
      Requires power to operate.

actor-yagntc =
   .name = Genetic Mutator Device
   .description = Makes vehicles invisible.
    Kills infantry.
    
      Requires power to operate.

actor-yappet =
   .name = Psychic Dominator
   .description = Release powerful energy that damages structures and mind controls units.
    
      Requires power to operate.

actor-yacomd =
   .name = Yuri's Command Center
actor-yapppt =
   .name = Psychic Dominator
actor-yarock =
   .name = Rocket Launch Pad
actor-yapsyb =
   .name = Psychic Beacon

actor-yaeast02 =
   .name = Yuri Statue
   .description = Big defense structure armed with lasers.
    
      Strong vs Infantry, Vehicles
      Weak vs Aircraft
    
    Abilities:
    - Can detect Steath units

actor-yagate =
   .name = Psi-Corps Gate
actor-psirefn =
   .name = Ore Refinery

## yuri-vehicles.yaml
actor-smin =
   .name = Slave Miner
   .description = Gathers and processes ore.
    
    Abilities:
    - Cannot be mind controlled
    
    Upgradeable with:
    - Grinder Treads
    - Chaos Tank Compensators (Yurigrad)

actor-ltnk =
   .name = Lasher Light Tank
   .description = Psi-Corps Main Battle Tank.
    
      Strong vs Vehicles
      Weak vs Infantry, Aircraft
    
    Upgradeable with:
    - Grinder Treads
    - Autoloaders
    - Chaos Tank Compensators (Yurigrad)
    - Laser Capacitors (Lazarus Corps)

actor-ytnk =
   .name = Gatling Tank
   .description = Anti-infantry and anti-air vehicle.
    Fires faster as it continues to fire.
    
      Strong vs Infantry, Aircraft
      Weak vs Vehicles
    
    Abilities:
    - Can detect Stealth units
    
    Upgradeable with:
    - Chaos Tank Compensators (Yurigrad)
    - Chainguns

actor-caos =
   .name = Chaos Drone
   .description = Drone capable of releasing gas that causes enemy units to fire random units.
    Units affected by the gas deal more damage.
    
      Strong vs Infantry, Vehicles
      Weak vs Aircraft
    
    Abilities
    - Can be deployed to release gas continuously
    - Cannot be mind controlled
    
    Upgradeable with:
    - Chaos Tank Compensators (Yurigrad)

actor-tele =
   .name = Magnetron
   .description = Long range magnetic field generator.
    Freezes vehicles and damages structures.
    
      Strong vs Buildings, Vehicles
      Weak vs Infantry, Aircraft
    
    Upgradeable with:
    - Chaos Tank Compensators (Yurigrad)

actor-mind =
   .name = Master Mind
   .description = Heavy vehicle capable of mind controlling multipile enemy units.
    Starts taking damage if controlling more than 3.
    
      Strong vs Infantry, Vehicles
      Weak vs Aircraft
    
    Abilities:
    - Can crush enemy vehicles
    
    Upgradeable with:
    - Grinder Treads
    - Chaos Tank Compensators (Yurigrad)
    - Mastery of Mind (Antarctica)
